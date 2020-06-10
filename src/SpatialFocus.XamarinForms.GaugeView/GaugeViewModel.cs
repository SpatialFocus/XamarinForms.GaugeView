// <copyright file="GaugeViewModel.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.XamarinForms.GaugeView
{
	using System;
	using System.ComponentModel;
	using System.Globalization;
	using System.Windows.Input;
	using Prism.Commands;
	using SkiaSharp;
	using SkiaSharp.Views.Forms;
	using Xamarin.Forms;

	public class GaugeViewModel : INotifyPropertyChanged
	{
		public GaugeViewModel()
		{
			PaintCommand = new DelegateCommand<PaintCommandParameter>(Paint);

			HighlightedLineStyle = new SKPaint { Style = SKPaintStyle.Stroke, Color = HighlightedColor.ToSKColor(), StrokeWidth = 10 };
			LineStyle = new SKPaint { Style = SKPaintStyle.Stroke, Color = Color.ToSKColor(), StrokeWidth = 5 };
			TextStyle = new SKPaint { TextSize = 40.0f, IsAntialias = true, IsStroke = false };
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ICommand PaintCommand { get; }

		public Color Color { get; set; } = Color.DarkSlateGray;

		public double DisplayRange { get; set; } = 10;

		public Color HighlightedColor { get; set; } = Color.Red;

		public ICommand InvalidateSurfaceCommand { get; set; }

		public GaugeOrientation Orientation { get; set; } = GaugeOrientation.Horizontal;

		public int PartitionGroupSize { get; set; } = 5;

		public double PartitionSize { get; set; } = 0.5;

		public Func<double, string> SetLabel { get; set; }

		public double TargetValue { get; set; }

		public double Value { get; set; }

		protected SKPaint HighlightedLineStyle { get; set; }

		protected SKPaint LineStyle { get; set; }

		protected float Margin { get; set; } = 20;

		protected float TextMargin { get; set; } = 10;

		protected SKPaint TextStyle { get; set; }

		public static GaugeViewModel Compass()
		{
			return new GaugeViewModel()
			{
				SetLabel = x =>
				{
					// TODO: Review
					double degree = (x + 360) % 360;

					return degree switch
					{
						0 => "N",
						22.5 => "NNE",
						45 => "NE",
						67.5 => "ENE",
						90 => "E",
						112.5 => "ESE",
						135 => "SE",
						157.5 => "SSE",
						180 => "S",
						202.5 => "SSW",
						225 => "SW",
						247.5 => "WSW",
						270 => "W",
						292.5 => "WNW",
						315 => "NW",
						3337.5 => "NNW",
						_ => null
					};
				},
				DisplayRange = 20,
			};
		}

		public static GaugeViewModel Gradometer()
		{
			return new GaugeViewModel()
			{
				Orientation = GaugeOrientation.Vertical,
				SetLabel = x => x % 5 == 0 ? x.ToString("N0", CultureInfo.InvariantCulture) : null,
			};
		}

		protected void OnValueChanged()
		{
			InvalidateSurfaceCommand?.Execute(null);
		}

		internal void Paint(PaintCommandParameter parameter)
		{
			if (parameter == null)
			{
				throw new ArgumentException("Parameter must not be null.", nameof(parameter));
			}

			SKImageInfo info = parameter.Info;
			SKSurface surface = parameter.Surface;
			SKCanvas canvas = surface.Canvas;

			canvas.Clear();

			double startValue = Value - (DisplayRange / 2);
			double endValue = (Value + (DisplayRange / 2)).Previous(PartitionSize);

			double currentValue = startValue.Next(PartitionSize);

			float textSize = CalculateTextSize();

			do
			{
				DrawPartition(canvas, info, textSize, startValue, endValue, currentValue);

				currentValue = (currentValue + PartitionSize).Nearest(PartitionSize);
			}
			while (currentValue <= endValue);

			if (startValue <= TargetValue && TargetValue <= endValue)
			{
				DrawPartition(canvas, info, textSize, startValue, endValue, TargetValue, true);
			}

			if (Orientation == GaugeOrientation.Horizontal)
			{
				canvas.DrawLine(Margin, textSize + TextMargin, info.Width - Margin, textSize + TextMargin, LineStyle);
			}
			else
			{
				canvas.DrawLine(textSize + TextMargin, Margin, textSize + TextMargin, info.Height - Margin, LineStyle);
			}
		}

		private float CalculateTextSize()
		{
			SKRect textBounds = default;
			TextStyle.MeasureText("NW", ref textBounds);

			float textSize = Orientation == GaugeOrientation.Horizontal ? textBounds.Height : textBounds.Width;

			return textSize;
		}

		private void DrawPartition(SKCanvas canvas, SKImageInfo info, float textSize, double startValue, double endValue,
			double currentValue, bool highlight = false)
		{
			double unitDisplaySize =
				((Orientation == GaugeOrientation.Horizontal ? info.Width : info.Height) - (Margin * 2)) / DisplayRange;

			float position;

			if (Orientation == GaugeOrientation.Horizontal)
			{
				position = (float)(Margin + ((currentValue - startValue) * unitDisplaySize));
			}
			else
			{
				position = (float)(Margin + ((endValue - currentValue) * unitDisplaySize));
			}

			if (highlight)
			{
				if (Orientation == GaugeOrientation.Horizontal)
				{
					canvas.DrawLine(position, textSize + TextMargin, position, info.Height, HighlightedLineStyle);
				}
				else
				{
					canvas.DrawLine(textSize + TextMargin, position, info.Width, position, HighlightedLineStyle);
				}
			}
			else if (currentValue.Emphasize(PartitionSize, PartitionGroupSize))
			{
				if (Orientation == GaugeOrientation.Horizontal)
				{
					canvas.DrawLine(position, textSize + TextMargin, position, info.Height, LineStyle);
				}
				else
				{
					canvas.DrawLine(textSize + TextMargin, position, info.Width, position, LineStyle);
				}
			}
			else
			{
				if (Orientation == GaugeOrientation.Horizontal)
				{
					canvas.DrawLine(position, textSize + TextMargin, position,
						textSize + TextMargin + (float)((info.Height - textSize - TextMargin) * 0.66), LineStyle);
				}
				else
				{
					canvas.DrawLine(textSize + TextMargin, position,
						textSize + TextMargin + (float)((info.Width - textSize - TextMargin) * 0.66), position, LineStyle);
				}
			}

			string label = SetLabel?.Invoke(currentValue);

			if (!string.IsNullOrEmpty(label))
			{
				DrawText(canvas, position, label, textSize);
			}
		}

		private void DrawText(SKCanvas canvas, float position, string text, float reservedSize)
		{
			SKRect textBounds = default;
			TextStyle.MeasureText(text, ref textBounds);

			if (Orientation == GaugeOrientation.Horizontal)
			{
				canvas.DrawText(text, position - textBounds.MidX, textBounds.Top * -1, TextStyle);
			}
			else
			{
				canvas.DrawText(text, (textBounds.Left * -1) + (reservedSize - textBounds.Width), position - textBounds.MidY, TextStyle);
			}
		}
	}
}