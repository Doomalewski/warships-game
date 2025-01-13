using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.CellRelated
{
    public interface ICellState
    {
        void Hit(Cell context);
        void Display(Cell context);
    }
}
