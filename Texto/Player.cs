using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaInterface;

namespace Texto
{
	public class Player
	{
		public LuaTable ItemTable;

		public Player(string luaPath)
		{
			ItemTable = (LuaTable)GameWorld.Lua.DoFile(luaPath)[0];
		}

		public List<Item> GetVisibleInRangeItems()
		{
			List<Item> itemList = new List<Item>();
			
			foreach (Item item in GameWorld.Items.Values)
			{
				if ((bool)item.ItemTable["visible"] == false)
					continue;

				bool canSeeInsideLocation;

				if ((string)item.ItemTable["type"] != "room")
				{
					string key = (string)item.ItemTable["location"];

					if (key == "player")
						canSeeInsideLocation = true;
					else
					{
						Item itemLocation = GameWorld.Items[key];
						canSeeInsideLocation = (bool)itemLocation.ItemTable["canSeeInside"];
					}
				}
				else
					canSeeInsideLocation = true;

				string playerInsideRoom = (string)ItemTable["insideRoom"];
				string itemInsideRoom = (string)item.ItemTable["insideRoom"];

				if ((playerInsideRoom == itemInsideRoom ||
					playerInsideRoom == item.Identifier ||
					itemInsideRoom == "player") && canSeeInsideLocation)
					itemList.Add(item);

				//TODO, think about wheter this should be here.
				//	If you can look at a room from afar, you could make the look command check your position.
				//	so for example if you look at the woods from afar, the look would be:
				//		"You see a bunch of trees in the distance.".
				//  But if youre in the woods:
				//		"There are trees everywhere!".
				//LuaTable currentRoom = GameWorld.luaGetItem((string)ItemTable["insideRoom"]);
				//if (item.Identifier == (string)currentRoom["north"] ||
				//	item.Identifier == (string)currentRoom["south"] ||
				//	item.Identifier == (string)currentRoom["west"] ||
				//	item.Identifier == (string)currentRoom["east"])
				//	itemList.Add(item);
			}

			return itemList;
		}
	}
}
