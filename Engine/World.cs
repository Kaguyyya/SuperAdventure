using System;
using System.Collections.Generic;
using System.Text;


namespace Engine
{
    public static class World

    {
        public static readonly List<Item> items = new List<Item>();
        public static readonly List<Monster> monsters = new List<Monster>();
        public static readonly List<Quest> quests = new List<Quest>();
        public static readonly List<Location> locations = new List<Location>();
        public static readonly List<NPC> npcs = new List<NPC>();
        public static readonly List<Spells> spells = new List<Spells>();
        // ITEM IDS
        public const int ITEM_ID_RUSTY_SWORD = 1;
        public const int ITEM_ID_RAT_TAIL = 2;
        public const int ITEM_ID_PIECE_OF_FUR = 3;
        public const int ITEM_ID_SNAKE_FANG = 4;
        public const int ITEM_ID_SNAKESKIN = 5;
        public const int ITEM_ID_MACE = 6;
        public const int ITEM_ID_HEALING_POTION = 7;
        public const int ITEM_ID_SPIDER_FANG = 8;
        public const int ITEM_ID_SPIDER_SILK = 9;
        public const int ITEM_ID_ADVENTURER_PASS = 10;
        public const int ITEM_ID_IRONSWORD = 11;
        public const int ITEM_ID_CLOTH_SHOES = 12;
        public const int ITEM_ID_CLOTH_BODY = 13;
        public const int ITEM_ID_CLOTH_LEGS = 14;
        public const int ITEM_ID_IRON_SHOES = 15;
        public const int ITEM_ID_IRON_BODY = 16;
        public const int ITEM_ID_IRON_HANDS = 17;
        public const int ITEM_ID_IRON_LEGS = 18;
        public const int ITEM_ID_GREATER_HEALING_POTION = 19;
        // MONSTERS IDS
        public const int MONSTER_ID_RAT = 1;
        public const int MONSTER_ID_SNAKE = 2;
        public const int MONSTER_ID_GIANT_SPIDER = 3;
        // QUEST IDS
        public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;
        // LOCATION IDS
        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMIST_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;
        // NPCS IDS
        public const int LOCATION_ID_SHOPKEEPER_TOWN = 1;
        // Spell IDS
        public const int SPELL_ID_HEAL = 1;
        public const int SPELL_ID_GREATERHEAL = 2;
        public const int SPELL_ID_WILD_SLASH = 3;
        public const int SPELL_ID_ROBUST_STRIKE = 4;


        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateNpcs();
            PopulateLocation();
            PopulateSpells();
            

        }
        private static void PopulateSpells()
        {
            spells.Add(new Spells(SPELL_ID_HEAL,"Heal",15,2,1,true,false));
            spells.Add(new Spells(SPELL_ID_GREATERHEAL, "Greater Heal", 50, 5, 2, true, false));
            spells.Add(new Spells(SPELL_ID_WILD_SLASH, "Wild Slash", 10, 5, 2, false, true));
            spells.Add(new Spells(SPELL_ID_ROBUST_STRIKE, "Robust Strike", 5, 5, 1, false, true));
        }

        private static void PopulateItems()
        {
            items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "Rusty Sword", "Rusty Swords", 1, 2,20));
            items.Add(new Item(ITEM_ID_RAT_TAIL, "Rat tail", "Rat tails",3));
            items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Piece of fur", "Pieces of fur",3));
            items.Add(new Item(ITEM_ID_SNAKE_FANG, "Snake fang", "Snake fangs",3));
            items.Add(new Item(ITEM_ID_SNAKESKIN, "Snake Skin", "Snakeskins",3));
            items.Add(new Weapon(ITEM_ID_MACE, "Mace", "Maces", 3, 4,300));
            items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "Healing Potion ", "Healing Potion", 10,30));
            items.Add(new HealingPotion(ITEM_ID_GREATER_HEALING_POTION, "Greater Healing Potion ", "Greater Healing Potion ", 20, 60));
            items.Add(new Item(ITEM_ID_SPIDER_SILK, "Spider Silk", "Spider silks",3));
            items.Add(new Item(ITEM_ID_SPIDER_FANG, "Spider fang", "Spider fangs",3));
            items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Adventurer pass", "Adventurer passes",20));
            items.Add(new Weapon(ITEM_ID_IRONSWORD, "Iron Sword", "Iron Sword", 6, 10, 1000));
            items.Add(new Armor(ITEM_ID_CLOTH_BODY, "Cloth Armor", "Cloth Armor", "Cloth", 2, 100));
            items.Add(new Armor(ITEM_ID_CLOTH_SHOES, "Cloth Shoes", "Cloth Shoes", "Cloth", 1, 50));
            items.Add(new Armor(ITEM_ID_CLOTH_LEGS, "Cloth Legs", "Cloth Legs", "Cloth", 1, 80));
            items.Add(new Armor(ITEM_ID_IRON_BODY, "Iron Body", "Iron Body", "Iron", 5,500));
            items.Add(new Armor(ITEM_ID_IRON_SHOES, "Iron Shoes", "Iron Shoes", "Iron", 3, 250));
            items.Add(new Armor(ITEM_ID_IRON_LEGS, "Iron Legs", "Iron Legs", "Iron", 3, 400));

        }

        private static void PopulateNpcs()
        {

            Shopkeeper trendis = new Shopkeeper("Trendis", LOCATION_ID_SHOPKEEPER_TOWN, LocationByID(LOCATION_ID_TOWN_SQUARE));
            trendis.SellingItems.Add(new InventoryItem(ItemByID(ITEM_ID_MACE),1));
            trendis.SellingItems.Add(new InventoryItem(ItemByID(ITEM_ID_IRONSWORD), 1));
            trendis.SellingItems.Add(new InventoryItem(ItemByID(ITEM_ID_CLOTH_BODY), 1));
            trendis.SellingItems.Add(new InventoryItem(ItemByID(ITEM_ID_CLOTH_LEGS), 1));
            trendis.SellingItems.Add(new InventoryItem(ItemByID(ITEM_ID_CLOTH_SHOES), 1));
            trendis.SellingItems.Add(new InventoryItem(ItemByID(ITEM_ID_IRON_BODY), 1));
            trendis.SellingItems.Add(new InventoryItem(ItemByID(ITEM_ID_IRON_LEGS), 1));
            trendis.SellingItems.Add(new InventoryItem(ItemByID(ITEM_ID_IRON_SHOES), 1));
            trendis.SellingItems.Add(new InventoryItem(ItemByID(ITEM_ID_HEALING_POTION), 1));
            trendis.SellingItems.Add(new InventoryItem(ItemByID(ITEM_ID_GREATER_HEALING_POTION), 1));

            npcs.Add(trendis);
           
        }

         private static void PopulateMonsters()
        {
            Monster rat = new Monster(MONSTER_ID_RAT, "Rat",4, 8, 3, 10, 10, 10);
            rat.lootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false));
            rat.lootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 25, false));

            Monster snake = new Monster(MONSTER_ID_SNAKE, "Snake",2, 4, 3, 5, 5, 5);
            snake.lootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 75, true));
            snake.lootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKESKIN), 25, false));

            Monster giantSpider = new Monster(MONSTER_ID_GIANT_SPIDER, "Giant spider",10, 20, 5, 40, 10, 10);
            giantSpider.lootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 75, true));
            giantSpider.lootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 25, false));
            
            
            monsters.Add(rat);
            monsters.Add(snake);
            monsters.Add(giantSpider);
        
      
        }


        private static void PopulateQuests()
        {
            Quest clearAlchemistGarden =
                new Quest(
                    QUEST_ID_CLEAR_ALCHEMIST_GARDEN,
                    "Clear the alchemist´s garden",
                    "Kill rats in the aclhemist´s garden and bring back 3 rat tails. You will receive a healing potion and 10 gold pieces.", 20, 10);
            clearAlchemistGarden.questCompletionItem.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 3));
            clearAlchemistGarden.rewardItem = ItemByID(ITEM_ID_HEALING_POTION);

            Quest clearFarmersField =
                new Quest(
                    QUEST_ID_CLEAR_FARMERS_FIELD,
                    "Clear the farmer´s field",
                    "Kill snakes in the farmer´s field and bring back 3 snake fangs- You will receive and adventurer´s pass and 20 gold pieces", 20, 20);

            clearFarmersField.questCompletionItem.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SNAKE_FANG), 3));
            clearFarmersField.rewardItem = ItemByID(ITEM_ID_ADVENTURER_PASS);

            quests.Add(clearAlchemistGarden);
            quests.Add(clearFarmersField);
        }

        private static void PopulateLocation()
        {
            Location home = new Location(LOCATION_ID_HOME, "Home", "Your house. You really need to clean up the place.");
            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Town sqaure", "you see a fountain.");
            townSquare.hasShopKeeper = ShopKeeperById(LOCATION_ID_SHOPKEEPER_TOWN);
            Location alchemistsHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Alchemist´s hut", "There are many strange plants on the shelves ");
            alchemistsHut.questAvailableHere = QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);

            Location alchemistsGarden = new Location(LOCATION_ID_ALCHEMIST_GARDEN, "Alchemist´s garden", "Many plants are growing here");
            alchemistsGarden.monsterLivingHere = MonsterByID(MONSTER_ID_RAT);

            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Farmhouse", "there is a small farmhouse, with a farmer in front. ");
            farmhouse.questAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);

            Location farmersField = new Location(LOCATION_ID_FARM_FIELD, "Farmer´s field", "You see rows of vegetables growing here");
            farmersField.monsterLivingHere = MonsterByID(MONSTER_ID_SNAKE);
            

            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Guard post", "there is a large, tough-looking guard here", ItemByID(ITEM_ID_ADVENTURER_PASS));
            Location bridge = new Location(LOCATION_ID_BRIDGE, "Bridge", "A stone bridge crosses a wild river.");
            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Forest", "you see spider webs covering the trees in this forest.");
            spiderField.monsterLivingHere = MonsterByID(MONSTER_ID_GIANT_SPIDER);

            home.locationToNorth = townSquare;

            townSquare.locationToNorth = alchemistsHut;
            townSquare.locationToSouth = home;
            townSquare.locationToEast = guardPost;
            townSquare.locationToWest = farmhouse;

            farmhouse.locationToEast = townSquare;
            farmhouse.locationToWest = farmersField;
            farmersField.locationToEast = farmhouse;

            alchemistsHut.locationToSouth = townSquare;
            alchemistsHut.locationToNorth = alchemistsGarden;
            alchemistsGarden.locationToSouth = alchemistsHut;

            guardPost.locationToEast = bridge;
            guardPost.locationToWest = townSquare;

            bridge.locationToWest = guardPost;
            bridge.locationToEast = spiderField;

            spiderField.locationToWest = bridge;

            locations.Add(home);
            locations.Add(townSquare);
            locations.Add(guardPost);
            locations.Add(alchemistsHut);
            locations.Add(alchemistsGarden);
            locations.Add(farmhouse);
            locations.Add(farmersField);
            locations.Add(bridge);
            locations.Add(spiderField);

        }


       
        
        public static Item ItemByID(int id)
        {
            foreach (Item item in items)
            {
                if(item.id == id)
                {
                    return item;
                }
            }
            return null;
        }

        public static Monster MonsterByID(int id)
        {
            foreach (Monster monster in monsters)
            {
                if(monster.id == id)
                {
                    return monster;
                }
            }
            return null;
        }

        public static Quest QuestByID(int id)
        {
            foreach (Quest quest in quests)
            {
                if (quest.id == id)
                {
                    return quest;
                }
            }
            return null;
        }

        public static Location LocationByID(int id)
        {
            foreach (Location location in locations)
            {
                if(location.id == id)
                {
                    return location;
                }
            }
            return null;
        }

        public static Shopkeeper ShopKeeperById(int id)
        {
            foreach (Shopkeeper npc in npcs)
            {
                if(npc.id == id)
                {
                    return npc;
                }
            }
            return null;
        }

        public static Spells SpellsById(int id)
        {
            foreach(Spells spell in spells)
            {
                if(spell.id == id)
                {
                    return spell;
                }
            }
            return null;
        }
      
        

    }
}
