﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;

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
            LocustLeaders = CreateLocustLeaders();
            Factions = CreateFactions();

            WireUp(Squads, Missions, SquadMissions, Cities, Weapons, Tags, Gears, LocustLeaders, Factions);
            WireUp2(LocustLeaders, Factions);
        }

        public static IQueryable<T> Set<T>()
        {
            if (typeof(T) == typeof(City))
            {
                return (IQueryable<T>)Cities.AsQueryable();
            }

            if (typeof(T) == typeof(CogTag))
            {
                return (IQueryable<T>)Tags.AsQueryable();
            }

            if (typeof(T) == typeof(Faction))
            {
                return (IQueryable<T>)Factions.AsQueryable();
            }

            if (typeof(T) == typeof(Gear))
            {
                return (IQueryable<T>)Gears.AsQueryable();
            }

            if (typeof(T) == typeof(Mission))
            {
                return (IQueryable<T>)Missions.AsQueryable();
            }

            if (typeof(T) == typeof(Squad))
            {
                return (IQueryable<T>)Squads.AsQueryable();
            }

            if (typeof(T) == typeof(SquadMission))
            {
                return (IQueryable<T>)SquadMissions.AsQueryable();
            }

            if (typeof(T) == typeof(Weapon))
            {
                return (IQueryable<T>)Weapons.AsQueryable();
            }

            if (typeof(T) == typeof(LocustLeader))
            {
                return (IQueryable<T>)LocustLeaders.AsQueryable();
            }

            throw new NotImplementedException();
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
                new Weapon { Id = 2, Name = "Marcus' Gnasher", AmmunitionType = AmmunitionType.Shell, IsAutomatic = false, SynergyWithId = 1 },
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
            IReadOnlyList<Gear> gears,
            IReadOnlyList<LocustLeader> locustLeaders,
            IReadOnlyList<Faction> factions)
        {
            squadMissions[0].Mission = missions[0];
            squadMissions[0].Squad = squads[0]; 
            squadMissions[1].Mission = missions[1]; 
            squadMissions[1].Squad = squads[0]; 
            squadMissions[2].Mission = missions[2];
            squadMissions[2].Squad = squads[1];

            missions[0].ParticipatingSquads = new List<SquadMission> { squadMissions[0] };
            missions[1].ParticipatingSquads = new List<SquadMission> { squadMissions[1] };
            missions[2].ParticipatingSquads = new List<SquadMission> { squadMissions[2] };
            squads[0].Missions = new List<SquadMission> { squadMissions[0], squadMissions[1] };
            squads[1].Missions = new List<SquadMission> { squadMissions[2] };

            weapons[1].SynergyWith = weapons[0];
            //weapons[0].SynergyWith = weapons[1]; // TODO: ?

            // dom
            gears[0].AssignedCity = cities[1];
            gears[0].CityOfBirth = cities[1];
            gears[0].Squad = squads[0];
            gears[0].Tag = tags[0];
            gears[0].Weapons = new List<Weapon> { weapons[2], weapons[3] };

            // cole
            gears[1].CityOfBirth = cities[2];
            gears[1].AssignedCity = cities[0];
            gears[1].Squad = squads[0];
            gears[1].Tag = tags[1];
            gears[1].Weapons = new List<Weapon> { weapons[4], weapons[5] };

            // paduk
            gears[2].CityOfBirth = cities[3];
            gears[2].AssignedCity = cities[3];
            gears[2].Squad = squads[1];
            gears[2].Tag = tags[2];
            gears[2].Weapons = new List<Weapon> { weapons[8], };

            // baird
            gears[3].CityOfBirth = cities[3];
            gears[3].AssignedCity = cities[0];
            gears[3].Squad = squads[1];
            gears[3].Tag = tags[3];
            gears[3].Weapons = new List<Weapon> { weapons[6], weapons[7] };
            ((Officer)gears[3]).Reports = new List<Gear> { gears[2] };

            // marcus
            gears[4].CityOfBirth = cities[0];
            gears[4].Squad = squads[0];
            gears[4].Tag = tags[4];
            gears[4].Weapons = new List<Weapon> { weapons[0], weapons[1] };
            ((Officer)gears[4]).Reports = new List<Gear> { gears[0], gears[1], gears[3] };

            cities[0].BornGears = new List<Gear> { gears[4] };
            cities[1].BornGears = new List<Gear> { gears[0] };
            cities[2].BornGears = new List<Gear> { gears[1] };
            cities[3].BornGears = new List<Gear> { gears[2], gears[3] };
            cities[0].StationedGears = new List<Gear> { gears[1], gears[3] };
            cities[1].StationedGears = new List<Gear> { gears[0] };
            cities[2].StationedGears = new List<Gear>();
            cities[3].StationedGears = new List<Gear> { gears[2] };

            weapons[0].Owner = gears[4];
            weapons[1].Owner = gears[4];
            weapons[2].Owner = gears[0];
            weapons[3].Owner = gears[0];
            weapons[4].Owner = gears[1];
            weapons[5].Owner = gears[1];
            weapons[6].Owner = gears[3];
            weapons[7].Owner = gears[3];
            weapons[8].Owner = gears[2];

            ((LocustCommander)locustLeaders[3]).DefeatedBy = gears[4];
            ((LocustCommander)locustLeaders[3]).CommandingFaction = ((LocustHorde)factions[0]);
            ((LocustCommander)locustLeaders[5]).CommandingFaction = ((LocustHorde)factions[1]);

            ((LocustHorde)factions[0]).Commander = ((LocustCommander)locustLeaders[3]);
            //((LocustHorde)factions[0]).Leaders = new List<LocustLeader> { locustLeaders[0], locustLeaders[1], locustLeaders[2], locustLeaders[3] };
            ((LocustHorde)factions[1]).Commander = ((LocustCommander)locustLeaders[5]);
            //((LocustHorde)factions[1]).Leaders = new List<LocustLeader> { locustLeaders[4], locustLeaders[5] };
        }

        public static void WireUp2(
            IReadOnlyList<LocustLeader> locustLeaders,
            IReadOnlyList<Faction> factions)
        {
            ((LocustHorde)factions[0]).Leaders = new List<LocustLeader> { locustLeaders[0], locustLeaders[1], locustLeaders[2], locustLeaders[3] };
            ((LocustHorde)factions[1]).Leaders = new List<LocustLeader> { locustLeaders[4], locustLeaders[5] };
        }
    }
}
