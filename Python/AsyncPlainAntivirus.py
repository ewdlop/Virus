import asyncio
import aiofiles
import aionotify
import pyclamd
import hashlib
import os
import logging
from pathlib import Path
from concurrent.futures import ThreadPoolExecutor

# Configuration
QUARANTINE_DIR = "/path/to/quarantine"
LOG_FILE = "async_plainAntivirus.log"
SCAN_INTERVAL = 300  # 5 minutes for periodic scans
MONITOR_DIRS = ["/home", "/tmp"]  # Directories to monitor

# Setup logging
logging.basicConfig(
    filename=LOG_FILE,
    level=logging.INFO,
    format='%(asctime)s - %(message)s',
    datefmt='%Y-%m-%d %H:%M:%S'
)

class AsyncPlainAntivirus:
    def __init__(self):
        self.executor = ThreadPoolExecutor()
        self.loop = asyncio.get_event_loop()
        self.cd = pyclamd.ClamdAgnostic()
        
        try:
            self.cd.ping()
        except pyclamd.ConnectionError:
            logging.error("ClamAV daemon not running. Start it first.")
            raise

    async def scan_file(self, file_path):
        try:
            return await self.loop.run_in_executor(
                self.executor,
                self.cd.scan_file,
                file_path
            )
        except Exception as e:
            logging.error(f"Scan error: {e}")
            return None

    async def quarantine_file(self, file_path, virus_name):
        if not os.path.exists(QUARANTINE_DIR):
            os.makedirs(QUARANTINE_DIR)

        async with aiofiles.open(file_path, 'rb') as f:
            content = await f.read()
            file_hash = hashlib.sha256(content).hexdigest()
            
        quarantined_path = os.path.join(QUARANTINE_DIR, f"{file_hash}.malware")
        await self.loop.run_in_executor(
            self.executor,
            os.rename,
            file_path,
            quarantined_path
        )
        logging.info(f"Quarantined {file_path} as {quarantined_path} - Detected: {virus_name}")

async def file_monitor(antivirus):
    watcher = aionotify.Watcher()
    for directory in MONITOR_DIRS:
        watcher.watch(
            alias=directory,
            path=directory,
            flags=aionotify.Flags.CREATE | aionotify.Flags.MODIFY
        )
    
    await watcher.setup(asyncio.get_event_loop())
    
    try:
        while True:
            event = await watcher.get_event()
            file_path = os.path.join(event.alias, event.name)
            
            if not event.name.startswith('.') and not os.path.isdir(file_path):
                logging.info(f"Detected change in {file_path}")
                scan_result = await antivirus.scan_file(file_path)
                
                if scan_result and file_path in scan_result:
                    virus_name = scan_result[file_path][1]
                    await antivirus.quarantine_file(file_path, virus_name)
    finally:
        watcher.close()

async def periodic_scan(antivirus):
    while True:
        logging.info("Starting periodic scan...")
        for directory in MONITOR_DIRS:
            for root, _, files in os.walk(directory):
                for file in files:
                    file_path = os.path.join(root, file)
                    scan_result = await antivirus.scan_file(file_path)
                    if scan_result and file_path in scan_result:
                        virus_name = scan_result[file_path][1]
                        await antivirus.quarantine_file(file_path, virus_name)
        await asyncio.sleep(SCAN_INTERVAL)

async def main():
    try:
        antivirus = AsyncPlainAntivirus()
        monitor_task = asyncio.create_task(file_monitor(antivirus))
        scan_task = asyncio.create_task(periodic_scan(antivirus))
        await asyncio.gather(monitor_task, scan_task)
    except Exception as e:
        logging.error(f"Critical error: {e}")
        raise

if __name__ == "__main__":
    print("Starting async antivirus...")
    try:
        asyncio.run(main())
    except KeyboardInterrupt:
        print("\nAntivirus service stopped")
