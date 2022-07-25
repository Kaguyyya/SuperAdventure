using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class InventoryItem
    {
        public Item details { get; set; }
        public int quantity { get; set; }
        

        public InventoryItem(Item Details, int Quantity)
        {
            details = Details;
            quantity = Quantity;
         


        }
    }
}
