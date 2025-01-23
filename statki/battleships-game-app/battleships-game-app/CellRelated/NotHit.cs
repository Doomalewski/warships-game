using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.CellRelated
{
    public class NotHit : ICellState
    {
        public void Hit(Cell context)
        {
            Console.WriteLine("Warhip Hit!");
            context.SetState(new WasHit());
        }

        public void Display(Cell context)
        {
            Console.Write(context.IconManager.GetIconForState(this)); // Display for NotHit cell
        }
    }
}
