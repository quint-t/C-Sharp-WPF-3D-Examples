using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace CartProject
{
    static class MeshGenerators
    {
        static public MeshGeometry3D GenerateRing(Point3D center, double radius, double thickness, double width, int precision)
        {
            var mesh = new MeshGeometry3D();

            for (int slice = 0; slice != precision; slice++)
            {
                double alpha = Math.PI * 2 * slice / precision;
                double sin = Math.Sin(alpha);
                double cos = Math.Cos(alpha);
                mesh.Positions.Add(new Vector3D(0, radius * cos, radius * sin) + center);
                mesh.Positions.Add(new Vector3D(0, (radius + thickness) * cos, (radius + thickness) * sin) + center);

                mesh.Positions.Add(new Vector3D(-width, radius * cos, radius * sin) + center);
                mesh.Positions.Add(new Vector3D(-width, (radius + thickness) * cos, (radius + thickness) * sin) + center);

                if (slice < precision - 1)
                {
                    mesh.TriangleIndices.Add(slice * 4);
                    mesh.TriangleIndices.Add(slice * 4 + 1);
                    mesh.TriangleIndices.Add(slice * 4 + 4);

                    mesh.TriangleIndices.Add(slice * 4 + 3);
                    mesh.TriangleIndices.Add(slice * 4 + 2);
                    mesh.TriangleIndices.Add(slice * 4 + 6);

                    mesh.TriangleIndices.Add(slice * 4);
                    mesh.TriangleIndices.Add(slice * 4 + 4);
                    mesh.TriangleIndices.Add(slice * 4 + 2);

                    mesh.TriangleIndices.Add(slice * 4 + 4);
                    mesh.TriangleIndices.Add(slice * 4 + 6);
                    mesh.TriangleIndices.Add(slice * 4 + 2);
                }

                if (slice > 0)
                {
                    mesh.TriangleIndices.Add(slice * 4);
                    mesh.TriangleIndices.Add(slice * 4 - 3);
                    mesh.TriangleIndices.Add(slice * 4 + 1);

                    mesh.TriangleIndices.Add(slice * 4 + 2);
                    mesh.TriangleIndices.Add(slice * 4 + 3);
                    mesh.TriangleIndices.Add(slice * 4 - 1);

                    mesh.TriangleIndices.Add(slice * 4 + 3);
                    mesh.TriangleIndices.Add(slice * 4 + 1);
                    mesh.TriangleIndices.Add(slice * 4 - 1);

                    mesh.TriangleIndices.Add(slice * 4 - 3);
                    mesh.TriangleIndices.Add(slice * 4 - 1);
                    mesh.TriangleIndices.Add(slice * 4 + 1);
                }
            }

            var lastSlice = precision - 1;

            mesh.TriangleIndices.Add(lastSlice * 4);
            mesh.TriangleIndices.Add(lastSlice * 4 + 1);
            mesh.TriangleIndices.Add(0);

            mesh.TriangleIndices.Add(lastSlice * 4 + 3);
            mesh.TriangleIndices.Add(lastSlice * 4 + 2);
            mesh.TriangleIndices.Add(2);

            mesh.TriangleIndices.Add(lastSlice * 4);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(lastSlice * 4 + 2);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(lastSlice * 4 + 2);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(lastSlice * 4 + 1);
            mesh.TriangleIndices.Add(1);

            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(lastSlice * 4 + 3);

            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(lastSlice * 4 + 3);

            mesh.TriangleIndices.Add(lastSlice * 4 + 1);
            mesh.TriangleIndices.Add(lastSlice * 4 + 3);
            mesh.TriangleIndices.Add(1);

            mesh.Freeze();
            return mesh;
        }

        static public MeshGeometry3D GenerateCylinder(Point3D center, double h, double radius, int slices)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            for (int stack = 0; stack <= 4; stack++)
            {
                double y = stack < 2 ? (h / 2) : (-h / 2);

                for (int slice = 0; slice <= slices; slice++)
                {
                    double theta = slice * 2 * Math.PI / slices;
                    double x = (stack == 0 || stack == 3) ? 0 : radius * Math.Sin(theta);
                    double z = (stack == 0 || stack == 3) ? 0 : radius * Math.Cos(theta);
                    double fullTextureHeight = 2 * radius + h;
                    Vector3D normal = new Vector3D(x, y, z);
                    mesh.Normals.Add(normal);
                    mesh.Positions.Add(normal + center);
                    mesh.TextureCoordinates.Add(new Point((double)slice / slices,
                            stack == 0 ? 0 :
                            stack == 1 ? radius / fullTextureHeight :
                            stack == 2 ? (radius + h) / fullTextureHeight :
                            1
                        ));
                    int n = slices + 1;
                    if (stack != 0)
                    {
                        mesh.TriangleIndices.Add((stack + 0) * n + slice);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice);
                        mesh.TriangleIndices.Add((stack + 0) * n + slice + 1);
                    }
                    if (stack != 3)
                    {
                        mesh.TriangleIndices.Add((stack + 0) * n + slice + 1);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice + 1);
                    }
                }
            }
            mesh.Freeze();
            return mesh;
        }
    }
}

