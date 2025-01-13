using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.CellRelated
{
    public class Cell
    {
        public int Id { get; private set; }
        public Position Position { get; private set; }
        public ICellState State { get; private set; }
        public  Icon IconManager { get; private set; }

        public Cell(Position pos,Icon iconManager)
        {
            Position = pos;
            State = new NotHit();
            IconManager = iconManager;
        }

        public void Hit()
        {
            State.Hit(this);
        }

        public void Display(Cell context)
        {
            State.Display(context);
        }

        public void SetState(ICellState state)
        {
            State = state;
        }
    }
}
