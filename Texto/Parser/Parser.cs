using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Texto.Parser
{
	public static class Parser
	{
		//TODO: sometimes adverbs can be adjectives.
		//TODO: Names like "James Douglas".

		public static Sentence Parse(string stringSentence)
		{
			stringSentence = stringSentence.ToLower();
			//"say \"x\" to him"
			string[] sentenceArray;
			if (stringSentence.Count((char c) => c == '\"') == 2)
			{
				string quote = stringSentence.Substring(stringSentence.IndexOf('\"'), stringSentence.LastIndexOf('\"') - stringSentence.IndexOf('\"') + 1);
				quote = quote.Replace("\"", "");
				string sentencePart1 = stringSentence.Substring(0, stringSentence.IndexOf('\"'));
				string sentencePart2 = stringSentence.Substring(stringSentence.LastIndexOf('\"') + 1);
				List<string> sentenceList = new List<string>();
				sentenceList.AddRange(sentencePart1.Split(new[] { ' ' }));
				sentenceList.Add(quote);
				sentenceList.AddRange(sentencePart2.Split(new[] { ' ' }));
				for (int i = 0; i < sentenceList.Count; i++)
					if (sentenceList[i] == "" || sentenceList[i] == " ")
						sentenceList.RemoveAt(i);
				sentenceArray = sentenceList.ToArray();
			}
			else
				sentenceArray = stringSentence.Split(new[] { ' ' });

			var sentence = new Sentence();
			//string[] sentenceArray = stringSentence.Split(new[] {' '});

			var takenWords = new List<int>();

			sentence.Verb = sentenceArray[0];
			takenWords.Add(0);

			sentence = FindObjects(sentenceArray, takenWords, sentence);
			sentence = FindModifier(sentenceArray, takenWords, sentence);
			sentence = FindAdverbs(sentenceArray, takenWords, sentence);

			return sentence;
		}

		public static Sentence FindModifier(string[] sentenceArray, List<int> takenWords, Sentence sentence)
		{
			//Find verb modifier
			for (int i = 1; i < sentenceArray.Length; i++)
			{
				if (takenWords.Contains(i))
					continue;

				if (Words.FindTypeOfItem(sentenceArray[i]).HasFlag(WordTypes.Modifier))
				{
					sentence.VerbModifier = sentenceArray[i];

					takenWords.Add(i);

					for (int n = i + 1; n < sentenceArray.Length; n++)
					{
						if (takenWords.Contains(n))
							continue;

						if (sentenceArray[n] == "the" || sentenceArray[n] == "and")
						{
							takenWords.Add(n);
							continue;
						}

						if (n == sentenceArray.Length - 1 || Words.FindTypeOfItem(sentenceArray[n + 1]) != WordTypes.Other)
						{
							sentence.VerbModifierObject.Name = sentenceArray[n];
							takenWords.Add(n);
							break;
						}

						sentence.VerbModifierObject.Adjectives.Add(sentenceArray[n]);
						takenWords.Add(n);
					}

					//if (sentenceArray.Length > i && sentenceArray[i + 1] == "the")
					//{
					//    takenWords.Add(i + 1);

					//    for (int n = i + 1; n < sentenceArray.Length; n++)
					//    {
					//        if (takenWords.Contains(n))
					//            continue;

					//        if (sentenceArray[n] == "and")
					//        {
					//            takenWords.Add(n);
					//            continue;
					//        }

					//        //If the name/pronoun/noun exists exit loop.
					//        if (Words.DoesNameExist(sentenceArray[n]))
					//        {
					//            sentence.VerbModifierObject.Name = sentenceArray[n];
					//            takenWords.Add(n);
					//            break;
					//        }

					//        sentence.VerbModifierObject.Adjectives.Add(sentenceArray[n]);
					//        takenWords.Add(n);
					//    }

					//    break;
					//}
					//if (sentenceArray.Length > i && Words.FindTypeOfItem(sentenceArray[i + 1]) == "other")
					//{
					//    sentence.VerbModifierObject.Name = sentenceArray[i + 1];
					//    takenWords.Add(i + 1);
					//    break;
					//}
				}

			}

			return sentence;
		}

		public static Sentence FindObjects(string[] sentenceArray, List<int> takenWords, Sentence sentence)
		{
			//Find preposition and the indirect object after it.
			for (int i = 1; i < sentenceArray.Length; i++)
			{
				if (takenWords.Contains(i))
					continue;

				if (Words.FindTypeOfItem(sentenceArray[i]).HasFlag(WordTypes.Preposition))
				{
					takenWords.Add(i);
					sentence.Preposition = sentenceArray[i];

					for (int n = i + 1; n < sentenceArray.Length; n++)
					{
						if (takenWords.Contains(n))
							continue;

						if (sentenceArray[n] == "the" || sentenceArray[n] == "and")
						{
							takenWords.Add(n);
							continue;	
						}

						if (n == sentenceArray.Length - 1 || Words.FindTypeOfItem(sentenceArray[n + 1]) != WordTypes.Other)
						{
							sentence.IndObject.Name = sentenceArray[n].ToLower();
							takenWords.Add(n);
							break;
						}

						sentence.IndObject.Adjectives.Add(sentenceArray[n]);
						takenWords.Add(n);
					}
				}
			}

			//The final object should be the direct object.
			var dirObj = new GrammarObject();
			for (int n = 0; n < sentenceArray.Length; n++)
			{
				if (takenWords.Contains(n))
					continue;

				if (sentenceArray[n] == "the" || sentenceArray[n] == "and")
				{
					takenWords.Add(n);
					continue;
				}

				if (n == sentenceArray.Length - 1 || Words.FindTypeOfItem(sentenceArray[n + 1]) != WordTypes.Other ||
					sentenceArray[n + 1] == "the")
				{
					dirObj.Name = sentenceArray[n];
					takenWords.Add(n);
					break;
				}

				dirObj.Adjectives.Add(sentenceArray[n]);
				takenWords.Add(n);
			}

			//If there was no preposition, and also therefore no indirect object before it, but still the verb
			//can take a indirect object, the indirect object could be directly after the verb.
			//As in: "Give him the object". Which would mean that dirObj is actually the indirect object.
			//So now we have to check (if there was no indirect object detected.) if there is another object
			//to be discovered, if there is, that object is the direct, and dirObj the indirect.
			if (sentence.IndObject.Name == null)
			{
				for (int n = 0; n < sentenceArray.Length; n++)
				{
					if (takenWords.Contains(n))
						continue;

					if (sentenceArray[n] == "the" || sentenceArray[n] == "and")
					{
						takenWords.Add(n);
						continue;
					}

					if (n == sentenceArray.Length - 1 || Words.FindTypeOfItem(sentenceArray[n + 1]) != WordTypes.Other ||
						sentenceArray[n + 1] == "the")
					{
						sentence.DirObject.Name = sentenceArray[n];
						takenWords.Add(n);
						break;
					}

					sentence.DirObject.Adjectives.Add(sentenceArray[n]);
					takenWords.Add(n);
				}

				if (sentence.DirObject.Name != null)
					sentence.IndObject = dirObj;
				else
					sentence.DirObject = dirObj;
			}
			else
				sentence.DirObject = dirObj;

			return sentence;
		}

		public static Sentence FindAdverbs(string[] sentenceArray, List<int> takenWords, Sentence sentence)
		{
			for (int i = 1; i < sentenceArray.Length; i++)
			{
				if (takenWords.Contains(i))
					continue;
				sentence.Adverbs.Add(sentenceArray[i]);
			}

			return sentence;
		}
	}
}
