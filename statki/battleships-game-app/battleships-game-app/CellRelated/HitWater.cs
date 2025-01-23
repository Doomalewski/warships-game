using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.CellRelated
{
    public class HitWater : ICellState
    {
        public void Hit(Cell context)
        {
            Console.WriteLine("You have shot here already!");
        }

        public void Display(Cell context)
        {
            Console.Write(context.IconManager.GetIconForState(this)); // Display for HitWater cell
        }
    }
}
