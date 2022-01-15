using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalProject
{
    public partial class EnterScore : Form
    {
        public EnterScore()
        {
            InitializeComponent();
        }

        private void btnEnterScore_Click(object sender, EventArgs e)
        {
            if (txtInitials.Text == "")
            {
                MessageBox.Show("Please Enter Your Initials"); //error catching for entering information (numbers and symbols allowed)
                return;
            }
            else
            {
                Highscore.highscores.Add(new Highscore(txtInitials.Text, Game.score));
                try
                {
                    using (Stream stream = File.Open("Highscores.bin", FileMode.OpenOrCreate)) //opens file or creates one if none found
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(stream, Highscore.highscores);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                MainMenu.game.DialogResult = DialogResult.OK;
                this.DialogResult = DialogResult.OK;
                MainMenu.gameStarted = false;
            }
        }

        private void EnterScore_Load(object sender, EventArgs e)
        {
            lblScore.Text = Game.score.ToString(); //populates label with score from last game
        }

        private void EnterScore_FormClosed(object sender, FormClosedEventArgs e) //if enter score button not clicked, exits form without saving the score
        {
            MainMenu.game.DialogResult = DialogResult.OK;
            this.DialogResult = DialogResult.OK;
            MainMenu.gameStarted = false;
        }
    }
}
