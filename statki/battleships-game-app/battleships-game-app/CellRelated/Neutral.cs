using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.CellRelated
{
    public class Neutral : ICellState
    {
        public void Hit(Cell context)
        {
            Console.WriteLine("Miss! Neutral cell hit.");
            context.SetState(new HitWater());
        }

        public void Display(Cell context)
        {
            Console.Write(context.IconManager.GetIconForState(this)); // Display for Neutral cell
        }
    }
}
