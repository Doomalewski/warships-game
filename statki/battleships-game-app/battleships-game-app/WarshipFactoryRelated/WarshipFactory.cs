using battleships_game_app.CellRelated;
using battleships_game_app.GameManagerRelated;
using battleships_game_app.WarshipRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.WarshipFactoryRelated
{
    public abstract class WarshipFactory
    {
        public abstract IWarship CreateWarship(Board board,Position startPosition);
    }
}
