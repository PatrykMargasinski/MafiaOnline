using MafiaOnline.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
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
        public Task<bool> VerifyPasswordAsync(Player player, string pass);
    }

    public class SecurityUtils : ISecurityUtils
    {
        private readonly IConfiguration _config;
        private readonly UserManager<Player> _userManager;

        private string aes_key;
        private string aes_iv;
        public SecurityUtils(IConfiguration config, UserManager<Player> userManager)
        {
            _config = config;
            _userManager = userManager;
            aes_key = _config.GetValue<string>("Security:EncryptKey");
            aes_iv = _config.GetValue<string>("Security:IV");
        }

        private readonly byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        public string Encrypt(string plainText)
        {
            byte[] encrypted;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = Convert.FromBase64String(aes_key);
                aes.IV = Convert.FromBase64String(aes_iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }

                        encrypted = ms.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string encryptedText)
        {
            string decrypted = null;
            byte[] cipher = Convert.FromBase64String(encryptedText);

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = Convert.FromBase64String(aes_key);
                aes.IV = Convert.FromBase64String(aes_iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform dec = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, dec, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }
            }

            return decrypted;
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
        public async Task<bool> VerifyPasswordAsync(Player player, string pass)
        {
            return await _userManager.CheckPasswordAsync(player, pass);
        }
    }
}
