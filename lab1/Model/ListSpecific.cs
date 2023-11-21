using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace lab1.Model
{
    public class ListSpecific : ObservableCollection<Specific>
    {
        public ListSpecific()
        {
            DbSet<Specific> specifics = PageMain.DataEntitiesSKLAD.Specific;
            var querySpec = from spec in specifics select spec;
            foreach (Specific specc in querySpec)
            {
                this.Add(specc);
            }
        }
    }
}
