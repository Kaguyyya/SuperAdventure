using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
   public class QuestCompletionItem
    {
        public Item details { get; set; }
        public int quantity { get; set; }

        public QuestCompletionItem(Item Details, int Quantity)
        {
            details = Details;
            quantity = Quantity;
        }

    }
}
