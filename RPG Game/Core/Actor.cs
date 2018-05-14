﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Interfaces;
using RLNET;
using RogueSharp;

namespace RPG_Game.Core
{
	public class Actor : IActor, IDrawable
	{
		//IActor
		public string Name { get; set; }
		public int Awareness { get; set; }

		//IDrawable
		public RLColor Color { get; set; }
		public char Symbol { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public void Draw(RLConsole console, IMap map)
		{
			//Don't draw actors in cells that have yet to be explored
			if (!map.GetCell(X, Y).IsExplored)
			{
				return;
			}
			//Only Draw actor with color and symbol when they are in the field of view
			if (map.IsInFov(X, Y))
			{
				console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
			}
			else
			{
				console.Set(X, Y, Colors.Floor, Colors.FloorBackground, '.');
			}
		}
	}
}
