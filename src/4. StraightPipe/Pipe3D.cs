using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Pipe3D
{
    enum Variant {
        CylinderWithBases = 0,
        CylinderWithoutBases = 1,
        Pipe = 2
    };

    public class Pipe3D : Window
    {
        const Variant variant = Variant.CylinderWithBases;

        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new Pipe3D());
        }

        public Pipe3D()
        {
            Title = "Pipe3D";

            // Create Viewport3D as content of window
            Viewport3D viewport = new Viewport3D();
            Content = viewport;

            // GenerateCylinder with GenerateShim
            const double r1 = 0.5, r2 = 0.4, h = 1;
            const int slices = 36;
            const int stacks = 36;
            MeshGeometry3D cylinder1Mesh = GenerateCylinder(new Point3D(0, 0, 0), h, r1,
                                                             stacks, slices, 1.0, 1.0);
            MeshGeometry3D cylinder2Mesh, shim1Mesh, shim2Mesh;

            if (variant == Variant.CylinderWithBases)
            {
                cylinder2Mesh = new MeshGeometry3D();
                shim1Mesh = GenerateBase(cylinder1Mesh, slices, true, false);
                shim2Mesh = GenerateBase(cylinder1Mesh, slices, false, false);
            }
            else if (variant == Variant.Pipe)
            {
                cylinder2Mesh = GenerateCylinder(new Point3D(0, 0, 0), h, r2, stacks, slices, 1.0, 1.0);
                shim1Mesh = GenerateShim(cylinder1Mesh, cylinder2Mesh, slices, true, false);
                shim2Mesh = GenerateShim(cylinder1Mesh, cylinder2Mesh, slices, false, false);
            }
            else
            {
                cylinder2Mesh = new MeshGeometry3D();
                shim1Mesh = new MeshGeometry3D();
                shim2Mesh = new MeshGeometry3D();
            }

            cylinder1Mesh.Freeze();
            cylinder2Mesh.Freeze();
            shim1Mesh.Freeze();
            shim2Mesh.Freeze();

            // GeometryModel3D for MeshGeometry3D
            GeometryModel3D cylinder1Geomod = new GeometryModel3D();
            cylinder1Geomod.Geometry = cylinder1Mesh;
            cylinder1Geomod.Material = new DiffuseMaterial(Brushes.Silver);
            cylinder1Geomod.BackMaterial = new DiffuseMaterial(Brushes.White);

            GeometryModel3D cylinder2Geomod = new GeometryModel3D();
            cylinder2Geomod.Geometry = cylinder2Mesh;
            cylinder2Geomod.Material = new DiffuseMaterial(Brushes.Silver);
            cylinder2Geomod.BackMaterial = new DiffuseMaterial(Brushes.White);

            GeometryModel3D shim1Geomod = new GeometryModel3D();
            shim1Geomod.Geometry = shim1Mesh;
            shim1Geomod.Material = new DiffuseMaterial(Brushes.Black);
            shim1Geomod.BackMaterial = new DiffuseMaterial(Brushes.White);

            GeometryModel3D shim2Geomod = new GeometryModel3D();
            shim2Geomod.Geometry = shim2Mesh;
            shim2Geomod.Material = new DiffuseMaterial(Brushes.Black);
            shim2Geomod.BackMaterial = new DiffuseMaterial(Brushes.White);

            // Model3DGroup
            Model3DGroup pipe3dGroup = new Model3DGroup();
            pipe3dGroup.Children.Add(cylinder1Geomod);
            pipe3dGroup.Children.Add(cylinder2Geomod);
            pipe3dGroup.Children.Add(shim1Geomod);
            pipe3dGroup.Children.Add(shim2Geomod);

            Model3DGroup lightGroup = new Model3DGroup();
            lightGroup.Children.Add(new AmbientLight(Color.FromRgb(128, 128, 128)));
            lightGroup.Children.Add(new DirectionalLight(Color.FromRgb(128, 128, 128),
                                     new Vector3D(2, -2, -1)));

            // ModelVisual3D
            ModelVisual3D pipe3dModvis = new ModelVisual3D();
            pipe3dModvis.Content = pipe3dGroup;
            viewport.Children.Add(pipe3dModvis);

            ModelVisual3D lightModvis = new ModelVisual3D();
            lightModvis.Content = lightGroup;
            viewport.Children.Add(lightModvis);

            // Camera
            PerspectiveCamera cam = new PerspectiveCamera(new Point3D(0, 0, 11),
                                                          new Vector3D(0, 0, -1),
                                                          new Vector3D(0, 1, 0),
                                                          45);
            viewport.Camera = cam;

            // RotateTransform3D for Pipe3D
            AxisAngleRotation3D axisAngleRotation = new AxisAngleRotation3D(new Vector3D(1, 1, 1), 0);
            RotateTransform3D rotateTransform = new RotateTransform3D(axisAngleRotation);
            pipe3dModvis.Transform = rotateTransform;

            // RotateTransform3D for CurvedPipe3D
            Transform3DGroup curvedPipe3dTransformGroup = new Transform3DGroup();
            AxisAngleRotation3D curvedPipe3dAxisAngleRotation = new AxisAngleRotation3D(new Vector3D(1, 1, 1), 120);
            RotateTransform3D curvedPipe3dRotateTransform = new RotateTransform3D(curvedPipe3dAxisAngleRotation);
            curvedPipe3dTransformGroup.Children.Add(curvedPipe3dRotateTransform);
            curvedPipe3dTransformGroup.Children.Add(rotateTransform);

            // Animation for RotateTransform3D
            DoubleAnimation animation = new DoubleAnimation(360, new Duration(TimeSpan.FromSeconds(5)));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            axisAngleRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
        }

        MeshGeometry3D GenerateCylinder(in Point3D center, double height, double radius,
                                        int stacks, int slices, double Part = 1.0, double part = 1.0)
        {
            MeshGeometry3D cylinderMesh = new MeshGeometry3D();
            if (stacks <= 2 || slices <= 2) return cylinderMesh;
            double dh = (height / stacks);
            for (int stack = 0; stack <= stacks; stack++)
            {
                if (stack > Part * stacks)
                {
                    continue;
                }
                double y = center.Y + stack * dh;
                for (int slice = 0; slice <= slices; slice++)
                {
                    if (slice > part * slices)
                    {
                        continue;
                    }
                    double theta = slice * 2 * Math.PI / slices;
                    double x = radius * Math.Sin(theta) + center.X;
                    double z = radius * Math.Cos(theta) + center.Z;
                    cylinderMesh.Positions.Add(new Point3D(x, y, z));
                }
            }
            for (int stack = 0; stack < stacks; stack++)
            {
                if (stack > Part * stacks)
                {
                    continue;
                }
                for (int slice = 0; slice < slices; slice++)
                {
                    if (slice > part * slices)
                    {
                        continue;
                    }
                    int n = slices + 1;
                    cylinderMesh.TriangleIndices.Add((stack + 0) * n + slice + 1);
                    cylinderMesh.TriangleIndices.Add((stack + 0) * n + slice);
                    cylinderMesh.TriangleIndices.Add((stack + 1) * n + slice);

                    cylinderMesh.TriangleIndices.Add((stack + 0) * n + slice + 1);
                    cylinderMesh.TriangleIndices.Add((stack + 1) * n + slice);
                    cylinderMesh.TriangleIndices.Add((stack + 1) * n + slice + 1);
                }
            }
            return cylinderMesh;
        }

        MeshGeometry3D GenerateShim(in MeshGeometry3D mesh1, in MeshGeometry3D mesh2,
                                    int slices, bool byFirst, bool order)
        {
            MeshGeometry3D shimMesh = new MeshGeometry3D();
            if (mesh1.Positions.Count <= 2 || mesh2.Positions.Count <= 2)
            {
                return shimMesh;
            }
            for (int slice = 0; slice < slices; slice++)
            {
                int n1 = mesh1.Positions.Count;
                int n2 = mesh2.Positions.Count;

                Point3D p1 = mesh1.Positions[byFirst ? slice : n1 - slice - 1];
                Point3D p2 = mesh1.Positions[byFirst ? slice + 1 : n1 - slice - 2];

                Point3D p3 = mesh2.Positions[byFirst ? slice : n2 - slice - 1];
                Point3D p4 = mesh2.Positions[byFirst ? slice + 1 : n2 - slice - 2];

                shimMesh.Positions.Add(p1);
                shimMesh.Positions.Add(p2);
                shimMesh.Positions.Add(p3);
                shimMesh.Positions.Add(p4);

                if (order)
                {
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 3); // p2
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 4); // p1
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 2); // p3

                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 3); // p2
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 2); // p3
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 1); // p4
                }
                else
                {
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 4); // p1
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 3); // p2
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 2); // p3

                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 2); // p3
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 3); // p2
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 1); // p4
                }
            }
            return shimMesh;
        }

        MeshGeometry3D GenerateBase(in MeshGeometry3D mesh1, int slices, bool byFirst, bool order)
        {
            MeshGeometry3D shimMesh = new MeshGeometry3D();
            if (mesh1.Positions.Count <= 2)
            {
                return shimMesh;
            }
            int n = mesh1.Positions.Count;
            Point3D centerPoint = new Point3D(0, 0, 0);
            for (int slice = 0; slice < slices; ++slice)
            {
                centerPoint.X += mesh1.Positions[byFirst ? slice : n - slice - 1].X;
                centerPoint.Y += mesh1.Positions[byFirst ? slice : n - slice - 1].Y;
                centerPoint.Z += mesh1.Positions[byFirst ? slice : n - slice - 1].Z;
            }
            centerPoint.X /= slices;
            centerPoint.Y /= slices;
            centerPoint.Z /= slices;
            shimMesh.Positions.Add(centerPoint);

            for (int slice = 0; slice < slices; slice++)
            {
                Point3D p1 = mesh1.Positions[byFirst ? slice : n - slice - 1];
                Point3D p2 = mesh1.Positions[byFirst ? slice + 1 : n - slice - 2];

                shimMesh.Positions.Add(p1);
                shimMesh.Positions.Add(p2);

                if (order)
                {
                    shimMesh.TriangleIndices.Add(0); // p2
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 1); // p3
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 2); // p1
                }
                else
                {
                    shimMesh.TriangleIndices.Add(0); // p2
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 2); // p1
                    shimMesh.TriangleIndices.Add(shimMesh.Positions.Count - 1); // p3
                }
            }
            return shimMesh;
        }
    }
}

