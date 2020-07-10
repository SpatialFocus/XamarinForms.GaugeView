// <copyright file="Euler.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.XamarinForms.GaugeView.Sample
{
	public class Euler
	{
		public Euler(double roll, double pitch, double yaw)
		{
			Roll = roll;
			Pitch = pitch;
			Yaw = yaw;
		}

		public double Pitch { get; }

		public double Roll { get; }

		public double Yaw { get; }
	}
}