using System;
using System.Linq;
using System.Reflection;
using MelonLoader;
using KMod.Managers;

namespace KMod
{
	public class ConfigValue<T>
	{
		private readonly MelonPreferences_Entry<T> _entry;

		public T Value
		{
			get => _entry.Value;
			set => _entry.Value = value;
		}

		public T DefaultValue => _entry.DefaultValue;

		public event Action OnValueChanged;

		[Obsolete("Obsolete")]
		public ConfigValue(string name, T defaultValue, string displayName = null, string description = null, bool isHidden = false, string filePath = null)
		{
			if (!ConfigManager.Instances.ContainsKey(Assembly.GetCallingAssembly().Location))
			{
				throw new Exception("ConfigManager was not found. Please create it first.");
			}
			MelonPreferences_Category melonPreferences_Category = MelonPreferences.CreateCategory(ConfigManager.Instances[Assembly.GetCallingAssembly().Location]);
			if (filePath != null)
			{
				melonPreferences_Category.SetFilePath(filePath);
			}
			string identifier = string.Concat(Enumerable.Where(name, (char c) => char.IsLetter(c) || char.IsNumber(c)));
			_entry = melonPreferences_Category.GetEntry<T>(identifier) ?? melonPreferences_Category.CreateEntry(identifier, defaultValue, displayName, description, isHidden);
			_entry.OnValueChangedUntyped += delegate
			{
				this.OnValueChanged?.Invoke();
			};
		}

		public static implicit operator T(ConfigValue<T> conf)
		{
			return conf._entry.Value;
		}

		public void SetValue(T value)
		{
			_entry.Value = value;
			MelonPreferences.Save();
		}

		public override string ToString()
		{
			return _entry.Value.ToString();
		}
	}
}
