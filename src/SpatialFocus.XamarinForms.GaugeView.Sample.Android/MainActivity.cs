// <copyright file="MainActivity.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.XamarinForms.GaugeView.Sample.Droid
{
	using Android.App;
	using Android.Content.PM;
	using Android.OS;
	using Android.Runtime;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;
	using Platform = Xamarin.Essentials.Platform;

	[Activity(Label = "SpatialFocus.XamarinForms.GaugeView.Sample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			FormsAppCompatActivity.TabLayoutResource = Resource.Layout.Tabbar;
			FormsAppCompatActivity.ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			Platform.Init(this, savedInstanceState);
			Forms.Init(this, savedInstanceState);
			LoadApplication(new App());
		}
	}
}