// <copyright file="Euler.shared.cs" company="Spatial Focus">
// Copyright (c) Spatial Focus. All rights reserved.
// Licensed under Proprietary license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Focus.Apps.Common.Sensor
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