# Specify the directory to decrypt
$path = "/mnt"

# Get all files in the directory and subdirectories
$files = Get-ChildItem -Path $path -Recurse -File

foreach ($file in $files) {
    try {
        # Decrypt the file using the Windows Encrypting File System (EFS)
        $file.FullName | ForEach-Object { cipher /D $_ }
        Write-Output "Decrypted: $($file.FullName)"
    } catch {
        Write-Warning "Failed to decrypt: $($file.FullName)"
    }
}
