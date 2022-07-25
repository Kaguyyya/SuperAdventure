using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    class Armor : Item
    {
        public string armorType { get; set; }
        public int armor { get; set; }

        public Armor(int ID, string Name, string NamePlural,string ArmorType,int Armor, int SellingValue) :base(ID,Name,NamePlural,SellingValue)
        {
            armorType = ArmorType;
            armor = Armor;
        }

    }
}
