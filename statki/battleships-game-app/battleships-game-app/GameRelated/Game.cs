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
        public Player player1 { get; set; }
        public Player player2 { get; set; }
        public Board Board1 { get; set; }
        public Board Board2 { get; set; }
        public Board CurrentBoard { get; set; }
        public int? Hardeness {  get; set; }
        public bool? winner { get; set; }
        public CommandInvoker GameHistory;
        public Stack<BoardMemento> SavedStates;
        public Game(Player p1, Player p2, Board board1,Board board2) 
        {
            this.player1 = p1;
            this.player2 = p2;
            this.Board1 = board1;
            this.Board2 = board2;
            this.CurrentBoard = board2;
            winner = null;
            Hardeness = null;
        }

    }
}
