using MafiaOnline.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace MafiaOnline.DataAccess.Database
{
    public static class Seeder
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Boss>().HasData(PrepareBosses());
            modelBuilder.Entity<Player>().HasData(PreparePlayers());
            modelBuilder.Entity<Agent>().HasData(PrepareAgents());
            modelBuilder.Entity<Mission>().HasData(PrepareMissions());
            modelBuilder.Entity<PerformingMission>().HasData(PreparePerformingMissions());
            modelBuilder.Entity<AgentForSale>().HasData(PrepareAgentsOnSale());
        }

        private static IList<Boss> PrepareBosses()
        {
            return new List<Boss>
            {
                new Boss {Id=1, FirstName = "Patricio", LastName = "Rico", Money = 5000, LastSeen = DateTime.Parse("2020-01-01") },
                new Boss {Id=2, FirstName = "Rodrigo", LastName = "Margherita", Money = 5000, LastSeen = DateTime.Parse("2020-01-01")  }
            };
        }

        private static IList<Player> PreparePlayers()
        {
            return new List<Player> {
                new Player{Id=1, Nick="mafia", HashedPassword="tlnK6HiwFF4+b4DRVaVdRlIPtzduirsf8W3+nbXlLWlf9c/J", BossId=1}, //password: a
                new Player{Id=2, Nick="tomek", HashedPassword="d2JZt0Jz9UzgW1l544W2WnOaX14u/pfGUDYTQzv5AEWk3W7D", BossId=2} //password: b
            };
        }

        private static IList<Agent> PrepareAgents()
        {
            return new List<Agent> {
                new Agent{Id=1, BossId=1, State = AgentState.Active, FirstName="Jotaro", LastName="Kujo", Strength=10, Intelligence=10, Dexterity=10, Upkeep=100},
                new Agent{Id=2, BossId=1, State = AgentState.Active, FirstName="Adam", LastName="Mickiewicz", Strength=5, Intelligence=5, Dexterity=5, Upkeep=50},
                new Agent{Id=3, BossId=2, State = AgentState.Active, FirstName="Natalia", LastName="Natsu", Strength=7, Intelligence=3, Dexterity=4, Upkeep=70},
                new Agent{Id=4, State = AgentState.OnSale, FirstName="Eleonora", LastName="Lora", Strength=8, Intelligence=0, Dexterity=7, Upkeep=30},
                new Agent{Id=5, BossId=1, State = AgentState.Active, FirstName="Robert", LastName="Makłowicz", Strength=3, Intelligence=5, Dexterity=1, Upkeep=200},
            };
        }

        private static IList<Mission> PrepareMissions()
        {
            return new List<Mission> {
                new Mission{Id=1, Name="Bank robbery", DifficultyLevel=7, Loot=5000, Duration=30, StrengthPercentage=20, DexterityPercentage=60, IntelligencePercentage=20},
                new Mission{Id=2, Name="Senator assassination", DifficultyLevel=9, Loot=10000, Duration=10, StrengthPercentage=80, DexterityPercentage=20, IntelligencePercentage=0},
                new Mission{Id=3, Name="Party", DifficultyLevel=2, Loot=100, Duration=10, StrengthPercentage=60, DexterityPercentage=20, IntelligencePercentage=20},
                new Mission{Id=4, Name="Buy a coffee", DifficultyLevel=1, Loot=10, Duration=5, StrengthPercentage=20, DexterityPercentage=60, IntelligencePercentage=20},
                new Mission{Id=5, Name="Money laundering", DifficultyLevel=5, Loot=1000, Duration=55, StrengthPercentage=20, DexterityPercentage=20, IntelligencePercentage=60},
                new Mission{Id=6, Name="Car theft", DifficultyLevel=6, Loot=2000, Duration=3600, StrengthPercentage=20, DexterityPercentage=60, IntelligencePercentage=20},
                new Mission{Id=7, Name="Arms trade", DifficultyLevel=8, Loot=4000, Duration=15, StrengthPercentage=40, DexterityPercentage=20, IntelligencePercentage=40}
            };
        }

        private static IList<PerformingMission> PreparePerformingMissions()
        {
            return new List<PerformingMission>
            {
            };
        }

        private static IList<AgentForSale> PrepareAgentsOnSale()
        {
            return new List<AgentForSale>
            {
                new AgentForSale (){ Id=1, AgentId=4, Price=5000 }
            };
        }
    }
}