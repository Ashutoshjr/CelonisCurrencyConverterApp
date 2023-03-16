using System;
using System.Windows.Input;

namespace CurrencyConverter
{
    internal class ActionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action action;

        public ActionCommand(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return action != null;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}