using System;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace CartProject
{
    class Cart
    {
        Wheel[] wheels;
        ModelVisual3D model;
        TranslateTransform3D transform;
        DoubleAnimation animation;

        public Cart()
        {
            wheels = new Wheel[4];
            wheels[0] = new Wheel(new Point3D(0, 0, 0), 0.2, 0.05, 0.4, 0.1, 20, 0.01, 50);
            wheels[1] = new Wheel(new Point3D(0, 0, 1), 0.2, 0.05, 0.4, 0.1, 20, 0.01, 50);
            wheels[2] = new Wheel(new Point3D(1, 0, 0), 0.2, 0.05, 0.4, 0.1, 20, 0.01, 50);
            wheels[3] = new Wheel(new Point3D(1, 0, 1), 0.2, 0.05, 0.4, 0.1, 20, 0.01, 50);

            transform = new TranslateTransform3D();

            model = GenerateModel();
            model.Transform = transform;

            animation = new DoubleAnimation();
            animation.From = -5;
            animation.To = 5;
            animation.RepeatBehavior = RepeatBehavior.Forever;
        }

        public ModelVisual3D Model { get { return model; } }

        private ModelVisual3D AddCartBodyCylindre(Point3D center, double height, double radius, AxisAngleRotation3D rotation, Color color, int slices)
        {
            var cartBodyMesh = new GeometryModel3D();
            cartBodyMesh.Geometry = MeshGenerators.GenerateCylinder(center, height, radius, slices);
            cartBodyMesh.Transform = new RotateTransform3D(rotation, center);
            cartBodyMesh.Material = new DiffuseMaterial(new SolidColorBrush(color));
            var cartBodyPart = new ModelVisual3D();
            cartBodyPart.Content = cartBodyMesh;
            return cartBodyPart;
        }

        private ModelVisual3D GenerateModel()
        {
            var model = new ModelVisual3D();

            foreach (var wheel in wheels)
            {
                model.Children.Add(wheel.Model);
            }

            model.Children.Add(AddCartBodyCylindre(
                    new Point3D(0.3, 0, 0), 1, 0.05,
                    new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90), Colors.Gray, 10));

            model.Children.Add(AddCartBodyCylindre(
                new Point3D(0.3, 0, 1), 1, 0.05,
                new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90), Colors.Gray, 10));

            // main body
            var center = new Point3D(0.3, 0.2, 0.5);
            var height = 2;
            var radius = 0.3;
            var transform3DGroup = new Transform3DGroup();
            transform3DGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90), center));
            transform3DGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 45), center));

            var cartBodyMesh = new GeometryModel3D();
            cartBodyMesh.Geometry = MeshGenerators.GenerateCylinder(center, height, radius, 4);
            cartBodyMesh.Transform = transform3DGroup;
            cartBodyMesh.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Gray));
            var cartBody = new ModelVisual3D();
            cartBody.Content = cartBodyMesh;
            model.Children.Add(cartBody);

            return model;
        }

        public void Run(double speed)
        {
            animation.Duration = TimeSpan.FromSeconds(10 / speed);
            transform.BeginAnimation(TranslateTransform3D.OffsetZProperty, animation);
            foreach (var wheel in wheels)
            {
                wheel.Run(speed);
            }
        }
    }
}

