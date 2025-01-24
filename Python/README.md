Creating a full-fledged antivirus is complex, but here's a basic Python example using ClamAV's engine and real-time monitoring. This example requires `clamav` and `pyclamd` installed.

```python
import pyclamd
import time
import hashlib
import os
import logging
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler

# Configuration
QUARANTINE_DIR = "/path/to/quarantine"
LOG_FILE = "antivirus.log"
SCAN_INTERVAL = 300  # 5 minutes for periodic scans

# Setup logging
logging.basicConfig(
    filename=LOG_FILE,
    level=logging.INFO,
    format='%(asctime)s - %(message)s',
    datefmt='%Y-%m-%d %H:%M:%S'
)

class Antivirus:
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
    antivirus = Antivirus()
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
    antivirus = Antivirus()
    for root, dirs, files in os.walk(path_to_scan):
        for file in files:
            file_path = os.path.join(root, file)
            virus_name = antivirus.scan_file(file_path)
            if virus_name:
                antivirus.quarantine_file(file_path, virus_name)

if __name__ == "__main__":
    print("Starting antivirus...")
    real_time_monitoring()
```

### Requirements:
1. Install ClamAV and keep virus definitions updated (`freshclam`)
2. Python dependencies:
   ```bash
   pip install pyclamd watchdog
   ```

### Features:
1. Real-time file monitoring using watchdog
2. Automatic quarantine of infected files
3. SHA256 hashing for quarantined files
4. Logging of all actions
5. Uses ClamAV's detection engine

### Limitations:
- Requires ClamAV daemon running
- No heuristic analysis
- Basic quarantine system
- No GUI

### Security Notes:
1. Regularly update ClamAV with `freshclam`
2. Run with appropriate permissions
3. Review quarantine directory regularly

This is a simplified example. Real antivirus software includes multiple detection methods (signature/heuristic/behavioral), sandboxing, and deeper system integration.
