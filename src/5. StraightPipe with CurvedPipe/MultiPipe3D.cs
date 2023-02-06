using Pipe3D;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace MultiPipe3D
{
    public class MultiPipe3D : Window
    {
        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new MultiPipe3D());
        }

        public MultiPipe3D()
        {
            Title = "MultiPipe3D";

            // Create Viewport3D as content of window
            Viewport3D viewport = new Viewport3D();
            Content = viewport;

            const double r1 = 0.5, r2 = 0.4, h = 1;
            const int slices = 36, stacks = 36;
            IPipe pipe1 = new StraightPipe(new Point3D(0.5, 0, 0.5), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe2 = new CurvedPipe(new Point3D(0.5, 1, 0.5), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            List<IPipe> pipes = new List<IPipe>
            {
                pipe1,
                pipe2
            };

            // ModelVisual3D
            foreach (IPipe pipe in pipes)
            {
                viewport.Children.Add(pipe.ModelVisual());
            }
            viewport.Children.Add(new XYZ(0, 0, 0, 3, 3, 3, 0.1).ModelVisual());

            // Light
            Model3DGroup lightGroup = new Model3DGroup();
            lightGroup.Children.Add(new AmbientLight(Color.FromRgb(128, 128, 128)));
            lightGroup.Children.Add(new DirectionalLight(Color.FromRgb(128, 128, 128),
                                     new Vector3D(-2, -2, -2)));

            ModelVisual3D lightModvis = new ModelVisual3D();
            lightModvis.Content = lightGroup;
            viewport.Children.Add(lightModvis);

            // Camera
            PerspectiveCamera cam = new PerspectiveCamera(new Point3D(0, 1.5, 11),
                                                          new Vector3D(0, 0, -1),
                                                          new Vector3D(0, 1, 0),
                                                          40);
            viewport.Camera = cam;

            // Animation for camera
            AxisAngleRotation3D axisAngleRotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            DoubleAnimation animation = new DoubleAnimation(360, new Duration(TimeSpan.FromSeconds(10)));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            axisAngleRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
            RotateTransform3D camRotateTransform = new RotateTransform3D(axisAngleRotation);
            cam.Transform = camRotateTransform;
        }
    }
}

