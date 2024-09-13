using System.Security.Cryptography;
using System.Text;

namespace Common.Services
{
    public class SecureCommunication
    {
        private RSA rsa;
        private Aes aes;
        private string sharedSecretKey = "123456";  // 공유 비밀키 (실제 사용 시 강력한 키로 교체 필요)

        public SecureCommunication()
        {
            rsa = RSA.Create();  // RSA 키 쌍 생성
            aes = Aes.Create();  // AES 암호화 객체 생성
            aes.Key = Encoding.UTF8.GetBytes(sharedSecretKey.PadRight(32));  // 키 사이즈 맞춤
            aes.IV = new byte[16];  // 간단한 예제를 위해 IV는 0으로 초기화 (실제 사용 시 안전한 랜덤 IV 사용)
        }

        public byte[] HashMessage(string message)
        {
            using (HashAlgorithm hashAlgorithm = SHA256.Create())
            {
                return hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(message));
            }
        }

        public byte[] SignData(byte[] hash)
        {
            return rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        public byte[] EncryptData(byte[] data)
        {
            using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }

        public byte[] CombineMessageAndSignature(byte[] messageBytes, byte[] signature)
        {
            byte[] combined = new byte[messageBytes.Length + signature.Length];
            Buffer.BlockCopy(messageBytes, 0, combined, 0, messageBytes.Length);
            Buffer.BlockCopy(signature, 0, combined, messageBytes.Length, signature.Length);
            return combined;
        }

        public static void Main()
        {
            string originalMessage = "Hello, this is a secure message.";
            SecureCommunication secComm = new SecureCommunication();

            byte[] hashedMessage = secComm.HashMessage(originalMessage);
            byte[] signature = secComm.SignData(hashedMessage);
            byte[] combinedMessage = secComm.CombineMessageAndSignature(Encoding.UTF8.GetBytes(originalMessage), signature);
            byte[] encryptedMessage = secComm.EncryptData(combinedMessage);

            Console.WriteLine("Encrypted Message: " + Convert.ToBase64String(encryptedMessage));
        }
    }
}
