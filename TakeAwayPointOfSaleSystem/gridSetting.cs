using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeAwayPointOfSaleSystem
{
    class gridSetting
    {
        private string categorytable;
        private string procedureAdd;
        private string procedureDelete;
        private string mainTable;
        private string procedureGet;

        public void setTable(string t, string g)
        {
            mainTable = t;
            procedureGet = g;
        }
        public void setProprety(string t,  string a, string d)
        {
            categorytable = t;
            procedureAdd = a;
            procedureDelete = d;
        }

        public string getCategoryTable()
        {
            return categorytable;
        }

        public string getTable()
        {
            return mainTable;
        }

        public string getProcedureName()
        {
            return procedureAdd;
        }

        public string getProcedureDelete()
        {
            return procedureDelete;
        }

        public string getProcedureGet()
        {
            return procedureGet;
        }
    }
}
