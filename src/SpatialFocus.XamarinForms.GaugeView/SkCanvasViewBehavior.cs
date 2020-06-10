// <copyright file="SkCanvasViewBehavior.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.XamarinForms.GaugeView
{
	using System.Windows.Input;
	using Prism.Behaviors;
	using Prism.Commands;
	using SkiaSharp.Views.Forms;
	using Xamarin.Forms;

	internal class SkCanvasViewBehavior : BehaviorBase<SKCanvasView>
	{
		public static BindableProperty InvalidateSurfaceCommandProperty = BindableProperty.Create(
			nameof(SkCanvasViewBehavior.InvalidateSurfaceCommand), typeof(ICommand), typeof(SkCanvasViewBehavior), null,
			BindingMode.OneWayToSource);

		public SkCanvasViewBehavior()
		{
			InvalidateSurfaceCommand = new DelegateCommand(() => { AssociatedObject.InvalidateSurface(); });
		}

		public ICommand InvalidateSurfaceCommand
		{
			get => (ICommand)GetValue(SkCanvasViewBehavior.InvalidateSurfaceCommandProperty);
			set => SetValue(SkCanvasViewBehavior.InvalidateSurfaceCommandProperty, value);
		}
	}
}