using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
   public class Weapon : Item
    {
      
        public int minimumDamage { get; set; }
        public int maximumDamage { get; set; }

        public Weapon(int ID, string Name, string NamePlural, int MinimumDamage, int MaximumDamage,int SellingValue) : base(ID, Name, NamePlural,SellingValue)
        {
            minimumDamage = MinimumDamage;
            maximumDamage = MaximumDamage;
        }
    }
}
