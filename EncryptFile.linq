<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

class FileEncryption
{
	public static void Main(string[] args)
	{
		string inputFilePath = @$"{Path.GetDirectoryName (Util.CurrentQueryPath)}\InputFile.txt";

		string decryptedFilePath =  @$"{Path.GetDirectoryName (Util.CurrentQueryPath)}\DecryptedFile.txt";

		string key = "ThisIsA256BitKey1234567890123456"; // 32-byte key for AES-256

		// Encrypt the file
		byte[] IV = RandomIVGenerator.GenerateRandomBytes(16);

		string encryptedFilePath = @$"{Path.GetDirectoryName(Util.CurrentQueryPath)}\{BitConverter.ToString(IV)}.enc";
				
		EncryptFile(inputFilePath, encryptedFilePath, key, IV);
		Console.WriteLine($"File encrypted: {encryptedFilePath}");

		// Decrypt the file
		DecryptFile(encryptedFilePath, decryptedFilePath, key, Convert.FromHexString(Path.GetFileName(encryptedFilePath).Split(".")[0].Trim().Replace("-", "").Replace(" ", "")));
		Console.WriteLine($"File decrypted: {decryptedFilePath}");
		
		OpenFile(decryptedFilePath);
	}

	public static void OpenFile(string filePath)
	{
		try
		{
			Process process = new Process();
			process.StartInfo.FileName = filePath;
			process.StartInfo.UseShellExecute = true;
			process.Start();
		}
		catch (Exception ex)
		{
			Console.WriteLine("Could not open the file: " + ex.Message);
		}
	}

	public static void EncryptFile(string inputFilePath, string outputFilePath, string key, byte[] IV)
	{
		using (Aes aes = Aes.Create())
		{
			aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
			aes.IV = IV; // Zero IV for simplicity, but use a random IV in production

			using (FileStream inputFileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
			using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
			using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
			{
				inputFileStream.CopyTo(cryptoStream);
			}
		}
	}

	public static void DecryptFile(string inputFilePath, string outputFilePath, string key, byte[] IV)
	{
		using (Aes aes = Aes.Create())
		{
			aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
			aes.IV = IV; // Must match the IV used during encryption

			using (FileStream inputFileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
			using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
			using (CryptoStream cryptoStream = new CryptoStream(inputFileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
			{
				cryptoStream.CopyTo(outputFileStream);
			}
		}
	}
}

public static class RandomIVGenerator
{

	public static byte[] GenerateRandomBytes(int size)
	{
		byte[] randomBytes = new byte[size];
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(randomBytes);
		}
		return randomBytes;
	}
}