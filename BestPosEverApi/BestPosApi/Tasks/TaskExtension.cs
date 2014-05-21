using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simpler;

namespace WebApplication1.Tasks
{
	public static class TaskExtension
	{
		public static T ExecuteMe<T> (this T task) where T : SimpleTask
		{
			task.Execute();
			return task;
		}
	}
}