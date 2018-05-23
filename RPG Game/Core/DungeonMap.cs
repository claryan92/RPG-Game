using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RLNET;
using RPG_Game.Systems;

namespace RPG_Game.Core
{
	public class DungeonMap : Map
	{
		public List<Rectangle> Rooms { get; set; }
		public List<Door> Doors { get; set; }
		private readonly List<Monster> _monsters;

		public DungeonMap()
		{
			//Initialize the list of rooms when we create a new DungeonMap
			Rooms = new List<Rectangle>();
			_monsters = new List<Monster>();
			Doors = new List<Door>();
		}
		//Draw method is called each time the map is updated
		//renders all of the symbols/colors for each cell to the map subconsole
		public void Draw(RLConsole mapConsole, RLConsole statConsole)
		{
			foreach (Cell cell in GetAllCells())
			{
				SetConsoleSymbolForCell(mapConsole, cell);
			}

			foreach (Door door in Doors)
			{
				door.Draw(mapConsole, this);
			}
			//Index for the monster stat position
			int i = 0;
			//Iterate through each monster on the map and draw it after drawing the cells
			foreach (Monster monster in _monsters)
			{
				//When the monster is in the FOV draw the monster's stats
				if (IsInFov(monster.X, monster.Y))
				{
					monster.Draw(mapConsole, this);
					//Pass in the index to DrawStats and increment it afterwards
					monster.DrawStats(statConsole, i);
					i++;
				}
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
				//Try to open a door if one exists
				OpenDoor(actor, x, y);
				if (actor is Player)
				{
					UpdatePlayerFieldOfView();
				}
				return true;
			}
			return false;
		}

		public Door GetDoor(int x, int y)
		{
			return Doors.SingleOrDefault(d => d.X == x && d.Y == y);
		}

		//Actor opens the door at the x, y position
		private void OpenDoor(Actor actor, int x, int y)
		{
			Door door = GetDoor(x, y);
			if (door != null && door.IsOpen)
			{
				door.IsOpen = true;
				var cell = GetCell(x, y);
				//Once the door is opened it should be marked as transparent and no longer block the FOV
				SetCellProperties(x, y, true, cell.IsWalkable, cell.IsExplored);
				Game.MessageLog.Add($"{actor.Name} opened a door");
			}
		}

		public void SetIsWalkable(int x, int y, bool isWalkable)
		{
			Cell cell = GetCell(x, y);
			SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
		}

		//Called by MapGenerator to add the player to the map
		public void AddPlayer(Player player)
		{
			Game.Player = player;
			SetIsWalkable(player.X, player.Y, false);
			UpdatePlayerFieldOfView();
			Game.SchedulingSystem.Add(player);
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

		public void AddMonster(Monster monster)
		{
			_monsters.Add(monster);
			SetIsWalkable(monster.X, monster.Y, false);
			Game.SchedulingSystem.Add(monster);
		}

		public void RemoveMonster(Monster monster)
		{
			_monsters.Remove(monster);
			SetIsWalkable(monster.X, monster.Y, true);
			Game.SchedulingSystem.Remove(monster);
		}

		public Monster GetMonsterAt(int x, int y)
		{
			return _monsters.FirstOrDefault(m => m.X == x && m.Y == y);
		}

		//Look for a random location in the room that is walkable
		public Point GetRandomWalkableLocationInRoom(Rectangle room)
		{
			if (DoesRoomHaveWalkableSpace(room))
			{
				for (int i = 0; i < 100; i++)
				{
					int x = Game.Random.Next(1, room.Width - 2) + room.X;
					int y = Game.Random.Next(1, room.Height - 2) + room.Y;
					if (IsWalkable(x, y))
					{
						return new Point(x, y);
					}
				}
			}
			return null;
		}

		//iterate through each cell in the room and return true if the cell is walkable
		public bool DoesRoomHaveWalkableSpace(Rectangle room)
		{
			for (int x = 1; x <= room.Width - 2; x++)
			{
				for (int y = 1; y <= room.Height - 2; y++)
				{
					if (IsWalkable(x + room.X, y + room.Y))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
