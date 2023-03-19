using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobinsonM_C968_InventoryManagementSystem
{
    public class Inventory
    {
        public Inventory()
        {
            AllParts = new BindingList<Part>();
            Products = new BindingList<Product>();
        }
        public BindingList<Product> Products { get; set; }
        public BindingList<Part> AllParts { get; set; }

        public void addProduct(Product product)
        {
            int productIDIncrement = Products.Count() + 1;
            product.ProductID = productIDIncrement;
            Products.Add(product);
        }
        public bool removeProduct(int id)
        {
            var product = Products.Where(p => p.ProductID == id).FirstOrDefault();
            return Products.Remove(product);
        }
        public Product lookupProduct(int id)
        {
            return Products.Where(p=>p.ProductID == id).FirstOrDefault();
        }
        public void updateProduct(int id, Product product)
        {
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].ProductID == id)
                   Products[i] = product;
            }
        }
        public void addPart(Part part)
        {
            AllParts.Add(part);
        }

        public bool deletePart(Part part)
        {
            return AllParts.Remove(part);
        }

        public Part lookupPart(int id)
        {
            return AllParts.Where(part=>part.PartID == id).FirstOrDefault();
        }

        public void updatePart(int id, Part _part)
        {
            for (int i = 0; i < AllParts.Count; i++)
            {
                if (AllParts[i].PartID == id)
                    AllParts[i] = _part;
            }
        }

        public void LoadSampleData()
        {
            var products = new BindingList<Product>();
            products.Add(new Product
            {
                Name = "Wheel",
                InStock = 5,
                Max = 10,
                Min = 1,
                Price = Convert.ToDecimal("50.00"),
                ProductID = 1
            });

            var parts = new BindingList<Part>();
            parts.Add(new InHouse {
                Name = "Wheel",
                InStock = 5,
                Max = 10,
                Min = 1,
                Price = Convert.ToDecimal("50.00"),
                MachineID = 1,
                PartID = 1
            });
            parts.Add(new Outsourced
            {
                Name = "Seat",
                InStock = 5,
                Max = 10,
                Min = 1,
                Price = Convert.ToDecimal("20.00"),
                CompanyName = "Toyota",
                PartID = 2
            });

            Products = products;
            AllParts = parts;
        }
    }
}
