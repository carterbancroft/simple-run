using System;
using System.Linq;
using System.Collections.Generic;
using SimpleRun.DataAccess;
using SimpleRun.Models;

namespace SimpleRun.Extensions
{
	public static class RunExtensions
	{
		public static void Create(this Run newRun, List<RunPosition> newPositions = null) {
			lock (Database.Main) {
				Database.Main.RunInTransaction(() => {
					Database.Main.Insert(newRun);

					if (newPositions == null || newPositions.Count == 0)
						return;
						
					newPositions.ForEach(p => p.RunID = newRun.ID);

					Database.Main.InsertAll(newPositions);
				});
			}
		}
	}
}

