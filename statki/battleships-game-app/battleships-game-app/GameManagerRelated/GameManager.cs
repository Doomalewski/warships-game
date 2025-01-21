using battleships_game_app.CellRelated;
using battleships_game_app.CommandRelated;
using battleships_game_app.GameRelated;
using battleships_game_app.WarshipFactoryRelated;
using battleships_game_app.WarshipRelated;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.GameManagerRelated
{
    internal class GameManager
    {
        public Game Game { get; private set; }

        public void SetGame(Game game)
        {
            Game = game;
        }

        public void InitStandardGame(Player p1, Player p2)
        {
            if (p1 == null || p2 == null)
            {
                throw new ArgumentNullException("Both players must be provided.");
            }

            int boardWidth = 10;
            int boardHeight = 10;
            var board1 = new Board(boardWidth, boardHeight);
            var board2 = new Board(boardWidth, boardHeight);

            InitializeBoardCells(board1);
            InitializeBoardCells(board2);


            Game = new Game(p1, p2, board1, board2)
            {
                GameHistory = new CommandInvoker(),
                SavedStates = new Stack<BoardMemento>()
            };

            Console.WriteLine("Game initialized successfully.");
        }

        private void InitializeBoardCells(Board board)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            board.Fields.Clear();

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    var position = new Position(i, j);
                    var iconManager = new Icon();
                    var cell = new Cell(position, iconManager);
                    cell.SetState(new Neutral());
                    cell.SetVisibility(true);
                    board.Fields.Add(cell);
                }
            }

            Console.WriteLine($"Board initialized with {board.Fields.Count} cells.");
        }

        public void PrintBoard()
        {
            if (Game.CurrentBoard == null || Game.CurrentBoard.Fields == null)
            {
                Console.WriteLine("Board is not initialized.");
                return;
            }
            Console.Clear();
            Console.WriteLine("Current Board:");

            // Column headers (A, B, C, ...)
            Console.Write("   "); // Offset for row numbers
            for (int col = 0; col < Game.CurrentBoard.Width; col++)
            {
                Console.Write($" {Convert.ToChar('A' + col)} ");
            }
            Console.WriteLine();

            // Draw the board
            for (int row = 0; row < Game.CurrentBoard.Height; row++)
            {
                // Row number (1, 2, 3, ...)
                Console.Write($"{row + 1,2} ");

                for (int col = 0; col < Game.CurrentBoard.Width; col++)
                {
                    // Get the cell based on its position
                    var cell = Game.CurrentBoard.Fields.FirstOrDefault(c => c.Position.X == row && c.Position.Y == col);

                    if (cell != null)
                    {
                        Console.Write(" "); // Add space between cells for better readability
                        cell.Display(cell);
                        Console.Write(" "); // Add space between cells for better readability
                    }
                    else
                    {
                        // If no cell, use default symbol
                        Console.Write(" ~ ");
                    }
                }

                Console.WriteLine(); // New line after each row
            }

            Console.WriteLine(); // Empty line at the end of the board
        }

        public void PrintBoardNoClear()
        {
            if (Game.CurrentBoard == null || Game.CurrentBoard.Fields == null)
            {
                Console.WriteLine("Board is not initialized.");
                return;
            }
            Console.WriteLine("Current Board:");

            // Column headers (A, B, C, ...)
            Console.Write("   "); // Offset for row numbers
            for (int col = 0; col < Game.CurrentBoard.Width; col++)
            {
                Console.Write($" {Convert.ToChar('A' + col)} ");
            }
            Console.WriteLine();

            // Draw the board
            for (int row = 0; row < Game.CurrentBoard.Height; row++)
            {
                // Row number (1, 2, 3, ...)
                Console.Write($"{row + 1,2} ");

                for (int col = 0; col < Game.CurrentBoard.Width; col++)
                {
                    // Get the cell based on its position
                    var cell = Game.CurrentBoard.Fields.FirstOrDefault(c => c.Position.X == row && c.Position.Y == col);

                    if (cell != null)
                    {
                        // Display the icon of the cell
                        Console.Write(" "); // Add space between cells for better readability
                        cell.Display(cell);
                        Console.Write(" "); // Add space between cells for better readability
                    }
                    else
                    {
                        // If no cell, use default symbol
                        Console.Write(" ~ ");
                    }
                }

                Console.WriteLine(); // New line after each row
            }

            Console.WriteLine(); // Empty line at the end of the board
        }
        public void AddShips(Player player1, Player player2)
        {
            Console.WriteLine($"Player {player1.Name}, set up your ships on board.");
            AddShipsForPlayer(player1, Game.Board1);

            Console.WriteLine($"Player {player2.Name}, set up your ships on board.");
            AddShipsForPlayer(player2, Game.Board2);
        }

        private void AddShipsForPlayer(Player player, Board board)
        {
            Game.CurrentBoard = board; 
            while (true)
            {
                PrintBoard();
                //displayShips(player);
                Console.WriteLine("Enter starting position (e.g., A1) or type 'done' to finish:");
                var positionInput = Console.ReadLine();

                if (positionInput?.ToLower() == "done")
                    break;

                if (!TryParsePosition(positionInput, out Position startPosition))
                {
                    Console.WriteLine("Invalid position. Try again.");
                    continue;
                }

                Console.WriteLine("Enter ship length (2 or 3):");
                if (!int.TryParse(Console.ReadLine(), out int length) || (length != 2 && length != 3))
                {
                    Console.WriteLine("Invalid length. Only 2 or 3 are allowed.");
                    continue;
                }

                var factory = new StandardShipFactory(length);
                var ship = (StandardShip)factory.CreateWarship(board, startPosition);
                bool shipPlaced = false;

                while (!shipPlaced)
                {
                    PrintBoard();
                    Console.WriteLine("Use arrow keys to move, 'r' to rotate, 'p' to place, and 'c' to cancel.");
                    var key = Console.ReadKey(intercept: true);

                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            TryMoveShip(ship, -1, 0);
                            break;

                        case ConsoleKey.DownArrow:
                            TryMoveShip(ship, 1, 0);
                            break;

                        case ConsoleKey.LeftArrow:
                            TryMoveShip(ship, 0, -1);
                            break;

                        case ConsoleKey.RightArrow:
                            TryMoveShip(ship, 0, 1);
                            break;

                        case ConsoleKey.R:
                            RotateShip(ship);
                            break;

                        case ConsoleKey.P:
                            if (CanPlaceShip(ship, board))
                            {
                                PlaceShipOnBoard(ship, board);
                                Console.WriteLine("Ship placed.");
                                shipPlaced = true;
                                player.Ships.Add(ship);
                            }
                            else
                            {
                                Console.WriteLine("Cannot place the ship here. Try again.");
                            }
                            break;

                        case ConsoleKey.C:
                            Console.WriteLine("Cancelled ship placement.");
                            ship.Destroy(board);
                            return;

                        default:
                            Console.WriteLine("Invalid key. Try again.");
                            break;
                    }
                }
            }
        }


        private void TryMoveShip(Warship ship, int deltaX, int deltaY)
        {
            try
            {
                ship.Move(deltaX, deltaY);
                Console.WriteLine("Ship moved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot move ship: {ex.Message}");
            }
        }

        private void RotateShip(StandardShip ship)
        {
            try
            {
                ship.Rotate();
                Console.WriteLine("Ship rotated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot rotate ship: {ex.Message}");
            }
        }
        private bool CanPlaceShip(IWarship ship, Board board)
        {
            var temporaryPositions = ship.GetBody().Select(cell => cell.Position).ToHashSet();

            foreach (var position in temporaryPositions)
            {
                // Sprawdź, czy pozycja jest poza granicami planszy
                if (position.X < 0 || position.Y < 0 ||
                    position.X >= board.Height || position.Y >= board.Width)
                {
                    return false;
                }

                // Sprawdź, czy pozycja koliduje z innymi statkami
                if (board.Fields.Any(existingCell =>
                    !temporaryPositions.Contains(existingCell.Position) && // Ignoruj komórki tymczasowe statku
                    existingCell.Position.Equals(position)))
                {
                    return false;
                }
            }

            return true;
        }
        private void PlaceShipOnBoard(IWarship ship, Board board)
        {
            foreach (var cell in ship.GetBody())
            {
                var boardCell = board.GetCell(cell.Position);
                if (boardCell != null)
                {
                    boardCell.SetState(new NotHit()); // Mark the cell as occupied by the ship
                }
            }
        }

        private bool TryParsePosition(string input, out Position position)
        {
            position = null;

            if (string.IsNullOrEmpty(input) || input.Length < 2)
                return false;

            char column = input[0];
            if (!char.IsLetter(column))
                return false;

            string rowString = input.Substring(1);
            if (!int.TryParse(rowString, out int row))
                return false;

            position = new Position(row - 1, char.ToUpper(column) - 'A');
            return true;
        }
        private void displayShips(Player player)
        {
            foreach(var ship in player.Ships)
            {
                ship.DisplayInfo();
            }
        }
        public void StartGameLoop()
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[green bold]ALL SHIPS HAVE BEEN PLACED![/]").RuleStyle("yellow").Centered());
            Thread.Sleep(3000);

            Console.Clear();
            AnsiConsole.Write(new Rule("[red bold]LET THE WAR BEGIN![/]").RuleStyle("yellow").Centered());
            Thread.Sleep(3000);
            while (true)
            {
                if (CheckIfLost(Game.player1))
                {
                    Console.WriteLine($"{Game.player2.Name} wins!");
                    break;
                }

                if (CheckIfLost(Game.player2))
                {
                    Console.WriteLine($"{Game.player1.Name} wins!");
                    break;
                }
                ShotDisplayp1();
            }
            
        }

        private bool CheckIfLost(Player player)
        {
            return player.Ships.All(ship => ship.GetBody().All(cell => cell.State is WasHit));
        }
        private void ShotDisplayp1()
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[blue bold]YOUR SEA...[/]").RuleStyle("yellow").Centered());
            Console.WriteLine();

            Game.CurrentBoard = Game.Board1;
            PrintBoardNoClear();
            Game.Board2.ToggleVisibility();

            Game.CurrentBoard = Game.Board2;
            AnsiConsole.Write(new Rule("[red bold]ENEMIES SEA...[/]").RuleStyle("blue").Centered());
            Console.WriteLine();
            PrintBoardNoClear();
            Game.Board2.ToggleVisibility();
            Console.ReadKey();
        }
    }
}
