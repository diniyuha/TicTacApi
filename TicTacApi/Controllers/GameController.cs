using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicTacApi.Exceptions;
using TicTacApi.Services;

namespace TicTacApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly TicTacGameService _gameServiceTicTac;
    
        public GameController(TicTacGameService gameServiceTicTac)
        {
            _gameServiceTicTac = gameServiceTicTac;
       }

        [HttpPost("init")]
        public async Task<Guid> CreateGame()
        {
            var newGameId = await _gameServiceTicTac.CreateGame();
            return newGameId;
        }

        [HttpPost("move")]
        public async Task<IActionResult> MakeMove(Guid gameId, int cellIndex)
        {
            try
            {
                var game = await _gameServiceTicTac.MakeMove(gameId, cellIndex);
                return Ok(game);
            }
            catch (GameException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}