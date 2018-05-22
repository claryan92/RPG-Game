using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RPG_Game.Behaviors;
using RPG_Game.Systems;

namespace RPG_Game.Core
{
	public class Monster : Actor
	{
		public int? TurnsAlerted { get; set; }

		public void DrawStats(RLConsole statConsole, int position)
		{
			int yPosition = 13 + (position * 2);
			statConsole.Print(1, yPosition, Symbol.ToString(), Color);

			//Determine the width of the health bar by dividing the current health by the maxHealth
			int width = Convert.ToInt32(((double)Health / (double)MaxHealth) * 16.0);
			int remainingWidth = 16 - width;

			//Set the background colors of the health bar to show the monster damage
			statConsole.SetBackColor(3, yPosition, width, 1, Swatch.Primary);
			statConsole.SetBackColor(3 + width, yPosition, remainingWidth, 1, Swatch.PrimaryDarkest);
			statConsole.Print(2, yPosition, $":{Name}", Swatch.DbLight);
		}

		public virtual void PerformAction(CommandSystem commandSystem)
		{
			var behavior = new StandardMoveAndAttack();
			behavior.Act(this, commandSystem);
		}
	}
}
