using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Newtonsoft.Json.Linq;
using NoSqlRepositories;
using NoSqlRepositoryInterfaces;
using Services;
using Services.Matchmaking;

namespace Tests
{
    [TestClass]
    public class MatchmakingTests
    {
        private readonly WaitingPlayersRepository _waitingPlayersRepository = new();
        private readonly MatchRepository _matchRepository = new();

        [TestMethod]
        public async Task TestRandom100k()
        {
            Array values = Enum.GetValues(typeof(ERank));
            Dictionary<int, int> outcomeByScore = [];
            for (int i = 0; i < 11; i++)
                outcomeByScore.Add(i, 0);

            for (int j = 0; j < 1000000; j++)
            {
                Random random = new(j);

                await _waitingPlayersRepository.Clear();
                for (int i = 0; i < 10; i++)
                {
                    await _waitingPlayersRepository.Add(
                        new WaitingPlayerDTO()
                        {
                            Id = i,
                            Nickname = i.ToString(),
                            Rank =
                                (ERank?)values.GetValue(random.Next(values.Length)) ?? ERank.Iron4,
                            Priorities = new PrioritiesDTO()
                            {
                                Top = random.Next(1, 6),
                                Jng = random.Next(1, 6),
                                Mid = random.Next(1, 6),
                                Bot = random.Next(1, 6),
                                Supp = random.Next(1, 6),
                            }
                        }
                    );
                }
                MatchmakingService service = new(_waitingPlayersRepository, _matchRepository);

                var match = await service.Matchmake();
                outcomeByScore[match.UnassignedPlayers]++;
            }

            Assert.AreEqual(0, outcomeByScore.Where(s => s.Key != 0).Select(s => s.Value).Sum());
        }

        private async Task<WaitingPlayerDTO> GetPlayer(int id)
        {
            return await _waitingPlayersRepository.Get(id) ?? throw new KeyNotFoundException();
        }

        [TestMethod]
        public async Task Test2PeopleWantingASingleRole()
        {
            await SetDefaultPlayers();

            (await GetPlayer(0)).Priorities.Top = 5;
            (await GetPlayer(1)).Priorities.Top = 5;

            MatchmakingService service = new(_waitingPlayersRepository, _matchRepository);
            var match = await service.Matchmake();

            if (match.Red.Players[EPosition.Top].Id == 0)
                Assert.AreEqual(1, match.Blue.Players[EPosition.Top].Id);
            else
                Assert.AreEqual(0, match.Blue.Players[EPosition.Top].Id);
        }

        [TestMethod]
        public async Task Test3PeopleForTheSameRole()
        {
            await SetDefaultPlayers();

            (await GetPlayer(0)).Priorities.Top = 5;
            (await GetPlayer(1)).Priorities.Top = 5;
            (await GetPlayer(2)).Priorities.Top = 5;

            MatchmakingService service = new(_waitingPlayersRepository, _matchRepository);
            var match = await service.Matchmake();

            List<int> topPlayers = [0, 1, 2];
            Assert.IsTrue(
                topPlayers.Contains(match.Red.Players[EPosition.Top].Id)
                    && topPlayers.Contains(match.Blue.Players[EPosition.Top].Id)
                    && match.Blue.Players[EPosition.Top].Id != match.Red.Players[EPosition.Top].Id
            );

            Assert.AreEqual(0, match.UnassignedPlayers);
        }

        [TestMethod]
        public async Task TestOnePlayerWantingTwoRolesAndAnotherSingleRole()
        {
            await SetDefaultPlayers();

            (await GetPlayer(0)).Priorities.Top = 5;
            (await GetPlayer(0)).Priorities.Jng = 4;
            (await GetPlayer(1)).Priorities.Jng = 5;

            MatchmakingService service = new(_waitingPlayersRepository, _matchRepository);
            var match = await service.Matchmake();

            Assert.AreEqual(true, IsPositionCorrect(match, EPosition.Top, 0));
            Assert.AreEqual(true, IsPositionCorrect(match, EPosition.Jng, 1));
        }

        [TestMethod]
        public async Task TestOnePlayerWantingOneRoleMoreThanOtherRoles()
        {
            await SetDefaultPlayers();

            (await GetPlayer(0)).Priorities.Top = 1;
            (await GetPlayer(0)).Priorities.Jng = 1;
            (await GetPlayer(0)).Priorities.Bot = 1;
            (await GetPlayer(0)).Priorities.Supp = 1;

            MatchmakingService service = new(_waitingPlayersRepository, _matchRepository);
            var match = await service.Matchmake();

            Assert.AreEqual(true, IsPositionCorrect(match, EPosition.Mid, 0));
        }

        //private bool ArePositionsCorrect(
        //    MatchDTO match,
        //    EPosition position,
        //    int player1,
        //    int player2
        //)
        //{
        //    if (match.Red.Players[position].Id == player1)
        //        return match.Blue.Players[position].Id == player2;
        //    else if (match.Red.Players[position].Id == player2)
        //        return match.Blue.Players[position].Id == player1;
        //    else
        //        return false;
        //}

        private static bool IsPositionCorrect(MatchDTO match, EPosition position, int player)
        {
            if (match.Red.Players[position].Id == player)
                return true;
            else if (match.Red.Players[position].Id == player)
                return true;
            else
                return false;
        }

        private async Task SetDefaultPlayers()
        {
            await _waitingPlayersRepository.Clear();
            for (int i = 0; i < 10; i++)
            {
                await _waitingPlayersRepository.Add(
                    new WaitingPlayerDTO()
                    {
                        Id = i,
                        Nickname = i.ToString(),
                        Rank = ERank.Emerald1,
                        Priorities = new PrioritiesDTO()
                        {
                            Top = 3,
                            Jng = 3,
                            Mid = 3,
                            Bot = 3,
                            Supp = 3,
                        }
                    }
                );
            }
        }
    }
}
