using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace RPG_Game.Core
{
	public class Monster : Actor
	{
		public void DrawStats(RLConsole statConsole, int position)
		{
			//start at Y=13 (below the player stats)
			int yPosition = 13 + (position * 2);
			statConsole.Print(1, yPosition, Symbol.ToString(), Color);

			//Determine width of the health bar by dividing the current health by the max health
			int width = Convert.ToInt32(((double) Health / (double) MaxHealth) * 16);
			int remainingWidth = 16 - width;

			//Set Background colors of health bar to indicate monster health damage
			statConsole.SetBackColor(3, yPosition, width, 1, Swatch.Primary);
			statConsole.SetBackColor(3 + width, yPosition, remainingWidth, 1, Swatch.PrimaryDarkest);

			//Print the monster's name over the top of the health bar
			statConsole.Print(2, yPosition, $":{Name}", Swatch.DbLight);
		}
	}
}
