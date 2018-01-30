using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class TTTForm : Form
    {
        public TTTForm()
        {
            InitializeComponent();
        }

        const string USER_SYMBOL = "X";
        const string COMPUTER_SYMBOL = "O";
        const string EMPTY = "";

        //this is the size of the board
        const int SIZE = 5;

        // constants for the 2 diagonals
        const int TOP_LEFT_TO_BOTTOM_RIGHT = 1;
        const int TOP_RIGHT_TO_BOTTOM_LEFT = 2;

        // constants for IsWinner
        const int NONE = -1;
        const int ROW = 1;
        const int COLUMN = 2;
        const int DIAGONAL = 3;

        string[,] board = new string[SIZE, SIZE];

        public void FillBoard()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for(int col = 0; col<SIZE; col++)
                {
                    board[row, col] = EMPTY;
                }
            } 
        }
        // This method takes a row and column as parameters and 
        // returns a reference to a label on the form in that position
        private Label GetSquare(int row, int column)
        {
            int labelNumber = row * SIZE + column + 1;
            return (Label)(this.Controls["label" + labelNumber.ToString()]);
        }

        // This method does the "reverse" process of GetSquare
        // It takes a label on the form as it's parameter and
        // returns the row and column of that square as output parameters
        private void GetRowAndColumn(Label l, out int row, out int column)
        {
            int position = int.Parse(l.Name.Substring(5));
            row = (position - 1) / SIZE;
            column = (position - 1) % SIZE;
        }

        // This method takes a row (in the range of 0 - 4) and returns true if 
        // the row on the form contains 5 Xs or 5 Os.
        // Use it as a model for writing IsColumnWinner
        //CHANGE
        private bool IsRowWinner(int row)
        {
            string symbol = board[row, 0];
            for (int col = 1; col < SIZE; col++)
            {
                if (symbol == EMPTY || board[row, col] != symbol)
                    return false;
            }
            return true;
        }

        //* TODO:  finish all of these that return true
        private bool IsAnyRowWinner()
        {
            bool winner = false;
            for (int rowNum = 0; rowNum < SIZE; rowNum++)
            {
                if (IsRowWinner(rowNum))
                {
                    winner = true;
                }
                rowNum++;
            }
            return winner;
        }

        //CHANGE
        private bool IsColumnWinner(int col)
        {
                string symbol = board[0, col];
                for (int row = 1; row < SIZE; row++)
                {
                    if (symbol == EMPTY || board[row, col] != symbol)
                        return false;
                }
                return true;
        }

        private bool IsAnyColumnWinner()
        {
            bool winner = false;
            for (int colNum = 0; colNum < SIZE; colNum++)
            {
                if (IsColumnWinner(colNum))
                {
                    winner = true;
                }
                colNum++;
            }
            return winner;
        }

        //CHANGE
        private bool IsDiagonal1Winner()
        {
            string symbol = board[0, 0];
            for (int row = 1, col = 1; row < SIZE; row++, col++)
            {
                if (symbol == EMPTY || board[row, col] != symbol)
                    return false;
            }
            return true;
        }

        //CHANGE
        private bool IsDiagonal2Winner()
        {
            string symbol = board[0, (SIZE - 1)];
            for (int row = 1, col = SIZE - 2; row < SIZE; row++, col--)
            {
                if (symbol == EMPTY || board[row, col] != symbol)
                    return false;
            }
            return true;
        }

        private bool IsAnyDiagonalWinner()
        {
            if(IsDiagonal1Winner() || IsDiagonal2Winner())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Checks to see if all squares are full
        //CHANGE
        private bool IsFull()
        {

            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    if (board[row, col] == EMPTY)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // This method determines if any row, column or diagonal on the board is a winner.
        // It returns true or false and the output parameters will contain appropriate values
        // when the method returns true.  See constant definitions at top of form.
        private bool IsWinner(out int whichDimension, out int whichOne)
        {
            // rows
            for (int row = 0; row < SIZE; row++)
            {
                if (IsRowWinner(row))
                {
                    whichDimension = ROW;
                    whichOne = row;
                    return true;
                }
            }
            // columns
            for (int column = 0; column < SIZE; column++)
            {
                if (IsColumnWinner(column))
                {
                    whichDimension = COLUMN;
                    whichOne = column;
                    return true;
                }
            }
            // diagonals
            if (IsDiagonal1Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_LEFT_TO_BOTTOM_RIGHT;
                return true;
            }
            if (IsDiagonal2Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_RIGHT_TO_BOTTOM_LEFT;
                return true;
            }
            whichDimension = NONE;
            whichOne = NONE;
            return false;
        }

        // I wrote this method to show you how to call IsWinner
        private bool IsTie()
        {
            int winningDimension, winningValue;
            return (IsFull() && !IsWinner(out winningDimension, out winningValue));
        }

        // This method takes an integer in the range 0 - 4 that represents a column
        // as it's parameter and changes the font color of that cell to red.
        private void HighlightColumn(int col)
        {
            for (int row = 0; row < SIZE; row++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        // This method changes the font color of the top right to bottom left diagonal to red
        // I did this diagonal because it's harder than the other one
        private void HighlightDiagonal2()
        {
            for (int row = 0, col = SIZE - 1; row < SIZE; row++, col--)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        // This method will highlight either diagonal, depending on the parameter that you pass
        private void HighlightDiagonal(int whichDiagonal)
        {
            if (whichDiagonal == TOP_LEFT_TO_BOTTOM_RIGHT)
                HighlightDiagonal1();
            else
                HighlightDiagonal2();

        }

        //* TODO:  finish these 2
        private void HighlightRow(int row)
        {
            for (int col = 0; col < SIZE; col++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        private void HighlightDiagonal1()
        {
            for (int row = 0, col = 0; row < SIZE; row++, col++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;

            }
        }

        //* TODO:  finish this
        //This highlights the winning row, column, or diagonal
        private void HighlightWinner(string player, int winningDimension, int winningValue)
        {
            switch (winningDimension)
            {
                case ROW:
                    HighlightRow(winningValue);
                   
                    break;
              
                case COLUMN:
                    HighlightColumn(winningValue);
                    
                    break;

                case DIAGONAL:
                    HighlightDiagonal(winningValue);
                  
                    break;
            }
        }

        //* TODO:  finish these 2
        //Resets all squares to play again
        private void ResetSquares()
        {
            for (int r = 0; r < SIZE; r++)
            {
                for (int c = 0; c < SIZE; c++)
                {
                    Label square = GetSquare(r, c);
                    square.Text = "";
                    square.ForeColor = Color.Black;
                }
            }
        }

        //Computer makes a move
        //CHANGE, and check for winner in event handler?
        private void MakeComputerMove()
        {
            int row;
            int column;
            Label lab;

            do
            {
                Random rand = new Random();
                row = rand.Next(5);
                column = rand.Next(5);
                lab = GetSquare(row, column);
            } while (lab.Text != "");

            lab.Text = COMPUTER_SYMBOL.ToString();
            DisableSquare(lab);

            int winningDimension, winningValue;
            if (IsWinner(out winningDimension, out winningValue))
            {
                string player = "Computer ";
                HighlightWinner(player, winningDimension, winningValue);
                resultLabel.Text = (player + "wins!");
                DisableAllSquares();


            }

            else if (IsFull())
            {
                MessageBox.Show("It's a Tie!");
            }
        }

        // Setting the enabled property changes the look and feel of the cell.
        // Instead, this code removes the event handler from each square.
        // Use it when someone wins or the board is full to prevent clicking a square.
        private void DisableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    DisableSquare(square);
                }
            }
        }

        // Inside the click event handler you have a reference to the label that was clicked
        // Use this method (and pass that label as a parameter) to disable just that one square
        private void DisableSquare(Label square)
        {
            square.Click -= new System.EventHandler(this.label_Click);
        }

        // You'll need this method to allow the user to start a new game
        private void EnableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    square.Click += new System.EventHandler(this.label_Click);
                }
            }
        }

        //* TODO:  finish the event handlers
        private void label_Click(object sender, EventArgs e)
        {
            int winningDimension = NONE;
            int winningValue = NONE;

            Label clickedLabel = (Label)sender; //this will change
            if (clickedLabel.Text == "")
            {
                int row, column;
                GetRowAndColumn(clickedLabel, out row, out column);

                clickedLabel.Text = USER_SYMBOL.ToString();
                DisableSquare(clickedLabel);

                if (IsWinner(out winningDimension, out winningValue))
                {
                    string player = "User ";
                    HighlightWinner(player, winningDimension, winningValue);
                    resultLabel.Text = (player + "wins!");

                    DisableAllSquares();
                    
                }

                else if (IsFull())
                {
                    MessageBox.Show("It's a Tie!");
                }

                else  
                {
                    MakeComputerMove();
                }
                
            }
        }

        //resets everything to play again
        private void newGameButton_Click(object sender, EventArgs e)
        {
            EnableAllSquares();
            ResetSquares();
            resultLabel.Text = "";
            
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
