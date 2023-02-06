using _3DTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SolarSystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new MainWindow());
        }


        public MainWindow()
        {
            InitializeComponent();

            this.Background = Brushes.Black;

            Viewport3D viewport = new Viewport3D();
            Content = viewport;

            Model3DGroup lightGroup = new Model3DGroup();
            lightGroup.Children.Add(new PointLight(Colors.White, new Point3D(0, 0, 0)));

            ModelVisual3D lightModvis = new ModelVisual3D();
            lightModvis.Content = lightGroup;
            viewport.Children.Add(lightModvis);

            PerspectiveCamera cam = new PerspectiveCamera(new Point3D(0, 0, 8),
                new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), 45);

            Trackball trackball = new Trackball();
            cam.Transform = trackball.Transform;
            trackball.EventSource = this;

            viewport.Camera = cam;

            Dictionary<string, SpaceObject> solarSystem = new Dictionary<string, SpaceObject>();
            
            solarSystem.Add("Sun", new SpaceObject(radius: 0.5, isStar: true, brush: GetBrush.Sun));
            solarSystem.Add("Earth", new SpaceObject(radius: 0.12, rotationTime: 1, zAngleRotation: 23.44, orbitRadius: 4, orbitRotationTime: 365, center: solarSystem["Sun"], brush: GetBrush.Earth));
            solarSystem.Add("Moon", new SpaceObject(radius: 0.03474, rotationTime: 1, orbitRadius: 0.5, orbitRotationTime: 1, center: solarSystem["Earth"], brush: GetBrush.Moon));
            solarSystem.Add("Mars", new SpaceObject(radius: 0.09, rotationTime: 1, zAngleRotation: 25, orbitRadius: 7, orbitRotationTime: 687, center: solarSystem["Sun"], brush: GetBrush.Mars));
            solarSystem.Add("Phobos", new SpaceObject(radius: 0.05, rotationTime: 0.8, orbitRadius: 0.6, orbitRotationTime: 3, center: solarSystem["Mars"], brush: GetBrush.Moon));
            solarSystem.Add("Deimos", new SpaceObject(radius: 0.05, rotationTime: 2, orbitRadius: 1, orbitRotationTime: 0.6, center: solarSystem["Mars"], brush: GetBrush.Moon));
            solarSystem.Add("Venus", new SpaceObject(radius: 0.087, rotationTime: 243, zAngleRotation: 3, orbitRadius: 2, orbitRotationTime: 225, center: solarSystem["Sun"], reverseRotation: true, brush: GetBrush.Venus));
            solarSystem.Add("Mercury", new SpaceObject(radius: 0.035, rotationTime: 57, zAngleRotation: 7, orbitRadius: 1, orbitRotationTime: 88, center: solarSystem["Sun"], brush: GetBrush.Mercury));

            /* Larger version
            solarSystem.Add("Sun", new SpaceObject(radius: 0.5, isStar: true, brush: GetBrush.Sun));
            solarSystem.Add("Earth", new SpaceObject(radius: 0.2, rotationTime: 1, zAngleRotation: 23.44, orbitRadius: 4, orbitRotationTime: 365, center: solarSystem["Sun"], brush: GetBrush.Earth));
            solarSystem.Add("Moon", new SpaceObject(radius: 0.1, rotationTime: 1, orbitRadius: 0.5, orbitRotationTime: 1, center: solarSystem["Earth"], brush: GetBrush.Moon));
            solarSystem.Add("Mars", new SpaceObject(radius: 0.2, rotationTime: 1, zAngleRotation: 25, orbitRadius: 7, orbitRotationTime: 687, center: solarSystem["Sun"], brush: GetBrush.Mars));
            solarSystem.Add("Phobos", new SpaceObject(radius: 0.1, rotationTime: 0.8, orbitRadius: 0.6, orbitRotationTime: 3, center: solarSystem["Mars"], brush: GetBrush.Moon));
            solarSystem.Add("Deimos", new SpaceObject(radius: 0.1, rotationTime: 2, orbitRadius: 1, orbitRotationTime: 0.6, center: solarSystem["Mars"], brush: GetBrush.Moon));
            solarSystem.Add("Venus", new SpaceObject(radius: 0.2, rotationTime: 243, zAngleRotation: 3, orbitRadius: 2, orbitRotationTime: 225, center: solarSystem["Sun"], reverseRotation: true, brush: GetBrush.Venus));
            solarSystem.Add("Mercury", new SpaceObject(radius: 0.2, rotationTime: 57, zAngleRotation: 7, orbitRadius: 1, orbitRotationTime: 88, center: solarSystem["Sun"], brush: GetBrush.Mercury));
            */

            solarSystem.Values.ToList().ForEach(celestialObject => viewport.Children.Add(celestialObject.Model3D));

            /* Separating plane
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(new Point3D(-10, 0, -10));
            mesh.Positions.Add(new Point3D(-10, 0, 10));
            mesh.Positions.Add(new Point3D(10, 0, -10));
            mesh.Positions.Add(new Point3D(10, 0, 10));
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(3);

            GeometryModel3D geomod = new GeometryModel3D();
            geomod.Geometry = mesh;
            geomod.Material = new EmissiveMaterial(Brushes.Silver);
            geomod.BackMaterial = new EmissiveMaterial(Brushes.Silver);

            // ModelVisual3D
            ModelVisual3D modvis = new ModelVisual3D();
            modvis.Content = geomod;
            viewport.Children.Add(modvis);
            */
        }
    }
}

