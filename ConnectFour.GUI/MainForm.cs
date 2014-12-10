﻿using System.Drawing;
using System.Windows.Forms;
using ConnectFour.Logic;

namespace ConnectFour
{
    public partial class MainForm : Form, IPlayer, IOutput
    {
        private Button[,] buttons;
        private GameControl gameControl;
            
        public MainForm()
        {
            InitializeComponent();
            drawGameField();
            gameControl = new GameControl(this);
            newGame();
        }

        private void drawGameField()
        {
            buttons = new Button[7,6];
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    buttons[x, y] = new Button();
                    buttons[x, y].Location = new Point(10 + x*50, 10 + y*50);
                    buttons[x,y].Size = new Size(50, 50);
                    buttons[x, y].BackColor = Color.Gray;
                    buttons[x, y].Click += Button_Click;
                    Controls.Add(buttons[x,y]);
                }
            }
            buttonLocking(false);
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (buttons[x, y].Equals(sender))
                    {
                        gameControl.Move(x, y);
                    }
                }
            }
        }

        public void ClearField()
        {
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    buttons[x, y].BackColor = Color.Gray;
                }
            }
            buttonLocking(false);
        }

        public void SetField(Point field, int player)
        {
            buttons[field.X, field.Y].BackColor = player == 1 ? Color.Yellow : Color.Red;
        }

        private void buttonLocking(bool enabled)
        {
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    buttons[x, y].Enabled = enabled;
                }
            }
        }

        public void Turn()
        {
            buttonLocking(true);
        }

        public void Win(Point point, int player)
        {
            SetField(point, player);
            MessageBox.Show("Spieler " + player + " hat gewonnen!", "Win!");
            newGame();
        }

        public void Draw(Point point, int currentPlayer)
        {
            SetField(point, currentPlayer);
            MessageBox.Show("Unentschieden!", "Draw!");
            newGame();
        }

        private void newGame()
        {
            ClearField();
            gameControl.NewGame(this, new NegMaxPlayer(gameControl));
        }
    }
}