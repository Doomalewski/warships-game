using battleships_game_app.CellRelated;
using System;
using System.Collections.Generic;
using System.Linq;

namespace battleships_game_app.GameManagerRelated
{
    public class Board
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public List<Cell> Fields { get; private set; } // Lista komórek na planszy

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            Fields = new List<Cell>();

            // Inicjalizowanie komórek na podstawie szerokości i wysokości planszy
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var position = new Position(x, y);
                    Fields.Add(new Cell(position, new Icon())); // Zakładając, że masz odpowiednią klasę Icon
                }
            }
        }

        // Metoda ustawiająca komórkę
        public void SetCell(Cell cell)
        {
            var existingCell = GetCell(cell.Position);
            if (existingCell != null)
            {
                existingCell.SetState(cell.State); // Zmiana stanu komórki
            }
            else
            {
                throw new InvalidOperationException("Cell does not belong to this board.");
            }
        }

        // Metoda do pobierania komórki na podstawie pozycji
        public Cell GetCell(Position position)
        {
            return Fields.FirstOrDefault(cell => cell.Position.X == position.X && cell.Position.Y == position.Y);
        }

        // Metoda sprawdzająca, czy gra została przegrana
        public bool Lost()
        {
            // Logika do określenia, czy gra jest przegrana (np. czy wszystkie statki zostały zatopione)
            return Fields.All(cell => cell.State is Sunk); // Sprawdzanie, czy każda komórka jest zatopiona
        }
    }
}
