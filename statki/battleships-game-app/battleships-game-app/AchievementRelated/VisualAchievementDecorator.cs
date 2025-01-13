using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.AchievementRelated
{
    public class VisualAchievementDecorator : IAchievement 
    {
        public IAchievement AchievementToDec;
        public VisualAchievementDecorator(IAchievement achievement) {
            this.AchievementToDec = achievement;
        }
        public void Print()
        {

        }
    }
}
