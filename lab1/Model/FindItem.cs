using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Model
{
    public class FindItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int count_from, count_to;
        public int price_from, price_to;
        public int unit, specific, act_id;
    }
}
