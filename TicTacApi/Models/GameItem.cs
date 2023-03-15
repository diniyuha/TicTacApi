using System;

namespace TicTacApi.Models
{
    public class GameItem
    {
        public Guid Id { get; set; }
        public string Field { get; set; }
        public StatusGame StatusGame { get; set; }
    }
}