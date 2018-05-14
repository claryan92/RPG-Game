using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RPG_Game.Core;
using RogueSharp;
using RPG_Game.Systems;

namespace RPG_Game
{
	public class Game
	{
		public static Player Player { get; set; }
		public static DungeonMap DungeonMap { get; private set; }
		
		//Screen height and width are in # of tiles
		private static readonly int _screenWidth = 100;
		private static readonly int _screenHeight = 70;
		private static RLRootConsole _rootConsole;

		//Map Console
		private static readonly int _mapWidth = 80;
		private static readonly int _mapHeight = 48;
		private static RLConsole _mapConsole;

		//Message Console
		private static readonly int _messageWidth = 80;
		private static readonly int _messageHeight = 11;
		private static RLConsole _messageConsole;

		//Stat Console
		private static readonly int _statWidth = 20;
		private static readonly int _statHeight = 70;
		private static RLConsole _statConsole;

		//Inventory Console
		private static readonly int _inventoryWidth = 80;
		private static readonly int _inventoryHeight = 11;
		private static RLConsole _inventoryConsole;

		public static void Main()
		{
			string fontFileName = "terminal8x8.png";
			string consoleTitle = "RPG_Game - Level 1";
			//Tells RLNet to use the bitmap font and that each tile is 8x8 pixels
			_rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);

			// Initialize the sub consoles that we will Blit to the root console
			_mapConsole = new RLConsole(_mapWidth, _mapHeight);
			_messageConsole = new RLConsole(_messageWidth, _messageHeight);
			_statConsole = new RLConsole(_statWidth, _statHeight);
			_inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

			Player = new Player();
			MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight);
			DungeonMap = mapGenerator.CreateMap();
			DungeonMap.UpdatePlayerFieldOfView();

			_rootConsole.Update += OnRootConsoleUpdate;
			_rootConsole.Render += OnRootConsoleRender;
			_rootConsole.Run();
		}

		private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
		{

			//Set background color and text color for each console

			_messageConsole.SetBackColor(0, 0, _messageWidth, _messageWidth, Swatch.DbDeepWater);
			_messageConsole.Print(1, 1, "Messages", Colors.TextHeading);

			_statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.DbOldStone);
			_statConsole.Print(1, 1, "Stats", Colors.TextHeading);

			_inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
			_inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);
			
		}

		private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
		{
			//Blit the sub console to the root console in the correct locations
			RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
			RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
			RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
			RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

			//Draws the console
			_rootConsole.Draw();
			DungeonMap.Draw(_mapConsole);
			Player.Draw(_mapConsole, DungeonMap);
		}

	}
}
