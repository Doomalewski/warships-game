using battleships_game_app.CellRelated;
using battleships_game_app.GameManagerRelated;
using battleships_game_app.GameRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.CommandRelated
{
    internal interface ICommand
    {
        public void Execute();
        public void Undo();
    }
}
