using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Data.Entity;
using lab1.Model;

namespace lab1.Logic
{
    public class WorkerManager
    {
        public static SKLAD_WPF DataEntitiesSKLAD { get; set; } = new SKLAD_WPF();
        PageMain pm;
        public WorkerManager(PageMain pm)
        {
            this.pm = pm;
            //DataEntitiesSKLAD = new SKLAD_WPF();
        }

        public static int ActSum(LoadAct act)
        {
            int sum = 0;
            var queryActs = (from ite in DataEntitiesSKLAD.Item
                             orderby ite.ID_Item
                             where ite.ID_Act == act.ID_Pocket
                             select ite).ToList();
            foreach (Item itt in queryActs)
            {
                sum += itt.Price;
            }
            return sum;
        }
        public static int ActCount(LoadAct act)
        {
            var queryActs = (from ite in DataEntitiesSKLAD.Item
                             orderby ite.ID_Item
                             where ite.ID_Act == act.ID_Pocket
                             select ite).ToList();
            return queryActs.Count();
        }
    }
}
