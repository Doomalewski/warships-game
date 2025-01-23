using battleships_game_app.CellRelated;
using battleships_game_app.GameManagerRelated;
using battleships_game_app.GameRelated;

namespace battleships_game_app.CommandRelated
{
    internal class FireCommand : ICommand
    {
        private readonly Board board;
        private readonly Player player;
        private readonly Position position;
        private Cell? targetCell;
        private ICellState? previousState;

        public FireCommand(Board board, Position position, Player player)
        {
            this.board = board ?? throw new ArgumentNullException(nameof(board));
            this.player = player ?? throw new ArgumentNullException(nameof(player));
            this.position = position ?? throw new ArgumentNullException(nameof(position));
        }

        public void Execute()
        {
            // Znajdź komórkę na planszy na podstawie pozycji
            var targetCell = board.Fields.FirstOrDefault(cell =>
                cell.Position.X == position.X && cell.Position.Y == position.Y);
            if (targetCell == null)
            {
                throw new InvalidOperationException("Invalid position: Cell not found.");
            }
            this.targetCell = targetCell;


            // Zapamiętaj poprzedni stan komórki
            previousState = targetCell.State;

            // Wykonaj strzał (zmień stan komórki)
            targetCell.Hit();
        }

        public void Undo()
        {
            if (targetCell == null || previousState == null)
            {
                throw new InvalidOperationException("Cannot undo: No action to undo.");
            }

            // Przywróć poprzedni stan komórki
            targetCell.SetState(previousState);
        }

        public bool WasHit()
        {
            // Sprawdź, czy stan komórki po wykonaniu strzału to `Hit`
            return targetCell?.State is WasHit;
        }
    }
}
