using MafiaOnline.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Utils
{
    public interface ISecurityUtils
    {
        public string Decrypt(string text);
        public string Encrypt(string text);
        public string Hash(string text);
        public bool VerifyPassword(Player player, string pass);
    }

    public class SecurityUtils : ISecurityUtils
    {
        private readonly IConfiguration _config;
        public SecurityUtils(IConfiguration config)
        {
            _config = config;
        }

        private readonly byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        /// <summary>
        /// Decrypts a text
        /// </summary>
        public string Decrypt(string text)
        {
            byte[] bytes = Convert.FromBase64String(text);
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = MD5.Create();
            var key = _config.GetValue<string>("Security:EncryptKey");
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(key));
            crypt.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] decryptedBytes = new byte[bytes.Length];
                    cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                    return Encoding.Unicode.GetString(decryptedBytes);
                }
            }
        }

        /// <summary>
        /// Encrypts a text
        /// </summary>
        public string Encrypt(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = MD5.Create();
            crypt.BlockSize = 128;
            var key = _config.GetValue<string>("Security:EncryptKey");
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(key));
            crypt.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// Hashes a text
        /// </summary>
        public string Hash(string text)
        {
            byte[] salt = new byte[16];
            var cryptoService = new RNGCryptoServiceProvider();
            cryptoService.GetBytes(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(text, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Checks if entered password is correct
        /// </summary>
        public bool VerifyPassword(Player player, string pass)
        {
            string savedPasswordHash = player.HashedPassword;
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(pass, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}
