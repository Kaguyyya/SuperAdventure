using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Monster : LivingCreature
    {
        public int id { get; set; }
        public string name { get; set; }
        public int minimumDamage { get; set; }
        public int maximumDamage { get; set; }
        public int rewardExperiencePoints { get; set; }
        public int rewardGold { get; set; }
        public List<LootItem> lootTable { get; set; }
        

        public Monster(int ID, string Name, int MinimumDamage, int MaximumDamage, int RewardExperiencePoints, int RewardGold, int CurrentHitPoints, int MaximumHitPoints) : base(CurrentHitPoints, MaximumHitPoints)
        {
            id = ID;
            name = Name;
            maximumDamage = MaximumDamage;
            rewardExperiencePoints = RewardExperiencePoints;
            rewardGold = RewardGold;
            lootTable = new List<LootItem>() ;
            minimumDamage = MinimumDamage;
        }
    }
}
