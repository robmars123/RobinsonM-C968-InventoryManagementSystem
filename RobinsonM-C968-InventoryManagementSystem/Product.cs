using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobinsonM_C968_InventoryManagementSystem
{
    public class Product
    {
        public static int globalProductID;
        public Product()
        {
            //this.ProductID = Interlocked.Increment(ref globalProductID);
            AssociatedParts = new BindingList<Part>();
        }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public BindingList<Part> AssociatedParts { get; set; }
        public void addAssociatedPart(Part part)
        {
            AssociatedParts.Add(part);
        }
        public bool removeAssociatedPart(int id)
        {
            return AssociatedParts.Remove(lookupAssociatedPart(id));
        }
        public Part lookupAssociatedPart(int id)
        {
            return AssociatedParts.Where(part => part.PartID == id).FirstOrDefault();
        }
    }
}
