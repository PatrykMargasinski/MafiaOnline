using MafiaOnline.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace MafiaOnline.DataAccess.Database
{
    public static class Seeder
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(PrepareRoles());
            modelBuilder.Entity<Boss>().HasData(PrepareBosses());
            modelBuilder.Entity<Player>().HasData(PreparePlayers());
            modelBuilder.Entity<Agent>().HasData(PrepareAgents());
            modelBuilder.Entity<Mission>().HasData(PrepareMissions());
            modelBuilder.Entity<PerformingMission>().HasData(PreparePerformingMissions());
            modelBuilder.Entity<Name>().HasData(PrepareNames());
            modelBuilder.Entity<MissionTemplate>().HasData(PrepareMissionTemplates());
            modelBuilder.Entity<MapElement>().HasData(PrepareMapElements());
            modelBuilder.Entity<Headquarters>().HasData(PrepareHeadquarters());
            modelBuilder.Entity<News>().HasData(PrepareNews());
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(PreparePlayerRoles());
            modelBuilder.Entity<State>().HasData(PrepareState());
            modelBuilder.Entity<Substate>().HasData(PrepareSubstate());
        }

        private static IList<State> PrepareState()
        {
            var states = new List<State>()
            {
                new State{ Id = 1, Name = "Renegate" },
                new State{ Id = 2, Name = "ForSale"},
                new State{ Id = 3, Name = "OnMission"},
                new State{ Id = 4, Name = "Active"},
                new State{ Id = 5, Name = "Moving", HasSubstates = true},
                new State{ Id = 6, Name = "Ambushing"},
                new State{ Id = 7, Name = "Available"},
                new State{ Id = 8, Name = "Performing"},
            };
            return states;
        }

        private static IList<Substate> PrepareSubstate()
        {
            var states = new List<Substate>()
            {   
                new Substate{ Id = 1, StateId = 5, Name = "MovingOnMission"},
                new Substate{ Id = 2, StateId = 5, Name = "Patrolling"},
                new Substate{ Id = 3, StateId = 5, Name = "MovingWithLoot"},
            };
            return states;
        }

        private static IList<News> PrepareNews()
        {
            var news = new List<News>();
            var version2news = new News { Id = 1, Subject = "Version 2.0 out!", HTMLContent = "", Priority = 5 };
            var version2newsHTML = "We are glad that version 2.0 has just seen the light of day. There are many features the new version has provided. <br>The most important of them are:<ul><li>Map - all game events are already taking place on the map</li><li>3 important map elements have been introduced: heaquarters, missions, ambushes</li><li>Every boss has own headquarters. All the agents leave right here. They also come back here.<li>Missions are map elements - the agent must reach the mission to perform it. </li><li>Ambushes are another map elements. When agent moves on a map he may fall into a ambush along the way.</li></ul>Every new feature is described in the \"About\" section";
            version2news.HTMLContent = version2newsHTML;
            news.Add(version2news);

            var newsNews = new News { Id = 2, Subject = "News added", HTMLContent = "Now we can inform you about some interesting news related to the game", Priority = 1 };
            news.Add(newsNews);
            return news;
        }

        private static IList<Boss> PrepareBosses()
        {
            return new List<Boss>
            {
                new Boss {Id=1, FirstName = "Patricio", LastName = "Rico", Money = 5000, LastSeen = DateTime.Parse("2020-01-01") },
                new Boss {Id=2, FirstName = "Rodrigo", LastName = "Margherita", Money = 5000, LastSeen = DateTime.Parse("2020-01-01")  }
            };
        }

        private static IList<IdentityRole> PrepareRoles()
        {
            return new List<IdentityRole>
            {
                new IdentityRole {Id="2c5e174e-3b0e-446f-86af-483d56fd7210", Name="Administrator", NormalizedName="ADMINISTRATOR" },
                new IdentityRole {Id="2c5e174e-3b0e-1234-86af-483d56fd7210", Name="Player" , NormalizedName="PLAYER" }
            };
        }

        private static IList<Player> PreparePlayers()
        {
            var hasher = new PasswordHasher<Player>();
            var players = new List<Player>{
                new Player{Id="8e445865-a24d-4543-a6c6-9443d048cdb9", UserName="mafia", NormalizedUserName="MAFIA", Email="mafiaonlineteam@gmail.com", NormalizedEmail="MAFIAONLINETEAM@GMAIL.COM", BossId=1},
                new Player{Id="8e445865-a24d-4543-1234-9443d048cdb9", UserName="tomek", NormalizedUserName="TOMEK", Email="mafiaonlineteam2@gmail.com", NormalizedEmail="MAFIAONLINETEAM2@GMAIL.COM", BossId=2}
            };
            players.ForEach(x => x.PasswordHash = hasher.HashPassword(x, "a"));
            return players;
        }

        private static IList<IdentityUserRole<string>> PreparePlayerRoles()
        {
            return new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string> {RoleId="2c5e174e-3b0e-446f-86af-483d56fd7210", UserId="8e445865-a24d-4543-a6c6-9443d048cdb9" },
                new IdentityUserRole<string> {RoleId="2c5e174e-3b0e-446f-86af-483d56fd7210", UserId="8e445865-a24d-4543-1234-9443d048cdb9" }
            };
        }

        private static IList<Agent> PrepareAgents()
        {
            return new List<Agent> {
                new Agent{Id=1, BossId=1, StateIdEnum = AgentState.Active, FirstName="Jotaro", LastName="Kujo", Strength=10, Intelligence=10, Dexterity=10, Upkeep=100, IsFromBossFamily = false},
                new Agent{Id=2, BossId=1, StateIdEnum = AgentState.Active, FirstName="Adam", LastName="Mickiewicz", Strength=5, Intelligence=5, Dexterity=5, Upkeep=50, IsFromBossFamily = false},
                new Agent{Id=3, BossId=2, StateIdEnum = AgentState.Active, FirstName="Natalia", LastName="Natsu", Strength=7, Intelligence=3, Dexterity=4, Upkeep=70, IsFromBossFamily = false},
                new Agent{Id=4, StateIdEnum = AgentState.Renegate, FirstName="Eleonora", LastName="Lora", Strength=8, Intelligence=0, Dexterity=7, Upkeep=30, IsFromBossFamily = false},
                new Agent{Id=5, BossId=1, StateIdEnum = AgentState.Active, FirstName="Robert", LastName="Makłowicz", Strength=3, Intelligence=5, Dexterity=1, Upkeep=200, IsFromBossFamily = false},
                new Agent{Id=6, BossId=1, StateIdEnum = AgentState.Active, FirstName="Ricardo", LastName="Rico", Strength=10, Intelligence=10, Dexterity=10, Upkeep=100, IsFromBossFamily = true},
            };
        }

        private static IList<Mission> PrepareMissions()
        {
            return new List<Mission> {
                new Mission{Id=1, MapElementId=3, Name="Bank robbery", DifficultyLevel=7, Loot=5000, Duration=30, StrengthPercentage=20, DexterityPercentage=60, IntelligencePercentage=20, StateIdEnum=MissionState.Available, RepeatableMission = true},
                new Mission{Id=2, MapElementId=4, Name="Senator assassination", DifficultyLevel=9, Loot=10000, Duration=10, StrengthPercentage=80, DexterityPercentage=20, IntelligencePercentage=0, StateIdEnum=MissionState.Available, RepeatableMission = true},
                new Mission{Id=3, MapElementId=5, Name="Party", DifficultyLevel=2, Loot=100, Duration=10, StrengthPercentage=60, DexterityPercentage=20, IntelligencePercentage=20, StateIdEnum=MissionState.Available, RepeatableMission = true},
                new Mission{Id=4, MapElementId=6, Name="Buy a coffee", DifficultyLevel=1, Loot=10, Duration=5, StrengthPercentage=20, DexterityPercentage=60, IntelligencePercentage=20, StateIdEnum=MissionState.Available, RepeatableMission = true},
                new Mission{Id=5, MapElementId=7, Name="Money laundering", DifficultyLevel=5, Loot=1000, Duration=55, StrengthPercentage=20, DexterityPercentage=20, IntelligencePercentage=60, StateIdEnum=MissionState.Available, RepeatableMission = true},
                new Mission{Id=6, MapElementId=8, Name="Car theft", DifficultyLevel=6, Loot=2000, Duration=3600, StrengthPercentage=20, DexterityPercentage=60, IntelligencePercentage=20, StateIdEnum=MissionState.Available, RepeatableMission = true},
                new Mission{Id=7, MapElementId=9, Name="Arms trade", DifficultyLevel=8, Loot=4000, Duration=15, StrengthPercentage=40, DexterityPercentage=20, IntelligencePercentage=40, StateIdEnum=MissionState.Available, RepeatableMission = true}
            };
        }

        private static IList<PerformingMission> PreparePerformingMissions()
        {
            return new List<PerformingMission>
            {
            };
        }

        private static IList<Name> PrepareNames()
        {
            return new List<Name>
            {
                new Name (){ Id=1,Text="Adele",Type=NameType.FirstName},
                new Name (){ Id=2,Text="Alessia",Type=NameType.FirstName},
                new Name (){ Id=3,Text="Alice",Type=NameType.FirstName},
                new Name (){ Id=4,Text="Anita",Type=NameType.FirstName},
                new Name (){ Id=5,Text="Anna",Type=NameType.FirstName},
                new Name (){ Id=6,Text="Arianna",Type=NameType.FirstName},
                new Name (){ Id=7,Text="Asia",Type=NameType.FirstName},
                new Name (){ Id=8,Text="Aurora",Type=NameType.FirstName},
                new Name (){ Id=9,Text="Azzurra",Type=NameType.FirstName},
                new Name (){ Id=10,Text="Beatrice",Type=NameType.FirstName},
                new Name (){ Id=11,Text="Benedetta",Type=NameType.FirstName},
                new Name (){ Id=12,Text="Bianca",Type=NameType.FirstName},
                new Name (){ Id=13,Text="Camilla",Type=NameType.FirstName},
                new Name (){ Id=14,Text="Carlotta",Type=NameType.FirstName},
                new Name (){ Id=15,Text="Caterina",Type=NameType.FirstName},
                new Name (){ Id=16,Text="Cecilia",Type=NameType.FirstName},
                new Name (){ Id=17,Text="Chiara",Type=NameType.FirstName},
                new Name (){ Id=18,Text="Chloe",Type=NameType.FirstName},
                new Name (){ Id=19,Text="Elena",Type=NameType.FirstName},
                new Name (){ Id=20,Text="Eleonora",Type=NameType.FirstName},
                new Name (){ Id=21,Text="Elisa",Type=NameType.FirstName},
                new Name (){ Id=22,Text="Emily",Type=NameType.FirstName},
                new Name (){ Id=23,Text="Emma",Type=NameType.FirstName},
                new Name (){ Id=24,Text="Eva",Type=NameType.FirstName},
                new Name (){ Id=25,Text="Francesca",Type=NameType.FirstName},
                new Name (){ Id=26,Text="Gaia",Type=NameType.FirstName},
                new Name (){ Id=27,Text="Giada",Type=NameType.FirstName},
                new Name (){ Id=28,Text="Ginerva",Type=NameType.FirstName},
                new Name (){ Id=29,Text="Gioia",Type=NameType.FirstName},
                new Name (){ Id=30,Text="Giorgia",Type=NameType.FirstName},
                new Name (){ Id=31,Text="Giulia",Type=NameType.FirstName},
                new Name (){ Id=32,Text="Greta",Type=NameType.FirstName},
                new Name (){ Id=33,Text="Irene",Type=NameType.FirstName},
                new Name (){ Id=34,Text="Isabel",Type=NameType.FirstName},
                new Name (){ Id=35,Text="Ludovica",Type=NameType.FirstName},
                new Name (){ Id=36,Text="Margherita",Type=NameType.FirstName},
                new Name (){ Id=37,Text="Maria",Type=NameType.FirstName},
                new Name (){ Id=38,Text="Marta",Type=NameType.FirstName},
                new Name (){ Id=39,Text="Martina",Type=NameType.FirstName},
                new Name (){ Id=40,Text="Matilde",Type=NameType.FirstName},
                new Name (){ Id=41,Text="Melissa",Type=NameType.FirstName},
                new Name (){ Id=42,Text="Mia",Type=NameType.FirstName},
                new Name (){ Id=43,Text="Miriam",Type=NameType.FirstName},
                new Name (){ Id=44,Text="Nicole",Type=NameType.FirstName},
                new Name (){ Id=45,Text="Noemi",Type=NameType.FirstName},
                new Name (){ Id=46,Text="Rebecca",Type=NameType.FirstName},
                new Name (){ Id=47,Text="Sara",Type=NameType.FirstName},
                new Name (){ Id=48,Text="Sofia",Type=NameType.FirstName},
                new Name (){ Id=49,Text="Viola",Type=NameType.FirstName},
                new Name (){ Id=50,Text="Vittoria",Type=NameType.FirstName},
                new Name (){ Id=51,Text="Abramo",Type=NameType.FirstName},
                new Name (){ Id=52,Text="Alessandro",Type=NameType.FirstName},
                new Name (){ Id=53,Text="Alessio",Type=NameType.FirstName},
                new Name (){ Id=54,Text="Andrea",Type=NameType.FirstName},
                new Name (){ Id=55,Text="Antonio",Type=NameType.FirstName},
                new Name (){ Id=56,Text="Brando",Type=NameType.FirstName},
                new Name (){ Id=57,Text="Christian",Type=NameType.FirstName},
                new Name (){ Id=58,Text="Daniel",Type=NameType.FirstName},
                new Name (){ Id=59,Text="Davide",Type=NameType.FirstName},
                new Name (){ Id=60,Text="Diego",Type=NameType.FirstName},
                new Name (){ Id=61,Text="Domenico",Type=NameType.FirstName},
                new Name (){ Id=62,Text="Edoardo",Type=NameType.FirstName},
                new Name (){ Id=63,Text="Elia",Type=NameType.FirstName},
                new Name (){ Id=64,Text="Emanuele",Type=NameType.FirstName},
                new Name (){ Id=65,Text="Enea",Type=NameType.FirstName},
                new Name (){ Id=66,Text="Federico",Type=NameType.FirstName},
                new Name (){ Id=67,Text="Filippo",Type=NameType.FirstName},
                new Name (){ Id=68,Text="Francesco",Type=NameType.FirstName},
                new Name (){ Id=69,Text="Franco",Type=NameType.FirstName},
                new Name (){ Id=70,Text="Gabriel",Type=NameType.FirstName},
                new Name (){ Id=71,Text="Giacomo",Type=NameType.FirstName},
                new Name (){ Id=72,Text="Gioele",Type=NameType.FirstName},
                new Name (){ Id=73,Text="Giorgio",Type=NameType.FirstName},
                new Name (){ Id=74,Text="Giovanni",Type=NameType.FirstName},
                new Name (){ Id=75,Text="Giulio",Type=NameType.FirstName},
                new Name (){ Id=76,Text="Giuseppe",Type=NameType.FirstName},
                new Name (){ Id=77,Text="Jacopo",Type=NameType.FirstName},
                new Name (){ Id=78,Text="Leonardo",Type=NameType.FirstName},
                new Name (){ Id=79,Text="Lorenzo",Type=NameType.FirstName},
                new Name (){ Id=80,Text="Luca",Type=NameType.FirstName},
                new Name (){ Id=81,Text="Luigi",Type=NameType.FirstName},
                new Name (){ Id=82,Text="Manuel",Type=NameType.FirstName},
                new Name (){ Id=83,Text="Marco",Type=NameType.FirstName},
                new Name (){ Id=84,Text="Matteo",Type=NameType.FirstName},
                new Name (){ Id=85,Text="Mattia",Type=NameType.FirstName},
                new Name (){ Id=86,Text="Michele",Type=NameType.FirstName},
                new Name (){ Id=87,Text="Nathan",Type=NameType.FirstName},
                new Name (){ Id=88,Text="Nicola",Type=NameType.FirstName},
                new Name (){ Id=89,Text="Nicolo",Type=NameType.FirstName},
                new Name (){ Id=90,Text="Pietro",Type=NameType.FirstName},
                new Name (){ Id=91,Text="Raffaele",Type=NameType.FirstName},
                new Name (){ Id=92,Text="Riccardo",Type=NameType.FirstName},
                new Name (){ Id=93,Text="Salvatore",Type=NameType.FirstName},
                new Name (){ Id=94,Text="Samuel",Type=NameType.FirstName},
                new Name (){ Id=95,Text="Simone",Type=NameType.FirstName},
                new Name (){ Id=96,Text="Stefano",Type=NameType.FirstName},
                new Name (){ Id=97,Text="Thomas",Type=NameType.FirstName},
                new Name (){ Id=98,Text="Tommaso",Type=NameType.FirstName},
                new Name (){ Id=99,Text="Valerius",Type=NameType.FirstName},
                new Name (){ Id=100,Text="Vincenzo",Type=NameType.FirstName},
                new Name (){ Id=101,Text="Fasano",Type=NameType.LastName},
                new Name (){ Id=102,Text="Lo Iacono",Type=NameType.LastName},
                new Name (){ Id=103,Text="Montani",Type=NameType.LastName},
                new Name (){ Id=104,Text="Cerminaro",Type=NameType.LastName},
                new Name (){ Id=105,Text="Paganini",Type=NameType.LastName},
                new Name (){ Id=106,Text="Di Pinto",Type=NameType.LastName},
                new Name (){ Id=107,Text="La Fratta",Type=NameType.LastName},
                new Name (){ Id=108,Text="Antonelli",Type=NameType.LastName},
                new Name (){ Id=109,Text="Bellofatto",Type=NameType.LastName},
                new Name (){ Id=110,Text="Sama",Type=NameType.LastName},
                new Name (){ Id=111,Text="Virginia",Type=NameType.LastName},
                new Name (){ Id=112,Text="Rucci",Type=NameType.LastName},
                new Name (){ Id=113,Text="Schifano",Type=NameType.LastName},
                new Name (){ Id=114,Text="Michele",Type=NameType.LastName},
                new Name (){ Id=115,Text="Geronimo",Type=NameType.LastName},
                new Name (){ Id=116,Text="Silvestri",Type=NameType.LastName},
                new Name (){ Id=117,Text="Falasca",Type=NameType.LastName},
                new Name (){ Id=118,Text="Blancato",Type=NameType.LastName},
                new Name (){ Id=119,Text="Raimondo",Type=NameType.LastName},
                new Name (){ Id=120,Text="Luzi",Type=NameType.LastName},
                new Name (){ Id=121,Text="Riviera",Type=NameType.LastName},
                new Name (){ Id=122,Text="Morreale",Type=NameType.LastName},
                new Name (){ Id=123,Text="Cozzi",Type=NameType.LastName},
                new Name (){ Id=124,Text="Pera",Type=NameType.LastName},
                new Name (){ Id=125,Text="Ditta",Type=NameType.LastName},
                new Name (){ Id=126,Text="Peduto",Type=NameType.LastName},
                new Name (){ Id=127,Text="Azzarello",Type=NameType.LastName},
                new Name (){ Id=128,Text="Maiorino",Type=NameType.LastName},
                new Name (){ Id=129,Text="Bonaccorsi",Type=NameType.LastName},
                new Name (){ Id=130,Text="Valentino",Type=NameType.LastName},
                new Name (){ Id=131,Text="Di Croce",Type=NameType.LastName},
                new Name (){ Id=132,Text="Lucido",Type=NameType.LastName},
                new Name (){ Id=133,Text="Satriano",Type=NameType.LastName},
                new Name (){ Id=134,Text="Mollo",Type=NameType.LastName},
                new Name (){ Id=135,Text="Acocella",Type=NameType.LastName},
                new Name (){ Id=136,Text="Messina",Type=NameType.LastName},
                new Name (){ Id=137,Text="Portella",Type=NameType.LastName},
                new Name (){ Id=138,Text="Dalpiaz",Type=NameType.LastName},
                new Name (){ Id=139,Text="Vanacore",Type=NameType.LastName},
                new Name (){ Id=140,Text="Ciarrocchi",Type=NameType.LastName},
                new Name (){ Id=141,Text="Girolamo",Type=NameType.LastName},
                new Name (){ Id=142,Text="Granieri",Type=NameType.LastName},
                new Name (){ Id=143,Text="D'Aleo",Type=NameType.LastName},
                new Name (){ Id=144,Text="Talluto",Type=NameType.LastName},
                new Name (){ Id=145,Text="La Viola",Type=NameType.LastName},
                new Name (){ Id=146,Text="Colaizzi",Type=NameType.LastName},
                new Name (){ Id=147,Text="Frezza",Type=NameType.LastName},
                new Name (){ Id=148,Text="Barsotti",Type=NameType.LastName},
                new Name (){ Id=149,Text="Riccardo",Type=NameType.LastName},
                new Name (){ Id=150,Text="Dena",Type=NameType.LastName},
                new Name (){ Id=151,Text="Gregorio",Type=NameType.LastName},
                new Name (){ Id=152,Text="Parrinello",Type=NameType.LastName}
            };
        }

        private static IList<MissionTemplate> PrepareMissionTemplates()
        {
            return new List<MissionTemplate>
            {
                new MissionTemplate (){ Id=1, Name="Assassination", MinDifficulty=4, MaxDifficulty=8, MinLoot = 1000, MaxLoot=5000, StrengthPercentage=60, DexterityPercentage=20, IntelligencePercentage=20, MinDuration=10, MaxDuration=30 },
                new MissionTemplate (){ Id=2, Name="Theft", MinDifficulty=4, MaxDifficulty=8, MinLoot = 1000, MaxLoot=5000, StrengthPercentage=20, DexterityPercentage=60, IntelligencePercentage=20, MinDuration=10, MaxDuration=30 },
                new MissionTemplate (){ Id=3, Name="Money laundering", MinDifficulty=4, MaxDifficulty=8, MinLoot = 1000, MaxLoot=5000, StrengthPercentage=20, DexterityPercentage=20, IntelligencePercentage=60, MinDuration=10, MaxDuration=30 },
                new MissionTemplate (){ Id=4, Name = "Bank robbery", MinDifficulty=6, MaxDifficulty=10, MinLoot = 4000, MaxLoot=10000, StrengthPercentage=60, DexterityPercentage=20, IntelligencePercentage=20, MinDuration=10, MaxDuration=30 },
                new MissionTemplate (){ Id=5, Name = "Drug production", MinDifficulty=6, MaxDifficulty=10, MinLoot = 4000, MaxLoot=10000, StrengthPercentage=20, DexterityPercentage=60, IntelligencePercentage=20, MinDuration=10, MaxDuration=30 },
                new MissionTemplate (){ Id=6, Name = "Arms trade", MinDifficulty=6, MaxDifficulty=10, MinLoot = 4000, MaxLoot=10000, StrengthPercentage=20, DexterityPercentage=20, IntelligencePercentage=60, MinDuration=10, MaxDuration=30 },
                new MissionTemplate (){ Id=7, Name = "Vandalism", MinDifficulty=3, MaxDifficulty=7, MinLoot = 2000, MaxLoot=2000, StrengthPercentage=80, DexterityPercentage=10, IntelligencePercentage=10, MinDuration=10, MaxDuration=30 },
                new MissionTemplate (){ Id=8, Name = "Arson on the building", MinDifficulty=3, MaxDifficulty=7, MinLoot = 2000, MaxLoot=5000, StrengthPercentage=10, DexterityPercentage=80, IntelligencePercentage=10, MinDuration=10, MaxDuration=30 },
                new MissionTemplate (){ Id=9, Name = "Gambling manilupation", MinDifficulty=3, MaxDifficulty=7, MinLoot = 2000, MaxLoot=5000, StrengthPercentage=10, DexterityPercentage=10, IntelligencePercentage=80, MinDuration=10, MaxDuration=30 },
                new MissionTemplate (){ Id=10, Name = "Blackmail", MinDifficulty=4, MaxDifficulty=6, MinLoot = 3000, MaxLoot=5000, StrengthPercentage=40, DexterityPercentage=30, IntelligencePercentage=30, MinDuration=10, MaxDuration=30 },
                new MissionTemplate (){ Id=11, Name = "Drug smuggling", MinDifficulty=4, MaxDifficulty=6, MinLoot = 3000, MaxLoot=5000, StrengthPercentage=30, DexterityPercentage=40, IntelligencePercentage=30, MinDuration=10, MaxDuration=30 },
                new MissionTemplate (){ Id=12, Name = "Deal you can not throw away", MinDifficulty=6, MaxDifficulty=7, MinLoot = 3000, MaxLoot=5000, StrengthPercentage=30, DexterityPercentage=30, IntelligencePercentage=40, MinDuration=10, MaxDuration=30 },
            };
        }

        private static IList<Headquarters> PrepareHeadquarters()
        {
            return new List<Headquarters>
            {
                new Headquarters (){Id=1, MapElementId=1, BossId=1, Name="The house of Patricio Rico" },
                new Headquarters (){Id=2, MapElementId=2, BossId=2, Name="Margherita rules here"  },
            };
        }

        private static IList<MapElement> PrepareMapElements()
        {
            return new List<MapElement>
            {
                new MapElement (){Id=1, Type = MapElementType.Headquarters, X = 2, Y = 1, BossId=1, Description="Headquarters of Patricio Rico" },
                new MapElement (){Id=2, Type = MapElementType.Headquarters, X = 14, Y = 1, BossId=2, Description="Headquarters of Rodrigo Margherita"},
                new MapElement (){Id=3, Type = MapElementType.Mission, X = 1, Y = 3, Description="Mission: Bank robbery" },
                new MapElement (){Id=4, Type = MapElementType.Mission, X = 3, Y = 5, Description="Mission: Senator assassination" },
                new MapElement (){Id=5, Type = MapElementType.Mission, X = 3, Y = 7, Description="Mission: Party"  },
                new MapElement (){Id=6, Type = MapElementType.Mission, X = 5, Y = 3, Description="Mission: Buy a coffee"  },
                new MapElement (){Id=7, Type = MapElementType.Mission, X = 14, Y = 1, Description="Mission: Money laundering"  },
                new MapElement (){Id=8, Type = MapElementType.Mission, X = 13, Y = 3, Description="Mission: Car theft"  },
                new MapElement (){Id=9, Type = MapElementType.Mission, X = 14, Y = 5, Description="Mission: Arms trade"  },
            };
        }
    }
}