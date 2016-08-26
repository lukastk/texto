using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LuaInterface;

namespace Texto.Parser
{
	[Flags()]
	public enum WordTypes
	{
		None = 0x0,
		Modifier = 0x1,
		Preposition = 0x2,
		Adverb = 0x3,
		Other = 0x4
	}

	public static class Words
	{
		public static List<string> PronounList = new List<string>();
		public static List<string> ModifierList = new List<string>();
		public static List<string> PrepositionList = new List<string>();
		public static List<string> AdverbList = new List<string>();
		public static List<string> NameList = new List<string>(); //List of names and nouns.

		public static void Initialize(string gameDir)
		{
			//Read the pronouns, modifiers, prepositions off a list.
			var fs = new FileStream(gameDir + "\\wordList.txt", FileMode.Open, FileAccess.Read);
			TextReader tr = new StreamReader(fs);
			string file = tr.ReadToEnd();
			file = file.Replace("\r\n", "");
			string[] fileArray = file.Split(new []{'('});
			foreach (string wordList in fileArray)
			{
				if (wordList == "")
					continue;

				List<string> words = null;
				if (wordList.StartsWith("Pronouns"))
				{
					words = PronounList;
				}
				else if (wordList.StartsWith("Modifiers"))
				{
					words = ModifierList;
				}
				else if (wordList.StartsWith("Prepositions"))
				{
					words = PrepositionList;
				}
				else if (wordList.StartsWith("Adverbs"))
				{
					words = AdverbList;
				}

				foreach (string word in wordList.Split(new []{ ')' })[1].Split(new []{ ';' }))
				{
					if (word == "")
						continue;
					words.Add(word);
				}
			}
		}

		public static void GetItemNames()
		{
			//Goes through Settings.GameWorld.ItemsList (and RoomList) for names and nouns.
			
			foreach (Item item in GameWorld.Items.Values)
			{
				NameList.AddRange(item.Names);
			}
		}

		public static WordTypes FindTypeOfItem(string word)
		{
			//Finds out the type of the word given. Or more precisely, if the word is a "object" type word,
			//in other words a pronoun/noun/name or some thing else. It returns other if it is a "object" word.

			WordTypes wordType = WordTypes.None;

			if (ModifierList.Contains(word))
				wordType = wordType | WordTypes.Modifier;
			if (PrepositionList.Contains(word))
				wordType = wordType | WordTypes.Preposition;
			if (AdverbList.Contains(word))
				wordType = wordType | WordTypes.Adverb;

			if (wordType == WordTypes.None)
				return WordTypes.Other;
			else
				return wordType;
		}

		public static bool DoesNameExist(string word)
		{
			//Finds if the words is a pronoun/noun/name.

			if (PronounList.Contains(word) || NameList.Contains(word))
				return true;

			return false;
		}
	}

	public struct ItemName
	{
		public string Noun;
		public List<string> Adjectives;
	}
}
