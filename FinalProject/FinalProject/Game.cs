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
    public partial class Game : Form
    {
        Random rand = new Random();
        static public string difficulty;
        public static Timer t1 = new Timer(); //timer to move helicopter down, move obstacles left and check for crash
        public static Timer t2 = new Timer(); //timer to call helicopter MoveUp method
        public static Timer t3 = new Timer(); //timer to add new obstacles
        public static Timer t4 = new Timer(); //timer to remove old obstacles from Obstacles list
        public static int score;
        public static bool insaneModeUnlocked;
        public Game()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            Helicopter.screenX = this.ClientSize.Width;
            Helicopter.screenY = this.ClientSize.Height;
            t1.Tick += T1_Tick;
            t2.Tick += T2_Tick;
            t3.Interval = 1200;
            t3.Tick += T3_Tick;
            t4.Interval = 10000;
            t4.Tick += T4_Tick;
        }
        private void T4_Tick(object sender, EventArgs e)
        {
                Obstacle.Obstacles.RemoveAt(0);
        }

        private void T3_Tick(object sender, EventArgs e)
        {
            if (difficulty == "easy")
            {
                t3.Interval = 2000; //every 2 seconds, a new obstacle will appear
                EasyObstacle.Obstacles.Add(new EasyObstacle(this.ClientSize.Width, rand.Next(0, this.ClientSize.Height)));
            }
            else if (difficulty == "medium")
                MediumObstacle.Obstacles.Add(new MediumObstacle(this.ClientSize.Width, rand.Next(0, this.ClientSize.Height))); //every 1.2 seconds, a new obstacle will appear
            else if (difficulty == "hard")
            {
                t3.Interval = 900; //every 900 milliseconds, a new obstacle will appear
                HardObstacle.Obstacles.Add(new HardObstacle(this.ClientSize.Width, rand.Next(0, this.ClientSize.Height)));
            }
            else if (difficulty == "insane")
            {
                t3.Interval = 500;//every half a second, a new obstacle will appear
                InsaneObstacle.Obstacles.Add(new InsaneObstacle(this.ClientSize.Width, rand.Next(0, this.ClientSize.Height)));
            }
            else Obstacle.Obstacles.Add(new Obstacle(this.ClientSize.Width, rand.Next(0, this.ClientSize.Height))); //every 1.2 seconds, a new obstacle will appear
        }

        private void T2_Tick(object sender, EventArgs e)
        {
            foreach (Helicopter b in Helicopter.choppas)
                b.MoveUp();
        }

        private void T1_Tick(object sender, EventArgs e)
        {
            score++;
            lblScoreNum.Text = score.ToString();
            if (score >= 1000 && difficulty == "hard")
                insaneModeUnlocked = true;
            foreach (Helicopter b in Helicopter.choppas)
                b.MoveDown();
            foreach (Obstacle o in Obstacle.Obstacles)
                o.MoveLeft();
            if (Helicopter.heliCrashed == false)
                foreach (Helicopter h in Helicopter.choppas)
                    h.CheckForCrash();
            this.Invalidate(false);
        }

        private void btnStart_Click(object sender, EventArgs e) //starts the game, enables the timers and makes buttons invisible
        {
            btnMainMenu.Visible = false;
            Helicopter.choppas.Add(new Helicopter(100, 100));
            if (difficulty == "easy")
                EasyObstacle.Obstacles.Add(new EasyObstacle(this.ClientSize.Width, rand.Next(0, this.ClientSize.Height)));
            else if (difficulty == "medium")
                MediumObstacle.Obstacles.Add(new MediumObstacle(this.ClientSize.Width, rand.Next(0, this.ClientSize.Height)));
            else if (difficulty == "hard")
                HardObstacle.Obstacles.Add(new HardObstacle(this.ClientSize.Width, rand.Next(0, this.ClientSize.Height)));
            else if (difficulty == "insane")
            {
                InsaneObstacle.Obstacles.Add(new InsaneObstacle(this.ClientSize.Width, rand.Next(0, this.ClientSize.Height)));
            } else Obstacle.Obstacles.Add(new Obstacle(this.ClientSize.Width, rand.Next(0, this.ClientSize.Height)));
            Helicopter.choppas[Helicopter.choppas.Count - 1].HeliEvent += Game_HeliEvent;
            t1.Enabled = true;
            t3.Enabled = true;
            t4.Enabled = true;
            btnStart.Visible = false;
            lblScore.Visible = true;
            lblScoreNum.Visible = true;
        }

        private void Game_HeliEvent(MethodInvoker method)
        {
            method(); //calls method delegate tp play audio
        }

        private void Form1_Paint(object sender, PaintEventArgs e) //displays game objects
        {
            if (Helicopter.choppas.Count > 0)
                foreach (Helicopter b in Helicopter.choppas)
                    b.Draw(e.Graphics);
            if (Obstacle.Obstacles.Count > 0)
                foreach (Obstacle o in Obstacle.Obstacles)
                    o.Draw(e.Graphics);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e) //enables timer
        {
            t2.Enabled = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e) //disables timer
        {
            t2.Enabled = false;
        }
        
        public void NewGame() //resets game objects
        {
            Helicopter.choppas.Clear();
            Obstacle.Obstacles.Clear();
            score = 0;
            lblScoreNum.Text = score.ToString();
            btnStart.Visible = true;
            btnMainMenu.Visible = true;
            Helicopter.explosionCounter = 0;
            Helicopter.heliCrashed = false;
        }

        private void Pause() //stops timers and opens pause screen, changes form text as well
        {
            MainMenu.game.Text = "Flying Fury - Paused";
            t1.Stop();
            t2.Stop();
            t3.Stop();
            PauseMenu pm = new PauseMenu();
            pm.ShowDialog();
        }

        public static void Resume() //resumes timers and changes form text back
        {
            MainMenu.game.Text = "Flying Fury";
            t1.Start();
            t2.Start();
            t3.Start();
            Helicopter.flying.controls.play();
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e) //shows main menu
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Game_MouseClick(object sender, MouseEventArgs e) //deciphers mouse click as either control for helicopter or to pause the game
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    break;
                case MouseButtons.Right:
                    Pause();
                    break;
            }
        }

        private void btnMainMenu_Click(object sender, EventArgs e) //shows main menu, disappears when game starts
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
