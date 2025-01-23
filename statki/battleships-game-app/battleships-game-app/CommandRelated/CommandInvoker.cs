using System.Collections.Generic;

namespace battleships_game_app.CommandRelated
{
    internal class CommandInvoker
    {
        private readonly Stack<ICommand> _executedCommands; // Stos poleceń wykonanych
        private readonly Stack<ICommand> _undoneCommands;   // Stos poleceń cofniętych

        public CommandInvoker()
        {
            _executedCommands = new Stack<ICommand>();
            _undoneCommands = new Stack<ICommand>();
        }

        // Wykonaj polecenie i zapisz je na stosie
        public void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.Execute();             // Wykonanie polecenia
            _executedCommands.Push(command); // Dodanie polecenia do stosu wykonanych
            _undoneCommands.Clear();        // Czyść stos cofniętych poleceń (resetujemy historię cofnięć)
        }

        // Cofnij ostatnie polecenie
        public void UndoCommand()
        {
            if (_executedCommands.Count == 0)
                throw new InvalidOperationException("No commands to undo.");

            var commandToUndo = _executedCommands.Pop(); // Pobierz ostatnie wykonane polecenie
            commandToUndo.Undo();                        // Cofnij je
            _undoneCommands.Push(commandToUndo);         // Dodaj do stosu cofniętych poleceń
        }

        // Przywróć ostatnie cofnięte polecenie
        public void RedoCommand()
        {
            if (_undoneCommands.Count == 0)
                throw new InvalidOperationException("No commands to redo.");

            var commandToRedo = _undoneCommands.Pop();   // Pobierz ostatnie cofnięte polecenie
            commandToRedo.Execute();                    // Wykonaj je ponownie
            _executedCommands.Push(commandToRedo);      // Dodaj z powrotem do stosu wykonanych
        }
        public bool HasCommands()
        {
            return _executedCommands.Count > 0;
        }
    }
}
