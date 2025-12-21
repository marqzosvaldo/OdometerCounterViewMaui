using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls;

namespace OdometerCounterViewMaui;

public class OdometerDigit : ContentView {
    private Grid _stripGrid;
    private double _digitHeight;
    private double _verticalOffset;

    public OdometerDigit(double fontSize, Color textColor, double height, string fontFamily, double offset, bool debugMode) {
        IsClippedToBounds = true;
        HeightRequest = height;
        WidthRequest = fontSize * 0.9;

        _digitHeight = height;
        _verticalOffset = offset;

        _stripGrid = new Grid {
            RowSpacing = 0,
            Padding = 0,
            Margin = 0,
            WidthRequest = fontSize * 0.9,
            VerticalOptions = LayoutOptions.Start
        };

        for (int i = 0; i <= 10; i++) {
            _stripGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(height, GridUnitType.Absolute) });

            int val = i % 10;

            var label = new Label {
                Text = val.ToString(),
                FontSize = fontSize,
                TextColor = textColor,
                FontFamily = fontFamily,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                HeightRequest = height,
                LineBreakMode = LineBreakMode.NoWrap,
                Padding = 0
            };

            if (debugMode) {
                label.BackgroundColor = i % 2 == 0
                    ? Color.FromRgba("#44FF0000")
                    : Color.FromRgba("#440000FF");

                var centerLine = new BoxView {
                    Color = Colors.Yellow,
                    HeightRequest = 2,
                    WidthRequest = fontSize * 0.9,
                    VerticalOptions = LayoutOptions.Center,
                    InputTransparent = true
                };

                _stripGrid.Add(label, 0, i);
                _stripGrid.Add(centerLine, 0, i);
            } else {
                _stripGrid.Add(label, 0, i);
            }
        }

        Content = _stripGrid;
    }

    public void SetPosition(double value) {
        double effectiveVal = (value % 10 + 10) % 10;
        double yOffset = -(effectiveVal * _digitHeight) + _verticalOffset;
        _stripGrid.TranslationY = yOffset;
    }

    public void UpdateVisuals(double fontSize, Color textColor, double height, string fontFamily, double offset, bool debugMode) {
        _digitHeight = height;
        _verticalOffset = offset;

        this.HeightRequest = height;
        this.WidthRequest = fontSize * 0.9;
        
        if (_stripGrid != null) {
            _stripGrid.WidthRequest = fontSize * 0.9;

            foreach (var rowDef in _stripGrid.RowDefinitions) {
                rowDef.Height = new GridLength(height, GridUnitType.Absolute);
            }

            for (int i = 0; i < _stripGrid.Children.Count; i++) {
                var child = _stripGrid.Children[i];
                if (child is Label label) {
                    label.FontSize = fontSize;
                    label.TextColor = textColor;
                    label.FontFamily = fontFamily;
                    label.HeightRequest = height;
                    
                    if (debugMode) {
                        int val = int.Parse(label.Text);
                        label.BackgroundColor = val % 2 == 0
                            ? Color.FromRgba("#44FF0000")
                            : Color.FromRgba("#440000FF");
                    } else {
                        label.BackgroundColor = Colors.Transparent;
                    }
                }
                else if (child is BoxView box && debugMode) {
                     box.WidthRequest = fontSize * 0.9;
                }
            }
        }
    }
}
