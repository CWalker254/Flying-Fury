using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProject
{
    class Helicopter
    {
        #region Fields
        enum HelicopterState { Flying, Crashed};
        HelicopterState heliState; //instance of state machine used to call delegates
        public delegate void HeliHandler(MethodInvoker method);
        public event HeliHandler HeliEvent; //instance of delegate used to invoke a method to play audio
        private PictureBox heliImage = new PictureBox();
        private static List<PictureBox> images = new List<PictureBox>();
        public static List<Helicopter> choppas = new List<Helicopter>();
        private int picPoint = 0;
        public static int screenX, screenY;
        public int vertFly = 20;
        public static EnterScore enterScore = new EnterScore();
        static public WMPLib.WindowsMediaPlayer flying = new WMPLib.WindowsMediaPlayer(); //gameplay music
        static public WMPLib.WindowsMediaPlayer crashed = new WMPLib.WindowsMediaPlayer(); //explosion sound effect
        static private Timer explosionTimer = new Timer(); //timer enabled when helicopter crashed in order to show explosion animation
        static public int explosionCounter;
        static public bool heliCrashed; 
        #endregion
        #region Constructors
        static Helicopter()
        {
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    images.Add(new PictureBox());
                    images[i].Image = new Bitmap("heli" + (i + 1) + ".png");
                }
            }
            catch
            {
                MessageBox.Show("Pictures must be located in the same directory as the executable");
            }
        }
        public Helicopter(int x, int y)
        {
            heliImage.Image = images[0].Image;
            heliImage.Left = x;
            heliImage.Top = y;
            flying.URL = "Game.mp3";
            flying.controls.stop();
            heliState = HelicopterState.Flying;
            crashed.URL = "Explosion.mp3";
            crashed.controls.stop();
            explosionTimer.Interval = 100;
            explosionTimer.Enabled = false;
            explosionTimer.Tick += ExplosionTimer_Tick;
        }

        private void ExplosionTimer_Tick(object sender, EventArgs e)
        {
            explosionCounter++;
            if (explosionCounter >= 20)
                EndGame(); // disables timers and and opens Enter Score screen
            if (++picPoint > images.Count - 1)
                picPoint = 0;
            heliImage.Image = images[picPoint].Image;
        }
        #endregion
        #region Methods
        
        public void GameMusic()
        {
            flying.controls.play();
        }
        public void MoveDown() //simulates gravity by moving the helicopter down automatically as well as test for helicopter flying off screen
        {
            if (++picPoint > images.Count - 1)
                picPoint = 0;
            heliImage.Image = images[picPoint].Image;
            heliImage.Top += vertFly;
            if (heliImage.Top > screenY + 20)
            {
                heliState = HelicopterState.Crashed;
            }
            switch (heliState)
            {
                case HelicopterState.Flying:
                    HeliEvent?.Invoke(GameMusic);
                    break;
                case HelicopterState.Crashed:
                    HeliEvent?.Invoke(GameOver);
                    break;
            }
        }
        public void MoveUp() //moves the helicopter up when the left mouse button is held down as well as test for helicopter flying off screen
        {
            if (++picPoint > images.Count - 1)
                picPoint = 0;
            heliImage.Image = images[picPoint].Image;
            heliImage.Top -= (vertFly * 2);
            if (heliImage.Top < -80)
            {
                heliState = HelicopterState.Crashed;
            }
        }
        public void Draw(Graphics g) //diplays the helicopter
        {
            g.DrawImage(heliImage.Image, new Point(heliImage.Left, heliImage.Top));
        }
        private bool CrashTest(Helicopter heli, Obstacle ob) //tests to see if helicopter object collides with obstacle object
        {
            if (heli.heliImage.Left + heli.heliImage.Width - 20 < ob.barrier.Left)
                return false;
            if (ob.barrier.Left + ob.barrier.Width < heli.heliImage.Left)
                return false;
            if (heli.heliImage.Top + heli.heliImage.Height - 25 < ob.barrier.Top)
                return false;
            if (ob.barrier.Top + ob.barrier.Height - 20 < heli.heliImage.Top)
                return false;
            return true;
        }
        public void CheckForCrash() //calls GameOver method if objects collide
        {
            if (choppas.Count > 0)
            {
                for (int c = 0; c < choppas.Count; c++)
                    for (int o = 0; o < Obstacle.Obstacles.Count; o++)
                        if (CrashTest(choppas[c], Obstacle.Obstacles[o]))
                        {
                            foreach (Helicopter h in Helicopter.choppas)
                                h.heliState = HelicopterState.Crashed;
                            GameOver();
                        }
            }
        }
        public static void GameOver() //loads explosion images and changes audio
        {
            heliCrashed = true;
            images.Clear();
            try
            {
                for (int i = 0; i < 11; i++)
                {
                    images.Add(new PictureBox());
                    images[i].Image = new Bitmap("e" + (i + 1) + ".png");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pictures must be located in the same directory as the executable");
            }
            explosionTimer.Enabled = true;
            flying.controls.stop();
            crashed.controls.play();
        }
        public static void EndGame() //disables controls and exits the game
        {
            if (explosionCounter >= 20)
            {
                explosionCounter = 0;
                Game.t1.Enabled = false;
                Game.t2.Enabled = false;
                Game.t3.Enabled = false;
                Game.t4.Enabled = false;
                explosionTimer.Enabled = false;
                MessageBox.Show("Game Over!");
                EnterScore es = new EnterScore();
                es.ShowDialog();
                images.Clear();
                for (int i = 0; i < 3; i++)
                {
                    images.Add(new PictureBox());
                    images[i].Image = new Bitmap("heli" + (i + 1) + ".png");
                }
            }
        }
        #endregion
    }
}
