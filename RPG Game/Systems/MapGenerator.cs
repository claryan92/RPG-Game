using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using RPG_Game.Core;

namespace RPG_Game.Systems
{
	class MapGenerator
	{
		private readonly int _width;
		private readonly int _height;

		private readonly DungeonMap _map;

		//Constructin a new MapGenerator requires dimensions for the maps it will create
		public MapGenerator(int width, int height)
		{
			_width = width;
			_height = height;
			_map = new DungeonMap();
		}

		//Generate a new map with a simple open floor plan and walls on the outside
		public DungeonMap CreateMap()
		{
			//Initialize every cell in the map by setting walkable, transparency and explored to true
			_map.Initialize(_width, _height);
			foreach (Cell cell in _map.GetAllCells())
			{
				_map.SetCellProperties(cell.X, cell.Y, true, true, true);
			}
			//set the first and last rows of the map to not transparent or walkable
			foreach (Cell cell in _map.GetCellsInRows(0, _height - 1))
			{
				_map.SetCellProperties(cell.X, cell.Y, false, false, true);
			}
			//Set the first and last columns in the map to not transparent or walkable
			foreach (Cell cell in _map.GetCellsInColumns(0, _width - 1))
			{
				_map.SetCellProperties(cell.X, cell.Y, false, false, true);
			}

			return _map;
		}
	}
}
