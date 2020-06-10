// <copyright file="DoubleExtensions.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.XamarinForms.GaugeView
{
	using System;

	internal static class DoubleExtensions
	{
		public static bool Emphasize(this double current, double partitionSize, int partitionGroupSize) =>
			(int)Math.Round(current / partitionSize) % partitionGroupSize == 0;

		public static double Nearest(this double current, double partitionSize) => Math.Round(current / partitionSize) * partitionSize;

		public static double Next(this double current, double partitionSize) => Math.Ceiling(current / partitionSize) * partitionSize;

		public static double Previous(this double current, double partitionSize) => Math.Floor(current / partitionSize) * partitionSize;
	}
}