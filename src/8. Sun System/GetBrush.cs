using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
namespace SolarSystem
{
    static class GetBrush
    {
        static DrawingBrush CreateDrawingBrush(Brush[] brushes)
        {
            DrawingGroup drawgrp = new DrawingGroup();

            for (int i = 0; i < brushes.Length; i++)
            {
                RectangleGeometry rectgeo =
                    new RectangleGeometry(new Rect(10 * i, 0, 10, 580));

                GeometryDrawing geodraw =
                    new GeometryDrawing(brushes[i], null, rectgeo);

                drawgrp.Children.Add(geodraw);
            }
            DrawingBrush drawbrsh = new DrawingBrush(drawgrp);
            drawbrsh.Freeze();

            return drawbrsh;
        }

        public static DrawingBrush Earth
        {
            get => CreateDrawingBrush(
                (new Array[16])
                    .Select((e, i) => new SolidColorBrush(i % 2 == 0 ? (Color)ColorConverter.ConvertFromString("#1A82FF")
                                                          : (Color)ColorConverter.ConvertFromString("#77D45F"))
                ).ToArray());
        }

        public static DrawingBrush Mars
        {
            get => CreateDrawingBrush(
                (new Array[16])
                    .Select((e, i) => new SolidColorBrush(i % 2 == 0 ? (Color)ColorConverter.ConvertFromString("#C64B07")
                                                          : (Color)ColorConverter.ConvertFromString("#B03D04"))
                ).ToArray());
        }

        public static DrawingBrush Venus
        {
            get => CreateDrawingBrush(
                (new Array[16])
                    .Select((e, i) => new SolidColorBrush(i % 2 == 0 ? (Color)ColorConverter.ConvertFromString("#EE9811")
                                                          : (Color)ColorConverter.ConvertFromString("#BF6C0C"))
                ).ToArray());
        }

        public static DrawingBrush Mercury
        {
            get => CreateDrawingBrush(
                (new Array[16])
                    .Select((e, i) => new SolidColorBrush(i % 2 == 0 ? (Color)ColorConverter.ConvertFromString("#BF985F")
                                                          : (Color)ColorConverter.ConvertFromString("#996C45"))
                ).ToArray());
        }

        public static DrawingBrush Moon
        {
            get => CreateDrawingBrush(
                (new Array[16])
                    .Select((e, i) => new SolidColorBrush(i % 2 == 0 ? (Color)ColorConverter.ConvertFromString("#696969")
                                                          : (Color)ColorConverter.ConvertFromString("#808080"))
                ).ToArray());
        }

        public static DrawingBrush Sun
        {
            get => CreateDrawingBrush(
                (new Array[16])
                    .Select((e, i) => new SolidColorBrush(i % 2 == 0 ? (Color)ColorConverter.ConvertFromString("#FDA500")
                                                          : (Color)ColorConverter.ConvertFromString("#FECE00"))
                ).ToArray());
        }
    }
}

