/**
 * Copyright (C) Ehsan Haghpanah, 2012.
 * All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;

namespace Changer
{
	public enum RunningMode
	{
		AsConsole,
		AsService
	}

	static public class Configuration
	{
		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public static RunningMode Mode
		{
			get
			{
				const string title = "RunningMode";
				var value = ConfigurationManager.AppSettings[title];
				if (value == null)
				{
					logger.Warn("ConfigurationIsMissing:{0}|DefaultSettingWasUsed(RunningMode=AsConsole)", title);
					return RunningMode.AsConsole;
				}

				if (value.Equals("AsConsole", StringComparison.OrdinalIgnoreCase))
					return RunningMode.AsConsole;
				if (value.Equals("AsService", StringComparison.OrdinalIgnoreCase))
					return RunningMode.AsService;

				logger.Warn("ConfigurationIncorrect:{0}|DefaultSettingWasUsed(RunningMode=AsConsole)", title);
				return RunningMode.AsConsole;
			}
		}

		public static string SourceImagePath
		{
			get
			{
				const string title = "SourceImagePath";
				var value = ConfigurationManager.AppSettings[title];
				if (value == null)
					throw new Exception($"ConfigurationIsMissing:{title}");

				return value;
			}
		}

		public static string SourceImageName
		{
			get
			{
				const string title = "SourceImageName";
				var value = ConfigurationManager.AppSettings[title];
				if (value == null)
					throw new Exception($"ConfigurationIsMissing:{title}");

				return value;
			}
		}

		public static string TargetImageName
		{
			get
			{
				const string title = "TargetImageName";
				var value = ConfigurationManager.AppSettings[title];
				if (value == null)
					throw new Exception($"ConfigurationIsMissing:{title}");

				return value;
			}
		}

		public static DateTime StartDate
		{
			get
			{
				const string title = "StartDate";
				var value = ConfigurationManager.AppSettings[title];
				if (value == null)
				{
					logger.Warn("ConfigurationIsMissing:{0}|DefaultSettingWasUsed(StartDate=(2017, 6, 22))", title);
					return new DateTime(2017, 6, 22);
				}

				try
				{
					var l = (Dictionary<string, string>) JsonConvert.DeserializeObject(value, typeof (Dictionary<string, string>));
					var flag = true;
					flag = flag && l.ContainsKey("Y") && !string.IsNullOrEmpty(l["Y"]);
					flag = flag && l.ContainsKey("M") && !string.IsNullOrEmpty(l["M"]);
					flag = flag && l.ContainsKey("D") && !string.IsNullOrEmpty(l["D"]);
					if (!flag)
					{
						logger.Warn("ConfigurationIncorrect:{0}|DefaultSettingWasUsed(StartDate=(2017, 6, 22))", title);
						return new DateTime(2017, 6, 22);
					}

					return new DateTime(int.Parse(l["Y"]), int.Parse(l["M"]), int.Parse(l["D"]));
				}
				catch (Exception p)
				{
					logger.Warn("ConfigurationIncorrect:{0}|DefaultSettingWasUsed(StartDate=(2017, 6, 22))|Exception:{1}", title, p);
					return new DateTime(2017, 6, 22);
				}
			}
		}
	}
}
