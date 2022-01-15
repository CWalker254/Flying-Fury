using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    [Serializable]
    class Highscore
    {
        public static List<Highscore> highscores = new List<Highscore>();
        public string Initials { get; set; } //gets the initials entered by the user when the game is over
        public int Score { get; set; } //gets the score achieved when the game is over 
        public Highscore(string initials, int score)
        {
            Initials = initials;
            Score = score;
        }
    }
}
