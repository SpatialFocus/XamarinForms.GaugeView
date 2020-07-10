// <copyright file="MainPageViewModel.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.XamarinForms.GaugeView.Sample
{
	using Xamarin.Essentials;

	public class MainPageViewModel
	{
		public MainPageViewModel()
		{
			HorizontalGauge = GaugeViewModel.Compass(GaugeUnit.Mils());
			VerticalGauge = GaugeViewModel.Gradometer(GaugeUnit.Mils());

			OrientationSensor.ReadingChanged += OrientationChanged;
			OrientationSensor.Start(SensorSpeed.Default);
		}

		public GaugeViewModel HorizontalGauge { get; }

		public GaugeViewModel VerticalGauge { get; }

		private void OrientationChanged(object sender, OrientationSensorChangedEventArgs args)
		{
			Euler euler = args.Reading.Orientation.ToEuler();

			double headingInMils = euler.Yaw / 360 * 6400;
			double elevationInMils = euler.Pitch * -1 / 360 * 6400;

			HorizontalGauge.Value = ((headingInMils % 6400) + 6400) % 6400;
			VerticalGauge.Value = elevationInMils;
		}
	}
}