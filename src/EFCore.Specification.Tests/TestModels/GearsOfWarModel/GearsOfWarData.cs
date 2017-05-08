// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Specification.Tests.TestModels.GearsOfWarModel
{
    public static class GearsOfWarData
    {
        public static IReadOnlyList<City> Cities { get; }
        public static IReadOnlyList<CogTag> Tags { get; }
        public static IReadOnlyList<Faction> Factions { get; }
        public static IReadOnlyList<Gear> Gears { get; }
        public static IReadOnlyList<Mission> Missions { get; }
        public static IReadOnlyList<Squad> Squads { get; }
        public static IReadOnlyList<SquadMission> SquadMissions { get; }
        public static IReadOnlyList<Weapon> Weapons { get; }
        public static IReadOnlyList<LocustLeader> LocustLeaders { get; }

        static GearsOfWarData()
        {
            Squads = CreateSquads();
            Missions = CreateMissions();
            SquadMissions = CreateSquadMissions();
            Cities = CreateCities();
            Weapons = CreateWeapons();
            Tags = CreateTags();
            Gears = CreateGears();
        }

        public static IReadOnlyList<Squad> CreateSquads()
            => new List<Squad>
            {
                new Squad { Id = 1, Name = "Delta", Members = new List<Gear>() },
                new Squad { Id = 2, Name = "Kilo", Members = new List<Gear>() }
            };

        public static IReadOnlyList<Mission> CreateMissions()
            => new List<Mission>
            {
                new Mission { Id = 1, CodeName = "Lightmass Offensive", Timeline = new DateTimeOffset(2, 1, 2, 10, 0, 0, new TimeSpan()) },
                new Mission { Id = 2, CodeName = "Lightmass Offensive", Timeline = new DateTimeOffset(2, 1, 2, 10, 0, 0, new TimeSpan()) },
                new Mission { Id = 3, CodeName = "Halvo Bay defense",  Timeline = new DateTimeOffset(10, 5, 3, 12, 0, 0, new TimeSpan()) }
            };

        public static IReadOnlyList<SquadMission> CreateSquadMissions()
            => new List<SquadMission>
            {
                new SquadMission(),
                new SquadMission(),
                new SquadMission(),
            };

        public static IReadOnlyList<City> CreateCities()
            => new List<City>
            {
                new City { Location = "Jacinto's location", Name = "Jacinto", BornGears = new List<Gear>(), StationedGears = new List<Gear>() },
                new City { Location = "Ephyra's location", Name = "Ephyra", BornGears = new List<Gear>(), StationedGears = new List<Gear>() },
                new City { Location = "Hanover's location", Name = "Hanover", BornGears = new List<Gear>(), StationedGears = new List<Gear>() },
                new City { Location = "Unknown", Name = "Unknown", BornGears = new List<Gear>(), StationedGears = new List<Gear>() }
            };

        public static IReadOnlyList<Weapon> CreateWeapons()
            => new List<Weapon>
            {
                new Weapon { Id = 1, Name = "Marcus' Lancer", AmmunitionType = AmmunitionType.Cartridge, IsAutomatic = true },
                new Weapon { Id = 2, Name = "Marcus' Gnasher", AmmunitionType = AmmunitionType.Shell, IsAutomatic = false/*, SynergyWith = marcusLancer*/ },
                new Weapon { Id = 3, Name = "Dom's Hammerburst", AmmunitionType = AmmunitionType.Cartridge, IsAutomatic = false },
                new Weapon { Id = 4, Name = "Dom's Gnasher", AmmunitionType = AmmunitionType.Shell, IsAutomatic = false },
                new Weapon { Id = 5, Name = "Cole's Gnasher", AmmunitionType = AmmunitionType.Shell, IsAutomatic = false },
                new Weapon { Id = 6, Name = "Cole's Mulcher", AmmunitionType = AmmunitionType.Cartridge, IsAutomatic = true },
                new Weapon { Id = 7, Name = "Baird's Lancer", AmmunitionType = AmmunitionType.Cartridge, IsAutomatic = true },
                new Weapon { Id = 8, Name = "Baird's Gnasher", AmmunitionType = AmmunitionType.Shell, IsAutomatic = false },
                new Weapon { Id = 9, Name = "Paduk's Markza", AmmunitionType = AmmunitionType.Cartridge, IsAutomatic = false },
                new Weapon { Id = 10, Name = "Mauler's Flail", IsAutomatic = false }
            };

        public static IReadOnlyList<CogTag> CreateTags()
            => new List<CogTag>
            {
                new CogTag { Id = Guid.Parse("DF36F493-463F-4123-83F9-6B135DEEB7BA"), Note = "Dom's Tag" },
                new CogTag { Id = Guid.Parse("A8AD98F9-E023-4E2A-9A70-C2728455BD34"), Note = "Cole's Tag" },
                new CogTag { Id = Guid.Parse("A7BE028A-0CF2-448F-AB55-CE8BC5D8CF69"), Note = "Paduk's Tag" },
                new CogTag { Id = Guid.Parse("70534E05-782C-4052-8720-C2C54481CE5F"), Note = "Bairds's Tag" },
                new CogTag { Id = Guid.Parse("34C8D86E-A4AC-4BE5-827F-584DDA348A07"), Note = "Marcus's Tag" },
                new CogTag { Id = Guid.Parse("B39A6FBA-9026-4D69-828E-FD7068673E57"), Note = "K.I.A." }
            };

        public static IReadOnlyList<Gear> CreateGears()
            => new List<Gear>
            {
                new Gear
                {
                    Nickname = "Dom",
                    FullName = "Dominic Santiago",
                    HasSoulPatch = false,
                    SquadId = 1,
                    Rank = MilitaryRank.Corporal,
                    CityOrBirthName = "Ephyra",
                    LeaderNickname = "Marcus",
                    LeaderSquadId = 1,
                },
                new Gear
                {
                    Nickname = "Cole Train",
                    FullName = "Augustus Cole",
                    HasSoulPatch = false,
                    SquadId = 1,
                    Rank = MilitaryRank.Private,
                    CityOrBirthName = "Hanover",
                    LeaderNickname = "Marcus",
                    LeaderSquadId = 1,
                },
                new Gear
                {
                    Nickname = "Paduk",
                    FullName = "Garron Paduk",
                    HasSoulPatch = false,
                    SquadId = 2,
                    Rank = MilitaryRank.Private,
                    CityOrBirthName = "Unknown",
                    LeaderNickname = "Baird",
                    LeaderSquadId = 1,
                },
                new Officer
                {
                    Nickname = "Baird",
                    FullName = "Damon Baird",
                    HasSoulPatch = true,
                    SquadId = 1,
                    Rank = MilitaryRank.Corporal,
                    CityOrBirthName = "Unknown",
                    LeaderNickname = "Marcus",
                    LeaderSquadId = 1,
                },
                new Officer
                {
                    Nickname = "Marcus",
                    FullName = "Marcus Fenix",
                    HasSoulPatch = true,
                    SquadId = 1,
                    Rank = MilitaryRank.Sergeant,
                    CityOrBirthName = "Jacinto"
                }
            };

        public static IReadOnlyList<LocustLeader> CreateLocustLeaders()
            => new List<LocustLeader>
            {
                new LocustLeader { Name = "General Karn", ThreatLevel = 3 },
                new LocustLeader { Name = "General RAAM", ThreatLevel = 4 },
                new LocustLeader { Name = "High Priest Skorge", ThreatLevel = 1 },
                new LocustCommander { Name = "Queen Myrrah", ThreatLevel = 5, DefeatedByNickname = "Marcus", DefeatedBySquadId = 1 },
                new LocustLeader { Name = "The Speaker", ThreatLevel = 3 },
                new LocustCommander { Name = "Unknown", ThreatLevel = 0 },
            };

        public static IReadOnlyList<Faction> CreateFactions()
            => new List<Faction>
            {
                new LocustHorde { Id = 1, Name = "Locust", Eradicated = true },
                //    Commander = myrrah,
                //Leaders = new List<LocustLeader> { karn, raam, skorge, myrrah }
                new LocustHorde { Id = 2, Name = "Swarm", Eradicated = false }
                //    Commander = swarmCommander,
                //Leaders = new List<LocustLeader> { theSpeaker },
            };

        public static void WireUp(
            IReadOnlyList<Squad> squads,
            IReadOnlyList<Mission> missions,
            IReadOnlyList<SquadMission> squadMissions,
            IReadOnlyList<City> cities,
            IReadOnlyList<Weapon> weapons,
            IReadOnlyList<CogTag> tags,
            IReadOnlyList<Gear> gear,
            IReadOnlyList<LocustLeader> locustLeader,
            IReadOnlyList<Faction> faction)
        {
            squadMissions[0].Mission = missions[0];
            squadMissions[0].Squad = squads[0]; 
            squadMissions[1].Mission = missions[1]; 
            squadMissions[1].Squad = squads[0]; 
            squadMissions[2].Mission = missions[2];
            squadMissions[2].Squad = squads[1];

            weapons[1].SynergyWith = weapons[0];

            // dom
            gear[0].AssignedCity = cities[1];
            gear[0].CityOfBirth = cities[1];
            gear[0].Squad = squads[0];
            gear[0].Tag = tags[0];
            gear[0].Weapons = new List<Weapon> { weapons[2], weapons[3] };

            // cole
            gear[1].CityOfBirth = cities[2];
            gear[1].AssignedCity = cities[0];
            gear[1].Squad = squads[0];
            gear[1].Tag = tags[1];
            gear[1].Weapons = new List<Weapon> { weapons[4], weapons[5] };

            // paduk
            gear[2].CityOfBirth = cities[3];
            gear[2].AssignedCity = cities[3];
            gear[2].Squad = squads[1];
            gear[2].Tag = tags[2];
            gear[2].Weapons = new List<Weapon> { weapons[8], };

            // baird
            gear[3].CityOfBirth = cities[3];
            gear[3].AssignedCity = cities[0];
            gear[3].Squad = squads[1];
            gear[3].Tag = tags[3];
            gear[3].Weapons = new List<Weapon> { weapons[6], weapons[7] };
            ((Officer)gear[3]).Reports = new List<Gear> { gear[2] };

            // marcus
            gear[4].CityOfBirth = cities[0];
            gear[4].Squad = squads[0];
            gear[4].Tag = tags[4];
            gear[4].Weapons = new List<Weapon> { weapons[0], weapons[1] };
            ((Officer)gear[4]).Reports = new List<Gear> { gear[0], gear[1], gear[3] };

            ((LocustCommander)LocustLeaders[3]).DefeatedBy = gear[4];
            ((LocustCommander)LocustLeaders[3]).CommandingFaction = ((LocustHorde)faction[0]);
            ((LocustCommander)LocustLeaders[5]).CommandingFaction = ((LocustHorde)faction[1]);
        }
    }
}
