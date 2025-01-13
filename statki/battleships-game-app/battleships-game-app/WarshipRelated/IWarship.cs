using battleships_game_app.CellRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace battleships_game_app.WarshipRelated
{
    public interface IWarship
    {
        void AddCell(Cell cell);
        bool IsSunk { get; }
        List<Cell> GetBody();

    }
}
