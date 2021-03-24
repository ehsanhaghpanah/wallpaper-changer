/**
 * Copyright (C) Ehsan Haghpanah, 2012.
 * All rights reserved.
 */

using System;

namespace Changer
{
	static public class Program
	{
		[STAThread]
		static public void Main(string[] args)
		{
			//DateTime[] l =
			//{
			//	new DateTime(2017, 6, 22), new DateTime(2017, 7, 16),
			//	new DateTime(2017, 7, 17), new DateTime(2017, 7, 20),
			//	new DateTime(2017, 7, 21), new DateTime(2017, 8, 13),
			//	new DateTime(2017, 8, 14), new DateTime(2017, 8, 16),
			//	new DateTime(2017, 8, 17), new DateTime(2017, 8, 30),
			//	new DateTime(2017, 8, 31), new DateTime(2017, 9, 9),
			//	new DateTime(2017, 9, 10), new DateTime(2017, 9, 25),
			//	new DateTime(2017, 9, 26), new DateTime(2017, 9, 27),
			//};

			//for (int i = 0; i < l.Length; i = i + 2)
			//{
			//	Console.WriteLine(l[i + 1].Subtract(l[i]).Days);
			//}

			var wallpaper = new Wallpaper();
			wallpaper.Set();

			//var l = wallpaper.Load();
			//var h = l[0].T;
			//for (int i = 0; i < l.Count - 1; i++)
			//{
			//	Console.WriteLine(l[i + 1].T - l[i].T);
			//}
		}
	}
}