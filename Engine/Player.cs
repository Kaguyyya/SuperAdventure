using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml;


namespace Engine
{
    public class Player : LivingCreature
    {
        public List<InventoryItem> inventory { get; set; }
        public List<PlayerQuest> quests { get; set; }
        private int _gold;
        public int gold 
        { 
            get { return _gold; }
            set
            {
                _gold = value;
                OnPropertyChanged("Gold");
            }
        }
        private int _experiencePoints;
        public int experiencePoints
        {
            get { return _experiencePoints; }
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged("ExperiencePoints");
                OnPropertyChanged("ExpLeft");
            }
        }
        public int expLeft
        {
            get { return ((amountExpToLvLUp - experiencePoints)); }
        }


        private int _level;
        public int level
        {
            get { return _level; }
            private set
            {
                _level = value;
                OnPropertyChanged("Level");
            }
        }
        public int amountExpToLvLUp { get; set; }
        private int _currentMp;
        public int currentMp
        {
            get { return _currentMp; }
            set
            {
                _currentMp = value;
                OnPropertyChanged("CurrentMp");
            }
        }
            
        public int maxMp { get; set; }
        public List <InventoryItem> boughtItem {get; set;}
        public List<Spells> spellsToLearn { get; set; }
        public List <Spells> spellsToUse { get; set; }
        public bool hasDied = false;
        public bool playerHasLVLUP = false;   
        public Location currentLocation { get; set; }
        public Weapon currentWeapon { get; set; }
        public Spells currentSpell { get; set; }



        public Player(int CurrentHitPoints, int MaximumHitPoints,int CurrentMp, int MaxMp, int Gold, int ExperiencePoints, int Level, int ExpToLvlUp) : base(CurrentHitPoints, MaximumHitPoints)
        {
            gold = Gold;
            experiencePoints = ExperiencePoints;
            level = Level;
            inventory = new List<InventoryItem>();
            quests = new List<PlayerQuest>();
            boughtItem = new List<InventoryItem>();
            currentMp = CurrentMp;
            maxMp = MaxMp;
            spellsToLearn = new List<Spells>();
            spellsToUse = new List<Spells>();
            amountExpToLvLUp = ExpToLvlUp;
        }   

        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(10, 10, 10, 10, 0, 0, 1,3);
            player.inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));
            player.currentLocation = World.LocationByID(World.LOCATION_ID_HOME);

            return player;
        }

        public static Player CreatePlayerFromXmlString(string xmlPlayerData)
        {
            try
            {
                XmlDocument playerData = new XmlDocument();
                playerData.LoadXml(xmlPlayerData);
                // BASE STATS
                int currentHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentHitPoints").InnerText);
                int maximumHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaximumHitPoints").InnerText);
                int currentMp = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentMp").InnerText);
                int maxMp = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaxMp").InnerText);
                int experiencePoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/ExperiencePoints").InnerText);
                int amountExpToLvlUp = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/AmountExpToLvlUp").InnerText);
                int lvl = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Level").InnerText);
                int gold = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Gold").InnerText);


                Player player = new Player(currentHitPoints, maximumHitPoints, currentMp, maxMp, gold, experiencePoints, lvl,amountExpToLvlUp);
                // OBJECTS
                int currentLocationId = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentLocation").InnerText);
                if(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon") != null)
                {
                    int currentWeaponId = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon").InnerText);
                    player.currentWeapon = (Weapon)World.ItemByID(currentWeaponId);
                }
                if(playerData.SelectSingleNode("/Player/Stats/CurrentSpell")!= null)
                {
                    int currentSpellId = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentSpell").InnerText);
                    player.currentSpell = World.SpellsById(currentSpellId);
                }
                player.currentLocation = World.LocationByID(currentLocationId);
                
                // loading the players inventory with all his items
                foreach(XmlNode node in playerData.SelectNodes("/Player/Inventory/InventoryItem"))
                {
                    // Creating the id and quantity variables and storing the xml datas in them to later use them in the for loop to
                    // add the items to the players inventory
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);

                    for(int i = 0; i<quantity; i++)
                    {
                        player.AddItemToInventory(World.ItemByID(id));
                    }
                }
                // loading the players quest list
                foreach(XmlNode node in playerData.SelectNodes("Player/PlayerQuest/Quest"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    bool isCompleted = Convert.ToBoolean(node.Attributes["IsCompleted"].Value);

                    PlayerQuest playerQuest = new PlayerQuest(World.QuestByID(id));
                    playerQuest.isCompleted = isCompleted;

                    player.quests.Add(playerQuest);
                }

                // loading the players spells

                foreach(XmlNode node in playerData.SelectNodes("Player/Spells/Spell"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);

                    Spells spell = World.SpellsById(id);
                    player.spellsToUse.Add(spell);
                }
                return player;       
            }
            catch
            {
                // if there was an error with the XML data, return a default player object
                return Player.CreateDefaultPlayer();
            }
        }
                
                    

        

        public bool HasItemRequiredToEnterThisLocation(Location location)
        {
            if (location.itemRequiredToEnter == null)
            {
                // there is no item required to enter return true

                return true;
            }
            // See if the player has the required item

            return inventory.Exists(ii => ii.details.id == location.itemRequiredToEnter.id);          
        }

        public bool HasThisQuest(Quest quest)
        {

            return quests.Exists(pq => pq.details.id == quest.id);
        }



        public bool CompletedthisQuest(Quest quest)

        {
            return quests.Exists(pq => pq.details.id == quest.id && pq.isCompleted);
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            // See if the player has all the items needed to complete the quest here
            foreach (QuestCompletionItem qci in quest.questCompletionItem)
            {
                if (!inventory.Exists(ii => ii.details.id == qci.details.id && ii.quantity >= qci.quantity))
                {

                    return false;
                    
                }
            }

            // If we got here, then the player must have all the required items, and enough of them, to complete the quest.
            return true;

        }
        

           public void RemoveQuestCompletionItems(Quest quest)

           { 

            foreach(QuestCompletionItem qci in quest.questCompletionItem)

            {
                InventoryItem item = inventory.SingleOrDefault(ii => ii.details.id == qci.details.id);
                
                    if(item != null)
                    {
                    //Subtract the quantity from the players inventory that was needed to complete the quest
                    item.quantity -= qci.quantity;
                   
                    }
                
            }
           }


        public void AddItemToInventory(Item itemToAdd)
        {

            InventoryItem item = inventory.SingleOrDefault(ii => ii.details.id == itemToAdd.id);

            if(item == null)
            {
                // they didnt have the item in their inventory so add it with a quantity of 1

                inventory.Add(new InventoryItem(itemToAdd, 1));
            }
            else
            {
                // they have it already so increase the quantity by 1
                item.quantity++;
            }
        }
            



        public void MarkQuestAsCompleted(Quest quest)

        {
            PlayerQuest playerQuest = quests.SingleOrDefault(pq => pq.details.id == quest.id);
            
            if (playerQuest != null)
            {
                playerQuest.isCompleted = true;
            }
        }

        public void AddExpierencePoints(int experiencePointsToAdd)
        {
            if(level >=60)

            { return; }

            experiencePoints += experiencePointsToAdd;
            

           while(experiencePoints>=amountExpToLvLUp)
            {                                            
                    // we lvled up
                    // LvL up the player , check for new spells to add  and set new amount to lvl up                  
                    level += 1;
                    AddSpells();
                experiencePoints = experiencePoints - amountExpToLvLUp;

               
                // set new lvl cap
                    amountExpToLvLUp *= 2;
                    maximumHitPoints += 5;
                    maxMp += 1;
                    playerHasLVLUP = true;
                                  
            }

            
        }
        public void SpellstoLearn(Player player)
        {
            foreach(Spells spell in World.spells)
            {
                player.spellsToLearn.Add(spell);
            }
        }


        public void AddSpells()
        {
            if (spellsToLearn.Count > 0)
            {
                List<Spells> spellsToAdd = new List<Spells>();
                List<Spells> spellsToDelete = new List<Spells>();
                bool learnedThatSpell = false;

                foreach (Spells spellsLearn in spellsToLearn)

                {
                    if (spellsToUse.Count > 0)
                    {
                        foreach (Spells spellsUse in spellsToUse)
                        {
                            if (spellsUse.name == spellsLearn.name)
                            {
                                learnedThatSpell = true;
                                // we already learned that spell get out
                                break;
                            }
                        }
                    }
                    if(learnedThatSpell)
                    {
                        spellsToDelete.Add(spellsLearn);
                        learnedThatSpell = false;
                    }
                    else if( spellsToUse.Count>0 && !learnedThatSpell && level >= spellsLearn.lvlRequiredtoLearnSpell)
                    {
                        spellsToAdd.Add(spellsLearn);
                    }
                    if (spellsToUse.Count == 0 && spellsToAdd.Count == 0 && level >= spellsLearn.lvlRequiredtoLearnSpell)
                    {
                        // the player didnt learn any skills so add his first skill
                        spellsToAdd.Add(spellsLearn);
                    }
                    if (spellsToAdd.Count > 0 )
                    {
                        foreach (Spells spellsAdd in spellsToAdd)
                        {
                            spellsToUse.Add(spellsAdd);
                            spellsToDelete.Add(spellsAdd);
                        }
                        spellsToAdd.Clear();
                    }

                }
                // Remove spells from player learn list
                foreach(Spells sp in spellsToDelete)
                {
                    spellsToLearn.Remove(sp);
                }
                

            }
        }

        public string ToXmlString()
        {

            XmlDocument playerData = new XmlDocument();

            // Create the Top level Xml node
            XmlNode player = playerData.CreateElement("Player");
            playerData.AppendChild(player);

            // Create the "Stats" child node to hold the players values
            XmlNode stats = playerData.CreateElement("Stats");
            player.AppendChild(stats);

            // Creating each player stat and saving them as child to stats node

            // defining and creating the subnote in the xml data
            XmlNode currentHitPoints = playerData.CreateElement("CurrentHitPoints");
            //giving the currenthitpoints the data from the player
            currentHitPoints.AppendChild(playerData.CreateTextNode(this.currentHitPoints.ToString()));
            // saving the P node as child to stats
            stats.AppendChild(currentHitPoints);

            XmlNode maximumHitPoints = playerData.CreateElement("MaximumHitPoints");
            maximumHitPoints.AppendChild(playerData.CreateTextNode(this.maximumHitPoints.ToString()));
            stats.AppendChild(maximumHitPoints);

            XmlNode currentMp = playerData.CreateElement("CurrentMp");
            currentMp.AppendChild(playerData.CreateTextNode(this.currentMp.ToString()));
            stats.AppendChild(currentMp);

            XmlNode maxMp = playerData.CreateElement("MaxMp");
            maxMp.AppendChild(playerData.CreateTextNode(this.maxMp.ToString()));
            stats.AppendChild(maxMp);

            XmlNode experiencePoints = playerData.CreateElement("ExperiencePoints");
            experiencePoints.AppendChild(playerData.CreateTextNode(this.experiencePoints.ToString()));
            stats.AppendChild(experiencePoints);

            XmlNode amountExpToLvlUp = playerData.CreateElement("AmountExpToLvlUp");
            amountExpToLvlUp.AppendChild(playerData.CreateTextNode(this.amountExpToLvLUp.ToString()));
            stats.AppendChild(amountExpToLvlUp);

            XmlNode lvl = playerData.CreateElement("Level");
            lvl.AppendChild(playerData.CreateTextNode(this.level.ToString()));
            stats.AppendChild(lvl);

            XmlNode gold = playerData.CreateElement("Gold");
            gold.AppendChild(playerData.CreateTextNode(this.gold.ToString()));
            stats.AppendChild(gold);

            XmlNode currentLocation = playerData.CreateElement("CurrentLocation");
            currentLocation.AppendChild(playerData.CreateTextNode(this.currentLocation.id.ToString()));
            stats.AppendChild(currentLocation);
            if (currentWeapon != null)
            {
                XmlNode currentWeapon = playerData.CreateElement("CurrentWeapon");
                currentWeapon.AppendChild(playerData.CreateTextNode(this.currentWeapon.id.ToString()));
                stats.AppendChild(currentWeapon);
            }
            if (currentSpell != null)
            {
                XmlNode currentSpell = playerData.CreateElement("CurrentSpell");
                currentSpell.AppendChild(playerData.CreateTextNode(this.currentSpell.id.ToString()));
                stats.AppendChild(currentSpell);
            }

            // Create the Inventory node that will hold each inventory item and save it as a sub node to the player node

            XmlNode inventory = playerData.CreateElement("Inventory");
            player.AppendChild(inventory);

            // Create an inventoryItem for each InventoryItem in the players inventory and save that it to the inventory as a childnode


            foreach (InventoryItem ii in this.inventory)
            {
                // Creating the inventory subnode
                XmlNode inventoryItem = playerData.CreateElement("InventoryItem");
                // giving it the id value and saving it to the inventoryItem attributes 
                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = ii.details.id.ToString();
                inventoryItem.Attributes.Append(idAttribute);
                // giving it the quantityAttribute and saving it to the inventoryItem attributes
                XmlAttribute quantityAttribute = playerData.CreateAttribute("Quantity");
                quantityAttribute.Value = ii.quantity.ToString();
                inventoryItem.Attributes.Append(quantityAttribute);
                // Saving the inventoryItem with its attributes to the inventory parent node
                inventory.AppendChild(inventoryItem);


            }

            // after the foreach the inventory should be converted to the xml

            // Creating the PlayerQuest Node that will hold each of  the Quests in the playerquest list

            XmlNode playerQuest = playerData.CreateElement("PlayerQuest");
            player.AppendChild(playerQuest);

            foreach (PlayerQuest pq in this.quests)
            {
                // creating the quest
                XmlNode quest = playerData.CreateElement("Quest");

                // adding the quest object id and saving it to the quest node
                XmlAttribute idAtt = playerData.CreateAttribute("ID");
                idAtt.Value = pq.details.id.ToString();
                quest.Attributes.Append(idAtt);

                XmlAttribute isCompletedAtt = playerData.CreateAttribute("IsCompleted");
                isCompletedAtt.Value = pq.isCompleted.ToString();
                quest.Attributes.Append(isCompletedAtt);
                // saving the populated quest to the PlayerQuest as subnode
                playerQuest.AppendChild(quest);

            }

            XmlNode spells = playerData.CreateElement("Spells");
            player.AppendChild(spells);

            foreach(Spells sp in this.spellsToUse)
            {

                XmlNode spell = playerData.CreateElement("Spell");

                XmlAttribute idAtt = playerData.CreateAttribute("ID");
                idAtt.Value = sp.id.ToString();
                spell.Attributes.Append(idAtt);

                spells.AppendChild(spell);
            }
            return playerData.InnerXml; // The XML document , as a string  so we can save the data to a disk
        }

    }
}
