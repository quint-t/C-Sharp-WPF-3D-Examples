using System;
using System.Windows;
using System.Windows.Controls;

namespace FractalTree
{
    internal class LabelledSlider : ContentControl
    {
        public LabelledSlider(string labelString, double defaultValue, double min, double max, Action<double> onValueChanged, bool isInteger = false)
        {
            Grid grid = new Grid();
            grid.Margin = new Thickness(3);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            // labelString
            Label label = new Label();
            label.Content = labelString;
            Grid.SetRow(label, 0);
            grid.Children.Add(label);

            // defaultValue
            Slider slider = new Slider();
            slider.Value = defaultValue;
            Grid.SetRow(slider, 1);
            grid.Children.Add(slider);

            // min and max
            slider.Minimum = min;
            slider.Maximum = max;

            // onValueChanged
            Label value = new Label();
            value.Content = defaultValue.ToString();
            value.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(value, 2);
            grid.Children.Add(value);

            slider.ValueChanged += (object sender, RoutedPropertyChangedEventArgs<double> args) =>
            {
                value.Content = args.NewValue.ToString();
                onValueChanged(args.NewValue);
            };

            // isInteger
            if (isInteger)
            {
                slider.IsSnapToTickEnabled = true;
                slider.TickFrequency = 1;
            }
            else
            {
                slider.IsSnapToTickEnabled = true;
                slider.TickFrequency = 0.01;
            }

            this.Content = grid;
        }
    }
}

