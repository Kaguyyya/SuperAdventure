using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
   public class Shopkeeper : NPC
    {           
        public Location LocationOfShopkeeper { get; set; }
        public List<InventoryItem> SellingItems { get; set; }
        




        public Shopkeeper(string Name, int Id ,Location LocationOfShopKeeper) : base(Name,Id)
        {

            name = Name;
            LocationOfShopkeeper = LocationOfShopKeeper;
            SellingItems = new List<InventoryItem>();
            
        }

        
    }
}
