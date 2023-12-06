using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace lab1.Model
{
    public class ListRole : ObservableCollection<UserType>
    {
        public ListRole()
        {
            DbSet<UserType> specifics = lab1.Logic.WorkerManager.DataEntitiesSKLAD.UserType;
            var querySpec = from spec in specifics select spec;
            foreach (var specc in querySpec)
            {
                this.Add(specc);
            }
        }
    }
}
