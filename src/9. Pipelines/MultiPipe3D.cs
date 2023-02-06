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

            /* First variant
            IPipe pipe1 = new StraightPipe(new Point3D(0, 0, 0), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe2 = new CurvedPipe(new Point3D(0, 1, 0), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe3 = new CurvedPipe(new Point3D(1, 1, 0), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe4 = new CurvedPipe(new Point3D(1, 0, 0), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe5 = new CurvedPipe(new Point3D(2, 0, 0), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe6 = new CurvedPipe(new Point3D(2, 0, 1), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe7 = new CurvedPipe(new Point3D(3, 0, 1), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe8 = new StraightPipe(new Point3D(3, 0, 0), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe9 = new CurvedPipe(new Point3D(3, 0, -1), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe10 = new StraightPipe(new Point3D(2, 0, -1), h, r1, r2, stacks, slices,
                                           Brushes.Silver, Brushes.White);
            IPipe pipe11 = new StraightPipe(new Point3D(1, 0, -1), h, r1, r2, stacks, slices,
                                           Brushes.Silver, Brushes.White);
            IPipe pipe12 = new CurvedPipe(new Point3D(0, 0, -1), r1, r2,
                                         stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe13 = new CurvedPipe(new Point3D(0, -1, -1), r1, r2,
                                         stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe14 = new CurvedPipe(new Point3D(0, -1, 0), r1, r2,
                                         stacks, slices, Brushes.Silver, Brushes.White);*/

            /* Second variant
            IPipe pipe1 = new CurvedPipe(new Point3D(0, 0, 0), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe2 = new StraightPipe(new Point3D(1, 0, 0), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe3 = new StraightPipe(new Point3D(0, 0, 1), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe4 = new CurvedPipe(new Point3D(0, 0, 2), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe5 = new CurvedPipe(new Point3D(2, 0, 0), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe6 = new StraightPipe(new Point3D(0, 1, 2), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe7 = new StraightPipe(new Point3D(2, 1, 0), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe8 = new CurvedPipe(new Point3D(0, 2, 2), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe9 = new CurvedPipe(new Point3D(2, 2, 0), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe10 = new StraightPipe(new Point3D(1, 2, 2), h, r1, r2, stacks, slices,
                                           Brushes.Silver, Brushes.White);
            IPipe pipe11 = new StraightPipe(new Point3D(2, 2, 1), h, r1, r2, stacks, slices,
                                           Brushes.Silver, Brushes.White);
            IPipe pipe12 = new CurvedPipe(new Point3D(2, 2, 2), r1, r2,
                                          stacks, slices, Brushes.Silver, Brushes.White);
            */

            /* Third variant */
            IPipe pipe1 = new CurvedPipe(new Point3D(0, 0, 0), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe2 = new StraightPipe(new Point3D(1, 0, 0), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe3 = new StraightPipe(new Point3D(2, 0, 0), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe4 = new CurvedPipe(new Point3D(3, 0, 0), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe5 = new CurvedPipe(new Point3D(3, 1, 0), r1, r2,
                                        stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe6 = new StraightPipe(new Point3D(2, 1, 0), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe7 = new CurvedPipe(new Point3D(1, 1, 0), r1, r2,
                                       stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe8 = new CurvedPipe(new Point3D(1, 2, 0), r1, r2,
                                       stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe9 = new CurvedPipe(new Point3D(0, 2, 0), r1, r2,
                                       stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe10 = new StraightPipe(new Point3D(0, 2, 1), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe11 = new StraightPipe(new Point3D(0, 2, 2), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe12 = new CurvedPipe(new Point3D(0, 2, 3), r1, r2,
                                       stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe13 = new StraightPipe(new Point3D(0, 1, 3), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe14 = new CurvedPipe(new Point3D(0, 0, 3), r1, r2,
                                       stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe15 = new CurvedPipe(new Point3D(0, 0, 2), r1, r2,
                                       stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe16 = new CurvedPipe(new Point3D(0, 1, 2), r1, r2,
                                       stacks, slices, Brushes.Silver, Brushes.White);
            IPipe pipe17 = new StraightPipe(new Point3D(0, 1, 1), h, r1, r2, stacks, slices,
                                          Brushes.Silver, Brushes.White);
            IPipe pipe18 = new CurvedPipe(new Point3D(0, 1, 0), r1, r2,
                                       stacks, slices, Brushes.Silver, Brushes.White);

            List<IPipe> pipes = new List<IPipe>();

            /* First variant
            pipes.Add(pipe1);
            pipes.Add(pipe2);
            pipes.Add(pipe3);
            pipes.Add(pipe4);
            pipes.Add(pipe5);
            pipes.Add(pipe6);
            pipes.Add(pipe7);
            pipes.Add(pipe8);
            pipes.Add(pipe9);
            pipes.Add(pipe10);
            pipes.Add(pipe11);
            pipes.Add(pipe12);
            pipes.Add(pipe13);
            pipes.Add(pipe14);*/

            /* Second variant
            pipes.Add(pipe1);
            pipes.Add(pipe2);
            pipes.Add(pipe3);
            pipes.Add(pipe4);
            pipes.Add(pipe5);
            pipes.Add(pipe6);
            pipes.Add(pipe7);
            pipes.Add(pipe8);
            pipes.Add(pipe9);
            pipes.Add(pipe10);
            pipes.Add(pipe11);
            pipes.Add(pipe12);*/

            /* Third variant */
            pipes.Add(pipe1);
            pipes.Add(pipe2);
            pipes.Add(pipe3);
            pipes.Add(pipe4);
            pipes.Add(pipe5);
            pipes.Add(pipe6);
            pipes.Add(pipe7);
            pipes.Add(pipe8);
            pipes.Add(pipe9);
            pipes.Add(pipe10);
            pipes.Add(pipe11);
            pipes.Add(pipe12);
            pipes.Add(pipe13);
            pipes.Add(pipe14);
            pipes.Add(pipe15);
            pipes.Add(pipe16);
            pipes.Add(pipe17);
            pipes.Add(pipe18);

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

            /* First variant
            pipe3.Rotate(0, 1, 0, 180);
            pipe4.Rotate(0, 0, 1, 90);
            pipe5.Rotate(0, 1, 0, 180);
            pipe5.Rotate(1, 0, 0, 270);
            pipe6.Rotate(1, 0, 0, 90);
            pipe7.Rotate(0, 1, 0, 90);
            pipe7.Rotate(0, 0, 1, 270);
            pipe8.Rotate(1, 0, 0, 90);
            pipe9.Rotate(0, 0, 1, 180);
            pipe9.Rotate(1, 0, 0, 90);
            pipe10.Rotate(1, 0, 0, 90);
            pipe10.Rotate(0, 1, 0, 90);
            pipe11.Rotate(1, 0, 0, 90);
            pipe11.Rotate(0, 1, 0, 90);
            pipe13.Rotate(1, 0, 0, 180);
            pipe13.Rotate(0, 1, 0, 270);
            pipe14.Rotate(0, 1, 0, 270);
            pipe14.Rotate(1, 0, 0, 180);*/

            /* Second variant
            pipe1.Rotate(1, 0, 0, 270);
            pipe2.Rotate(0, 0, 1, 90);
            pipe3.Rotate(0, 0, 1, 90);
            pipe3.Rotate(0, 1, 0, 90);
            pipe4.Rotate(0, 1, 0, 90);
            pipe4.Rotate(1, 0, 0, 90);
            pipe5.Rotate(0, 1, 0, 180);
            pipe5.Rotate(1, 0, 0, 180);
            pipe9.Rotate(0, 1, 0, 270);
            pipe10.Rotate(0, 0, 1, 90);
            pipe11.Rotate(1, 0, 0, 90);
            pipe12.Rotate(1, 0, 0, 90);
            pipe12.Rotate(0, 0, 1, 180);*/

            /* Third variant */
            pipe1.Rotate(0, 0, 1, 90);
            pipe2.Rotate(0, 0, 1, 90);
            pipe3.Rotate(0, 0, 1, 90);
            pipe4.Rotate(0, 0, 1, 180);
            pipe5.Rotate(0, 1, 0, 180);
            pipe6.Rotate(0, 0, 1, 90);
            pipe7.Rotate(0, 0, 1, 90);
            pipe8.Rotate(0, 1, 0, 180);
            pipe9.Rotate(1, 0, 0, 270);
            pipe10.Rotate(1, 0, 0, 90);
            pipe11.Rotate(1, 0, 0, 90);
            pipe12.Rotate(0, 1, 0, 90);
            pipe14.Rotate(1, 0, 0, 90);
            pipe14.Rotate(0, 0, 1, 90);
            pipe15.Rotate(1, 0, 0, 180);
            pipe15.Rotate(0, 1, 0, 270);
            pipe16.Rotate(0, 1, 0, 90);
            pipe17.Rotate(1, 0, 0, 90);
            pipe18.Rotate(0, 1, 0, 270);

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

