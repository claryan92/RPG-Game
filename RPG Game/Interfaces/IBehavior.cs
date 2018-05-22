using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Core;
using RPG_Game.Systems;

namespace RPG_Game.Interfaces
{
	public interface IBehavior
	{
		bool Act(Monster monster, CommandSystem commandSystem);
	}
}
