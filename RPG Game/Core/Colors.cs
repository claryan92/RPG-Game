﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace RPG_Game.Core
{
	class Colors
	{
		public static RLColor FloorBackground = RLColor.Black;
		public static RLColor Floor = Swatch.AlternateDarkest;
		public static RLColor FloorBackgroundFov = Swatch.DbDark;
		public static RLColor FloorFov = Swatch.Alternate;

		public static RLColor WallBackground = Swatch.SecondaryDarkest;
		public static RLColor Wall = Swatch.Secondary;
		public static RLColor WallBackgroundFov = Swatch.SecondaryDarker;
		public static RLColor WallFov = Swatch.SecondaryLighter;

		public static RLColor TextHeading = Swatch.DbLight;
		public static RLColor Text = Swatch.DbSky;
		public static RLColor Gold = Swatch.DbSun;

		public static RLColor Player = Swatch.DbLight;
		public static RLColor KoboldColor = Swatch.DbBrightWood;

		public static RLColor DoorBackground = Swatch.ComplimentDarkest;
		public static RLColor Door = Swatch.ComplimentLighter;
		public static RLColor DoorBackgroundFOV = Swatch.ComplimentDarker;
		public static RLColor DoorFov = Swatch.ComplimentLightest;
	}
}
