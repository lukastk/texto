using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaInterface;

namespace Texto
{
	public static class Debug
	{
		public static List<string> DebugText = new List<string>();
		static string currentLine;
		static int _indiceLevel;
		public static readonly string ic = "  "; //Indice Character TODO:change with config.

		static int indiceLevel
		{
			get { return indiceLevel; }
			set 
			{
				_indiceLevel = value;

				if (_indiceLevel < 0)
					_indiceLevel = 0;
			}
		}

		//Replaces field: currentLine with parameter: text.
		public static void SetCurrentLine(string text)
		{
			currentLine = text;
		}
		//Adds text to the current line.
		public static void AddText(string text)
		{
			currentLine += text;
		}
		//Finishes the current line.
		public static void FinishLine()
		{
			currentLine = GC(indiceLevel) + currentLine;

			DebugText.Add(currentLine);
			currentLine = "";
		}
		//Finishes the current line and also adds an indice to the indice-level.
		public static void FinishHeadLine()
		{
			currentLine = GC(indiceLevel) + currentLine;

			DebugText.Add(currentLine);
			currentLine = "";
			indiceLevel += 1;
		}
		//Finishes the current and also takes away an indice from the indice-level.
		public static void FinishLineAndSection()
		{
			currentLine = GC(indiceLevel) + currentLine;

			DebugText.Add(currentLine);
			currentLine = "";
			indiceLevel -= 1;
		}

		public static void AddLine(string text)
		{
			text = GC(indiceLevel) + text;

			DebugText.Add(text);
		}
		public static void AddLines(string text)
		{
			string[] lines = text.Split(new char[] { '\n' });

			foreach (string line in lines)
				AddLine(line);
		}

		public static string GetAllItemAndRoomInformation()
		{
			string info = "";

			//TODO Remember to do the bases too.

			//Player
			info += "Player:" + "\n";
			info += GetItemLuaTableInfo(GameWorld.Player.ItemTable, 0);

			//Normal items and rooms.
			foreach (Item item in GameWorld.Items.Values)
			{
				info += GetInfo(item, 0);
				info += "\n";
			}

			info += "Note: The grouping of fields and methods into inherited and not inherited is entirely based on their naming.";
			return info;
		}

		public static string GetInternalCSharpInfo(Item item, int gcAdd)
		{
			string info = "";

			info += GC(gcAdd + 1) + "CSharp internal object fields (" + item.Identifier + "):" + "\n";
			info += GC(gcAdd + 2) + "Identifier: " + item.Identifier + "\n";

			info += GC(gcAdd + 2) + "Names: {";
			foreach (string name in item.Names)
				info += name + ", ";
			if (info.Substring(info.Length - ", ".Length) == ", ")
				info = info.Remove(info.LastIndexOf(", "));
			info += "}" + "\n";

			info += GC(gcAdd + 2) + "Functions: {";
			foreach (string name in item.Actions)
				info += name + ", ";
			if (info.Substring(info.Length - ", ".Length) == ", ")
				info = info.Remove(info.LastIndexOf(", "));
			info += "}" + "\n";

			return info;
		}

		public static string GetItemLuaTableInfo(LuaTable luaTable, int gcAdd)
		{
			string info = "";

			dynamic itemTable = luaTable;
			List<string> functions = new List<string>();
			List<string> fields = new List<string>();
			List<string> inheritedFunctions = new List<string>();
			List<string> inheritedFields = new List<string>();
			info += GC(gcAdd + 1) + "Lua internal object (" + (string)itemTable["identifier"] + "):" + "\n";
			foreach (string key in itemTable.Keys)
			{
				string valueType = itemTable[key].GetType().ToString();

				string line = key + " = ";
				bool isField;

				switch (valueType)
				{
					case "System.String":
						line += itemTable[key] + "; type: " + valueType + "\n";
						isField = true;
						break;
					case "System.Double":
						line += itemTable[key] + "\"" + "; type: " + valueType + "\n";
						isField = true;
						break;
					case "System.Boolean":
						line += (bool)itemTable[key] + "; type: " + valueType + "\n";
						isField = true;
						break;
					case "LuaInterface.LuaTable":
						line += "{";
						dynamic table = itemTable[key];
						foreach (dynamic value in table.Values)
						{
							if (value.GetType().ToString() == "System.String" ||
								value.GetType().ToString() == "System.Double" ||
								value.GetType().ToString() == "System.Boolean")
								line += "\"" + value + "\", ";
							else
							{
								if (value.GetType().ToString() == "LuaInterface.LuaTable" &&
									value["identifier"] != null)
									line += value.GetType().ToString() + ":\"" + value["identifier"] + "\", ";
								else
									line += value.GetType().ToString() + ", ";
							}
						}

						if (line.Substring(line.Length - ", ".Length) == ", ")
							line = line.Remove(line.LastIndexOf(", "));
						line += "}" + "; type: " + valueType + "\n";
						isField = true;
						break;
					case "LuaInterface.LuaFunction":
						line += "lua function" + "\n";
						isField = false;
						break;
					default:
						line += "Non-string formatable type: " + valueType + "\n";
						isField = true;
						break;
				}

				if (isField)
					if (line[0] == '_')
						inheritedFields.Add(line);
					else
						fields.Add(line);
				else
					if (line[0] == '_')
						inheritedFunctions.Add(line);
					else
						functions.Add(line);
			}

			info += GC(gcAdd + 2) + "Fields:" + "\n";
			foreach (string line in fields)
				info += GC(gcAdd + 3) + line;
			info += GC(gcAdd + 2) + "Methods:" + "\n";
			foreach (string line in functions)
				info += GC(gcAdd + 3) + line;
			info += GC(gcAdd + 2) + "Inherited Fields:" + "\n";
			foreach (string line in inheritedFields)
				info += GC(gcAdd + 3) + line;
			info += GC(gcAdd + 2) + "Inherited Methods:" + "\n";
			foreach (string line in inheritedFunctions)
				info += GC(gcAdd + 3) + line;

			return info;
		}

		public static string GetInfo(Item item, int gcAdd)
		{
			string info = "";

			if (item.Type == "room")
				info += "Room";
			else
				info += "Item";

			info += " " + item.Identifier + ":" + "\n";

			info += GetInternalCSharpInfo(item, gcAdd);
			info += GetItemLuaTableInfo(item.ItemTable, gcAdd);

			return info;
		}

		//Gets a string with the specified amount of indices.
		public static string GC(int indiceNumber)
		{
			string str = "";
			for (int i = 0; i < indiceNumber; i++)
				str += ic;

			return str;
		}
	}
}
