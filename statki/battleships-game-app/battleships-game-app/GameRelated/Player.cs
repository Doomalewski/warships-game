using battleships_game_app.AchievementRelated;
using battleships_game_app.WarshipRelated;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.GameRelated
{
    internal class Player
    {
        public int PlayerId;
        public string Name;
        public int HighScore;
        public bool ComputerMode;
        public List<Warship> Ships;
        public List<Achievement> Achievements;
        public Player(string Name, bool chosenOption) 
        {
            this.Name = Name;
            this.ComputerMode = chosenOption;
            Ships = new List<Warship>();
            Achievements = new List<Achievement>();
        }
    }
}
