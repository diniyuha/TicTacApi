using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicTacApi.Exceptions;
using TicTacApi.Models;

namespace TicTacApi.Services
{
    public class TicTacGameService
    {
        private readonly GameContext _context;

        public TicTacGameService(GameContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateGame()
        {
            State[] field = new State[9];
            for (int i = 0; i < 9; i++)
            {
                field[i] = State.Blank;
            }

            var newGame = new GameItem
            {
                Id = Guid.NewGuid(),
                Field = GetStateString(field),
                StatusGame = StatusGame.GameInProcess
            };
            _context.Games.Add(newGame);
            await _context.SaveChangesAsync();

            return newGame.Id;
        }

        public async Task<GameModel> MakeMove(Guid gameId, int index)
        {
            var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == gameId);

            if (game == null)
            {
                throw new GameException("Game is not found");
            }

            if (game.StatusGame != StatusGame.GameInProcess)
            {
                throw new GameException("The game is completed");
            }

            if (index is < 1 or > 9)
            {
                throw new GameException("Index is out of range");
            }

            var field = GetStateArray(game.Field);

            if (field[index - 1] != State.Blank)
            {
                throw new GameException("Only empty cells can be specified");
            }

            var movesCount = field.Count(x => x != State.Blank);
            if (movesCount % 2 == 0)
            {
                field[index - 1] = State.Cross;
            }
            else
            {
                field[index - 1] = State.Zero;
            }

            var checkWinner = CheckWinner(field, movesCount);

            game.Field = GetStateString(field);
            game.StatusGame = checkWinner.statusGame;
            await _context.SaveChangesAsync();

            var gameModal = new GameModel
            {
                StatusGame = game.StatusGame,
                StatusGameName = game.StatusGame.ToString(),
                Field = game.Field.Select(x => int.Parse(x.ToString())).ToList(),
                WinIndexes = checkWinner.winIndexes.ToList()
            };

            return gameModal;
        }

        
        private static (StatusGame statusGame, int[] winIndexes) CheckWinner(State[] field, int movesCount)
        {
            int[,] winners =
            {
                {0, 1, 2},
                {3, 4, 5},
                {6, 7, 8},
                {0, 3, 6},
                {1, 4, 7},
                {2, 5, 8},
                {0, 4, 8},
                {2, 4, 6}
            };
            for (int i = 0; i < 8; i++)
            {
                bool same = AreSame(field[winners[i, 0]], field[winners[i, 1]], field[winners[i, 2]]);
                if (same)
                {
                    var state = field[winners[i, 0]];
                    var winIndexes = new [] {winners[i, 0] + 1, winners[i, 1] + 1, winners[i, 2] + 1};
                    return (state == State.Cross ? StatusGame.WinCrosses : StatusGame.WinZeros, winIndexes);
                }
            }

            return (movesCount < 8 ? StatusGame.GameInProcess : StatusGame.Draw,  new int[]{});
        }
        
        private static bool AreSame(State a, State b, State c)
        {
            return a == b && a == c && a != State.Blank;
        }

        private string GetStateString(State[] states)
        {
            StringBuilder bld = new StringBuilder();
            for (var i = 0; i < 9; i++)
            {
                bld.Append((int) states[i]);
            }

            return bld.ToString();
        }

        private State[] GetStateArray(string stateString)
        {
            State[] stateArray = new State[9];
            for (var i = 0; i < 9; i++)
            {
                stateArray[i] = (State) int.Parse(stateString[i].ToString());
            }

            return stateArray;
        }
    }
}