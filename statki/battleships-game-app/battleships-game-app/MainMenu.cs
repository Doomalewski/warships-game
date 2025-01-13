using battleships_game_app.GameManagerRelated;
using battleships_game_app.GameRelated;
using Spectre.Console;
using System;
using System.Drawing;

namespace battleships_game_app
{
    internal class MainMenu
    {
        private GameManager _gameManager = new GameManager();
        public void PrintMainMenu()
        {
            while (true)
            {
                Console.Clear();

                // Wyświetlanie nagłówka
                AnsiConsole.Write(
                    new FigletText("WARSHIPS")
                        .Centered()
                        .Color(Spectre.Console.Color.Red));

                // Wyświetlanie menu
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold yellow]Choose an option:[/]")
                        .PageSize(5)
                        .AddChoices(new[]
                        {
                            "Start New Game",
                            "Load Game",
                            "Add New Player",
                            "Exit"
                        }));

                // Obsługa wyboru użytkownika
                switch (choice)
                {
                    case "Start New Game":
                        StartNewGame();
                        break;
                    case "Load Game":
                        LoadGame();
                        break;
                    case "Add New Player":
                        AddNewPlayer();
                        break;
                    case "Exit":
                        Exit();
                        return; // Kończy pętlę i zamyka program
                }
            }
        }

        private void StartNewGame()
        {
            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Choose game type:[/]")
                    .PageSize(5)
                    .AddChoices(new[]
                    {
                        "Player vs Player",
                        "Player vs AI",
                        "Back"
                    }));

            switch (opt)
            {
                case "Player vs Player":
                    Console.WriteLine("Enter Player 1 name:");
                    string p1Name = Console.ReadLine();
                    Console.WriteLine("Enter Player 2 name:");
                    string p2Name = Console.ReadLine();

                    Player p1 = new Player(p1Name, false);
                    Player p2 = new Player(p2Name, false);

                    _gameManager.InitStandardGame(p1, p2);

                    Console.WriteLine($"{p1.Name}, place your ships.");
                    _gameManager.AddShips(p1);
                    Console.Clear();

                    Console.WriteLine($"{p2.Name}, place your ships.");
                    _gameManager.AddShips(p2);
                    Console.Clear();

                    Console.WriteLine("Both players have placed their ships. Here is the initial board:");
                    _gameManager.PrintBoard();

                    Console.ReadLine();
                    break;


                case "Player vs AI":
                    Console.WriteLine("Enter your name:");
                    string playerName = Console.ReadLine();

                    Player player = new Player(playerName, false);
                    //_gameManager.InitAIGame(player);
                    _gameManager.PrintBoard();
                    break;

                case "Back":
                    PrintMainMenu();
                    break;
            }
        }

        private void LoadGame()
        {
            AnsiConsole.MarkupLine("[yellow]Loading a saved game...[/]");
            // Dodaj logikę wczytywania gry z bazy danych
            WaitForKeyPress();
        }

        private void AddNewPlayer()
        {
            AnsiConsole.MarkupLine("[blue]Adding a new player...[/]");
            
            Console.Clear();
            Console.WriteLine("Write your nickname:");
            string name = Console.ReadLine();
            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[bold yellow]Would you like to play against AI or player?:[/]")
                .PageSize(5)
                .AddChoices(new[]
                {
                    "player",
                    "AI"
                }));
            bool choosenOpt;
            if (opt == "player")
                choosenOpt = false;
            else choosenOpt = true;
            Player player = new Player(name, choosenOpt);

            //logika dodania gracza do bazy danych

            AnsiConsole.MarkupLine("[green] Player saved!");
            PrintMainMenu();
        }

        private void Exit()
        {
            AnsiConsole.MarkupLine("[red]Exiting the game. Goodbye![/]");
            WaitForKeyPress();
        }

        private void WaitForKeyPress()
        {
            AnsiConsole.MarkupLine("[grey]Press any key to return to the menu...[/]");
            Console.ReadKey(true);
        }
    }
}
