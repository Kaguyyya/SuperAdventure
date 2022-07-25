using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
   public class NPC
    {
        public string name { get; set; }
        public int id { get; set; }

        public NPC(string Name, int Id)
        {
            name = Name;
            id = Id;
        }
    }
}
