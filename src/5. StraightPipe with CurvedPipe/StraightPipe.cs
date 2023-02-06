using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Pipe3D
{
    public class StraightPipe : IPipe
    {
        private Point3D centerPoint;
        private MeshGeometry3D cylinder1Mesh;
        private MeshGeometry3D cylinder2Mesh;
        private MeshGeometry3D shim1Mesh;
        private MeshGeometry3D shim2Mesh;
        private Model3DGroup pipe3dGroup;
        private Transform3DGroup transform3DGroup;
        private ModelVisual3D pipe3dModvis;
        private double centerX = 0, centerY = 0, centerZ = 0;

        public StraightPipe(Point3D center, double h, double r1, double r2, int stacks, int slices,
                    Brush material, Brush backMaterial, double Part = 1.0, double part = 1.0)
        {
            centerPoint = center;
            cylinder1Mesh = GenerateCylinder(centerPoint, h, r1, stacks, slices, Part, part);
            cylinder2Mesh = GenerateCylinder(centerPoint, h, r2, stacks, slices, Part, part);
            shim1Mesh = GenerateShim(cylinder1Mesh, cylinder2Mesh, slices, true, false);
            shim2Mesh = GenerateShim(cylinder1Mesh, cylinder2Mesh, slices, false, false);

            centerX = centerPoint.X;
            centerY = centerPoint.Y + h / 2;
            centerZ = centerPoint.Z;

            cylinder1Mesh.Freeze();
            cylinder2Mesh.Freeze();
            shim1Mesh.Freeze();
            shim2Mesh.Freeze();

            GeometryModel3D cylinder1Geomod = new GeometryModel3D();
            cylinder1Geomod.Geometry = cylinder1Mesh;
            cylinder1Geomod.Material = new DiffuseMaterial(material);
            cylinder1Geomod.BackMaterial = new DiffuseMaterial(backMaterial);

            GeometryModel3D cylinder2Geomod = new GeometryModel3D();
            cylinder2Geomod.Geometry = cylinder2Mesh;
            cylinder2Geomod.Material = new DiffuseMaterial(material);
            cylinder2Geomod.BackMaterial = new DiffuseMaterial(backMaterial);

            GeometryModel3D shim1Geomod = new GeometryModel3D();
            shim1Geomod.Geometry = shim1Mesh;
            shim1Geomod.Material = new DiffuseMaterial(material);
            shim1Geomod.BackMaterial = new DiffuseMaterial(backMaterial);

            GeometryModel3D shim2Geomod = new GeometryModel3D();
            shim2Geomod.Geometry = shim2Mesh;
            shim2Geomod.Material = new DiffuseMaterial(material);
            shim2Geomod.BackMaterial = new DiffuseMaterial(backMaterial);

            pipe3dGroup = new Model3DGroup();
            pipe3dGroup.Children.Add(cylinder1Geomod);
            pipe3dGroup.Children.Add(cylinder2Geomod);
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

        public static MeshGeometry3D GenerateCylinder(in Point3D center, double height, double radius,
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

