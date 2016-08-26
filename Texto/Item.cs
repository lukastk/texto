using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LuaInterface;

namespace Texto
{
	public class Item
	{
		public LuaTable ItemTable; //The LuaTable serving as the Item.
		public string Type;
		public string Identifier; //ItemTable.identifer
		public List<string> Names = new List<string>();
		public List<string> Actions = new List<string>(); //The functions that are in the ItemTable.

		public Item(string luaPath)
		{
			//Run script, it returns the object.
			ItemTable = (LuaTable)GameWorld.Lua.DoFile(luaPath)[0];
			Type = (string)ItemTable["type"];
			Identifier = (string)ItemTable["identifier"];

			if (ItemTable["names"] != null)
				foreach (string name in ((LuaTable)ItemTable["names"]).Values)
				{
					Names.Add(name);
				}

			//Get a list of all the "action" functions.
			foreach (string key in ItemTable.Keys)
			{
				if (ItemTable[key].GetType().ToString() == "LuaInterface.LuaFunction" &&
					key.StartsWith("action"))
				{
					Actions.Add(key.Substring("action".Length));
				}
			}
		}
	}
}
