using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobinsonM_C968_InventoryManagementSystem
{
    public abstract class Part
    {
        private int _partID;
        private string _name;
        private int _inventory;
        private decimal _price;
        private int _max;
        private int _min;
        public int PartID {
            get { return _partID; }  
            set { _partID = value; }
        }
        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        public decimal Price {
            get { return _price; }
            set { _price = value; }
        }
        public int InStock {
            get { return _inventory; }
            set { _inventory = value; }
        }
        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }
        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }
    }
}
