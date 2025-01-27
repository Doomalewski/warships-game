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

            int boardWidth = 6;
            int boardHeight = 6;
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
        public void AddShipsForComputerGame(Player player1, Player player2)
        {
            Console.WriteLine($"Player {player1.Name}, set up your ships on board.");
            AddShipsForPlayer(player1, Game.Board1);

            AddShipsForComputer(player2, Game.Board2);
        }
        private void AddShipsForComputer(Player player,Board board)
        {
                Game.CurrentBoard = board;

                Random random = new Random();
                int[] shipLengths = { 2, 3, 3 }; // Długości statków do ustawienia

                foreach (int length in shipLengths)
                {
                    bool shipPlaced = false;

                    while (!shipPlaced)
                    {
                        // Generowanie losowej pozycji startowej
                        int startRow = random.Next(0, board.Height);
                        int startCol = random.Next(0, board.Width);

                        // Losowa orientacja statku: true = pozioma, false = pionowa
                        bool isHorizontal = random.Next(0, 2) == 0;

                        // Tworzenie statku
                        var startPosition = new Position(startRow, startCol);
                        var factory = new StandardShipFactory(length);
                        var ship = (StandardShip)factory.CreateWarship(board, startPosition);
                        RotateShip(ship);
                        RotateShip(ship);

                    // Ustawienie orientacji statku
                    if (!isHorizontal)
                        {
                            RotateShip(ship);
                        }

                        // Sprawdzanie możliwości ustawienia statku na planszy
                        if (CanPlaceShip(ship, board))
                        {
                            PlaceShipOnBoard(ship, board);
                            Console.WriteLine($"Placed ship of length {length} at ({startRow}, {startCol}) {(isHorizontal ? "horizontally" : "vertically")}.");
                            shipPlaced = true;
                            player.Ships.Add(ship);
                        }
                        else
                        {
                            // Zniszczenie statku, jeśli nie można go ustawić
                            ship.Destroy(board);
                        }
                    }
            }

        }
        private void AddShipsForPlayer(Player player, Board board)
        {
            Game.CurrentBoard = board; 
            while (true)
            {
                PrintBoard();
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
                RotateShip(ship);
                RotateShip(ship);
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
            Console.Clear();
            AnsiConsole.Write(new Rule("[red bold]LET THE WAR BEGIN![/]").RuleStyle("yellow").Centered());

            bool undoTriggered = false; // Flaga wskazująca, czy wykonano undo
            Player currentPlayer = Game.player1; // Rozpoczyna pierwszy gracz

            while (true)
            {
                // Wyświetl plansze i wykonaj strzał tylko, jeśli nie wykonano undo
                if (!undoTriggered)
                {
                    ShotDisplay(currentPlayer);
                }

                // Sprawdź, czy przeciwnik przegrał
                Player opponent = currentPlayer == Game.player1 ? Game.player2 : Game.player1;
                if (CheckIfLost(opponent))
                {
                    Console.WriteLine($"{currentPlayer.Name} wins!");
                    break;
                }

                Console.WriteLine("Press 'X' to undo your last move or any other key to end turn:");
                var keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.X)
                {
                    if (Game.GameHistory.HasCommands())
                    {
                        UndoLastMove(currentPlayer);
                        undoTriggered = true; // Zaznacz, że undo zostało wykonane
                    }
                    else
                    {
                        Console.WriteLine("\nNo moves to undo.");
                        undoTriggered = false; // Nie zmieniaj gracza
                    }
                }
                else
                {
                    undoTriggered = false; // Cofnięcie wyłączone, kontynuuj grę
                    currentPlayer = opponent; // Zmień gracza
                }
            }
        }

        private Position AskForPosition()
        {
            // Słownik mapujący litery na indeksy
            Dictionary<char, int> columnMapping = new Dictionary<char, int>();
            for (int i = 0; i < 20; i++)
            {
                columnMapping.Add((char)('A' + i), i);
            }

            while (true)
            {
                Console.WriteLine("Enter position (e.g., A1):");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input) || input.Length < 2)
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;
                }

                char column = char.ToUpper(input[0]); // Pobierz pierwszą literę i zamień na wielką literę
                string rowString = input.Substring(1); // Reszta wejścia to numer wiersza

                if (!columnMapping.ContainsKey(column))
                {
                    Console.WriteLine("Invalid column. Please use letters (A-J).");
                    continue;
                }

                if (!int.TryParse(rowString, out int row) || row < 1)
                {
                    Console.WriteLine("Invalid row. Please use numbers (1-10).");
                    continue;
                }

                int x = row - 1; // Indeks wiersza (0-based)
                int y = columnMapping[column]; // Pobierz indeks kolumny z mapowania

                if (x < 0 || x >= Game.CurrentBoard.Height) // Sprawdzenie, czy wiersz mieści się w granicach planszy
                {
                    Console.WriteLine($"Invalid row. Please use numbers (1-{Game.CurrentBoard.Height}).");
                    continue;
                }

                if (y < 0 || y >= Game.CurrentBoard.Width) // Sprawdzenie, czy kolumna mieści się w granicach planszy
                {
                    Console.WriteLine($"Invalid column. Please use letters within the range (A-{(char)('A' + Game.CurrentBoard.Width - 1)}).");
                    continue;
                }

                return new Position(x, y); // Zwracamy poprawną pozycję
            }
        }
        private bool CheckIfLost(Player player)
        {
            return player.Ships.All(ship => ship.GetBody().All(cell => cell.State is WasHit));
        }
        private void ShotDisplay(Player currentPlayer)
        {
            while (true)
            {
                Console.Clear();

                // Ustawienie plansz na podstawie bieżącego gracza
                Board playerBoard = currentPlayer == Game.player1 ? Game.Board1 : Game.Board2;
                Board enemyBoard = currentPlayer == Game.player1 ? Game.Board2 : Game.Board1;

                // Wyświetlenie planszy gracza
                AnsiConsole.Write(new Rule("[blue bold]YOUR SEA...[/]").RuleStyle("yellow").Centered());
                Console.WriteLine();
                Game.CurrentBoard = playerBoard;
                PrintBoardNoClear();

                // Wyświetlenie planszy przeciwnika
                enemyBoard.ToggleVisibility();
                AnsiConsole.Write(new Rule("[red bold]ENEMIES SEA...[/]").RuleStyle("blue").Centered());
                Console.WriteLine();
                Game.CurrentBoard = enemyBoard;
                PrintBoardNoClear();
                enemyBoard.ToggleVisibility();

                // Wykonanie ruchu
                Shoot(currentPlayer);
                break; // Wyjście z pętli po wykonaniu strzału
            }
        }


        private void Shoot(Player player)
        {
            Position desiredShotPosition = AskForPosition();

            // Ustawienie planszy przeciwnika
            Board enemyBoard = player == Game.player1 ? Game.Board2 : Game.Board1;

            try
            {
                // Tworzenie komendy FireCommand
                FireCommand fireCommand = new FireCommand(enemyBoard, desiredShotPosition, player);

                // Dodanie komendy do historii gry
                Game.GameHistory.ExecuteCommand(fireCommand);

                // Obsługa trafienia lub pudła
                if (fireCommand.WasHit())
                {
                    Console.WriteLine("You hit a ship!");
                }
                else
                {
                    Console.WriteLine("You missed.");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void HandleUndoMove(Player currentPlayer)
        {
            Console.WriteLine("Press 'X' to undo your last move or any other key to continue:");

            // Odczytaj klawisz od użytkownika
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.X)
            {
                UndoLastMove(currentPlayer);
            }
            else
            {
                Console.WriteLine("\nContinuing the game...");
            }
        }

        private void UndoLastMove(Player currentPlayer)
        {
            if (Game.GameHistory.HasCommands())
            {
                try
                {
                    // Cofnij ostatnią komendę
                    Game.GameHistory.UndoCommand();
                    Console.WriteLine("Last move has been undone.");

                    // Powtórz ruch tego samego gracza
                    ShotDisplay(currentPlayer);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Undo failed: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No moves to undo.");
            }
        }

        public void InitGameVsPc(Player p1, Player computer)
        {
                if (p1 == null)
                {
                    throw new ArgumentNullException("Both players must be provided.");
                }

                int boardWidth = 6;
                int boardHeight = 6;
                var board1 = new Board(boardWidth, boardHeight);
                var board2 = new Board(boardWidth, boardHeight);

                InitializeBoardCells(board1);
                InitializeBoardCells(board2);

                Game = new Game(p1, computer, board1, board2)
                {
                    GameHistory = new CommandInvoker(),
                    SavedStates = new Stack<BoardMemento>()
                };

                Console.WriteLine("Game initialized successfully.");
        }
        public void StartGameLoopVsPc()
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[green bold]ALL SHIPS HAVE BEEN PLACED![/]").RuleStyle("yellow").Centered());
            AnsiConsole.Write(new Rule("[red bold]LET THE WAR BEGIN![/]").RuleStyle("yellow").Centered());

            bool undoTriggered = false;
            Player humanPlayer = Game.player1;
            Player computerPlayer = Game.player2; // Gracz reprezentujący komputer
            Player currentPlayer = humanPlayer;

            Console.WriteLine("Choose difficulty level for the computer: (1) Easy, (2) Medium, (3) Hard");
            int difficulty;
            while (!int.TryParse(Console.ReadLine(), out difficulty) || difficulty < 1 || difficulty > 3)
            {
                Console.WriteLine("Invalid choice. Please enter 1, 2, or 3:");
            }

            while (true)
            {
                // Wyświetlenie planszy i wykonanie strzału, jeśli nie cofnięto ruchu
                if (!undoTriggered && currentPlayer == humanPlayer)
                {
                    ShotDisplay(humanPlayer);
                }

                Player opponent = currentPlayer == humanPlayer ? computerPlayer : humanPlayer;

                // Sprawdzenie, czy przeciwnik przegrał
                if (CheckIfLost(opponent))
                {
                    Console.WriteLine($"{currentPlayer.Name} wins!");
                    break;
                }

                // Kolejka komputera
                if (currentPlayer == computerPlayer)
                {
                    Console.WriteLine("\nComputer's turn...");
                    PerformComputerMove(computerPlayer, Game.Board1, difficulty);
                    currentPlayer = humanPlayer; // Powrót do gracza
                }
                else
                {
                    Console.WriteLine("Press 'X' to undo your last move or any other key to end turn:");
                    var keyInfo = Console.ReadKey(intercept: true);

                    if (keyInfo.Key == ConsoleKey.X)
                    {
                        if (Game.GameHistory.HasCommands())
                        {
                            UndoLastMove(humanPlayer);
                            undoTriggered = true;
                        }
                        else
                        {
                            Console.WriteLine("\nNo moves to undo.");
                            undoTriggered = false;
                        }
                    }
                    else
                    {
                        undoTriggered = false;
                        currentPlayer = computerPlayer;
                    }
                }
            }
        }

        private void PerformComputerMove(Player computer, Board opponentBoard, int difficulty)
        {
            Random random = new Random();
            Position shot;

            switch (difficulty)
            {
                case 1:
                    var CellToHit = opponentBoard.GetRandomCell();
                    DirectShoot(CellToHit.Position);
                    break;

                case 2:
                    var CellToHit2 = opponentBoard.GetCellWithBias30To70();
                    DirectShoot(CellToHit2.Position);
                    break;

                case 3:
                    var CellToHit3 = opponentBoard.GetCellWithBias50To50();
                    DirectShoot(CellToHit3.Position);
                    break;

                default:
                    throw new InvalidOperationException("Invalid difficulty level.");
            }
        }
        private void DirectShoot(Position targetPosition)
        {
            // Ustawienie planszy przeciwnika
            Board enemyBoard = Game.Board1;
            var CellToHit = Game.Board1.GetCell(targetPosition);
            CellToHit.Hit();
        }
    }
}
