using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeAwayPointOfSaleSystem
{
    public class dish
    {
        private int id;
        private string Name;
        private string otherName;
        private string price;

        public dish(int i, string n, string o, string price)
        {
            id = i;
            Name = n;
            otherName = o;
        }

        public int getId()
        {
            return id;
        }

        public string getName()
        {
            return Name;
        }

        public string getOtherName()
        {
            return otherName;
        }

        public string getPrice()
        {
            return price;
        }
    }
}
