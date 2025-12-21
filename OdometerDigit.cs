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
        WidthRequest = fontSize * 0.9; // Un poco de margen horizontal

        _digitHeight = height;
        _verticalOffset = offset;

        // Usamos un Grid rígido para asegurar que la altura sea matemáticamente perfecta
        _stripGrid = new Grid {
            RowSpacing = 0,
            Padding = 0,
            Margin = 0,
            WidthRequest = fontSize * 0.9,
            VerticalOptions = LayoutOptions.Start
        };

        // Creamos la tira vertical: 0, 1, 2... 9, 0 (El 0 extra es para el ciclo infinito)
        for (int i = 0; i <= 10; i++) {
            // Definimos la fila con altura ABSOLUTA
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

            // --- LÓGICA DE DEBUG ---
            if (debugMode) {
                // 1. Fondo de color para ver el área real que ocupa el número
                label.BackgroundColor = i % 2 == 0
                    ? Color.FromRgba("#44FF0000") // Rojo suave
                    : Color.FromRgba("#440000FF"); // Azul suave

                // 2. LÍNEA DE CENTRO (NUEVO): 
                // Dibuja una línea amarilla en el centro exacto de la celda.
                // Si la línea no pasa por la mitad del número, ajusta el VerticalOffset.
                var centerLine = new BoxView {
                    Color = Colors.Yellow,
                    HeightRequest = 2,
                    WidthRequest = fontSize * 0.9,
                    VerticalOptions = LayoutOptions.Center, // Centro matemático del Grid
                    InputTransparent = true // Para que no bloquee nada
                };

                // Añadimos primero el Label
                _stripGrid.Add(label, 0, i);
                // Añadimos la línea encima
                _stripGrid.Add(centerLine, 0, i);
            } else {
                // Si no es debug, solo añadimos el Label
                _stripGrid.Add(label, 0, i);
            }
        }

        Content = _stripGrid;
    }

    public void SetPosition(double value) {
        // Módulo matemático para asegurar rango 0-9.99
        double effectiveVal = (value % 10 + 10) % 10;

        // FÓRMULA DE POSICIÓN:
        // Desplazamiento base + Tu Ajuste Manual (Offset)
        double yOffset = -(effectiveVal * _digitHeight) + _verticalOffset;

        _stripGrid.TranslationY = yOffset;
    }
}
