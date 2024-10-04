using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingredients.Object
{
    public class ItemInventory
    {
        private string _ListID;
        private string _Name;
        private string _FullName;

        public string ListID { get => _ListID; set => _ListID = value; }
        public string Name { get => _Name; set => _Name = value; }
        public string FullName { get => _FullName; set => _FullName = value; }
    }
}
