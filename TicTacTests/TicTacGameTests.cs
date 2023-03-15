using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TicTacApi.Exceptions;
using TicTacApi.Models;
using TicTacApi.Services;

namespace TicTacTests
{
    [TestFixture]
    public class TicTacGameTests
    {
        static DbContextOptions<GameContext> options = new DbContextOptionsBuilder<GameContext>()
            .UseInMemoryDatabase(databaseName: "TicTacGame")
            .Options;

        private GameContext _gameContext;
        private TicTacGameService _gameServiceTicTac;
        
   
        [SetUp]
        public void SetUp()
        {
            _gameContext = new GameContext(options);
            _gameContext.Database.EnsureDeleted();
            _gameServiceTicTac = new TicTacGameService(_gameContext);
        }

        [Test]
        public async Task CreateGame_ShouldReturnNewGameId()
        {
            var gameId = await _gameServiceTicTac.CreateGame();
            Assert.That(gameId, Is.Not.Null);
        }
        
        
        [Test]
        public async Task MoveGame_ShouldReturnStatusGameInProcess()
        {
            var gameId = await _gameServiceTicTac.CreateGame();
            var game = await _gameContext.Games.FirstOrDefaultAsync(x => x.Id == gameId);
            await _gameServiceTicTac.MakeMove(gameId, 1);
           
            Assert.That(gameId, Is.Not.Null);
            Assert.That(game.StatusGame, Is.EqualTo(StatusGame.GameInProcess));
        }


        [Test]
        public async Task MoveGame_ShouldReturnStatusDraw()
        {
            var gameId = await _gameServiceTicTac.CreateGame();
            var game = await _gameContext.Games.FirstOrDefaultAsync(x => x.Id == gameId);
            await _gameServiceTicTac.MakeMove(gameId, 5);
            await _gameServiceTicTac.MakeMove(gameId, 1);
            await _gameServiceTicTac.MakeMove(gameId, 7);  
            await _gameServiceTicTac.MakeMove(gameId, 3);  
            await _gameServiceTicTac.MakeMove(gameId, 2);  
            await _gameServiceTicTac.MakeMove(gameId, 8);  
            await _gameServiceTicTac.MakeMove(gameId, 4);  
            await _gameServiceTicTac.MakeMove(gameId, 6);  
            await _gameServiceTicTac.MakeMove(gameId, 9);  
            
            Assert.That(gameId, Is.Not.Null);
            Assert.That(game.StatusGame, Is.EqualTo(StatusGame.Draw));
        }
        
        [Test]
        public async Task MoveGame_ShouldReturnStatusWinnerZeros()
        {
            var gameId = await _gameServiceTicTac.CreateGame();
            var game = await _gameContext.Games.FirstOrDefaultAsync(x => x.Id == gameId);
            await _gameServiceTicTac.MakeMove(gameId, 5);
            await _gameServiceTicTac.MakeMove(gameId, 2);
            await _gameServiceTicTac.MakeMove(gameId, 7);  
            await _gameServiceTicTac.MakeMove(gameId, 3);  
            await _gameServiceTicTac.MakeMove(gameId, 9);  
            await _gameServiceTicTac.MakeMove(gameId, 1);

            Assert.That(gameId, Is.Not.Null);
            Assert.That(game.StatusGame, Is.EqualTo(StatusGame.WinZeros));
        }
        
        [Test]
        public async Task MoveGame_ShouldReturnStatusWinnerCrosses()
        {
            var gameId = await _gameServiceTicTac.CreateGame();
            var game = await _gameContext.Games.FirstOrDefaultAsync(x => x.Id == gameId);
            await _gameServiceTicTac.MakeMove(gameId, 5);
            await _gameServiceTicTac.MakeMove(gameId, 2);
            await _gameServiceTicTac.MakeMove(gameId, 1);  
            await _gameServiceTicTac.MakeMove(gameId, 9);  
            await _gameServiceTicTac.MakeMove(gameId, 7);  
            await _gameServiceTicTac.MakeMove(gameId, 4);
            await _gameServiceTicTac.MakeMove(gameId, 3);
            
            Assert.That(gameId, Is.Not.Null);
            Assert.That(game.StatusGame, Is.EqualTo(StatusGame.WinCrosses));
        }
        
        [Test]
        public async Task MoveGame_ShouldReturnExceptionNotEmptyCell()
        {
            var gameId = await _gameServiceTicTac.CreateGame();
             
            Assert.That(gameId, Is.Not.Null);
           
            await _gameServiceTicTac.MakeMove(gameId, 5);
            GameException ex = Assert.ThrowsAsync<GameException>(async () =>
            {
                await _gameServiceTicTac.MakeMove(gameId, 5);
            }) ?? throw new InvalidOperationException();
            Assert.That(ex.Message, Is.EqualTo("Only empty cells can be specified"));
        }
        
        [Test]
        public async Task MoveGame_ShouldReturnExceptionGameNotFound()
        {
            var gameId = await _gameServiceTicTac.CreateGame();
            var newGiud = Guid.NewGuid();
            
            Assert.That(gameId, Is.Not.Null);
            Assert.That(gameId, Is.Not.EqualTo(newGiud));
           
            GameException ex = Assert.ThrowsAsync<GameException>(async () =>
            {
                await _gameServiceTicTac.MakeMove(newGiud, 5);
            }) ?? throw new InvalidOperationException();
            Assert.That(ex.Message, Is.EqualTo("Game is not found"));
        }
        
        [Test]
        public async Task MoveGame_ShouldReturnExceptionGameCompleted()
        {
            var gameId = await _gameServiceTicTac.CreateGame();
         
            Assert.That(gameId, Is.Not.Null);
            
            await _gameServiceTicTac.MakeMove(gameId, 5);
            await _gameServiceTicTac.MakeMove(gameId, 2);
            await _gameServiceTicTac.MakeMove(gameId, 7);  
            await _gameServiceTicTac.MakeMove(gameId, 3);  
            await _gameServiceTicTac.MakeMove(gameId, 9);  
            await _gameServiceTicTac.MakeMove(gameId, 1);
            
            GameException ex = Assert.ThrowsAsync<GameException>(async () =>
            {
                await _gameServiceTicTac.MakeMove(gameId, 5);
            }) ?? throw new InvalidOperationException();
            Assert.That(ex.Message, Is.EqualTo("The game is completed"));
        }
        
        [Test]
        public async Task MoveGame_ShouldReturnExceptionOutOfRange()
        {
            var gameId = await _gameServiceTicTac.CreateGame();
            Assert.That(gameId, Is.Not.Null);
             
            GameException ex = Assert.ThrowsAsync<GameException>(async () =>
            {
                await _gameServiceTicTac.MakeMove(gameId, 12);
            }) ?? throw new InvalidOperationException();
            Assert.That(ex.Message, Is.EqualTo("Index is out of range"));
        }
    }
}