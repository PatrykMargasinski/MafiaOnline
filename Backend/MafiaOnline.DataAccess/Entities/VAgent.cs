using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MafiaOnline.DataAccess.Entities
{
    public partial class VAgent : Entity
    {
        public long? BossId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public long Upkeep { get; set; }
        public bool IsFromBossFamily { get; set; }
        public long StateId { get; set; }
        public long? SubstateId { get; set; }
        public DateTime? FinishTime { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                if (FirstName == null || LastName == null) return null;
                return $"{FirstName} {LastName}";
            }
        }

        [NotMapped]
        public AgentState? StateIdEnum
        {
            get => (AgentState?)StateId;
        }

        [NotMapped]
        public AgentSubstate? SubstateIdEnum
        {
            get => (AgentSubstate?)SubstateId;
        }
    }
}
