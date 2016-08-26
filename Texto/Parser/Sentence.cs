using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Texto.Parser
{
	public class Sentence
	{
		public string Verb = "";
		public List<string> Adverbs = new List<string>();
		public string Preposition = "";
		public GrammarObject DirObject = new GrammarObject();
		public GrammarObject IndObject = new GrammarObject();
		public string VerbModifier = "";
		public GrammarObject VerbModifierObject = new GrammarObject();
	}

	public class GrammarObject
	{
		public string Name = "";
		public List<string> Adjectives = new List<string>();
	}
}