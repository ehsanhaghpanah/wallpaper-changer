/**
 * Copyright (C) Ehsan Haghpanah, 2012.
 * All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Changer
{
	public class Sentence
	{
		public string Text { get; set; }
		public double Size { get; set; }
		public string Days { get; set; }

		public int T { get; set; }
		public int L { get; set; }
		public int W { get; set; }
		public int H { get; set; }
		public int D { get; set; }

		public bool V { get; set; }

		public bool IsVisible(int days)
		{
			if (Days.Equals("00"))
				return true;

			var leng = int.Parse(Days);
			return leng >= days;
		}
	}

	public class Wallpaper
	{
		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		private static readonly Random random = new Random((int) DateTime.Now.Ticks);

		const int SPI_SETDESKWALLPAPER = 20;
		const int SPIF_UPDATEINIFILE = 0x01;
		const int SPIF_SENDWININICHANGE = 0x02;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

		public void Set()
		{
			try
			{
				Calc();
				SystemParametersInfo(
					SPI_SETDESKWALLPAPER, 
					0, 
					Configuration.TargetImageName, 
					SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE
				);
			}
			catch (Exception p)
			{
				logger.Error(p);
			}
		}

		private void Calc()
		{
			try
			{
				var l = Load();

				var date_a = Configuration.StartDate;
				var date_b = DateTime.Today;
				var span_t = date_b.Subtract(date_a);

				var x = l.Select(o => int.Parse(o.Days)).Max();
				x = x - span_t.Days;
				x = x < 0 ? 0 : x;

				var y = l.Select(o => int.Parse(o.Days)).Sum();
				y = y - span_t.Days;
				y = y < 0 ? 0 : y;

				var n = GetImageName();
				var b = new Bitmap(n ?? Configuration.SourceImageName);

				var t = l.First().T;
				foreach (var s in l)
				{
					if (!s.V)
						continue;

					if (!s.IsVisible(span_t.Days))
						continue;

					t = t + s.D;

					if (s.Text.Equals("{0}")) s.Text = date_a.ToLongDateString();
					if (s.Text.Equals("{1}")) s.Text = date_b.ToLongDateString();
					if (s.Text.Equals("{2}")) s.Text = "(" + span_t.Days + ")";
					//if (s.Text.Equals("{2}")) s.Text = "(" + span_t.Days + ", " + x + ", " + y + ")";

					Draw(b, s.Text, new RectangleF(s.L, s.T, s.W, s.H), (float)s.Size);
					//Draw(b, s.Text, new RectangleF(s.L, t, s.W, s.H), (float) s.Size);
				}

				b.Save(Configuration.TargetImageName, ImageFormat.Jpeg);
			}
			catch (Exception p)
			{
				logger.Error(p);
			}
		}

		private void Draw(Bitmap b, string t, RectangleF r, float z)
		{
			using (var g = Graphics.FromImage(b))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				var f = new StringFormat
				{
					Alignment = StringAlignment.Far,
					LineAlignment = StringAlignment.Center
				};
				g.DrawString(t, new Font("Segoe UI Light", z, FontStyle.Regular), Brushes.White, r, f);
			}
		}

		private string GetImageName()
		{
			try
			{
				var list = Directory.GetFiles(Configuration.SourceImagePath);
				var numb = random.Next(0, list.Length);
				return list[numb];
			}
			catch (Exception p)
			{
				logger.Error(p);
				return null;
			}
		}

		private void Save(List<Sentence> list)
		{
			try
			{
				var f = new FileStream(@"data.txt", FileMode.CreateNew);
				var w = new StreamWriter(f);
				w.WriteLine(JsonConvert.SerializeObject(list, Formatting.Indented));
				w.Flush();
				w.Close();
			}
			catch (Exception p)
			{
				logger.Error(p);
			}
		}

		public List<Sentence> Load()
		{
			var list = new List<Sentence>();
			try
			{
				var f = new FileStream(@"sentences.txt", FileMode.OpenOrCreate);
				var r = new StreamReader(f);
				list = (List<Sentence>) JsonConvert.DeserializeObject(r.ReadToEnd(), typeof (List<Sentence>));
				r.Close();
			}
			catch (Exception p)
			{
				logger.Error(p);
			}

			return list;
		}
	}
}