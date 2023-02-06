using _3DTools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CartProject
{
    public class MainWindow : Window
    {
        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new MainWindow());
        }

        public MainWindow() {
            Viewport3D viewport = new Viewport3D();
            Content = viewport;

            var cart = new Cart();
            cart.Run(1);

            ModelVisual3D model = new ModelVisual3D();
            model.Children.Add(cart.Model);
            viewport.Children.Add(model);

            Model3DGroup modgrp = new Model3DGroup();
            modgrp.Children.Add(new DirectionalLight(Colors.White, new Vector3D(-1, -1, -1)));
            modgrp.Children.Add(new DirectionalLight(Colors.White, new Vector3D(1, 1, 1)));

            ModelVisual3D modvis = new ModelVisual3D();
            modvis.Content = modgrp;
            viewport.Children.Add(modvis);

            PerspectiveCamera cam = new PerspectiveCamera(new Point3D(10, 0, -3),
                            new Vector3D(-3, 0, 1), new Vector3D(0, 1, 0), 45);
            Trackball trackball = new Trackball();
            cam.Transform = trackball.Transform;
            trackball.EventSource = this;
            viewport.Camera = cam;
        }
    }
}

