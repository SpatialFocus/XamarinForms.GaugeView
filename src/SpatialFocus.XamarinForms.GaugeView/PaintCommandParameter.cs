// <copyright file="PaintCommandParameter.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.XamarinForms.GaugeView
{
	using SkiaSharp;

	internal class PaintCommandParameter
	{
		public PaintCommandParameter(SKImageInfo info, SKSurface surface)
		{
			Info = info;
			Surface = surface;
		}

		public SKImageInfo Info { get; }

		public SKSurface Surface { get; }
	}
}