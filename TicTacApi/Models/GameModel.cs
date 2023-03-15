using System.Collections.Generic;

namespace TicTacApi.Models
{
    public class GameModel
    {
        public List<int> Field { get; set; }
        public StatusGame StatusGame { get; set; }
        public string StatusGameName { get; set; }
        public List<int>? WinIndexes { get; set; }
    }
}