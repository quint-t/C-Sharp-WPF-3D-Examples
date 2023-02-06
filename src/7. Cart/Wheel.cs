using System;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace CartProject
{
    class Wheel
    {
        double bigRadius;
        double smallRadius;
        double width;
        double thickness;
        double spokeAmount;
        double spokeThikness;
        int precision;
        Point3D center;
        ModelVisual3D model;
        AxisAngleRotation3D axisRotation;
        DoubleAnimation animation;


        public Wheel(
            Point3D center,
            double bigRadius,
            double smallRadius,
            double width,
            double thickness,
            double spokeAmount,
            double spokeThikness,
            int precision)
        {
            this.center = center;
            this.bigRadius = bigRadius;
            this.smallRadius = smallRadius;
            this.width = width;
            this.thickness = thickness;
            this.spokeAmount = spokeAmount;
            this.spokeThikness = spokeThikness;
            this.precision = precision;

            model = GenerateModel();

            animation = new DoubleAnimation();
            animation.RepeatBehavior = RepeatBehavior.Forever;

            axisRotation = new AxisAngleRotation3D();
            axisRotation.Axis = new Vector3D(1, 0, 0);

            model.Transform = new RotateTransform3D(axisRotation, center);
        }

        public ModelVisual3D Model
        {
            get { return model; }
        }

        private ModelVisual3D GenerateModel()
        {
            var outerWheelGeometry = new GeometryModel3D();
            outerWheelGeometry.Geometry = MeshGenerators.GenerateRing(center, bigRadius, thickness, width, precision);
            outerWheelGeometry.Material = new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(80, 80, 80)));
            outerWheelGeometry.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));

            var outerWheelPart = new ModelVisual3D();
            outerWheelPart.Content = outerWheelGeometry;

            var innerWheelGeometry = new GeometryModel3D();
            innerWheelGeometry.Geometry = MeshGenerators.GenerateRing(center, 0, smallRadius, width, precision);
            innerWheelGeometry.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Gray));
            innerWheelGeometry.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));

            var innerWheelPart = new ModelVisual3D();
            innerWheelPart.Content = innerWheelGeometry;

            var model = new ModelVisual3D();
            model.Children.Add(innerWheelPart);
            model.Children.Add(outerWheelPart);

            for (int spokeNumber = 0; spokeNumber != spokeAmount; spokeNumber++)
            {
                var spokeGeometry = new GeometryModel3D();
                spokeGeometry.Geometry = MeshGenerators.GenerateCylinder(center + new Vector3D(-width / 2, bigRadius / 2, 0), bigRadius + thickness / 2, spokeThikness, 10);
                spokeGeometry.Material = new DiffuseMaterial(new SolidColorBrush(Colors.LightGray));
                spokeGeometry.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 360 * spokeNumber / spokeAmount), center);
                var spokeModel = new ModelVisual3D();
                spokeModel.Content = spokeGeometry;

                model.Children.Add(spokeModel);
            }

            return model;
        }

        public void Run(double speed)
        {
            if (speed > 0)
            {
                animation.From = 0;
                animation.To = 360;
            }
            else
            {
                animation.From = 360;
                animation.To = 0;
                speed *= -1;
            }

            animation.Duration = TimeSpan.FromSeconds(Math.PI * 2 * (bigRadius + thickness) / speed);
            axisRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
        }

        public void Stop()
        {
            axisRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, null);
        }
    }
}

