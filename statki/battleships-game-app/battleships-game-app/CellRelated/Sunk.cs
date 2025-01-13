using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.CellRelated
{
    public class Sunk : ICellState
    {
        public void Hit(Cell context)
        {
            Console.WriteLine("Cell is already sunk!");
        }

        public void Display(Cell context)
        {
            Console.Write(context.IconManager.GetIconForState(this)); // Display for Sunk cell
        }
    }
}
