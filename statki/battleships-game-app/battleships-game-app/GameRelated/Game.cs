using battleships_game_app.CommandRelated;
using battleships_game_app.GameManagerRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.GameRelated
{
    internal class Game
    {
        public int GameId { get; set; }
        public Player player { get; set; }
        public Player player2 { get; set; }
        public Board board { get; set; }
        public bool? winner { get; set; }
        public CommandInvoker GameHistory;
        public Stack<BoardMemento> SavedStates;
        public Game(Player p1, Player p2, Board board) 
        {
            this.player = p1;
            this.player2 = p2;
            this.board = board;
            winner = null;
        }

    }
}
