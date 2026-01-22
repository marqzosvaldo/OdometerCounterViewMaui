using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using System.Linq;

namespace OdometerCounterViewMaui;

public class OdometerView : HorizontalStackLayout {
    public static readonly BindableProperty TargetValueProperty =
        BindableProperty.Create(nameof(TargetValue), typeof(long), typeof(OdometerView), 0L, propertyChanged: OnTargetValueChanged);

    public long TargetValue {
        get => (long)GetValue(TargetValueProperty);
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

    public static readonly BindableProperty IsHapticFeedbackEnabledProperty =
        BindableProperty.Create(nameof(IsHapticFeedbackEnabled), typeof(bool), typeof(OdometerView), false);

    public bool IsHapticFeedbackEnabled {
        get => (bool)GetValue(IsHapticFeedbackEnabledProperty);
        set => SetValue(IsHapticFeedbackEnabledProperty, value);
    }

    public static readonly BindableProperty IsThousandSeparatorEnabledProperty =
        BindableProperty.Create(nameof(IsThousandSeparatorEnabled), typeof(bool), typeof(OdometerView), false, propertyChanged: OnIsThousandSeparatorEnabledChanged);

    public bool IsThousandSeparatorEnabled {
        get => (bool)GetValue(IsThousandSeparatorEnabledProperty);
        set => SetValue(IsThousandSeparatorEnabledProperty, value);
    }

    private static void OnIsThousandSeparatorEnabledChanged(BindableObject bindable, object oldValue, object newValue) {
        var control = (OdometerView)bindable;
        if ((bool)newValue && control.UseCompactNotation) {
             control.UseCompactNotation = false;
        }
        control.UpdateDigitVisuals();
    }

    public static readonly BindableProperty UseCompactNotationProperty =
        BindableProperty.Create(nameof(UseCompactNotation), typeof(bool), typeof(OdometerView), false, propertyChanged: OnUseCompactNotationChanged);

    public bool UseCompactNotation {
        get => (bool)GetValue(UseCompactNotationProperty);
        set => SetValue(UseCompactNotationProperty, value);
    }

    private static void OnUseCompactNotationChanged(BindableObject bindable, object oldValue, object newValue) {
        var control = (OdometerView)bindable;
        if ((bool)newValue && control.IsThousandSeparatorEnabled) {
             control.IsThousandSeparatorEnabled = false;
        }
        control.UpdateDigitVisuals();
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
            } else if (child is Label label) {
                label.FontSize = FontSize;
                label.TextColor = TextColor;
                label.FontFamily = FontFamily;
                label.TranslationY = VerticalOffset;
                label.VerticalTextAlignment = TextAlignment.Center;
                label.HeightRequest = effectiveHeight;
            }
        }
        
        
        string endStr = TargetValue.ToString();
        UpdateColumns(endStr.Length);
        UpdateDigitPositions(TargetValue, isFinished: true);
    }

    public OdometerView() {
        Spacing = 0;
        HorizontalOptions = LayoutOptions.Center;
        VerticalOptions = LayoutOptions.Center;
        IsClippedToBounds = true;

        Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object? sender, EventArgs e) {
        this.AbortAnimation("OdometerRun");
    }

    private static void OnTargetValueChanged(BindableObject bindable, object oldValue, object newValue) {
        var control = (OdometerView)bindable;
        control.AnimateCounter((long)oldValue, (long)newValue);
    }


    private void AnimateCounter(long start, long end) {
        this.AbortAnimation("OdometerRun");

        double scaledEnd = end;
        string suffix = "";
        
        if (UseCompactNotation) {
            (scaledEnd, suffix) = GetCompactValue(end);
        }

        string endStr = UseCompactNotation ? FormatCompactString(scaledEnd, suffix) : end.ToString();
        UpdateColumns(endStr.Length, suffix); // Pass suffix to know what to add

        double scaledStart = start;
        if (UseCompactNotation) {
            if (start != 0) {
                 double divisor = GetDivisor(end);
                 scaledStart = start / divisor;
            }
        }
        
        long lastVal = start; 
        double lastVisibleVal = scaledStart;

        long lastVibrationTicks = 0;

        var animation = new Animation(v =>
        {
            if (IsHapticFeedbackEnabled) {
                // Trigger haptic on integer change of the visual number
                if ((int)v != (int)lastVisibleVal) {
                    long currentTicks = DateTime.Now.Ticks;
                    if (currentTicks - lastVibrationTicks > 200_000) {
                        try {
                            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(8));
                            lastVibrationTicks = currentTicks;
                        } catch {
                        }
                    }
                    lastVisibleVal = v;
                }
            }
            UpdateDigitPositions(v, isFinished: false);
        }, scaledStart, scaledEnd);

        animation.Commit(this, "OdometerRun", 16, DurationMs, Easing.CubicOut, (v, c) =>
        {
            UpdateDigitPositions(scaledEnd, isFinished: true);
        });
    }
    
    // Helpers
    private double GetDivisor(long value) {
        long absVal = Math.Abs(value);
        if (absVal >= 1_000_000_000_000) return 1_000_000_000_000.0;
        if (absVal >= 1_000_000_000) return 1_000_000_000.0;
        if (absVal >= 1_000_000) return 1_000_000.0;
        if (absVal >= 1_000) return 1_000.0;
        return 1.0;
    }

    private (double, string) GetCompactValue(long value) {
        long absVal = Math.Abs(value);
        if (absVal >= 1_000_000_000_000) return (value / 1_000_000_000_000.0, "T");
        if (absVal >= 1_000_000_000) return (value / 1_000_000_000.0, "B");
        if (absVal >= 1_000_000) return (value / 1_000_000.0, "M");
        if (absVal >= 1_000) return (value / 1_000.0, "K");
        return (value, "");
    }

    private string FormatCompactString(double val, string suffix) {
        if (Math.Abs(val % 1) < 0.001) {
             return ((long)val).ToString();
        }
        return val.ToString("F1"); 
    }
    
    private void UpdateColumns(int charCount, string suffix = "") {
        BuildColumns(charCount, suffix);
    }

    private void BuildColumns(int charCount, string suffix) {
        double effectiveHeight = FontSize * 1.6;
        base.HeightRequest = effectiveHeight;

        Children.Clear();
        
        
        if (!UseCompactNotation) {
            for (int i = 0; i < charCount; i++) {
                Children.Add(new OdometerDigit(FontSize, TextColor, effectiveHeight, FontFamily, VerticalOffset, DebugMode));
                
                int power = charCount - 1 - i;
                if (IsThousandSeparatorEnabled && power > 0 && power % 3 == 0) {
                     Children.Add(CreateSeparatorLabel(","));
                }
            }
        } else {
            double endVal; string endSuffix;
            (endVal, endSuffix) = GetCompactValue(TargetValue);
            string numStr = FormatCompactString(endVal, endSuffix);
            
            foreach (char c in numStr) {
                if (char.IsDigit(c)) {
                    Children.Add(new OdometerDigit(FontSize, TextColor, effectiveHeight, FontFamily, VerticalOffset, DebugMode));
                } else {
                    Children.Add(CreateSeparatorLabel(c.ToString()));
                }
            }
            
            if (!string.IsNullOrEmpty(endSuffix)) {
                Children.Add(CreateSeparatorLabel(endSuffix));
            }
        }
    }
    
    private Label CreateSeparatorLabel(string text) {
        return new Label {
            Text = text,
            FontSize = FontSize,
            TextColor = TextColor,
            FontFamily = FontFamily,
            VerticalOptions = LayoutOptions.Center,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            HeightRequest = FontSize * 1.6,
            TranslationY = VerticalOffset,
            Margin = new Thickness(0) 
        };
    }

    private void UpdateDigitPositions(double totalValue, bool isFinished) {
        if (!UseCompactNotation) {
             UpdateDigitPositionsStandard(totalValue, isFinished);
             return;
        }

        double endVal; string endSuffix;
        (endVal, endSuffix) = GetCompactValue(TargetValue);
        string formatSpec = FormatCompactString(endVal, endSuffix); 
        
        var digitPowers = new List<int>();
        int currentPower = 0;
        int decimalPos = formatSpec.IndexOf('.');
        if (decimalPos == -1) {
            currentPower = formatSpec.Length - 1;
        } else {
            currentPower = decimalPos - 1;
        }
        
        foreach (char c in formatSpec) {
             if (char.IsDigit(c)) {
                 digitPowers.Add(currentPower);
                 currentPower--;
             } 
        }
        
        // Now drive the digits
        var digitControls = new List<OdometerDigit>();
        foreach(var child in Children) if(child is OdometerDigit d) digitControls.Add(d);
        
        for (int i = 0; i < digitControls.Count && i < digitPowers.Count; i++) {
            var digitControl = digitControls[i];
            int power = digitPowers[i];
            
            // Calculate value for this column
            double divisor = Math.Pow(10, power);
            double rawValue = totalValue / divisor;
            
            // Logic regarding bounce/scroll is identical to standard
            double valForColumn = rawValue; 
             
             double finalPosition;
             double opacity = 1.0;

             if (isFinished) {
                 finalPosition = Math.Floor(rawValue % 10);
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
                 
                 if (power == digitPowers.Last()) {
                      finalPosition = rawValue;
                      double fadeStrength = 0.6;
                      opacity = 1.0 - (fadeStrength * Math.Sin(remainder * Math.PI));
                 }
             }
             
             digitControl.SetPosition(finalPosition);
             digitControl.Opacity = opacity;
        }
    }

    private void UpdateDigitPositionsStandard(double totalValue, bool isFinished) {
        var digitControls = new List<OdometerDigit>();
        foreach(var child in Children) {
            if(child is OdometerDigit d) digitControls.Add(d);
        }

        for (int i = 0; i < digitControls.Count; i++) {
            var digitControl = digitControls[i];
            
            int power = digitControls.Count - 1 - i;
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
