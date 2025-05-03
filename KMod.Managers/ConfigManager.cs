using System;
using System.Collections.Generic;
using System.Reflection;

namespace KMod.Managers
{
	public class ConfigManager
	{
		public static readonly Dictionary<string, string> Instances = new Dictionary<string, string>();

		public ConfigManager(string categoryName)
		{
			if (Instances.ContainsValue(categoryName) || Instances.ContainsKey(Assembly.GetCallingAssembly().Location))
			{
				throw new Exception("ConfigManager already exists.");
			}
			Instances.Add(Assembly.GetCallingAssembly().Location, categoryName);
		}
	}
}
