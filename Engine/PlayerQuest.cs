using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class PlayerQuest
    {
        public Quest details { get; set; }
        public bool isCompleted { get; set; }

        public PlayerQuest(Quest Details )
        {
            details = Details;
            isCompleted = false;
        }
    }
}
