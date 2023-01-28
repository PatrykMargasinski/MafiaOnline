using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Utils
{
    public interface IRandomizer
    {
        public int Next();
        int Next(int x);
        int Next(int x, int y);
        string RandomAlphabeticString(short length);
    }

    public class Randomizer : IRandomizer
    {
        private readonly Random _random;
        public Randomizer()
        {
            _random = new Random();
        }

        /// <summary>
        /// Returns random numbe
        /// </summary>
        public int Next()
        {
            return _random.Next();
        }


        /// <summary>
        /// Returns random number from 0 to y
        /// </summary>
        public int Next(int x)
        {
            return _random.Next(x);
        }

        /// <summary>
        /// Returns random number from x to y
        /// </summary>
        public int Next(int x, int y)
        {
            return _random.Next(x, y);
        }

        /// <summary>
        /// Returns random string
        /// </summary>
        public string RandomAlphabeticString(short length)
        {
            const string valid = SecurityConsts.ALPHANUMERIC_CHARACTERS;
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return res.ToString();
        }
    }
}
