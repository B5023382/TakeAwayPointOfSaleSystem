using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeAwayPointOfSaleSystem
{
    class gridSetting
    {
        private string table;
        private string procedureAdd;
        private string procedureDelete;

        public void setCategoryTable(string t)
        {
            table = t;
        }
        public void setProprety( string a, string d)
        {
            procedureAdd = a;
            procedureDelete = d;
        }

        public string getTable()
        {
            return table;
        }

        public string getProcedureName()
        {
            return procedureAdd;
        }

        public string getProcedureDelete()
        {
            return procedureDelete;
        }
    }
}
