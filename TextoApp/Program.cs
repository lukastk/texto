using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Texto;
using LuaInterface;

namespace TextoApp
{
	class Program
	{
		static void Main(string[] args)
		{
			GameWorld.Initialize();
			GameWorld.SetShowMessageFunction(ShowMessage);
			GameWorld.SetAddMessageFunction(AddMessage);
			GameWorld.SetFlushMessageFunction(FlushMessage);
			GameWorld.Start();

			while (true)
			{
				Console.Write(">> ");
				GameWorld.NewEntry(Console.ReadLine());
			}
		}

		static void ShowMessage(string text)
		{
			Console.WriteLine(text);
		}
		static string message = "";
		static void AddMessage(string text)
		{
			message += text;
		}
		static void FlushMessage()
		{
			Console.WriteLine(message);
			message = "";
		}
	}
}
