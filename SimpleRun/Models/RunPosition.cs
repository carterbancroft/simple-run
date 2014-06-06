using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace SimpleRun.Models
{
	public class RunPosition
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double Speed { get; set; }
		public DateTime PositionCaptureTime { get; set; }

		[ForeignKey(typeof(Run))]
		public int RunID { get; set; }

		[ManyToOne]
		public Run Run { get; set; }
	}
}

