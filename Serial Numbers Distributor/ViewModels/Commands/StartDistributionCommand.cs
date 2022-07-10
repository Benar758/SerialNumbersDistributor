using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Serial_Numbers_Distributor.ViewModels.Commands
{
    internal class StartDistributionCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action _execute;

        public StartDistributionCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute.Invoke();
        }
    }
}
