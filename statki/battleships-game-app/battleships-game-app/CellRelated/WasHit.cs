using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.CellRelated
{
    public class WasHit : ICellState
    {
        public void Hit(Cell context)
        {
            Console.WriteLine("Cell was already hit!");
        }

        public void Display(Cell context)
        {
            Console.Write(context.IconManager.GetIconForState(this)); // Display for Hit cell
        }
    }
}
