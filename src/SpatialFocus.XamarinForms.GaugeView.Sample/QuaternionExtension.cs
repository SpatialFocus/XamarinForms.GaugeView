// <copyright file="QuaternionExtension.shared.cs" company="Spatial Focus">
// Copyright (c) Spatial Focus. All rights reserved.
// Licensed under Proprietary license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Focus.Apps.Common.Sensor
{
	using System;
	using System.Numerics;

	public static class QuaternionExtension
	{
		public static Euler ToEuler(this Quaternion quaternion)
		{
			float[] rotationMatrix = quaternion.ToOrientation();

			return new Euler(rotationMatrix[2].ToDegrees(), rotationMatrix[1].ToDegrees(), rotationMatrix[0].ToDegrees());
		}

		// See https://github.com/xamarin/XobotOS/blob/master/android/upstream/android/hardware/SensorManager.java#L1600
		public static float[] ToOrientation(this Quaternion quaternion)
		{
			float[] rotationVector = quaternion.ToRotationMatrix();

			return new[]
			{
				(float)Math.Atan2(rotationVector[1], rotationVector[5]), (float)Math.Asin(-rotationVector[9]),
				(float)Math.Atan2(-rotationVector[8], rotationVector[10]),
			};
		}

		private static double ToDegrees(this float rad) => rad / Math.PI * 180.0;

		// See https://github.com/xamarin/XobotOS/blob/master/android/upstream/android/hardware/SensorManager.java#L1987
		private static float[] ToRotationMatrix(this Quaternion quaternion)
		{
			float[] result = new float[16];

			float q1 = quaternion.X;
			float q2 = quaternion.Y;
			float q3 = quaternion.Z;
			float q0 = quaternion.W;

			float sqQ1 = 2 * q1 * q1;
			float sqQ2 = 2 * q2 * q2;
			float sqQ3 = 2 * q3 * q3;
			float q1Q2 = 2 * q1 * q2;
			float q3Q0 = 2 * q3 * q0;
			float q1Q3 = 2 * q1 * q3;
			float q2Q0 = 2 * q2 * q0;
			float q2Q3 = 2 * q2 * q3;
			float q1Q0 = 2 * q1 * q0;

			result[0] = 1 - sqQ2 - sqQ3;
			result[1] = q1Q2 - q3Q0;
			result[2] = q1Q3 + q2Q0;
			result[3] = 0.0f;

			result[4] = q1Q2 + q3Q0;
			result[5] = 1 - sqQ1 - sqQ3;
			result[6] = q2Q3 - q1Q0;
			result[7] = 0.0f;

			result[8] = q1Q3 - q2Q0;
			result[9] = q2Q3 + q1Q0;
			result[10] = 1 - sqQ1 - sqQ2;
			result[11] = 0.0f;

			result[12] = result[13] = result[14] = 0.0f;
			result[15] = 1.0f;

			return result;
		}
	}
}