using battleships_game_app.CellRelated;
using battleships_game_app.GameManagerRelated;
using battleships_game_app.WarshipRelated;

public class StandardShip : Warship
{
    private readonly int _length;
    private bool _isHorizontal;

    public StandardShip(int length, Position startPosition, Board board, bool isHorizontal = true)
        : base(board, length, startPosition, isHorizontal)  // Pass the values to the base class constructor
    {
        _length = length;
        _isHorizontal = isHorizontal;

        // Update the ship's body based on the start position and orientation
        //UpdateBody();
    }

}
