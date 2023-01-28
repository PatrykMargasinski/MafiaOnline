using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Const
{
    public class SecurityConsts
    {
        public const string ALPHANUMERIC_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public const int MINUTES_TO_REMOVE_RESET_PASSWORD_CODE = 2 * 60;
    }
}
