using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace lab1.Commands
{
    public class DataCommands
    {
        public static RoutedCommand Delete { get; set; }
        public static RoutedCommand Edit { get; set;}

        public static RoutedCommand Undo { get; set; }
        public static RoutedCommand Find { get; set; }
        public static RoutedCommand Add { get; set; }
        static DataCommands()
        {
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+E"));
            Edit = new RoutedCommand("Edit", typeof(DataCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.D, ModifierKeys.Control, "Ctrl+D"));
            Delete = new RoutedCommand("Delete", typeof(DataCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.Z, ModifierKeys.Control, "Ctrl+Z"));
            Undo = new RoutedCommand("Undo", typeof(DataCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F, ModifierKeys.Control, "Ctrl+F"));
            Find = new RoutedCommand("Find", typeof(DataCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.N, ModifierKeys.Control, "Ctrl+N"));
            Add = new RoutedCommand("New", typeof(DataCommands), inputs);
        }
    }
}
