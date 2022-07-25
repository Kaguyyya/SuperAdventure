using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Quest
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int rewardExperiencePoints { get; set; }
        public int rewardGold { get; set; }
        public Item rewardItem { get; set; }
        public List <QuestCompletionItem> questCompletionItem { get; set; }

        public Quest(int ID,string Name, string Description, int RewardExperience, int RewardGold)
        {
            id = ID;
            name = Name;
            description = Description;
            rewardExperiencePoints = RewardExperience;
            rewardGold = RewardGold;
            questCompletionItem = new List<QuestCompletionItem>();

        }


    }

}
