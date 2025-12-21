using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls;

namespace OdometerCounterViewMaui;

public class OdometerView : HorizontalStackLayout {
    public static readonly BindableProperty TargetValueProperty =
        BindableProperty.Create(nameof(TargetValue), typeof(int), typeof(OdometerView), 0, propertyChanged: OnTargetValueChanged);

    public int TargetValue {
        get => (int)GetValue(TargetValueProperty);
        set => SetValue(TargetValueProperty, value);
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(OdometerView), Colors.White, propertyChanged: OnVisualPropertyChanged);

    public Color TextColor {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new double HeightRequest
    {
        get => base.HeightRequest;
        private set => base.HeightRequest = value;
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new double MinimumHeightRequest
    {
        get => base.MinimumHeightRequest;
        private set => base.MinimumHeightRequest = value;
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new double MaximumHeightRequest
    {
        get => base.MaximumHeightRequest;
        private set => base.MaximumHeightRequest = value;
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new double WidthRequest
    {
        get => base.WidthRequest;
        private set => base.WidthRequest = value;
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new double MinimumWidthRequest
    {
        get => base.MinimumWidthRequest;
        private set => base.MinimumWidthRequest = value;
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new double MaximumWidthRequest
    {
        get => base.MaximumWidthRequest;
        private set => base.MaximumWidthRequest = value;
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(OdometerView), 50.0, propertyChanged: OnVisualPropertyChanged);

    public double FontSize {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty DurationMsProperty =
        BindableProperty.Create(nameof(DurationMs), typeof(uint), typeof(OdometerView), (uint)2000);

    public uint DurationMs {
        get => (uint)GetValue(DurationMsProperty);
        set => SetValue(DurationMsProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(OdometerView), "", propertyChanged: OnVisualPropertyChanged);

    public string FontFamily {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty VerticalOffsetProperty =
        BindableProperty.Create(nameof(VerticalOffset), typeof(double), typeof(OdometerView), 0.0, propertyChanged: OnVisualPropertyChanged);

    public double VerticalOffset {
        get => (double)GetValue(VerticalOffsetProperty);
        set => SetValue(VerticalOffsetProperty, value);
    }

    public static readonly BindableProperty DebugModeProperty =
        BindableProperty.Create(nameof(DebugMode), typeof(bool), typeof(OdometerView), false, propertyChanged: OnVisualPropertyChanged);

    public bool DebugMode {
        get => (bool)GetValue(DebugModeProperty);
        set => SetValue(DebugModeProperty, value);
    }

    private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue) {
        var control = (OdometerView)bindable;
        control.UpdateDigitVisuals();
    }

    private void UpdateDigitVisuals() {
        double effectiveHeight = FontSize * 1.6;
        base.HeightRequest = effectiveHeight;

        foreach (var child in Children) {
            if (child is OdometerDigit digit) {
                digit.UpdateVisuals(FontSize, TextColor, effectiveHeight, FontFamily, VerticalOffset, DebugMode);
            }
        }
        
        UpdateDigitPositions(TargetValue, isFinished: true);
    }

    public OdometerView() {
        Spacing = 0;
        HorizontalOptions = LayoutOptions.Center;
        VerticalOptions = LayoutOptions.Center;
        IsClippedToBounds = true;
    }

    private static void OnTargetValueChanged(BindableObject bindable, object oldValue, object newValue) {
        var control = (OdometerView)bindable;
        control.AnimateCounter(0, (int)newValue);
    }

    private void AnimateCounter(int start, int end) {
        this.AbortAnimation("OdometerRun");
        string endStr = end.ToString();
        UpdateColumns(endStr.Length);

        var animation = new Animation(v =>
        {
            UpdateDigitPositions(v, isFinished: false);
        }, start, end);

        animation.Commit(this, "OdometerRun", 16, DurationMs, Easing.CubicOut, (v, c) =>
        {
            UpdateDigitPositions(end, isFinished: true);
        });
    }

    private void UpdateColumns(int count) {
        double effectiveHeight = FontSize * 1.6;
        base.HeightRequest = effectiveHeight;

        while (Children.Count < count) {
            Children.Insert(0, new OdometerDigit(
                FontSize,
                TextColor,
                effectiveHeight,
                FontFamily,
                VerticalOffset,
                DebugMode
            ));
        }
        while (Children.Count > count) {
            Children.RemoveAt(0);
        }
    }

    private void UpdateDigitPositions(double totalValue, bool isFinished) {
        for (int i = 0; i < Children.Count; i++) {
            if (Children[i] is OdometerDigit digitControl) {
                int power = Children.Count - 1 - i;
                double divisor = Math.Pow(10, power);

                double rawValue = totalValue / divisor;
                double valForColumn = Math.Round(rawValue, 4);

                double finalPosition;

                double opacity = 1.0;

                if (isFinished) {
                    finalPosition = Math.Floor(valForColumn);
                    if (power == 0) finalPosition = valForColumn;
                } else {
                    double floorVal = Math.Floor(valForColumn);
                    double remainder = valForColumn - floorVal;

                    double threshold = 0.30;

                    finalPosition = floorVal;

                    if (remainder > threshold) {
                        double linearProgress = (remainder - threshold) / (1.0 - threshold);

                        double s = 0.6;
                        double t = linearProgress - 1;
                        double bounceProgress = (t * t * ((s + 1) * t + s) + 1);

                        finalPosition += bounceProgress;

                        double fadeStrength = 0.6;
                        opacity = 1.0 - (fadeStrength * Math.Sin(linearProgress * Math.PI));
                    }

                    if (power == 0) {
                        finalPosition = rawValue;
                        double fadeStrength = 0.6;
                        opacity = 1.0 - (fadeStrength * Math.Sin(remainder * Math.PI));
                    }
                }

                digitControl.SetPosition(finalPosition);
                digitControl.Opacity = opacity;
            }
        }
    }
}
