using MafiaOnline.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Entities
{
    public class ActivationToken
    {
        [FromQuery(Name = "token")]
        public string Token { get; set; }

        [FromQuery(Name = "email")]
        public string Email { get; set; }
    }
}
