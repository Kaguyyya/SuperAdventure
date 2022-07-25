using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using System.IO;

namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private Player _player;
        private Monster _currentMonster;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";

      

        public SuperAdventure()
        {
            
            InitializeComponent();                        
           // Generating player
         if(File.Exists(PLAYER_DATA_FILE_NAME))
            {
                _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
            }
            else
            {
                _player = Player.CreateDefaultPlayer();
            }

            MoveTo(_player.currentLocation);
            _player.SpellstoLearn(_player);            
            UpdateUI();
            // Add Data Binding to UI objects
            _player.AddItemToInventory(World.ItemByID(World.ITEM_ID_IRONSWORD));
            lblHitPoints.DataBindings.Add("Text", _player, "CurrentHitPoints");
            lblMp.DataBindings.Add("Text", _player, "CurrentMp");
            lblExperience.DataBindings.Add("Text", _player, "ExperiencePoints");
            lblAmountEXPToLvLUP.DataBindings.Add("Text", _player, "ExpLeft");
            lblLevel.DataBindings.Add("Text", _player, "Level");
            lblGold.DataBindings.Add("Text", _player, "Gold");
            
            cboPotion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboWeapons.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSpells.DropDownStyle = ComboBoxStyle.DropDownList;          
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.currentLocation.locationToNorth);
            
            
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.currentLocation.locationToEast);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.currentLocation.locationToSouth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.currentLocation.locationToWest);
        }

        private void MoveTo(Location newLocation)
        {
            // has the player the required item to enter?

            if (!_player.HasItemRequiredToEnterThisLocation(newLocation))

            {
                rtbMessages.Text += "You must have a " + newLocation.itemRequiredToEnter.name + " to enter this Location." + Environment.NewLine;
               return;
             }
            if (InCombat() && _player.hasDied == false)
            {
                rtbMessages.Text += "You cant move, you are currently fighting " + _currentMonster.name + Environment.NewLine;
                return;
            }
            // Update the player's current location
            _player.currentLocation = newLocation;
            
            // Show/hide available movement buttons
            btnNorth.Visible = (newLocation.locationToNorth != null);
            btnEast.Visible = (newLocation.locationToEast != null);
            btnSouth.Visible = (newLocation.locationToSouth != null);
            btnWest.Visible = (newLocation.locationToWest != null);
            btnShop.Visible = (newLocation.hasShopKeeper != null);
            // The player died
            if(_currentMonster != null && _player.hasDied)
            {
                
                _currentMonster = null;               
                _player.hasDied = false;
            }
            InCombat();
            // Display current location name and description
            rtbLocation.Text = newLocation.name + Environment.NewLine;
            rtbLocation.Text += newLocation.description + Environment.NewLine;
            // If the location is home completly heal the player
            if(newLocation.id == World.LOCATION_ID_HOME)
            {
                _player.currentHitPoints = _player.maximumHitPoints;
                UpdateUI();
            }
          
            // Does the location have a quest?
            if (newLocation.questAvailableHere != null)
            {
                // See if the player already has the quest, and if they've completed it
                bool playerAlreadyHasQuest = _player.HasThisQuest(newLocation.questAvailableHere);
                bool playerAlreadyCompletedQuest = _player.CompletedthisQuest(newLocation.questAvailableHere);

                

                // See if the player already has the quest
                if (playerAlreadyHasQuest)
                {
                    // If the player has not completed the quest yet
                    if (!playerAlreadyCompletedQuest)
                    {
                        bool playerHasAllItemsToCompleteQuest = _player.HasAllQuestCompletionItems(newLocation.questAvailableHere);
                
                    

                        // The player has all items required to complete the quest
                        if (playerHasAllItemsToCompleteQuest)
                        {
                            // Display message
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "You complete the '" + newLocation.questAvailableHere.name + "' quest." + Environment.NewLine;

                            _player.RemoveQuestCompletionItems(newLocation.questAvailableHere);

                            // Give quest rewards
                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text += newLocation.questAvailableHere.rewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;
                            rtbMessages.Text += newLocation.questAvailableHere.rewardGold.ToString() + " gold" + Environment.NewLine;
                            rtbMessages.Text += newLocation.questAvailableHere.rewardItem.name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            _player.AddExpierencePoints(newLocation.questAvailableHere.rewardExperiencePoints);
                            _player.gold += newLocation.questAvailableHere.rewardGold;

                            _player.AddItemToInventory(newLocation.questAvailableHere.rewardItem);

                            _player.MarkQuestAsCompleted(newLocation.questAvailableHere);
                        }
                    }
                }
                else
                {
                    // The player does not already have the quest

                    // Display the messages
                    rtbMessages.Text += "You receive the " + newLocation.questAvailableHere.name + " quest." + Environment.NewLine;
                    rtbMessages.Text += newLocation.questAvailableHere.description + Environment.NewLine;
                    rtbMessages.Text += "To complete it, return with:" + Environment.NewLine;
                    foreach (QuestCompletionItem qci in newLocation.questAvailableHere.questCompletionItem)
                    {
                        if (qci.quantity == 1)
                        {
                            rtbMessages.Text += qci.quantity.ToString() + " " + qci.details.name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.quantity.ToString() + " " + qci.details.namePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;

                    // Add the quest to the player's quest list
                    _player.quests.Add(new PlayerQuest(newLocation.questAvailableHere));
                }
            }         
           // Update UI
            UpdateUI();
        }

        private void UpdateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;

            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";

            dgvInventory.Rows.Clear();
            List<InventoryItem> itemsToDelete = new List<InventoryItem>();

            foreach (InventoryItem inventoryItem in _player.inventory)
            {
                if (inventoryItem.quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { inventoryItem.details.name, inventoryItem.quantity.ToString() });
                }
                else if (inventoryItem.quantity == 0)
                {
                    // get the item
                    itemsToDelete.Add(inventoryItem);

                }
            }
            if(itemsToDelete.Count != 0)
            {
                foreach(InventoryItem ii in itemsToDelete)
                {
                    _player.inventory.Remove(ii);
                }
            }
        }

        private void UpdateQuestListInUI()
        {
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";

            dgvQuests.Rows.Clear();

            foreach (PlayerQuest playerQuest in _player.quests)
            {
                dgvQuests.Rows.Add(new[] { playerQuest.details.name, playerQuest.isCompleted.ToString() });
            }
        }

        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem inventoryItem in _player.inventory)
            {
                if (inventoryItem.details is Weapon)
                {
                    if (inventoryItem.quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.details);
                    }
                }
            }

            if (weapons.Count == 0)
            {
                // The player doesn't have any weapons, so hide the weapon combobox and "Use" button
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.SelectedIndexChanged-=cboWeapons_SelectedIndexChanged;
                cboWeapons.DataSource = weapons;
                cboWeapons.SelectedIndexChanged+= cboWeapons_SelectedIndexChanged;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";
                if(_player.currentWeapon != null)
                {
                    cboWeapons.SelectedItem = _player.currentWeapon;
                }
                else
                {
                    cboWeapons.SelectedIndex = 0;
                    
                }       
            }
        }

        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem inventoryItem in _player.inventory)
            {
                if (inventoryItem.details is HealingPotion)
                {
                    if (inventoryItem.quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.details);
                    }
                }
            }

            if (healingPotions.Count == 0)
            {
                // The player doesn't have any potions, so hide the potion combobox and "Use" button
                cboPotion.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotion.DataSource = healingPotions;
                cboPotion.DisplayMember = "Name";
                cboPotion.ValueMember = "ID";

                cboPotion.SelectedIndex = 0;
            }
        }
        private void UpdateSpellListInUI()
        {
            List<Spells> spellsToCast = new List<Spells>();

            foreach(Spells spell in _player.spellsToUse)
            {
                spellsToCast.Add(spell);
            }
            if(spellsToCast.Count == 0)
            {
                // The player has no spells so hide combobox and buttons for spells
                cboSpells.Visible = false;
                btnSpellUse.Visible = false;

            }
            else
            {
                cboSpells.SelectedIndexChanged-=cboSpells_SelectedIndexChanged;
                cboSpells.DataSource = spellsToCast;
                cboSpells.SelectedIndexChanged+=cboSpells_SelectedIndexChanged;
                cboSpells.DisplayMember = "Name";
                cboSpells.ValueMember = "ID";

                if(_player.currentSpell != null)
                {
                    cboSpells.SelectedItem = _player.currentSpell;
                }
                else
                {
                    // default index of 1
                    cboSpells.SelectedIndex = 0;
                }
                
            }
        }

      

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
           
            // Get the current weapon from the combobox and save it into the variable
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

            // Determine the amount of dmg we dealing to the monster
            int damageToMonster = RandomNumberGenerator.NumberBetween(currentWeapon.minimumDamage, currentWeapon.maximumDamage);

            //apply the dmg to the monster currenthitpoints

            _currentMonster.currentHitPoints -= damageToMonster;

            // Display Message

            rtbMessages.Text += "You did " + damageToMonster.ToString() + " Damage to " + _currentMonster.name + Environment.NewLine;

            // Check if the Monster is dead

            if (_currentMonster.currentHitPoints <= 0)

            {
                //Monster died
                MonsterDefeated();
            }
            else
            {
                // Monster is still alive
                // Determine the amount of dmg the Monster does to the player
                MonsterDamageCalculation();            
            }

        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            HealingPotion currentPotion = (HealingPotion)cboPotion.SelectedItem;
            //Remove potion from the inventory
            foreach (InventoryItem ii in _player.inventory)
            {
                if(currentPotion.id == ii.details.id)
                {
                    
                    ii.quantity -= 1;
                    rtbMessages.Text += "You used " + currentPotion.name + Environment.NewLine;


                }
                
            }            
            // Heal player for the amount of the potion
            if(_player.currentHitPoints+ currentPotion.amountToHeal > _player.maximumHitPoints)
            {
                _player.currentHitPoints = _player.maximumHitPoints;
                rtbMessages.Text += "You healed yourself for " + currentPotion.amountToHeal.ToString() + " points" + Environment.NewLine;

            }
            else
            {
                _player.currentHitPoints += currentPotion.amountToHeal;
                rtbMessages.Text += "You healed yourself for " + currentPotion.amountToHeal.ToString() + " points" + Environment.NewLine;
            }
            UpdateUI();
            // Player healed give the monster a turn
            MonsterDamageCalculation();

        }

       
        private void SuperAdventure_Load(object sender, EventArgs e)
        {

        }

        private void btnShop_Click(object sender, EventArgs e)
        {
            // create new vendor 
            Vendor vendor = new Vendor(_player);
            vendor.ShowDialog();
            // cycle through bought item list and add items if we already got them
            foreach(InventoryItem boughtItem in vendor.player2.boughtItem)
            {
                int change = 1;
                foreach(InventoryItem ii in _player.inventory)
                {
                    
                    // see if we already have the item
                    if(boughtItem.details.name == ii.details.name)
                    {
                        ii.quantity += 1;
                        change += 1;
                        break;
                    }
                  

                }
                // we didnt have the item so change variable is still one add the item to the inventory
                 if (change == 1)
                {
                    _player.inventory.Add(boughtItem);
                }

            }         
            // we finished adding all items empty the bought list
            vendor.player2.boughtItem.Clear();           
            UpdateUI(); 
            
                      
        }
        private void UpdateUI()
        {            
            UpdateWeaponListInUI();
            UpdateQuestListInUI();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();
            _player.AddSpells();
            UpdateSpellListInUI();                    
           if(_player.playerHasLVLUP)
            {
                rtbMessages.Text += Environment.NewLine;
                rtbMessages.Text += "YOU LVL UP " + Environment.NewLine;
                rtbMessages.Text += "You reached LVL " + _player.level.ToString() + Environment.NewLine;
                rtbMessages.Text += "Your Max HP increased by 5 " + Environment.NewLine;
                rtbMessages.Text += "Your Max MP increased by 1 " + Environment.NewLine;
                _player.playerHasLVLUP = false;
                UpdateUI();
          }           
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Does the location have a monster?
            if (_player.currentLocation.monsterLivingHere != null && _currentMonster == null)
            {
                rtbMessages.Text += "You see a " + _player.currentLocation.monsterLivingHere.name + Environment.NewLine;

                // Make a new monster, using the values from the standard monster in the World.Monster list
                Monster standardMonster = World.MonsterByID(_player.currentLocation.monsterLivingHere.id);

                _currentMonster = new Monster(standardMonster.id, standardMonster.name, standardMonster.minimumDamage, standardMonster.maximumDamage,
                    standardMonster.rewardExperiencePoints, standardMonster.rewardGold, standardMonster.currentHitPoints, standardMonster.maximumHitPoints);

                foreach (LootItem lootItem in standardMonster.lootTable)
                {
                    _currentMonster.lootTable.Add(lootItem);
                }

                cboWeapons.Visible = true;
                cboPotion.Visible = true;
                btnUseWeapon.Visible = true;
                btnUsePotion.Visible = true;
                cboSpells.Visible = true;
                btnSpellUse.Visible = true;

            }
            else if(_player.currentLocation.monsterLivingHere != null && _currentMonster != null)
            {
                rtbMessages.Text += "You are currently fighting " + _currentMonster.name + Environment.NewLine;
            }
            else
            {
                _currentMonster = null;

                cboWeapons.Visible = false;
                cboPotion.Visible = false;
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
                btnSpellUse.Visible = false;
                cboSpells.Visible = false;
            }
            if(_player.currentLocation.monsterLivingHere != null)
            {
                UpdateUI();
            }

        }
        public bool InCombat()
        {
            if(_currentMonster == null)
            {
                
                // we dont fight a monster so hide comboboxes
                cboWeapons.Visible = false;
                cboPotion.Visible = false;
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
                cboSpells.Visible = false;
                btnSpellUse.Visible = false;
                return false;
            }
            if(_currentMonster != null)
            {

                //we have are fighting a monster so change them to true
                UpdateUI();
                return true;
            }
            return false;
        }

        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            // scroll it automatically
            rtbMessages.ScrollToCaret();
        }

        private void btnSpellUse_Click(object sender, EventArgs e)
        {
            // get the current spell
            Spells currentSpell = (Spells)cboSpells.SelectedItem;
            // is the current spell a damage spell
            if(currentSpell.isDamageSpell)
            {
                if (_player.currentMp >= currentSpell.manaCost)
                {
                    _currentMonster.currentHitPoints -= currentSpell.amountToHealOrDamage;
                    rtbMessages.Text += "You did " + currentSpell.amountToHealOrDamage.ToString() + " Damage to " + _currentMonster.name + Environment.NewLine;
                    _player.currentMp -= currentSpell.manaCost;
                    if (_currentMonster.currentHitPoints <= 0)
                    {
                        // Monster is dead
                        MonsterDefeated();
                    }
                    else
                    {
                        // Monster is stil alive give it a turn
                        MonsterDamageCalculation();
                    }

                }
                else
                {
                   rtbMessages.Text+= "You dont have enough Mana to use " + currentSpell.name + Environment.NewLine;

                }
                
            }
            // is the current spell a healing spell
            else if (currentSpell.isHealingSpell)
            {
                if(currentSpell.amountToHealOrDamage+_player.currentHitPoints >_player.maximumHitPoints)
                {
                    // does he have enough mana to cast the spell
                    if (currentSpell.manaCost <= _player.currentMp)
                    {
                        // Heal player just fully
                        _player.currentHitPoints = _player.maximumHitPoints;
                        rtbMessages.Text += "You healed yourself for " + currentSpell.amountToHealOrDamage.ToString() + " points" + Environment.NewLine;
                        _player.currentMp -= currentSpell.manaCost;
                        UpdateUI();
                        MonsterDamageCalculation();
                        UpdateUI();
                    }
                    else
                    {
                        rtbMessages.Text += "You dont have enough Mana to use " + currentSpell.name + Environment.NewLine;
                        
                    }

                }
                else
                {
                    // does he have enough mana to cast the spell
                    if (currentSpell.manaCost <= _player.currentMp)
                    {
                        // Heal them for the amount of the spell
                        _player.currentHitPoints += currentSpell.amountToHealOrDamage;
                        rtbMessages.Text += "You healed yourself for " + currentSpell.amountToHealOrDamage.ToString()+ " points" + Environment.NewLine;
                        _player.currentMp -= currentSpell.manaCost;
                        UpdateUI();
                        MonsterDamageCalculation();
                        UpdateUI();
                        
                    }
                    else
                    {
                        rtbMessages.Text += "You dont have enough Mana to use " + currentSpell.name + Environment.NewLine;
                    }
                }
                
                
                
            }
            UpdateUI();
           

        }
        public void MonsterDamageCalculation()
        {
            // Monster is still alive

            // Determine the amount of dmg the Monster does to the player

            int damageToPlayer = RandomNumberGenerator.NumberBetween(_currentMonster.minimumDamage, _currentMonster.maximumDamage);

            // display message 
            rtbMessages.Text += "The " + _currentMonster.name + " did " + damageToPlayer.ToString() + " Damage points." + Environment.NewLine;

            // Subtract dmg from player

            _player.currentHitPoints -= damageToPlayer;

            UpdateUI();

            // Player dies

            if (_player.currentHitPoints <= 0)
            {
                rtbMessages.Text += Environment.NewLine;
                int lostgold = _player.gold / 2;
                // Display message and subtract half of the players gold
                // also set the died variable to true so we can erase the monster if we move home
                rtbMessages.Text += "The Monster " + _currentMonster.name + " killed you." + Environment.NewLine;
                _player.gold -= lostgold;
                if (lostgold > 0)
                {
                    rtbMessages.Text += "You Lost " + lostgold.ToString() + " Gold" + Environment.NewLine;
                }
                _player.hasDied = true;

                // Move player to home
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                rtbMessages.Text += "You wake up in " + _player.currentLocation.name + Environment.NewLine;
                rtbMessages.Text += Environment.NewLine;

            }

        }
        public void MonsterDefeated()
        {
            //Monster is dead


            rtbMessages.Text += Environment.NewLine;
            rtbMessages.Text += "You Defeated the " + _currentMonster.name + Environment.NewLine;
            // give the player expierence points
            _player.AddExpierencePoints(_currentMonster.rewardExperiencePoints);
            rtbMessages.Text += "You receive " + _currentMonster.rewardExperiencePoints.ToString() + " EXP " + Environment.NewLine;

            // Give the player gold
            _player.gold += _currentMonster.rewardGold;
            rtbMessages.Text += "You receive " + _currentMonster.rewardGold.ToString() + " gold." + Environment.NewLine;

            //Create a list for the drops

            List<InventoryItem> drops = new List<InventoryItem>();

            // Add items to the droplist

            foreach (LootItem lootItem in _currentMonster.lootTable)
            {
                if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.dropPercentage)

                {
                    drops.Add(new InventoryItem(lootItem.details, 1));
                }
            }
            // If no items were added get default loot

            if (drops.Count == 0)
            {
                foreach (LootItem lootItem in _currentMonster.lootTable)
                {
                    if (lootItem.isDefaultItem)
                    {
                        drops.Add(new InventoryItem(lootItem.details, 1));
                    }
                }
            }

            // Add items to the players inventory

            foreach (InventoryItem ii in drops)
            {
                _player.AddItemToInventory(ii.details);

                if (ii.quantity == 1)
                {
                    rtbMessages.Text += "You loot " + ii.quantity.ToString() + " " + ii.details.name + Environment.NewLine;

                }

                else
                {
                    rtbMessages.Text += "You loot " + ii.quantity.ToString() + " " + ii.details.namePlural + Environment.NewLine;
                }

            }

            // Refresh players information and inventory controls
            UpdateUI();
            // Add a blank line in the message box for appearance
            rtbMessages.Text += Environment.NewLine;
            _currentMonster = null;
            InCombat();
        }

        private void SuperAdventure_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());
        }

        private void cboWeapons_SelectedIndexChanged(object sender,EventArgs e)
        {
            _player.currentWeapon = (Weapon)cboWeapons.SelectedItem;
        }

        private void cboSpells_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player.currentSpell = (Spells)cboSpells.SelectedItem;
        }
    }
}