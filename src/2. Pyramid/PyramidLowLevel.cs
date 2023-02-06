using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Media.Media3D;
using _3DTools;

namespace Pyramid
{
    public class PyramidLowLevel : Window
    {
        /*[STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new PyramidLowLevel());
        }*/

        public PyramidLowLevel()
        {
            Title = "PyramidLowLevel";

            // Create Viewport3D as content of window
            Viewport3D viewport = new Viewport3D();
            Content = viewport;

            // GenerateCylinder with GenerateShim
            const double length = 3, width = 3, height = 3;
            MeshGeometry3D pyramidMesh = GeneratePyramid(new Point3D(0, 0, 0), length, width, height);

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

            // Animation
            Vector3D axis = new Vector3D(1, 1, 1);
            Quaternion qCenter = new Quaternion(0, 0, 0, 0);
            double secondsPerCycle = 5;
            bool reverse = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Point3DCollection initPoints = pyramidMesh.Positions.Clone();
            CompositionTarget.Rendering += (object sender, EventArgs args) =>
            {
                this.OnRendering(ref pyramidMesh, ref initPoints, ref stopwatch, ref axis,
                                 ref qCenter, ref secondsPerCycle, ref reverse, ref sender, ref args);
            };
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

        void OnRendering(ref MeshGeometry3D mesh, ref Point3DCollection initPoints, ref Stopwatch stopwatch, ref Vector3D axis,
                         ref Quaternion qCenter, ref double secondsPerCycle, ref bool reverse, ref object sender, ref EventArgs args)
        {
            // Detach collection from MeshGeometry3D
            Point3DCollection points = new Point3DCollection();
            mesh.Positions.Clear();

            double s = secondsPerCycle * 2;

            // Calculation rotation quaternion
            double angle = 360.0 * (stopwatch.Elapsed.TotalSeconds % secondsPerCycle) / secondsPerCycle;
            if (((int)stopwatch.Elapsed.TotalSeconds) % (int)s == 0)
            {
                reverse = true;
            }
            else if (((int)stopwatch.Elapsed.TotalSeconds) % (int)s == (int)secondsPerCycle)
            {
                reverse = false;
            }
            double part = (stopwatch.Elapsed.TotalSeconds % secondsPerCycle) / secondsPerCycle;
            if (reverse)
            {
                part = 1.0 - part;
            }
            Quaternion qRotate = new Quaternion(axis, angle);
            Quaternion qConjugate = qRotate;
            qConjugate.Conjugate();

            // Apply rotation to each point
            foreach (Point3D point in initPoints)
            {
                Quaternion qPoint = new Quaternion(point.X, point.Y, point.Z, 0);
                qPoint -= qCenter;
                Quaternion qRotatedPoint = qRotate * qPoint * qConjugate;
                qRotatedPoint += qCenter;
                points.Add(new Point3D(qRotatedPoint.X * part, qRotatedPoint.Y * part, qRotatedPoint.Z * part));
            }

            // Re-attach collections to MeshGeometry3D
            mesh.Positions = points;
        }
    }
}

