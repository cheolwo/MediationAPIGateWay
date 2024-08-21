using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace MediationAPIGateWay.Service
{
    class AesEncryption
    {
        public static void Main()
        {
            string original = "This is a secret message.";
            using (Aes aes = Aes.Create())
            {
                aes.Key = GenerateRandomKey();
                aes.IV = GenerateRandomIV();

                // Encrypt the string to an array of bytes
                byte[] encrypted = EncryptStringToBytes_Aes(original, aes.Key, aes.IV);
                Console.WriteLine("Encrypted: " + Convert.ToBase64String(encrypted));

                // Decrypt the bytes to a string
                string decrypted = DecryptStringFromBytes_Aes(encrypted, aes.Key, aes.IV);
                Console.WriteLine("Decrypted: " + decrypted);
            }
        }

        static byte[] GenerateRandomKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                return aes.Key;
            }
        }

        static byte[] GenerateRandomIV()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateIV();
                return aes.IV;
            }
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            string plaintext = null;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
    public class RSADemo
    {
        public static void Main()
        {
            // 새 RSA 키 쌍 생성
            using (RSA rsa = RSA.Create())
            {
                string originalMessage = "Hello, RSA!";
                Console.WriteLine("Original Message: " + originalMessage);

                // 메시지 암호화
                byte[] encryptedMessage = EncryptData(Encoding.UTF8.GetBytes(originalMessage), rsa.ExportParameters(false));
                Console.WriteLine("Encrypted Message: " + Convert.ToBase64String(encryptedMessage));

                // 메시지 복호화
                string decryptedMessage = Encoding.UTF8.GetString(DecryptData(encryptedMessage, rsa.ExportParameters(true)));
                Console.WriteLine("Decrypted Message: " + decryptedMessage);
            }
        }

        public static byte[] EncryptData(byte[] dataToEncrypt, RSAParameters publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(publicKey);
                return rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);
            }
        }

        public static byte[] DecryptData(byte[] dataToDecrypt, RSAParameters privateKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                return rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.OaepSHA256);
            }
        }
    }
    
    [ApiController]
    [Route("[controller]")]
    public class DataCouplingController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetMessage(string message)
        {
            return ProcessMessage(message);
        }

        private string ProcessMessage(string input)
        {
            return $"Processed: {input}";
        }
    }
    public class UserDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class StampCouplingController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> PrintUserName([FromBody] UserDto user)
        {
            return $"Hello, {user.Name}";
        }
    }
    [ApiController]
    [Route("[controller]")]
    public class ControlCouplingController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetFormattedDate(bool asUtc)
        {
            var date = DateTime.Now;
            return asUtc ? date.ToUniversalTime().ToString() : date.ToString();
        }
    }
    [ApiController]
    [Route("[controller]")]
    public class ExternalCouplingController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ExternalCouplingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> GetDatabaseConnectionString()
        {
            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            return connectionString;
        }
    }
    public static class GlobalSettings
    {
        public static string GlobalData = "Important data";
    }

    [ApiController]
    [Route("[controller]")]
    public class CommonCouplingController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> ReadGlobalData()
        {
            return GlobalSettings.GlobalData;
        }

        [HttpPost]
        public void WriteGlobalData(string data)
        {
            GlobalSettings.GlobalData = data;
        }
    }
}
