using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RLNET;

namespace RPG_Game.Core
{
	public class DungeonMap : Map
	{
		//Draw method is called each time the map is updated
		//renders all of the symbols/colors for each cell to the map subconsole
		public void Draw(RLConsole mapConsole)
		{
			mapConsole.Clear();
			foreach (Cell cell in GetAllCells())
			{
				SetConsoleSymbolForCell(mapConsole, cell);
			}
		}

		private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
		{
			if (!cell.IsExplored)
			{
				return;
			}
			//When a cell is in the field-of-view it should have a lighter color
			if (IsInFov(cell.X, cell.Y))
			{
				//Shows if the cell is walkable or not
				//'.' for floor and '#' for walls
				if (cell.IsWalkable)
				{
					console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
				}
				else
				{
					console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
				}
			}
			else
			{
				if (cell.IsWalkable)
				{
					console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
				}
				else
				{
					console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
				}
			}
		}

		//Returns true when able to place the Actor on the cell and false if not
		public bool SetActorPosition(Actor actor, int x, int y)
		{
			//Only allow placement if cell is walkable
			if (GetCell(x, y).IsWalkable)
			{
				SetIsWalkable(actor.X, actor.Y, true);
				//Update the actor's position
				actor.X = x;
				actor.Y = y;
				//The new cell the actor is on is now not walkable
				SetIsWalkable(actor.X, actor.Y, false);
				if (actor is Player)
				{
					UpdatePlayerFieldOfView();
				}
				return true;
			}
			return false;
		}

		public void SetIsWalkable(int x, int y, bool isWalkable)
		{
			Cell cell = GetCell(x, y);
			SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
		}

		//Method will be called any time the player moves, and will update the field of view
		public void UpdatePlayerFieldOfView()
		{
			Player player = Game.Player;
			ComputeFov(player.X, player.Y, player.Awareness, true);
			foreach (Cell cell in GetAllCells())
			{
				if (IsInFov(cell.X, cell.Y))
				{
					SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
				}
			}
		}
	}
}
