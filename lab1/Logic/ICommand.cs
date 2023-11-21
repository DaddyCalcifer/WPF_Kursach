using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Logic
{
    internal interface ICommand1
    {
        void Execute(object par);
        bool CanExecute(object par);
        event EventHandler CanExecuteChanged;
    }
}
