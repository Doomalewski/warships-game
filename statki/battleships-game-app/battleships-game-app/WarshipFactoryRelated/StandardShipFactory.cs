using battleships_game_app.WarshipRelated;
using battleships_game_app.CellRelated;
using battleships_game_app.GameManagerRelated;

namespace battleships_game_app.WarshipFactoryRelated
{
    public class StandardShipFactory : WarshipFactory
    {
        private readonly int _length;
        private readonly bool _isHorizontal;

        public StandardShipFactory(int length,bool isHorizontal = true)
        {
            if (length < 2 || length > 3)
                throw new ArgumentException("Standard ship length must be 2 or 3.");

            _length = length;
            _isHorizontal = isHorizontal;
        }

        public override IWarship CreateWarship(Board board,Position startPosition)
        {
            return new StandardShip(_length, startPosition,board, _isHorizontal);
        }
    }
}
