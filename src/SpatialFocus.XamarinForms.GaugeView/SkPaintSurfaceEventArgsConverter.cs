// <copyright file="SkPaintSurfaceEventArgsConverter.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.XamarinForms.GaugeView
{
	using System;
	using System.Globalization;
	using SkiaSharp.Views.Forms;
	using Xamarin.Forms;

	internal class SkPaintSurfaceEventArgsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is SKPaintSurfaceEventArgs itemTappedEventArgs))
			{
				throw new ArgumentException("Expected value to be of type ItemTappedEventArgs", nameof(value));
			}

			return new PaintCommandParameter(itemTappedEventArgs.Info, itemTappedEventArgs.Surface);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			throw new NotImplementedException();
	}
}