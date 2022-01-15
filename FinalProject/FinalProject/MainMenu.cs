using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class MainMenu : Form
    {
        public static bool gameStarted; //variable to show if gameplay has started
        public static Game game = new Game(); //single instance of gameplay screen
        WMPLib.WindowsMediaPlayer introMusic = new WMPLib.WindowsMediaPlayer();
        public MainMenu()
        {
            InitializeComponent();
            introMusic.URL = "Menu.mp3";
            introMusic.PlayStateChange += IntroMusic_PlayStateChange;
        }

        private void IntroMusic_PlayStateChange(int NewState)
        {
            try
            {
                if (introMusic.playState == WMPLib.WMPPlayState.wmppsStopped && gameStarted == false) //if the music has stopped and the game hasn't started, the music will loop again
                    introMusic.controls.play();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnOptions_Click(object sender, EventArgs e) //open options menu
        {
            Options o = new Options();
            o.ShowDialog();
        }

        private void btnStart_Click(object sender, EventArgs e) //open gameplay screen
        {
            gameStarted = true;
            introMusic.controls.stop();
            this.Hide();
            MainMenu.game.NewGame(); //resets all game objects 
            MainMenu.game.ShowDialog();
            if (MainMenu.game.DialogResult == DialogResult.OK) //instead of creating a new instance of the main menu, after the dialog result for "game" is "OK", this main menu screen will appear again
            {
                this.Show();
                try
                {
                    introMusic.controls.play(); 
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void btnHighscores_Click(object sender, EventArgs e) //open highscores menu
        {
            Highscores h = new Highscores();
            h.ShowDialog();
        }
    }
}
