using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Pipe3D
{
    public class XYZ
    {
        private Point3D centerPoint;
        private MeshGeometry3D xMesh;
        private MeshGeometry3D yMesh;
        private MeshGeometry3D zMesh;
        private Model3DGroup xyzGroup;
        private ModelVisual3D xyzModvis;

        public XYZ(double x, double y, double z,
                   double xBound, double yBound, double zBound, double radius)
        {
            centerPoint = new Point3D(x, y, z);

            xMesh = GenerateLine(centerPoint, new Point3D(xBound, y, z), radius);
            yMesh = GenerateLine(centerPoint, new Point3D(x, yBound, z), radius);
            zMesh = GenerateLine(centerPoint, new Point3D(x, y, zBound), radius, true);
            xMesh.Freeze();
            yMesh.Freeze();
            zMesh.Freeze();

            GeometryModel3D xGeomod = new GeometryModel3D();
            xGeomod.Geometry = xMesh;
            xGeomod.Material = new DiffuseMaterial(Brushes.Green);
            xGeomod.BackMaterial = new DiffuseMaterial(Brushes.Green);

            GeometryModel3D yGeomod = new GeometryModel3D();
            yGeomod.Geometry = yMesh;
            yGeomod.Material = new DiffuseMaterial(Brushes.Red);
            yGeomod.BackMaterial = new DiffuseMaterial(Brushes.Red);

            GeometryModel3D zGeomod = new GeometryModel3D();
            zGeomod.Geometry = zMesh;
            zGeomod.Material = new DiffuseMaterial(Brushes.Blue);
            zGeomod.BackMaterial = new DiffuseMaterial(Brushes.Blue);

            xyzGroup = new Model3DGroup();
            xyzGroup.Children.Add(xGeomod);
            xyzGroup.Children.Add(yGeomod);
            xyzGroup.Children.Add(zGeomod);

            xyzModvis = new ModelVisual3D();
            xyzModvis.Content = xyzGroup;
        }
        public ModelVisual3D ModelVisual()
        {
            return xyzModvis;
        }

        public MeshGeometry3D GenerateLine(Point3D from, Point3D to, double radius, bool byX = false)
        {
            MeshGeometry3D line = new MeshGeometry3D();

            if (byX)
            {
                line.Positions.Add(new Point3D(from.X, // 0
                                               from.Y,
                                               from.Z));
                line.Positions.Add(new Point3D(from.X + radius * 2, // 1
                                               from.Y,
                                               from.Z));
                line.Positions.Add(new Point3D(to.X, // 2
                                               to.Y,
                                               to.Z));
                line.Positions.Add(new Point3D(to.X + radius * 2, // 3
                                               to.Y,
                                               to.Z));
            }
            else
            {
                line.Positions.Add(new Point3D(from.X, // 0
                                               from.Y,
                                               from.Z));
                line.Positions.Add(new Point3D(from.X, // 1
                                               from.Y,
                                               from.Z + radius * 2));
                line.Positions.Add(new Point3D(to.X, // 2
                                               to.Y,
                                               to.Z));
                line.Positions.Add(new Point3D(to.X, // 3
                                               to.Y,
                                               to.Z + radius * 2));
            }
            line.TriangleIndices.Add(1);
            line.TriangleIndices.Add(0);
            line.TriangleIndices.Add(2);

            line.TriangleIndices.Add(2);
            line.TriangleIndices.Add(3);
            line.TriangleIndices.Add(1);
            return line;
        }
    }
}

