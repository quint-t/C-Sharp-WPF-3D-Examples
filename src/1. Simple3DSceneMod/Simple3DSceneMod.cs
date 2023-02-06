using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Petzold.Simple3DSceneInCode
{
    public class Simple3DSceneMod : Window
    {
        PerspectiveCamera cam;
        private ScrollBar topHorizontalScroll = new ScrollBar();
        private ScrollBar bottomHorizontalScroll = new ScrollBar();
        private ScrollBar rightVerticalScroll = new ScrollBar();

        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new Simple3DSceneMod());
        }

        public Simple3DSceneMod()
        {
            Title = "Simple 3D Scene in Code Modified";

            // Make DockPanel content of window
            DockPanel dock = new DockPanel();
            Content = dock;

            // Create Top Horizontal Scrollbar for moving camera
            topHorizontalScroll.Orientation = Orientation.Horizontal;
            topHorizontalScroll.Value = 0;
            topHorizontalScroll.Minimum = 0;
            topHorizontalScroll.Maximum = 360;
            topHorizontalScroll.ValueChanged += TopHorizontalScrollBarOnValueChanged;
            dock.Children.Add(topHorizontalScroll);
            DockPanel.SetDock(topHorizontalScroll, Dock.Top);

            // Create Horizontal Scrollbar for moving camera
            bottomHorizontalScroll.Orientation = Orientation.Horizontal;
            bottomHorizontalScroll.Value = 0;
            bottomHorizontalScroll.Minimum = -6;
            bottomHorizontalScroll.Maximum = 6;
            bottomHorizontalScroll.ValueChanged += BottomHorizontalScrollBarOnValueChanged;
            dock.Children.Add(bottomHorizontalScroll);
            DockPanel.SetDock(bottomHorizontalScroll, Dock.Bottom);

            // Create Vertical Scrollbar for moving camera
            rightVerticalScroll.Orientation = Orientation.Vertical;
            rightVerticalScroll.Value = 0;
            rightVerticalScroll.Minimum = -2;
            rightVerticalScroll.Maximum = 2;
            rightVerticalScroll.RenderTransform = new RotateTransform(180);
            rightVerticalScroll.RenderTransformOrigin = new Point(0.5, 0.5);
            rightVerticalScroll.ValueChanged += RightVerticalScrollBarOnValueChanged;
            dock.Children.Add(rightVerticalScroll);
            DockPanel.SetDock(rightVerticalScroll, Dock.Right);

            // Create Viewport3D for 3D scene
            Viewport3D viewport = new Viewport3D();
            dock.Children.Add(viewport);

            // Define the MeshGeometry3D
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(new Point3D(0, 0, 0));

            mesh.Positions.Add(new Point3D(0, 1, -1));
            mesh.Positions.Add(new Point3D(0, 0, -1));
            mesh.Positions.Add(new Point3D(0, 1, 0));

            mesh.Positions.Add(new Point3D(-1, 1, 0));
            mesh.Positions.Add(new Point3D(-1, 0, 0));
            mesh.TriangleIndices = new Int32Collection(new int[] { 0, 1, 2,
                                                                   1, 3, 0,
                                                                   0, 4, 5,
                                                                   4, 3, 0 });

            // Define the GeometryModel3D
            GeometryModel3D geomod = new GeometryModel3D();
            geomod.Geometry = mesh;
            geomod.Material = new DiffuseMaterial(Brushes.Red);
            geomod.BackMaterial = new DiffuseMaterial(Brushes.Blue);

            // Create ModelVisual3D for GeometryModel3D
            ModelVisual3D modvis = new ModelVisual3D();
            modvis.Content = geomod;
            viewport.Children.Add(modvis);

            // Create another ModelVisual3D for light
            modvis = new ModelVisual3D();
            modvis.Content = new AmbientLight(Colors.White);
            viewport.Children.Add(modvis);

            // Create the camera
            cam = new PerspectiveCamera(new Point3D(0, 0, 5),
                                        new Vector3D(0, 0, -1),
                                        new Vector3D(0, 1, 0), 45);
            viewport.Camera = cam;
        }
        void TopHorizontalScrollBarOnValueChanged(object sender,
                            RoutedPropertyChangedEventArgs<double> args)
        {
            double positionX = 5 * Math.Sin(args.NewValue * Math.PI / 180);
            double positionZ = 5 * Math.Cos(args.NewValue * Math.PI / 180);
            cam.Position = new Point3D(positionX, cam.Position.Y, positionZ);
            cam.LookDirection = (new Vector3D(0, 0, 0) - new Vector3D(cam.Position.X, cam.Position.Y, cam.Position.Z));
            bottomHorizontalScroll.Value = positionX;
        }

        void BottomHorizontalScrollBarOnValueChanged(object sender,
                            RoutedPropertyChangedEventArgs<double> args)
        {
            cam.Position = new Point3D(args.NewValue, cam.Position.Y, cam.Position.Z);
        }

        void RightVerticalScrollBarOnValueChanged(object sender,
                            RoutedPropertyChangedEventArgs<double> args)
        {
            cam.Position = new Point3D(cam.Position.X, args.NewValue, cam.Position.Z);
        }
    }
}

