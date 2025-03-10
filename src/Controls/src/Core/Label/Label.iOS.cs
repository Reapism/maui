﻿#nullable disable
using System;
using Microsoft.Maui.Controls.Platform;

namespace Microsoft.Maui.Controls
{
	public partial class Label
	{
		public static void MapText(LabelHandler handler, Label label) => MapText((ILabelHandler)handler, label);

		public static void MapText(ILabelHandler handler, Label label)
		{
			Platform.LabelExtensions.UpdateText(handler.PlatformView, label);

			MapFormatting(handler, label);
		}

		public static void MapLineBreakMode(ILabelHandler handler, Label label)
		{
			handler.PlatformView?.UpdateLineBreakMode(label);
		}

		public static void MapMaxLines(ILabelHandler handler, Label label)
		{
			handler.PlatformView?.UpdateMaxLines(label);
		}

		static void MapFormatting(ILabelHandler handler, Label label)
		{
			// we need to re-apply color and font for HTML labels
			if (!label.HasFormattedTextSpans && label.TextType == TextType.Html)
			{
				handler.UpdateValue(nameof(ILabel.TextColor));
				handler.UpdateValue(nameof(ILabel.Font));
			}

			if (!IsPlainText(label))
				return;

			LabelHandler.MapFormatting(handler, label);
		}
	}
}
