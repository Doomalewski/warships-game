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
/*    public class LShipFactory : WarshipFactory
    {
        private readonly int _length;

        public LShipFactory(int length)
        {
            _length = length;
        }

        public override IWarship CreateWarship(Board board, int length,Position startPosition)
        {
            var ship = new LShip(board, length, startPosition);
            for (int i = 0; i < _length; i++)
            {
                ship.AddCell(new Cell(new Position(i, 0), new Icon()));
            }
            return ship;
        }
    }*/
}
