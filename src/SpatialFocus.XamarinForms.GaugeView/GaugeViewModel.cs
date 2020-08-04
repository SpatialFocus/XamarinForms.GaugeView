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

			ClearStyle = new SKPaint() { Color = SKColors.Empty, BlendMode = SKBlendMode.Src };
			DirectionArrowStyle = new SKPaint { Style = SKPaintStyle.Fill, Color = Color.ToSKColor(), IsAntialias = true };
			PrecisionIndicatorStyle = new SKPaint() { Color = PrecisionIndicatorColor.ToSKColor(), BlendMode = SKBlendMode.DstOver };
			HighlightedLineStyle = new SKPaint { Style = SKPaintStyle.Stroke, Color = HighlightedColor.ToSKColor(), StrokeWidth = 10 };
			LineStyle = new SKPaint { Style = SKPaintStyle.Stroke, Color = Color.ToSKColor(), StrokeWidth = 5 };
			TextStyle = new SKPaint { TextSize = 40.0f, IsAntialias = true, IsStroke = false };
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ICommand PaintCommand { get; }

		public Color Color { get; set; } = Color.DarkSlateGray;

		public Func<double, double, double> DisplayDirectionArrow { get; set; }

		public double DisplayRange { get; set; } = 10;

		public Color HighlightedColor { get; set; } = Color.Red;

		public ICommand InvalidateSurfaceCommand { get; set; }

		public GaugeOrientation Orientation { get; set; } = GaugeOrientation.Horizontal;

		public int PartitionGroupSize { get; set; } = 5;

		public double PartitionSize { get; set; } = 0.5;

		public double? Precision { get; set; }

		public Color PrecisionIndicatorColor { get; set; } = Color.LightPink;

		public Func<double, string> SetLabel { get; set; }

		public double TargetValue { get; set; }

		public GaugeUnit Unit { get; set; } = GaugeUnit.Degrees();

		public double Value { get; set; }

		protected SKPaint ClearStyle { get; set; }

		protected SKPaint DirectionArrowStyle { get; set; }

		protected SKPaint HighlightedLineStyle { get; set; }

		protected SKPaint LineStyle { get; set; }

		protected float Margin { get; set; } = 20;

		protected SKPaint PrecisionIndicatorStyle { get; set; }

		protected float TextMargin { get; set; } = 10;

		protected SKPaint TextStyle { get; set; }

		public static GaugeViewModel Compass(GaugeUnit unit)
		{
			return new GaugeViewModel()
			{
				Unit = unit,
				DisplayDirectionArrow = unit.NormalizedDifference,
				Precision = unit.Max * 0.005,
				SetLabel = x =>
				{
					double degree = (x + unit.Max) % unit.Max;

					// ReSharper disable CompareOfFloatsByEqualityOperator
					if (degree == 0)
					{
						return "N";
					}

					if (degree == 1.0 * unit.Max / 16)
					{
						return "NNE";
					}

					if (degree == 2.0 * unit.Max / 16)
					{
						return "NE";
					}

					if (degree == 3.0 * unit.Max / 16)
					{
						return "ENE";
					}

					if (degree == 4.0 * unit.Max / 16)
					{
						return "E";
					}

					if (degree == 5.0 * unit.Max / 16)
					{
						return "ESE";
					}

					if (degree == 6.0 * unit.Max / 16)
					{
						return "SE";
					}

					if (degree == 7.0 * unit.Max / 16)
					{
						return "SSE";
					}

					if (degree == 8.0 * unit.Max / 16)
					{
						return "S";
					}

					if (degree == 9.0 * unit.Max / 16)
					{
						return "SSW";
					}

					if (degree == 10.0 * unit.Max / 16)
					{
						return "SW";
					}

					if (degree == 11.0 * unit.Max / 16)
					{
						return "WSW";
					}

					if (degree == 12.0 * unit.Max / 16)
					{
						return "W";
					}

					if (degree == 13.0 * unit.Max / 16)
					{
						return "WNW";
					}

					if (degree == 14.0 * unit.Max / 16)
					{
						return "NW";
					}

					if (degree == 15.0 * unit.Max / 16)
					{
						return "NNW";
					}

					// ReSharper restore CompareOfFloatsByEqualityOperator
					return null;
				},
				PartitionSize = unit.Unit == GaugeUnit.UnitEnum.Mils ? 5 : 0.5,
				DisplayRange = unit.Unit == GaugeUnit.UnitEnum.Mils ? 200 : 20,
			};
		}

		public static GaugeViewModel Gradometer(GaugeUnit unit)
		{
			return new GaugeViewModel()
			{
				Unit = unit,
				DisplayDirectionArrow = unit.NormalizedDifference,
				Precision = unit.Max * 0.005,
				Orientation = GaugeOrientation.Vertical,
				SetLabel = x =>
				{
					if (unit.Max > 400)
					{
						return x % 100 == 0 ? x.ToString("###0", CultureInfo.InvariantCulture) : null;
					}

					return x % 5 == 0 ? x.ToString("N0", CultureInfo.InvariantCulture) : null;
				},
				PartitionSize = unit.Unit == GaugeUnit.UnitEnum.Mils ? 6.25 : 0.5,
				PartitionGroupSize = unit.Unit == GaugeUnit.UnitEnum.Mils ? 4 : 5,
				DisplayRange = unit.Unit == GaugeUnit.UnitEnum.Mils ? 200 : 10,
			};
		}

		protected void OnColorChanged()
		{
			LineStyle = new SKPaint { Style = SKPaintStyle.Stroke, Color = Color.ToSKColor(), StrokeWidth = 5 };
			DirectionArrowStyle = new SKPaint { Style = SKPaintStyle.Fill, Color = Color.ToSKColor(), IsAntialias = true };
		}

		protected void OnHighlightedColorChanged()
		{
			HighlightedLineStyle = new SKPaint { Style = SKPaintStyle.Stroke, Color = HighlightedColor.ToSKColor(), StrokeWidth = 10 };
		}

		protected void OnPrecisionIndicatorColorChanged()
		{
			PrecisionIndicatorStyle = new SKPaint() { Color = PrecisionIndicatorColor.ToSKColor(), BlendMode = SKBlendMode.DstOver };
		}

		protected void OnValueChanged()
		{
			Device.BeginInvokeOnMainThread(() => InvalidateSurfaceCommand?.Execute(null));
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

			DrawPrecisionIndicator(canvas, info, textSize, startValue, endValue);

			if ((startValue <= TargetValue && TargetValue <= endValue) || (startValue <= TargetValue + Unit.Max && TargetValue + Unit.Max <= endValue))
			{
				DrawPartition(canvas, info, textSize, startValue, endValue, TargetValue, true);
			}
			else if (startValue <= TargetValue - Unit.Max && TargetValue - Unit.Max <= endValue)
			{
				DrawPartition(canvas, info, textSize, startValue, endValue, TargetValue - Unit.Max, true);
			}

			if (Orientation == GaugeOrientation.Horizontal)
			{
				canvas.DrawLine(Margin, textSize + TextMargin, info.Width - Margin, textSize + TextMargin, LineStyle);
			}
			else
			{
				canvas.DrawLine(info.Width * 0.5f, Margin, info.Width * 0.5f, info.Height - Margin, LineStyle);
			}

			DrawDirectionArrow(canvas, info);
		}

		private float CalculateTextSize()
		{
			SKRect textBounds = default;
			TextStyle.MeasureText("NNW", ref textBounds);

			float textSize = Orientation == GaugeOrientation.Horizontal ? textBounds.Height : textBounds.Width;

			return textSize;
		}

		private void DrawDirectionArrow(SKCanvas canvas, SKImageInfo info)
		{
			if (DisplayDirectionArrow == null)
			{
				return;
			}

			double difference = DisplayDirectionArrow.Invoke(Value, TargetValue);

			if (Orientation == GaugeOrientation.Horizontal)
			{
				if (difference < -DisplayRange / 2)
				{
					canvas.DrawRect(new SKRect(info.Width - 70, 0, info.Width, info.Height), ClearStyle);

					SKPath path = new SKPath();

					path.MoveTo(info.Width - 60, 0.5f * info.Height + 40);
					path.LineTo(info.Width, 0.5f * info.Height);
					path.LineTo(info.Width - 60, 0.5f * info.Height - 40);
					path.Close();

					canvas.DrawPath(path, DirectionArrowStyle);
				}
				else if (difference > DisplayRange / 2)
				{
					canvas.DrawRect(new SKRect(0, 0, 70, info.Height), ClearStyle);

					SKPath path = new SKPath();

					path.MoveTo(0 + 60, 0.5f * info.Height + 40);
					path.LineTo(0, 0.5f * info.Height);
					path.LineTo(0 + 60, 0.5f * info.Height - 40);
					path.Close();

					canvas.DrawPath(path, DirectionArrowStyle);
				}
			}
			else
			{
				if (difference < -DisplayRange / 2)
				{
					canvas.DrawRect(new SKRect(0, 0, 0.5f * info.Width - 5, 50), ClearStyle);

					SKPath path = new SKPath();

					path.MoveTo(0.25f * info.Width + 30, 0 + 50);
					path.LineTo(0.25f * info.Width, 0);
					path.LineTo(0.25f * info.Width - 30, 0 + 50);
					path.Close();

					canvas.DrawPath(path, DirectionArrowStyle);
				}
				else if (difference > DisplayRange / 2)
				{
					canvas.DrawRect(new SKRect(0, info.Height - 50, 0.5f * info.Width - 5, info.Height), ClearStyle);

					SKPath path = new SKPath();

					path.MoveTo(0.25f * info.Width + 30, info.Height - 50);
					path.LineTo(0.25f * info.Width, info.Height);
					path.LineTo(0.25f * info.Width - 30, info.Height - 50);
					path.Close();

					canvas.DrawPath(path, DirectionArrowStyle);
				}
			}
		}

		private void DrawPartition(SKCanvas canvas, SKImageInfo info, float textSize, double startValue, double endValue,
			double currentValue, bool highlight = false)
		{
			double unitDisplaySize = ((Orientation == GaugeOrientation.Horizontal ? info.Width : info.Height) - Margin * 2) / DisplayRange;

			float position;

			if (Orientation == GaugeOrientation.Horizontal)
			{
				position = (float)(Margin + (Unit.NormalizedDifference(currentValue, startValue) * unitDisplaySize));
			}
			else
			{
				position = (float)(Margin + (endValue - currentValue) * unitDisplaySize);
			}

			if (highlight)
			{
				if (Orientation == GaugeOrientation.Horizontal)
				{
					canvas.DrawLine(position, textSize + TextMargin, position, info.Height, HighlightedLineStyle);
				}
				else
				{
					canvas.DrawLine(info.Width * 0.5f, position, info.Width, position, HighlightedLineStyle);
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
					canvas.DrawLine(info.Width * 0.5f, position, info.Width, position, LineStyle);
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
					canvas.DrawLine(info.Width * 0.5f, position, (float)(info.Width * 0.5f) + (float)(info.Width * 0.33), position,
						LineStyle);
				}
			}

			string label = SetLabel?.Invoke(currentValue);

			if (!string.IsNullOrEmpty(label))
			{
				DrawText(canvas, info, position, label);
			}
		}

		private void DrawPrecisionIndicator(SKCanvas canvas, SKImageInfo info, float textSize, double startValue, double endValue)
		{
			if (!Precision.HasValue)
			{
				return;
			}

			double unitDisplaySize =
				((Orientation == GaugeOrientation.Horizontal ? info.Width : info.Height) - (Margin * 2)) / DisplayRange;
			float s1;
			float s2;

			if (Orientation == GaugeOrientation.Horizontal)
			{
				var realStartValue = startValue;

				if (TargetValue - startValue > Unit.Max / 2)
				{
					realStartValue = startValue + Unit.Max;
				}
				else if (TargetValue - startValue < -Unit.Max / 2)
				{
					realStartValue = startValue - Unit.Max;
				}

				s1 = (float)(Margin + ((TargetValue - Precision - realStartValue) * unitDisplaySize));
				s2 = (float)(Margin + (((TargetValue + Precision) - realStartValue) * unitDisplaySize));
			}
			else
			{
				s1 = (float)(Margin + ((endValue - TargetValue - Precision) * unitDisplaySize));
				s2 = (float)(Margin + (((endValue - TargetValue) + Precision) * unitDisplaySize));
			}

			if (Orientation == GaugeOrientation.Horizontal)
			{
				canvas.DrawRect(
					new SKRect(s1, textSize + TextMargin, s2,
						textSize + TextMargin + (float)((info.Height - textSize - TextMargin) * 0.66)), PrecisionIndicatorStyle);
			}
			else
			{
				canvas.DrawRect(new SKRect(info.Width * 0.5f, s1, (float)(info.Width * 0.5f) + (float)(info.Width * 0.33), s2),
					PrecisionIndicatorStyle);
			}
		}

		private void DrawText(SKCanvas canvas, SKImageInfo info, float position, string text)
		{
			SKRect textBounds = default;
			TextStyle.MeasureText(text, ref textBounds);

			if (Orientation == GaugeOrientation.Horizontal)
			{
				canvas.DrawText(text, position - textBounds.MidX, textBounds.Top * -1, TextStyle);
			}
			else
			{
				canvas.DrawText(text, (float)(info.Width * 0.5 - textBounds.Left - textBounds.Width - TextMargin),
					position - textBounds.MidY, TextStyle);
			}
		}
	}
}