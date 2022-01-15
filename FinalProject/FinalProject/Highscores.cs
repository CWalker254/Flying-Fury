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
    public partial class Highscores : Form
    {
        public Highscores()
        {
            InitializeComponent();
        }

        private void Highscores_Load(object sender, EventArgs e)
        {
            try
            {
                using (Stream stream = File.Open("Highscores.bin", FileMode.Open)) //opens the saved highscores file
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    Highscore.highscores = (List<Highscore>)bf.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // Following code reference: https://www.codeproject.com/Tips/761275/How-to-Sort-a-List
            Highscore.highscores.Sort(delegate (Highscore x, Highscore y)
            {
                return y.Score.CompareTo(x.Score);
            });
            foreach (Highscore h in Highscore.highscores)
                lsHighscores.Items.Add(h.Initials + " : " + h.Score);
        }

        private void btnReturn_Click(object sender, EventArgs e) //shows main menu
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Highscores_FormClosed(object sender, FormClosedEventArgs e) //shows main menu
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
