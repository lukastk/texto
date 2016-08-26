using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaInterface;
using Texto.Parser;
using System.IO;

namespace Texto
{
	public delegate void ShowMessageFunction(string message);
	public delegate void AddMessageFunction(string message);
	public delegate void FlushMessageFunction();

	public class GameWorld //TODO rename
	{
		static GameWorld gameWorld = new GameWorld();

		private GameWorld() { }
		public static GameWorld Instance
		{
			get { return gameWorld; }
		}

		public static Lua Lua = new Lua(); //TODO If Lua doesnt need to be here, move it to game.
		public static Dictionary<string, Item> Items = new Dictionary<string, Item>(); //Holds both rooms and items.
		public static Player Player;

		public static bool NewEntry(string newEntry)
		{
			Sentence sentence = Parser.Parser.Parse(newEntry);

			bool checkShorthandsAgain = false;
			if (newEntry != "")
			{
				bool b = false;
				checkShorthandsAgain = CheckShorthands(newEntry, sentence, true, ref b);
			}

			#region Check Actions
			//Check for literal actions.
			if (newEntry != "")
			{
				foreach (Item item in Player.GetVisibleInRangeItems())
					foreach (string function in item.Actions)
						if (function.StartsWith("Lit"))
							foreach (string literalTrigger in
								((LuaTable)item.ItemTable["literalActions." + function.Substring("Lit".Length)]).Values)
								if (literalTrigger == newEntry)
								{
									((LuaFunction)item.ItemTable["action" + function]).Call(new object[] { item.ItemTable, newEntry });
									return true;
								}

			}

			if (sentence.Verb != "")
			{
				foreach (Item item in Player.GetVisibleInRangeItems())
				{
					string inputtedName = "";
					foreach (string adj in sentence.DirObject.Adjectives)
						inputtedName += adj + " ";
					inputtedName += sentence.DirObject.Name;

					string verb = Char.ToUpper(sentence.Verb[0]) + sentence.Verb.Substring(1);

					foreach (string name in item.Names)
					{
						if (name == inputtedName)
							if (item.ItemTable["action" + verb] != null)
							{
								((LuaFunction)item.ItemTable["action" + verb]).Call(new object[] { item.ItemTable, sentence });
								return true;
							}
					}
				}
			}

			if (sentence.Preposition != "")
			{
				foreach (Item item in Player.GetVisibleInRangeItems())
					foreach (string name in item.Names)
					{
						string inputtedName = "";
						foreach (string adj in sentence.IndObject.Adjectives)
							inputtedName += adj + " ";
						inputtedName += sentence.IndObject.Name;

						string verb = Char.ToUpper(sentence.Verb[0]) + sentence.Verb.Substring(1);
						string preposition = Char.ToUpper(sentence.Preposition[0]) + sentence.Preposition.Substring(1);

						if (name == inputtedName)
							if (item.ItemTable["actionInd" + verb + preposition] != null)
							{
								((LuaFunction)item.ItemTable["actionInd" + verb + preposition]).Call(new object[] { item.ItemTable, sentence });
								return true;
							}
					}
			}

			#endregion

			if (checkShorthandsAgain)
			{
				bool successful = false;
				CheckShorthands(newEntry, sentence, false, ref successful);

				return successful;
			}

			return false;
		}

		static bool CheckShorthands(string newEntry, Sentence sentence, bool firstTime, ref bool successful)
		{
			var triggers = (LuaTable)Lua["shorthands.triggers"];

			//Remove the first word (the trigger), and put the rest into a string (keyword).
			List<string> words = new List<string>(newEntry.Split(new char[] { ' ' }));
			string wordFirst = words[0];
			words.RemoveAt(0);
			string keyword = "";
			if (words.Count != 0)
			{
				foreach (string word in words)
					keyword += word + " ";
				keyword = keyword.Remove(keyword.Length - 1); //Remove the last useless " " at the end.
			}

			//Go through all the items to see if there is a match to the keyword, if there is no match it will not continue.
			//You have to do this because otherwise when for example "look at X" is inputted, and "look/" is a trigger,
			//then "at X" would be a keyword and it could lead to a call to the trigger function.
			//	Of course, if the keyword is an empty string this check is obsolete and it will continue.
			bool itemMatch = false;

			if (firstTime && keyword != "")
			{
				foreach (Item item in Items.Values)
					foreach (string name in item.Names)
						if (keyword == name)
							itemMatch = true;
			}
			else
				itemMatch = true;

			if (!itemMatch)
				return true;

			foreach (string shorthandKey in triggers.Keys)
			{
				foreach (string trigger in ((LuaTable)triggers[shorthandKey]).Values)
				{
					string trig = trigger; //If the trigger contains options, you have to seperate the options from the string.
					bool canTakeKeywords = false;

					if (trigger.Contains('/'))
					{
						canTakeKeywords = true;
						trig = trigger.Split(new[] { '/' })[0];
					}

					if (((canTakeKeywords && words.Count >= 1) || (!canTakeKeywords && words.Count == 0)) &&
						trig == wordFirst)
					{
						LuaFunction shorthandFunc = (LuaFunction)Lua["shorthands.functions." + shorthandKey];
						shorthandFunc.Call(new object[] { trigger, keyword, sentence });
						successful = true;
						return false;
					}
				}
			}

			successful = false;
			return false;
		}

		public static LuaTable luaGetItem(string item, bool hasToBeVisible = false)
		{
			if (item == "" || item == null)
				return null;

			if (item == "player")
				return Player.ItemTable;

			if (!Items.ContainsKey(item))
				return null;

			var itemTable = Items[item].ItemTable;

			if (hasToBeVisible)
			{
				var itemList = Player.GetVisibleInRangeItems();
				var tableList = from i in itemList select i.ItemTable;
				if (!tableList.Contains(itemTable))
					return null;
			}

			return itemTable;
		}
		public static LuaTable luaGetItemByName(string itemName, bool hasToBeVisible = false)
		{
			//TODO: check for multiple names that are the same.
			foreach (Item item in Items.Values)
			{
				foreach (string name in item.Names)
					if (name == itemName)
					{
						if (hasToBeVisible)
						{
							var itemList = Player.GetVisibleInRangeItems();
							var tableList = from i in itemList select i.ItemTable;
							if (!tableList.Contains(item.ItemTable))
								return null;
						}

						return item.ItemTable;
					}
			}

			return null;
		}

		public static void Initialize()
		{
			Lua.RegisterFunction("newEntry", Instance, Instance.GetType().GetMethod("NewEntry"));
			Lua.RegisterFunction("getItem", Instance, Instance.GetType().GetMethod("luaGetItem"));
			Lua.RegisterFunction("getItemByName", Instance, Instance.GetType().GetMethod("luaGetItemByName"));

			Lua.DoFile(Environment.CurrentDirectory + "/base/shorthands.lua");
			Lua.DoFile(Environment.CurrentDirectory + "/base/init.lua");
			Player = new Player(Environment.CurrentDirectory + "/base/player.lua");
			Lua.DoFile(Environment.CurrentDirectory + "/base/itemBase.lua");
			Lua.DoFile(Environment.CurrentDirectory + "/base/keyBase.lua");
			Lua.DoFile(Environment.CurrentDirectory + "/base/doorBase.lua");
			Lua.DoFile(Environment.CurrentDirectory + "/base/roomBase.lua");
			Words.Initialize(Environment.CurrentDirectory + "/game");

			//Initialize all the rooms and items. Go through the directory "game".
			var gameDir = new DirectoryInfo(Environment.CurrentDirectory + "/game");
			List<DirectoryInfo> directories = gameDir.GetDirectories().ToList();
			directories.Add(gameDir);
			foreach (var dir in directories)
			{
				var itemList = new List<Item>();
				foreach (var file in dir.GetFiles())
				{
					if (file.Name == "init.lua")
						continue;

					if (file.Extension.ToLower() == ".lua")
						itemList.Add(new Item(file.FullName));
					else if (file.Extension.ToLower() == ".ext")
						Lua.DoFile(file.FullName);
				}

				foreach (Item item in itemList)
					GameWorld.Items.Add(item.Identifier, item);
			}

			Words.GetItemNames();

			//Initialize all the items.
			foreach (Item item in GameWorld.Items.Values)
			{
				((LuaFunction)item.ItemTable["init"]).Call(new object[] { item.ItemTable });
			}
		}
		public static void SetShowMessageFunction(ShowMessageFunction func)
		{
			Lua.RegisterFunction("showMessage", Instance, func.Method);
		}
		public static void SetAddMessageFunction(AddMessageFunction func)
		{
			Lua.RegisterFunction("addMessage", Instance, func.Method);
		}
		public static void SetFlushMessageFunction(FlushMessageFunction func)
		{
			Lua.RegisterFunction("flushMessage", Instance, func.Method);
		}

		public static void Start()
		{
			Lua.DoFile(Environment.CurrentDirectory + "/game/init.lua");
		}
	}
}
