import pyclamd
import time
import hashlib
import os
import logging
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler

# Configuration
QUARANTINE_DIR = "/path/to/quarantine"
LOG_FILE = "plainAntivirus.log"
SCAN_INTERVAL = 300  # 5 minutes for periodic scans

# Setup logging
logging.basicConfig(
    filename=LOG_FILE,
    level=logging.INFO,
    format='%(asctime)s - %(message)s',
    datefmt='%Y-%m-%d %H:%M:%S'
)

class PlainAntivirus:
    def __init__(self):
        self.cd = pyclamd.ClamdAgnostic()
        try:
            self.cd.ping()
        except pyclamd.ConnectionError:
            print("ClamAV daemon not running. Start it first.")
            exit(1)

    def scan_file(self, file_path):
        try:
            scan_result = self.cd.scan_file(file_path)
            if scan_result:
                virus_name = scan_result[file_path][1]
                return virus_name
        except pyclamd.ScanError as e:
            logging.error(f"Scan error: {e}")
        return None

    def quarantine_file(self, file_path, virus_name):
        if not os.path.exists(QUARANTINE_DIR):
            os.makedirs(QUARANTINE_DIR)
        
        # Generate unique quarantine name
        file_hash = hashlib.sha256(open(file_path, 'rb').read()).hexdigest()
        quarantined_path = os.path.join(QUARANTINE_DIR, f"{file_hash}.malware")
        
        os.rename(file_path, quarantined_path)
        logging.info(f"Quarantined {file_path} as {quarantined_path} - Detected: {virus_name}")

class FileEventHandler(FileSystemEventHandler):
    def __init__(self, antivirus):
        self.antivirus = antivirus

    def on_created(self, event):
        if not event.is_directory:
            self.scan_and_handle(event.src_path)

    def on_modified(self, event):
        if not event.is_directory:
            self.scan_and_handle(event.src_path)

    def scan_and_handle(self, file_path):
        if os.path.abspath(file_path).startswith(os.path.abspath(QUARANTINE_DIR)):
            return
        
        virus_name = self.antivirus.scan_file(file_path)
        if virus_name:
            self.antivirus.quarantine_file(file_path, virus_name)

def real_time_monitoring():
    antivirus = PlainAntivirus()
    event_handler = FileEventHandler(antivirus)
    observer = Observer()
    observer.schedule(event_handler, path='/', recursive=True)
    observer.start()
    
    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        observer.stop()
    observer.join()

def full_scan(path_to_scan):
    antivirus = PlainAntivirus()
    for root, dirs, files in os.walk(path_to_scan):
        for file in files:
            file_path = os.path.join(root, file)
            virus_name = antivirus.scan_file(file_path)
            if virus_name:
                antivirus.quarantine_file(file_path, virus_name)

if __name__ == "__main__":
    print("Starting antivirus...")
    real_time_monitoring()
