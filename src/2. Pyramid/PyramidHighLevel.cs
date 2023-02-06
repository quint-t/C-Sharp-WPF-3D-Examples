using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using _3DTools;

namespace Pyramid
{
    public class PyramidHighLevel : Window
    {
        /*[STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new PyramidHighLevel());
        }*/

        public PyramidHighLevel()
        {
            Title = "PyramidHighLevel";

            // Create Viewport3D as content of window
            Viewport3D viewport = new Viewport3D();
            Content = viewport;

            // GenerateCylinder with GenerateShim
            const double length = 3, width = 3, height = 3;
            MeshGeometry3D pyramidMesh = GeneratePyramid(new Point3D(0, 0, 0), length, width, height);

            pyramidMesh.Freeze();

            // GeometryModel3D for MeshGeometry3D
            GeometryModel3D pyramidGeomod = new GeometryModel3D();
            pyramidGeomod.Geometry = pyramidMesh;
            pyramidGeomod.Material = new DiffuseMaterial(Brushes.Silver);
            pyramidGeomod.BackMaterial = new DiffuseMaterial(Brushes.White);

            // Model3DGroup
            Model3DGroup pyramidGroup = new Model3DGroup();
            pyramidGroup.Children.Add(pyramidGeomod);

            Model3DGroup lightGroup = new Model3DGroup();
            lightGroup.Children.Add(new AmbientLight(Color.FromRgb(128, 128, 128)));
            lightGroup.Children.Add(new DirectionalLight(Color.FromRgb(128, 128, 128),
                                     new Vector3D(2, -2, -1)));

            // ModelVisual3D
            ModelVisual3D pyramidModvis = new ModelVisual3D();
            pyramidModvis.Content = pyramidGroup;
            viewport.Children.Add(pyramidModvis);

            ModelVisual3D lightModvis = new ModelVisual3D();
            lightModvis.Content = lightGroup;
            viewport.Children.Add(lightModvis);

            // Camera
            PerspectiveCamera cam = new PerspectiveCamera(new Point3D(0, 0, 8),
                                                          new Vector3D(0, 0, -1),
                                                          new Vector3D(0, 1, 0),
                                                          45);
            viewport.Camera = cam;

            // Trackball
            Trackball trackball = new Trackball();
            trackball.EventSource = this;
            viewport.Camera.Transform = trackball.Transform;

            // RotateTransform3D for Pyramid
            AxisAngleRotation3D axisAngleRotation = new AxisAngleRotation3D(new Vector3D(1, 1, 1), 0);
            RotateTransform3D rotateTransform = new RotateTransform3D(axisAngleRotation);

            // ScaleTransform3D for Pyramid
            ScaleTransform3D scaleTransform = new ScaleTransform3D();

            // Transform3DGroup for Pyramid
            Transform3DGroup transformGroup = new Transform3DGroup();
            transformGroup.Children.Add(rotateTransform);
            transformGroup.Children.Add(scaleTransform);
            pyramidModvis.Transform = transformGroup;

            // Animation for RotateTransform3D
            DoubleAnimation rotateAnimation = new DoubleAnimation(360, new Duration(TimeSpan.FromSeconds(5)));
            rotateAnimation.RepeatBehavior = RepeatBehavior.Forever;
            axisAngleRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, rotateAnimation);

            // Animation for ScaleTransform3D
            DoubleAnimation scaleAnimation = new DoubleAnimation(0.0, new Duration(TimeSpan.FromSeconds(5)));
            scaleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            scaleAnimation.AutoReverse = true;
            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleYProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleZProperty, scaleAnimation);
        }

        MeshGeometry3D GeneratePyramid(in Point3D center, double length, double width, double height)
        {
            MeshGeometry3D pyramidMesh = new MeshGeometry3D();
            if (length <= 0 || width <= 0 || height <= 0) return pyramidMesh;
            double length2 = length / 2, width2 = width / 2;

            pyramidMesh.Positions.Add(new Point3D(center.X, center.Y + height, center.Z));
            pyramidMesh.Positions.Add(new Point3D(center.X - length2, center.Y, center.Z - width2));
            pyramidMesh.Positions.Add(new Point3D(center.X - length2, center.Y, center.Z + width2));
            pyramidMesh.Positions.Add(new Point3D(center.X + length2, center.Y, center.Z - width2));
            pyramidMesh.Positions.Add(new Point3D(center.X + length2, center.Y, center.Z + width2));

            // walls
            pyramidMesh.TriangleIndices.Add(0);
            pyramidMesh.TriangleIndices.Add(1);
            pyramidMesh.TriangleIndices.Add(2);

            pyramidMesh.TriangleIndices.Add(0);
            pyramidMesh.TriangleIndices.Add(3);
            pyramidMesh.TriangleIndices.Add(1);

            pyramidMesh.TriangleIndices.Add(0);
            pyramidMesh.TriangleIndices.Add(2);
            pyramidMesh.TriangleIndices.Add(4);

            pyramidMesh.TriangleIndices.Add(0);
            pyramidMesh.TriangleIndices.Add(4);
            pyramidMesh.TriangleIndices.Add(3);

            // bottom
            pyramidMesh.TriangleIndices.Add(1);
            pyramidMesh.TriangleIndices.Add(3);
            pyramidMesh.TriangleIndices.Add(2);

            pyramidMesh.TriangleIndices.Add(2);
            pyramidMesh.TriangleIndices.Add(3);
            pyramidMesh.TriangleIndices.Add(4);

            return pyramidMesh;
        }
    }
}

