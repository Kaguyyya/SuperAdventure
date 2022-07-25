using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class LootItem
    {
        public Item details { get; set; }
        public int dropPercentage { get; set; }
        public bool isDefaultItem { get; set; }

        public LootItem(Item Details , int DropPercentage, bool IsDefaultItem)
        {
            details = Details;
            dropPercentage = DropPercentage;
            isDefaultItem = IsDefaultItem;
        }

    }
}
