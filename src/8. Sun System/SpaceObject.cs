using System;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace SolarSystem
{
    class SpaceObject
    {
        double radius;
        double rotationTime;
        double xAngleRotation;
        double zAngleRotation;
        bool reverseRotation;

        double orbitRadius;
        double orbitRotationTime;
        bool reverseOrbitRotation;

        SpaceObject center;
        bool isStar;

        DrawingBrush brush;

        public SpaceObject(
            double radius = 0.1,
            double rotationTime = 0,
            double xAngleRotation = 0,
            double zAngleRotation = 0,
            bool reverseRotation = false,
            double orbitRadius = 0,
            double orbitRotationTime = 0,
            bool reverseOrbitRotation = false,
            SpaceObject center = null,
            bool isStar = false,
            DrawingBrush brush = null
            )
        {
            this.radius = radius;
            this.rotationTime = rotationTime;
            this.xAngleRotation = xAngleRotation;
            this.zAngleRotation = zAngleRotation;
            this.reverseRotation = reverseRotation;
            this.orbitRadius = orbitRadius;
            this.orbitRotationTime = orbitRotationTime;
            this.reverseOrbitRotation = reverseOrbitRotation;
            this.center = center;
            this.isStar = isStar;
            this.brush = brush;
        }

        private Point3D GetSelfCenter()
        {
            return (center?.GetSelfCenter() ?? new Point3D(0, 0, 0)) + new Vector3D(orbitRadius, 0, 0);
        }

        private RotateTransform3D GetRotation(Point3D rotationCenter, double time, bool reverse = false)
        {
            AxisAngleRotation3D rotation;
            DoubleAnimation animation;

            animation = new DoubleAnimation();
            animation.RepeatBehavior = RepeatBehavior.Forever;

            rotation = new AxisAngleRotation3D();
            rotation.Axis = new Vector3D(0, 1, 0);

            animation.From = reverse ? 360 : 0;
            animation.To = reverse ? 0 : 360;
            animation.Duration = TimeSpan.FromSeconds(time);

            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);

            return new RotateTransform3D(rotation, rotationCenter);
        }

        private RotateTransform3D GetSelfRotation()
        {
            return GetRotation(GetSelfCenter(), this.rotationTime, reverseRotation);
        }

        public ModelVisual3D Model3D
        {
            get
            {
                var selfCenterPosition = GetSelfCenter();

                var mesh = GenerateMesh.Sphere(selfCenterPosition, radius, 20, 20);
                mesh.Freeze();

                var geomod = new GeometryModel3D();
                geomod.Geometry = mesh;

                var materials = new MaterialGroup();
                materials.Children.Add(new DiffuseMaterial(this.brush ?? GetBrush.Earth));
                if (this.isStar) materials.Children.Add(new EmissiveMaterial(this.brush));

                geomod.Material = materials;

                ModelVisual3D modvis = new ModelVisual3D();
                modvis.Content = geomod;

                var transforms = new Transform3DGroup();
                if (this.rotationTime != 0) { transforms.Children.Add(GetSelfRotation()); }
                transforms.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), xAngleRotation), selfCenterPosition));
                transforms.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), zAngleRotation), selfCenterPosition));

                transforms.Children.Add(GetRotation(selfCenterPosition, orbitRotationTime, !reverseOrbitRotation));

                var nextCenter = this.center;
                var currentOrbitRotationTime = this.orbitRotationTime;
                var currentReverseOrbitRotation = this.reverseOrbitRotation;
                while (nextCenter != null)
                {
                    transforms.Children.Add(GetRotation(nextCenter.GetSelfCenter(), currentOrbitRotationTime, currentReverseOrbitRotation));

                    currentReverseOrbitRotation = nextCenter.reverseOrbitRotation;
                    currentOrbitRotationTime = nextCenter.orbitRotationTime;
                    nextCenter = nextCenter.center;
                }

                modvis.Transform = transforms;

                return modvis;
            }
        }

    }
}

