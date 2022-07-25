using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Spells
    {
        public int manaCost { get; set; }
        public int lvlRequiredtoLearnSpell { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public bool isHealingSpell { get; set; }
        public bool isDamageSpell { get; set; }
        public int amountToHealOrDamage { get; set; }


        public Spells(int Id, string Name, int AmountToHealOrDamage, int LvlRequiredtoLearnSpell, int ManaCost, bool IsHealingSpell, bool IsDamageSpell)
        {
            manaCost = ManaCost;
            lvlRequiredtoLearnSpell = LvlRequiredtoLearnSpell;
            name = Name;
            id = Id;
            isHealingSpell = IsHealingSpell;
            isDamageSpell = IsDamageSpell;
            amountToHealOrDamage = AmountToHealOrDamage;
        }




        public void UseSpell(Spells spell, Player player,Monster monster)
        {
            if(spell.isDamageSpell)
            {
                player.currentHitPoints += spell.amountToHealOrDamage;
            }
            else if (spell.isHealingSpell)
            {
                monster.currentHitPoints -= spell.amountToHealOrDamage;
            }
        }
    }
        
    
        
    
}
