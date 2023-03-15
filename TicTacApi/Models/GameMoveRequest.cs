using System;

namespace TicTacApi.Models
{
    public class GameMoveRequest
    {
        public Guid GameId { get; set; }
        public int CellIndex { get; set; }
    }
}