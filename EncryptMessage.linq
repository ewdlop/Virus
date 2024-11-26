<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

class AESEncryption
{
	public static void Main(string[] args)
	{
		string originalMessage = "This is a secret message!";
		string key = "ThisIsA256BitKey1234567890123456"; // Must be 32 bytes for AES-256

		Console.WriteLine("Original Message: " + originalMessage);
		
		Console.WriteLine(System.Text.ASCIIEncoding.Unicode.GetByteCount(key));
		Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetByteCount(key));

		// Generate IV
		byte[] IV = RandomIVGenerator.GenerateRandomBytes(16);
		Console.WriteLine(BitConverter.ToString(IV));
		
		// Encrypt the message
		string encryptedMessage = Encrypt(originalMessage, key, IV);
		Console.WriteLine("Encrypted Message: " + encryptedMessage);

		// Decrypt the message
		string decryptedMessage = Decrypt(encryptedMessage, key, IV);
		Console.WriteLine("Decrypted Message: " + decryptedMessage);

	}

	static class RandomIVGenerator
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
	
	public byte[] ToIV(string hexString)
	{
		return Convert.FromHexString(hexString.Trim().Replace("-","").Replace(" ",""));
	}

	public static string Encrypt(string plainText, string key, byte[] IV)
	{
		using (Aes aes = Aes.Create())
		{
			aes.Key = Encoding.UTF8.GetBytes(key);
			aes.IV = IV; // Initialization vector (set to 0 for simplicity)

			using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
			{
				using (var ms = new MemoryStream())
				{
					using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
					{
						using (var sw = new StreamWriter(cs))
						{
							sw.Write(plainText);
						}
						return Convert.ToBase64String(ms.ToArray());
					}
				}
			}
		}
	}

	public static string Decrypt(string cipherText, string key, byte[] IV)
	{
		using (Aes aes = Aes.Create())
		{
			aes.Key = Encoding.UTF8.GetBytes(key);
			aes.IV = IV; // Initialization vector (set to 0 for simplicity)

			using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
			{
				using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
				{
					using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
					{
						using (var sr = new StreamReader(cs))
						{
							return sr.ReadToEnd();
						}
					}
				}
			}
		}
	}
}


