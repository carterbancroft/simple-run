using System;
using SimpleRun.Models;

namespace SimpleRun
{
	public class UnitUtility
	{
		public static string DistanceUnitString
		{
			get {
				if (Settings.MeasurementType == DistanceUnit.Kilometers)
					return "km";

				return "mi";
			}
		}

		public static double ConversionValue
		{
			get {
				if (Settings.MeasurementType == DistanceUnit.Kilometers)
					return 0.001;

				return 0.000621371;
			}
		}
	}
}

