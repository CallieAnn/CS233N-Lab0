﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
    public partial class boardForm : Form
    {
        public boardForm()
        {
            InitializeComponent();
        }

        #region Instance Variables 
        const int NOT_PICKED_YET = -1;
        const int DECK = 20;

        int firstCardNumber = NOT_PICKED_YET;
        int secondCardNumber = NOT_PICKED_YET;
        int matches = 0;
        #endregion

        #region Methods

        // This method finds a picture box on the board based on it's number (1 - 20)
        // It takes an integer as it's parameter and returns the picture box controls
        // that's name contains that number
        private PictureBox GetCard(int i)
        {
            PictureBox card = (PictureBox)this.Controls["card" + i];
            return card;
        }

        // This method gets the filename for the image displayed in a picture box given it's number
        // It takes an integer as it's parameter and returns a string containing the 
        // filename for the image in the corresponding picture box
        private string GetCardFilename(int i)
        {
            return GetCard(i).Tag.ToString();
        }

        // This method changes the filename for a given picture box
        // It takes an integer and a string that represents a filename as it's parameters
        // It doesn't return a value but stores the filename for the image to be displayed
        // in the picture box.  It doesn't actually display the new image
        private void SetCardFilename(int i, string filename)
        {
            GetCard(i).Tag = filename;
        }

        // These 2 methods get the value (and suit) of the card in a given picturebox
        // Both methods take an integer as the parameter and return a string
        private string GetCardValue(int index)
        {
            return GetCardFilename(index).Substring(4, 1);
        }

        private string GetCardSuit(int index)
        {
            return GetCardFilename(index).Substring(5, 1);
        }

        // TODO:  students should write this one
        private bool IsMatch(int index1, int index2)
        {
            
            if (GetCardValue(index1) == GetCardValue(index2) && GetCardSuit(index1) == GetCardSuit(index2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // This method fills each picture box with a filename
        private void FillCardFilenames()
        {
            string[] values = { "a", "2", "j", "q", "k" };
            string[] suits = { "c", "d", "c", "d" };
            int i = 1;

            for (int suit = 0; suit <= 3; suit++)
            {
                for (int value = 0; value <= 4; value++)
                {
                    SetCardFilename(i, "card" + values[value] + suits[suit] + ".jpg");
                    i++;
                }
            }
        }

        // TODO:  students should write this one
        private void ShuffleCards()
        {
            Random generator = new Random();
            for (int cardNum = 1; cardNum <= DECK; cardNum++)
            {
                int randomNum = generator.Next(1, 21);
                string temp = GetCardFilename(cardNum);
                string tempRand = GetCardFilename(randomNum);
                SetCardFilename(cardNum, tempRand);
                SetCardFilename(randomNum, temp);

            }
        }

        // This method loads (shows) an image in a picture box.  Assumes that filenames
        // have been filled in an earlier call to FillCardFilenames
        private void LoadCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\Cards\\" + GetCardFilename(i));
        }

        // This method loads the image for the back of a card in a picture box
        private void LoadCardBack(int i)
        {
            PictureBox card = GetCard(i);
            card.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\Cards\\black_back.jpg");
        }

        // TODO:  students should write all of these
        // shows (loads) the backs of all of the cards
        private void LoadAllCardBacks()
        {
            int cardNum;

            for(cardNum = 1; cardNum <= DECK; cardNum++)
            {
                LoadCardBack(cardNum);
            }
        }

        // Hides a picture box
        private void HideCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Visible = false;
        }

        private void HideAllCards()
        {
            int cardNum;

            for (cardNum = 1; cardNum <= DECK; cardNum++)
            {
                HideCard(cardNum);
            }
        }

        // shows a picture box
        private void ShowCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Visible = true;

        }

        private void ShowAllCards()
        {
            int cardNum = 1;

            while (cardNum <= DECK)
            {
                ShowCard(cardNum);
                cardNum++;
            }
    
        }

        // disables a picture box
        private void DisableCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Enabled = false;
        }

        //User can't click on any card/picture box
        private void DisableAllCards()
        {
            for (int cardNum = 1; cardNum <= DECK; cardNum++)
            {
                DisableCard(cardNum);
            }
        }

        //User can click on picture box
        private void EnableCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Enabled = true;
        }

        //User can click on any card/picture box
        private void EnableAllCards()
        {
            for (int cardNum = 1; cardNum <= DECK; cardNum++)
            {
                EnableCard(cardNum);
            }
        }
    
        //Enables user to click on any visible card/picture box
        private void EnableAllVisibleCards()
        {
            for (int cardNum = 1; cardNum <= DECK; cardNum++)
            {
                PictureBox card = GetCard(cardNum);
                if (card.Visible == true)
                {
                    EnableCard(cardNum);
                }

            }

        }

        #endregion

        #region EventHandlers
        //Happens when the form loads
        private void boardForm_Load(object sender, EventArgs e)
        {
            /* 
             * Fill the picture boxes with filenames
             * Shuffle the cards
             * Load all of the card backs - 
             *      While you're testing you might want to load all of card faces
             *      to make sure that the cards are loaded successfully and that
             *      they're shuffled.  If you get all 2s, something is wrong.
            */
            FillCardFilenames();
            ShuffleCards();
            LoadAllCardBacks();
            //ShowAllCards();
        }

        //When user clicks on a card:
        private void card_Click(object sender, EventArgs e)
        {
            PictureBox card = (PictureBox)sender;
            int cardNumber = int.Parse(card.Name.Substring(4));

            /* 
             * if the first card isn't picked yet
             *      save the first card index
             *      load the card
             *      disable the card
             *  else (the user just picked the second card)
             *      save the second card index
             *      load the card
             *      disable all of the cards
             *      start the flip timer
             *  end if
            */

            if (firstCardNumber == NOT_PICKED_YET)
            {
                GetCard(cardNumber);
                firstCardNumber = cardNumber;
                LoadCard(cardNumber);
                DisableCard(cardNumber);
                
            }

            else
            {
                GetCard(cardNumber);
                secondCardNumber = cardNumber;
                LoadCard(cardNumber);
                DisableAllCards();
                flipTimer.Interval = 1000;
                flipTimer.Start();
            }
        }

        //once two cards are clicked, flip timer starts
        private void flipTimer_Tick(object sender, EventArgs e)
        {
            /*
             * stop the flip timer
             * if the first card and second card are a match
             *      increment the number of matches
             *      hide the first card
             *      hide the second card
             *      reset the first card number
             *      reset the second card number
             *      if the number of matches is 10
             *          show a message box
             *      else
             *          enable all of the cards left on the board
             *      end if
             * else
             *      flip the first card back over
             *      flip the second card back over
             *      reset the first card number
             *      reset the second card number
             *      enable all of the cards left on the board
             * end if
             */
            flipTimer.Stop();
            //Checks to see if cards match
            if(IsMatch(firstCardNumber, secondCardNumber))
            {
                matches++;
                HideCard(firstCardNumber);
                HideCard(secondCardNumber);
                firstCardNumber = NOT_PICKED_YET;
                secondCardNumber = NOT_PICKED_YET;
                if (matches == 10) //Winning displays a message box and resets to play again
                {
                    MessageBox.Show("You've Won!");
                    ShuffleCards();
                    LoadAllCardBacks();
                    ShowAllCards();
                    EnableAllCards();
                }
                else
                {
                    EnableAllVisibleCards();          
                }

            }
            else
            {
                LoadCardBack(firstCardNumber);
                LoadCardBack(secondCardNumber);
                firstCardNumber = NOT_PICKED_YET;
                secondCardNumber = NOT_PICKED_YET;
                EnableAllVisibleCards();
            }
        }
        #endregion
    }
}
