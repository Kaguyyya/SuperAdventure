using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Engine;

namespace SuperAdventure
{
    public partial class Vendor : Form
    {
        public Player player2 { get; set; }
        public bool playerHasBoughtItem { get; set; }
        public Vendor(Player player)
        {
            InitializeComponent();

            player2 = player;
            dgvItemList.RowHeadersVisible = false;
            dgvItemList.ColumnCount = 3;
            dgvItemList.Columns[0].Name = "Item";
            dgvItemList.Columns[0].Width = 200;
            dgvItemList.Columns[1].Name = "Quantity";
            dgvItemList.Columns[2].Name = "Price";
            lblGold.Text = player2.gold.ToString();
            // generate item shop
            foreach (InventoryItem ii in player.currentLocation.hasShopKeeper.SellingItems)
            {
                dgvItemList.Rows.Add(new[] { ii.details.name, ii.quantity.ToString(), ii.details.sellingValue.ToString() });




            }
        }
        // button click events are orderer as in Form Vendor


        private void button2_Click(object sender, EventArgs e)
        {
            BuyItems(0);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            BuyItems(1);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            BuyItems(2);

        }
        private void button12_Click(object sender, EventArgs e)
        {
            BuyItems(3);

        }

        private void button13_Click(object sender, EventArgs e)
        {
            BuyItems(4);
        }


        private void button14_Click(object sender, EventArgs e)
        {
            BuyItems(5);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            BuyItems(6);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            BuyItems(7);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            BuyItems(8);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            BuyItems(9);
        }

        private void BuyItems(int arrayPoint)
        {                    
                // Check if the player got enough gold
                if (player2.gold - player2.currentLocation.hasShopKeeper.SellingItems[arrayPoint].details.sellingValue >= 0)
                {
                    // subtract the gold
                    player2.gold -= player2.currentLocation.hasShopKeeper.SellingItems[arrayPoint].details.sellingValue;
                    lblGold.Text = player2.gold.ToString();

                    // get the item               
                    player2.boughtItem.Add(player2.currentLocation.hasShopKeeper.SellingItems[arrayPoint]);

                    // print out message
                    rtbMessagesVendor.Text += "You added " + player2.currentLocation.hasShopKeeper.SellingItems[arrayPoint].details.name + " to your buying list." + Environment.NewLine;
                }
                else
                {
                    // print out message he hasnt enough gold
                    rtbMessagesVendor.Text += "You dont have enough Gold to buy " + player2.currentLocation.hasShopKeeper.SellingItems[arrayPoint].details.name + Environment.NewLine;
                }           
        }

       
    }
}
