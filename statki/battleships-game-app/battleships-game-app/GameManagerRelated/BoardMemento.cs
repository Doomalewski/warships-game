using battleships_game_app.CommandRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.GameManagerRelated
{
    internal class BoardMemento
    {
        public Board board;
        public BoardMemento(Board board)
        {
            this.board = new Board(board.Height, board.Width);
            //this.board.Fields = board.Fields;
        }
    }
}
