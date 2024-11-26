# Specify the directory to encrypt
$path = "/mnt"

# Get all files in the directory and subdirectories
$files = Get-ChildItem -Path $path -Recurse -File

foreach ($file in $files) {
    try {
        # Encrypt the file using the Windows Encrypting File System (EFS)
        $file.FullName | ForEach-Object { cipher /E $_ }
        Write-Output "Encrypted: $($file.FullName)"
    } catch {
        Write-Warning "Failed to encrypt: $($file.FullName)"
    }
}
