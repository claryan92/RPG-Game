using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Interfaces;
using RPG_Game.Core;
using RPG_Game.Systems;
using RogueSharp;

namespace RPG_Game.Behaviors
{
	public class StandardMoveAndAttack : IBehavior
	{
		public bool Act(Monster monster, CommandSystem commandSystem)
		{
			DungeonMap dungeonMap = Game.DungeonMap;
			Player player = Game.Player;
			FieldOfView monsterFov = new FieldOfView(dungeonMap);

			//If the monster has not been alerted, calculate the FOV, use monster's awarenss for the distance in the FOV
			//If the player is in the FOV alert the monster and add a message indicating the alert
			if (!monster.TurnsAlerted.HasValue)
			{
				monsterFov.ComputeFov(monster.X, monster.Y, monster.Awareness, true);
				if (monsterFov.IsInFov(player.X, player.Y))
				{
					Game.MessageLog.Add($"{monster.Name} is looking to fight {player.Name}");
					monster.TurnsAlerted = 1;
				}
			}

			if (monster.TurnsAlerted.HasValue)
			{
				//Before finding a path, make the monster and player cells walkable
				dungeonMap.SetIsWalkable(monster.X, monster.Y, true);
				dungeonMap.SetIsWalkable(player.X, player.Y, true);

				PathFinder pathFinder = new PathFinder(dungeonMap);
				Path path = null;

				try
				{
					path = pathFinder.ShortestPath(
						dungeonMap.GetCell(monster.X, monster.Y),
						dungeonMap.GetCell(player.X, player.Y));
				}
				catch (PathNotFoundException)
				{
					//The monster can see the player but cannot find a path to him
					Game.MessageLog.Add($"{monster.Name} bides its time, waiting to strike");
				}

				dungeonMap.SetIsWalkable(monster.X, monster.Y, false);
				dungeonMap.SetIsWalkable(player.X, player.Y, false);

				//If there is a path available tell the CommandSystem to move the monster
				if (path != null)
				{
					try
					{
						commandSystem.MoveMonster(monster, path.Steps.First());
					}
					catch (NoMoreStepsException)
					{
						Game.MessageLog.Add($"{monster.Name} roars angrily");
					}
				}
				monster.TurnsAlerted++;
				//Lose alerted status every 15 turns
				//monster will stay alert as long as the player is in the FOV
				if (monster.TurnsAlerted > 15)
				{
					monster.TurnsAlerted = null;
				}
			}
			return true;
		}
	}
}
