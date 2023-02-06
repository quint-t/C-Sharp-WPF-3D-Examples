using _3DTools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FractalTree
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int angle = 45;
        private double length = 1.5;
        private double thickness = 0.2;
        private int level = 10;
        private double growDuration = 2;
        private double reductionRatio = 1.5;
        private Tree tree = new Tree();
        private Trackball trackball = new Trackball();

        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new MainWindow());
        }

        public MainWindow()
        {
            InitializeComponent();

            Title = "FractalTree";

            var angleSlider = new LabelledSlider("Угол между ветками", this.angle, 0, 180,
                           (double v) => { this.angle = Convert.ToInt32(v); this.tree.Update(angle: this.angle); }, isInteger: true);
            Grid.SetColumn(angleSlider, 0);

            var lengthSlider = new LabelledSlider("Длина ветки", this.length, 1.0, 2.0,
                           (double v) => { this.length = v; this.tree.Update(length: this.length); });
            Grid.SetColumn(lengthSlider, 1);

            var thicknessSlider = new LabelledSlider("Толщина ветки", this.thickness, 0.01, 1.0,
                           (double v) => { this.thickness = v; this.tree.Update(thickness: this.thickness); });
            Grid.SetColumn(thicknessSlider, 2);

            var levelSlider = new LabelledSlider("Число уровней дерева", this.level, 1, 20,
                           (double v) => { this.level = Convert.ToInt32(v); this.tree.Update(level: this.level); }, isInteger: true);
            Grid.SetColumn(levelSlider, 3);

            var growDurationSlider = new LabelledSlider("Время роста одной ветки (сек)", this.growDuration, 0.5, 5.0,
                           (double v) => { this.growDuration = v; this.tree.Update(growDuration: this.growDuration); });
            Grid.SetColumn(growDurationSlider, 4);

            var reductionRatioSlider = new LabelledSlider("Пропорция размеров (раз)", this.growDuration, 1.0, 2.0,
                           (double v) => { this.reductionRatio = v; this.tree.Update(reductionRatio: this.reductionRatio); });
            Grid.SetColumn(reductionRatioSlider, 5);

            this.tree.Update(angle: this.angle, length: this.length, thickness: this.thickness,
                             level: this.level, growDuration: this.growDuration, branchSmoothness: 20,
                             reductionRatio: this.reductionRatio);

            controlPanel.Children.Add(angleSlider);
            controlPanel.Children.Add(lengthSlider);
            controlPanel.Children.Add(thicknessSlider);
            controlPanel.Children.Add(levelSlider);
            controlPanel.Children.Add(growDurationSlider);
            controlPanel.Children.Add(reductionRatioSlider);

            updateButton.Click += (object sender, RoutedEventArgs args) =>
            {
                this.tree.StartGrowAnimation();
            };

            viewport.Children.Add(this.tree.Get3DModel());

            this.AddLight();

            PerspectiveCamera cam = new PerspectiveCamera(new Point3D(0, 0, 8),
                               new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), 45);
            viewport.Camera = cam;
            cam.Transform = trackball.Transform;
            trackball.EventSource = viewportBorder;
        }

        private void AddLight()
        {
            Model3DGroup lightGroup = new Model3DGroup();
            lightGroup.Children.Add(new DirectionalLight(Colors.White, new Vector3D(0, -1, 0)));
            lightGroup.Children.Add(new DirectionalLight(Colors.White, new Vector3D(-1, 0, -1)));

            ModelVisual3D lightModvis = new ModelVisual3D();
            lightModvis.Content = lightGroup;
            viewport.Children.Add(lightModvis);
        }
    }
}

