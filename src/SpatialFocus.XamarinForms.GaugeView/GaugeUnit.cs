// <copyright file="GaugeUnit.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.XamarinForms.GaugeView
{
	public class GaugeUnit
	{
		public enum UnitEnum
		{
			Degrees = 1,
			Gradians,
			Mils,
			Custom,
		}

		public int Max { get; protected set; }

		public UnitEnum Unit { get; protected set; }

		public static GaugeUnit Custom(int max)
		{
			return new GaugeUnit() { Max = max, Unit = UnitEnum.Custom };
		}

		public static GaugeUnit Degrees()
		{
			return new GaugeUnit() { Max = 360, Unit = UnitEnum.Degrees };
		}

		public static GaugeUnit Gradians()
		{
			return new GaugeUnit() { Max = 400, Unit = UnitEnum.Gradians };
		}

		public static GaugeUnit Mils()
		{
			return new GaugeUnit() { Max = 6400, Unit = UnitEnum.Mils };
		}

		public double Normalize(double degree)
		{
			return ((degree % Max) + Max) % Max;
		}

		public double NormalizedDifference(double angle1, double angle2)
		{
			double difference = Normalize(angle1 - angle2);

			// ReSharper disable once PossibleLossOfFraction
			if (difference > Max / 2)
			{
				difference -= Max;
			}

			// ReSharper disable once PossibleLossOfFraction
			if (difference < -Max / 2)
			{
				difference += Max;
			}

			return difference;
		}
	}
}