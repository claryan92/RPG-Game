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
