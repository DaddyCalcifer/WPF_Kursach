using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace lab1.Model
{
    public class ListItem : ObservableCollection<Item>
    {
        public ListItem()
        {
            DbSet<Item> specifics = PageMain.DataEntitiesSKLAD.Item;
            var querySpec = from spec in specifics select spec;
            foreach (var specc in querySpec)
            {
                this.Add(specc);
            }
        }
    }
}
