using battleships_game_app.CellRelated;
using battleships_game_app.GameManagerRelated;
using System;
using System.Collections.Generic;

namespace battleships_game_app.WarshipRelated
{
    public abstract class Warship : IWarship
    {
        protected List<Cell> Body = new();
        public Board WarshipBoard { get; private set; }
        public bool IsSunk => Body.TrueForAll(cell => cell.State is Sunk);

        // Konstruktor, który przypisuje planszę
        private bool _isHorizontal;
        private int _length;
        public Position _startPosition;


        public Warship(Board board, int length, Position startPosition, bool isHorizontal = true)
        {
            if (length < 2 || length > 3)
                throw new ArgumentException("Ship length must be 2 or 3.");

            WarshipBoard = board ?? throw new ArgumentNullException(nameof(board));
            _length = length;
            _isHorizontal = isHorizontal;
            _startPosition = startPosition; // Set the start position
            Body = new List<Cell>();
        }


        // Metoda do przypisywania statku do planszy
        public void AddCell(Cell cell)
        {
            if (WarshipBoard.GetCell(cell.Position) != null)  // Sprawdzamy, czy komórka należy do planszy
            {
                Body.Add(cell);
                cell.SetState(new NotHit());  // Zmieniamy stan komórki na "Ship"
            }
            else
            {
                throw new InvalidOperationException("Cell does not belong to the board.");
            }
        }
        public void Rotate()
        {
            // Usuń stare komórki ze statku
            RemoveOldCells();

            // Przełącz orientację
            _isHorizontal = !_isHorizontal;

            try
            {
                // Zaktualizuj ciało statku w nowej orientacji
                UpdateBody();
            }
            catch (InvalidOperationException ex)
            {
                // Cofnij zmianę orientacji w przypadku błędu
                _isHorizontal = !_isHorizontal;
                throw new InvalidOperationException("Cannot rotate ship: " + ex.Message);
            }
        }

        // Metoda do pobrania ciała statku
        public List<Cell> GetBody()
        {
            return Body;
        }

        // Metoda do umieszczania statku na planszy
        public void PlaceOnBoard(Position startPosition, int length, bool isHorizontal)
        {
            for (int i = 0; i < length; i++)
            {
                var offsetX = isHorizontal ? i : 0;
                var offsetY = isHorizontal ? 0 : i;

                var cellPosition = new Position(startPosition.X + offsetX, startPosition.Y + offsetY);
                var boardCell = WarshipBoard.GetCell(cellPosition);

                if (boardCell != null && boardCell.State is Neutral)  // Sprawdzamy, czy komórka jest pusta
                {
                    AddCell(boardCell);  // Dodajemy komórkę do ciała statku
                }
                else
                {
                    throw new InvalidOperationException("Cannot place ship at the specified location.");
                }
            }
        }

        // Jeśli musisz zmieniać planszę w trakcie działania, możesz dodać metodę ustawiającą planszę
        public void SetBoard(Board board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Board cannot be null.");
            }
            WarshipBoard = board;
        }
        // Metoda do przesuwania statku
        public void Move(int deltaX, int deltaY)
        {
            // Tymczasowa lista na nowe pozycje
            List<Position> newPositions = new();

            // Usuń stare komórki z planszy przed walidacją
            foreach (var cell in Body)
            {
                cell.SetState(new Neutral());
            }
            // Sprawdź, czy nowe pozycje są prawidłowe
            foreach (var cell in Body)
            {
                var newPosition = new Position(cell.Position.X + deltaX, cell.Position.Y + deltaY);
                var newCell = WarshipBoard.GetCell(newPosition);

                if (newCell == null || !(newCell.State is Neutral))
                {
                    // Przywróć stare komórki, jeśli walidacja nie przejdzie
                    foreach (var oldCell in Body)
                    {
                        oldCell.SetState(new NotHit());
                    }

                    throw new InvalidOperationException("Cannot move ship to the specified location.");
                }

                newPositions.Add(newPosition);
            }

            // Zaktualizuj ciało statku na podstawie nowych pozycji
            Body.Clear();
            foreach (var newPosition in newPositions)
            {
                var newCell = WarshipBoard.GetCell(newPosition);
                if (newCell != null)
                {
                    AddCell(newCell);
                    newCell.SetState(new NotHit()); // Ustaw stan jako zajęty przez statek
                }
            }
            _startPosition = new Position(_startPosition.X + deltaX, _startPosition.Y + deltaY);
        }

        public void UpdateBody()
        {
            Body.Clear();

            for (int i = 0; i < _length; i++)
            {
                int x = _startPosition.X + (_isHorizontal ? 0 : i);
                int y = _startPosition.Y + (_isHorizontal ? i : 0);

                var position = new Position(x, y);
                var boardCell = WarshipBoard.GetCell(position);

                if (boardCell == null || !(boardCell.State is Neutral))
                {
                    throw new InvalidOperationException("Cannot place ship: position is invalid or occupied.");
                }

                Body.Add(boardCell);
                boardCell.SetState(new NotHit()); // Ustaw stan komórki na zajętą przez statek
            }
        }

        public void RemoveOldCells()
        {
            foreach (var cell in Body)
            {
                var boardCell = WarshipBoard.GetCell(cell.Position);
                if (boardCell != null)
                {
                    boardCell.SetState(new Neutral()); // Resetujemy stan komórki
                }
            }
        }
        public void SetStartPosition(Position newStartPosition)
        {
            _startPosition = newStartPosition;

            try
            {
                UpdateBody(); // Update the ship's body after changing the start position
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Cannot set new start position: " + ex.Message);
            }
        }
    }
}
