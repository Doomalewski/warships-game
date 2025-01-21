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
        public bool Visible { get; private set; }

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
        public void SetVisibility(bool visible)
        {
            Visible = visible;
        }
        public void Display(Cell context)
        {
            if (Visible) {
                State.Display(context);
            }
            else
            {
                Console.Write("~");
            }

        }

        public void SetState(ICellState state)
        {
            State = state;
        }
        public void ToggleVisibility()
        { 
             Visible = !Visible; 
        }
    }
}
