using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class VBossWithPosition : Entity
    {
        public long Lp { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                if (FirstName == null || LastName == null) return null;
                return $"{FirstName} {LastName}";
            }
        }
        public long Money { get; set; }
        public long Position { get; set; }
    }
}
