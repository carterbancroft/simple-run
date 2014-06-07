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

		public static void Delete(this Run run) {
			var positionIds = run.Positions.Select(p => p.ID).ToList();

			lock (Database.Main) {
				Database.Main.RunInTransaction(() => {
					Database.Main.Execute("delete from RunPosition where ID in (?);", string.Join(",", positionIds));
					Database.Main.Delete(run);
				});
			}
		}
	}
}

