using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Pipe3D
{
    public class CurvedPipe : IPipe
    {
        private Point3D centerPoint;
        private MeshGeometry3D tor1Mesh;
        private MeshGeometry3D tor2Mesh;
        private MeshGeometry3D shim1Mesh;
        private MeshGeometry3D shim2Mesh;
        private Model3DGroup pipe3dGroup;
        private Transform3DGroup transform3DGroup;
        private ModelVisual3D pipe3dModvis;
        private double centerX = 0, centerY = 0, centerZ = 0;

        public CurvedPipe(Point3D center, double R, double r, int N, int n,
                          Brush material, Brush backMaterial)
        {
            centerPoint = center;
            tor1Mesh = GenerateTor(new Point3D(centerPoint.X + R, centerPoint.Y, centerPoint.Z),
                                   R, R, N, n, 0.25, 1.0);
            tor2Mesh = GenerateTor(new Point3D(centerPoint.X + R, centerPoint.Y, centerPoint.Z),
                                   R, r, N, n, 0.25, 1.0);
            shim1Mesh = GenerateShim(tor1Mesh, tor2Mesh, N, true, true);
            shim2Mesh = GenerateShim(tor1Mesh, tor2Mesh, N, false, true);

            centerX = centerPoint.X;
            centerY = centerPoint.Y + R;
            centerZ = centerPoint.Z;

            tor1Mesh.Freeze();
            tor2Mesh.Freeze();
            shim1Mesh.Freeze();
            shim2Mesh.Freeze();

            GeometryModel3D tor1Geomod = new GeometryModel3D();
            tor1Geomod.Geometry = tor1Mesh;
            tor1Geomod.Material = new DiffuseMaterial(material);
            tor1Geomod.BackMaterial = new DiffuseMaterial(backMaterial);

            GeometryModel3D tor2Geomod = new GeometryModel3D();
            tor2Geomod.Geometry = tor2Mesh;
            tor2Geomod.Material = new DiffuseMaterial(material);
            tor2Geomod.BackMaterial = new DiffuseMaterial(backMaterial);

            GeometryModel3D shim1Geomod = new GeometryModel3D();
            shim1Geomod.Geometry = shim1Mesh;
            shim1Geomod.Material = new DiffuseMaterial(material);
            shim1Geomod.BackMaterial = new DiffuseMaterial(backMaterial);

            GeometryModel3D shim2Geomod = new GeometryModel3D();
            shim2Geomod.Geometry = shim2Mesh;
            shim2Geomod.Material = new DiffuseMaterial(material);
            shim2Geomod.BackMaterial = new DiffuseMaterial(backMaterial);

            pipe3dGroup = new Model3DGroup();
            pipe3dGroup.Children.Add(tor1Geomod);
            pipe3dGroup.Children.Add(tor2Geomod);
            pipe3dGroup.Children.Add(shim1Geomod);
            pipe3dGroup.Children.Add(shim2Geomod);

            transform3DGroup = new Transform3DGroup();
            pipe3dModvis = new ModelVisual3D();
            pipe3dModvis.Content = pipe3dGroup;
            pipe3dModvis.Transform = transform3DGroup;
        }

        public ModelVisual3D ModelVisual()
        {
            return pipe3dModvis;
        }

        public void Rotate(double x, double y, double z, double angle)
        {
            RotateTransform3D rotateTransform3D = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(x, y, z), angle));
            rotateTransform3D.CenterX = centerX;
            rotateTransform3D.CenterY = centerY;
            rotateTransform3D.CenterZ = centerZ;
            transform3DGroup.Children.Add(rotateTransform3D);
        }

        public void UndoRotate()
        {
            if (transform3DGroup.Children.Count > 0)
            {
                transform3DGroup.Children.RemoveAt(transform3DGroup.Children.Count - 1);
            }
        }

        private static Point3D GetPositionTor(double R, double r, double a, double b)
        {
            double sinB = Math.Sin(b * Math.PI / 180);
            double cosB = Math.Cos(b * Math.PI / 180);
            double sinA = Math.Sin(a * Math.PI / 180);
            double cosA = Math.Cos(a * Math.PI / 180);
            Point3D point = new Point3D();
            point.X = -(R + r * cosA) * sinB;
            point.Y = (R + r * cosA) * cosB;
            point.Z = r * sinA;
            return point;
        }

        public static MeshGeometry3D GenerateTor(Point3D center, double R, double r, int N, int n,
                                   double Part = 1.0, double part = 1.0)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            if (n < 5 || N < 5) return mesh;
            for (int i = 0; i <= N; i++)
            {
                if (i > Part * N)
                {
                    continue;
                }
                for (int j = 0; j <= n; j++)
                {
                    if (j > part * n)
                    {
                        continue;
                    }
                    Point3D point = GetPositionTor(R, r, Math.Min(360.0, j * 360 / n), Math.Min(360.0, i * 360 / (N - 1)));
                    point += (Vector3D)center;
                    mesh.Positions.Add(point);
                }
            }
            for (int i = 0; i < N; i++)
            {
                if (i > Part * N)
                {
                    continue;
                }
                for (int j = 0; j < n; j++)
                {
                    if (j > part * n)
                    {
                        continue;
                    }
                    mesh.TriangleIndices.Add((i + 1) * n + j + 0);
                    mesh.TriangleIndices.Add((i + 0) * n + j + 0);
                    mesh.TriangleIndices.Add((i + 1) * n + j + 1);

                    mesh.TriangleIndices.Add((i + 1) * n + j + 1);
                    mesh.TriangleIndices.Add((i + 0) * n + j + 0);
                    mesh.TriangleIndices.Add((i + 0) * n + j + 1);
                }
            }
            return mesh;
        }

        public static MeshGeometry3D GenerateShim(in MeshGeometry3D mesh1, in MeshGeometry3D mesh2,
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
    }
}

