using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class HealingPotion : Item
    {
      
        public int amountToHeal { get; set; }

        public HealingPotion(int ID, string Name, string NamePlural, int AmountToHeal,int SellingValue) : base(ID, Name, NamePlural,SellingValue)
        {
            amountToHeal = AmountToHeal;
        }
       

    }
}
