using System;

namespace KMod
{
	public class ComponentPriority : Attribute
	{
		public int Priority;

		public ComponentPriority(int priority = 0)
		{
			Priority = priority;
		}
	}
}
