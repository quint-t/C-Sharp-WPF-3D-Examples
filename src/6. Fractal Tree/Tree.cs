using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace FractalTree
{
    class Tree
    {
        private Tree leftSubTree;
        private Tree rightSubTree;

        private Point3D startPoint;
        private Vector3D direction;
        private Vector3D planeSettingVector;

        private double branchSize;
        private double angleBetweenBranches;
        private double branchReductionRatio;
        private double branchWidthHeightRatio;
        private double branchGrowTime;

        private int branchSmoothness;
        private int step;
        private int treeHeight;

        private Transform3D treeTranslateTransform;
        private Transform3D treeRotationTransform;
        private Transform3D branchScaleTransform;
        private Transform3D leftBranchRotationTransform;
        private Transform3D rightBranchRotationTransform;

        private DoubleAnimation growAnimation;
        private Transform3D growTransform;

        private ModelVisual3D model;

        private static readonly List<MeshGeometry3D> cylinderCollection = new MeshGeometry3D[20].Select(
            (MeshGeometry3D element, int index) => Tree.GenerateCylinder(new Point3D(0, 0.5, 0), 1, 0.5, 36, 36)
            ).ToList();

        private static int maxBranchStep = 10;

        public Tree(
            Point3D? startPoint = null,
            Vector3D? direction = null,
            double branchSize = 1,
            double angleBetweenBranches = 90,
            double branchReductionRatio = 2,
            double branchWidthHeightRatio = 0.1,
            double branchGrowTime = 2,
            int branchSmoothness = 4,
            int treeHeight = 9,
            Vector3D? planeSettingVector = null,
            int step = 0)
        {
            this.startPoint = startPoint ?? new Point3D(0, -2, 0);
            this.direction = direction ?? new Vector3D(0, 1, 0);
            this.branchSize = branchSize;
            this.angleBetweenBranches = angleBetweenBranches;
            this.branchReductionRatio = branchReductionRatio;
            this.branchWidthHeightRatio = branchWidthHeightRatio;
            this.branchGrowTime = branchGrowTime;
            this.branchSmoothness = Math.Max(branchSmoothness, 4);
            this.treeHeight = treeHeight;
            this.planeSettingVector = planeSettingVector ?? new Vector3D(1, 1, 0);
            this.step = step;

            this.treeTranslateTransform = GetTreeTrasnlateTransform();
            this.branchScaleTransform = GetBranchSizeTransform();
            this.treeRotationTransform = GetTreeRotationTransform();
            this.leftBranchRotationTransform = GetBranchRotationTransform(step > 0 ? angleBetweenBranches / 2 : 1);
            this.rightBranchRotationTransform = GetBranchRotationTransform(step > 0 ? -angleBetweenBranches / 2 : -1);
            this.growTransform = GetGrowAnimatedTransform();

            if (step < maxBranchStep)
            {
                this.leftSubTree = CreateBranch(step > 0 ? angleBetweenBranches / 2 : 1);
                this.rightSubTree = CreateBranch(step > 0 ? -angleBetweenBranches / 2 : -1);
            }

            this.model = Generate3DModel();
        }

        private Tree CreateBranch(double angle)
        {
            Random rnd = new Random();

            var newBranchDirection = RotateVector(this.direction, Vector3D.CrossProduct(this.direction, this.planeSettingVector), angle);

            return new Tree(
                    startPoint: this.startPoint + newBranchDirection * this.branchSize,
                    direction: newBranchDirection,
                    branchSize: this.branchSize / this.branchReductionRatio,
                    angleBetweenBranches: this.angleBetweenBranches,
                    branchReductionRatio: this.branchReductionRatio,
                    branchWidthHeightRatio: this.branchWidthHeightRatio,
                    branchGrowTime: this.branchGrowTime,
                    branchSmoothness: this.branchSmoothness,
                    treeHeight: this.treeHeight,
                    planeSettingVector: new Vector3D(rnd.Next(0, 100), 0, rnd.Next(0, 100)),
                    step: step + 1
                );
        }

        private ModelVisual3D Generate3DModel()
        {
            var model = new ModelVisual3D();

            model.Children.Add(GetBranchModel(angleBetweenBranches / 2));
            model.Children.Add(GetBranchModel(-angleBetweenBranches / 2));

            if (this.leftSubTree != null) { model.Children.Add(this.leftSubTree.Get3DModel()); }
            if (this.rightSubTree != null) { model.Children.Add(this.rightSubTree.Get3DModel()); }

            return model;
        }

        public ModelVisual3D Get3DModel()
        {
            return this.model;
        }

        public void Update(Point3D? startPoint = null,
            Vector3D? direction = null,
            double? length = null,
            double? angle = null,
            double? reductionRatio = null,
            double? thickness = null,
            double? growDuration = null,
            int? branchSmoothness = null,
            int? level = null,
            Vector3D? planeSettingVector = null,
            int? step = null)
        {
            this.startPoint = startPoint ?? this.startPoint;
            this.direction = direction ?? this.direction;
            this.branchSize = length ?? this.branchSize;
            this.angleBetweenBranches = angle ?? this.angleBetweenBranches;
            this.branchReductionRatio = reductionRatio ?? this.branchReductionRatio;
            this.branchWidthHeightRatio = thickness ?? this.branchWidthHeightRatio;
            this.branchGrowTime = growDuration ?? this.branchGrowTime;
            this.branchSmoothness = Math.Max(branchSmoothness ?? this.branchSmoothness, 3);
            this.treeHeight = level ?? this.treeHeight;
            this.planeSettingVector = planeSettingVector ?? this.planeSettingVector;
            this.step = step ?? this.step;

            ((TranslateTransform3D)this.treeTranslateTransform).OffsetX = this.startPoint.X;
            ((TranslateTransform3D)this.treeTranslateTransform).OffsetY = this.startPoint.Y;
            ((TranslateTransform3D)this.treeTranslateTransform).OffsetZ = this.startPoint.Z;

            ((ScaleTransform3D)this.branchScaleTransform).ScaleX = this.branchWidthHeightRatio * Math.Sqrt(this.branchSize);
            ((ScaleTransform3D)this.branchScaleTransform).ScaleY = this.branchSize;
            ((ScaleTransform3D)this.branchScaleTransform).ScaleZ = this.branchWidthHeightRatio * Math.Sqrt(this.branchSize);
            ((ScaleTransform3D)this.branchScaleTransform).CenterX = this.startPoint.X;
            ((ScaleTransform3D)this.branchScaleTransform).CenterY = this.startPoint.Y;
            ((ScaleTransform3D)this.branchScaleTransform).CenterZ = this.startPoint.Z;

            ((RotateTransform3D)this.treeRotationTransform).CenterX = this.startPoint.X;
            ((RotateTransform3D)this.treeRotationTransform).CenterY = this.startPoint.Y;
            ((RotateTransform3D)this.treeRotationTransform).CenterZ = this.startPoint.Z;
            ((RotateTransform3D)this.treeRotationTransform).Rotation = new AxisAngleRotation3D(Vector3D.CrossProduct(this.direction, new Vector3D(0, 1, 0)), -GetDirectionAngle());

            ((RotateTransform3D)this.leftBranchRotationTransform).CenterX = this.startPoint.X;
            ((RotateTransform3D)this.leftBranchRotationTransform).CenterY = this.startPoint.Y;
            ((RotateTransform3D)this.leftBranchRotationTransform).CenterZ = this.startPoint.Z;
            ((RotateTransform3D)this.leftBranchRotationTransform).Rotation = new AxisAngleRotation3D(Vector3D.CrossProduct(this.direction, this.planeSettingVector), step > 0 ? this.angleBetweenBranches / 2 : 1);

            ((RotateTransform3D)this.rightBranchRotationTransform).CenterX = this.startPoint.X;
            ((RotateTransform3D)this.rightBranchRotationTransform).CenterY = this.startPoint.Y;
            ((RotateTransform3D)this.rightBranchRotationTransform).CenterZ = this.startPoint.Z;
            ((RotateTransform3D)this.rightBranchRotationTransform).Rotation = new AxisAngleRotation3D(Vector3D.CrossProduct(this.direction, this.planeSettingVector), step > 0 ? -this.angleBetweenBranches / 2 : -1);

            ((ScaleTransform3D)this.growTransform).CenterX = this.startPoint.X;
            ((ScaleTransform3D)this.growTransform).CenterY = this.startPoint.Y;
            ((ScaleTransform3D)this.growTransform).CenterZ = this.startPoint.Z;

            this.growAnimation.Duration = TimeSpan.FromSeconds(this.branchGrowTime);
            this.growAnimation.BeginTime = TimeSpan.FromSeconds(this.step * this.branchGrowTime);

            if (branchSmoothness != null) ((GeometryModel3D)((ModelVisual3D)model.Children[0]).Content).Geometry = cylinderCollection[this.branchSmoothness - 3];

            if (branchSmoothness != null) ((GeometryModel3D)((ModelVisual3D)model.Children[1]).Content).Geometry = cylinderCollection[this.branchSmoothness - 3];

            if (level != null) ((GeometryModel3D)((ModelVisual3D)model.Children[0]).Content).Material = GetBranchMaterial();

            if (level != null) ((GeometryModel3D)((ModelVisual3D)model.Children[1]).Content).Material = GetBranchMaterial();

            var leftBranchDirectionForStartPoint = RotateVector(this.direction, Vector3D.CrossProduct(this.direction, this.planeSettingVector), step > 0 ? this.angleBetweenBranches / 2 : 1);
            var leftBranchDirection = RotateVector(this.direction, Vector3D.CrossProduct(this.direction, this.planeSettingVector), this.angleBetweenBranches / 2);

            this.leftSubTree?.Update(startPoint: this.startPoint + leftBranchDirectionForStartPoint * this.branchSize,
                    direction: leftBranchDirection,
                    length: this.branchSize / this.branchReductionRatio,
                    angle: this.angleBetweenBranches,
                    reductionRatio: this.branchReductionRatio,
                    thickness: this.branchWidthHeightRatio,
                    growDuration: this.branchGrowTime,
                    branchSmoothness: this.branchSmoothness,
                    level: level,
                    planeSettingVector: Vector3D.CrossProduct(this.direction, this.planeSettingVector),
                    step: step + 1);

            var rightBranchDirectionForStartPoint = RotateVector(this.direction, Vector3D.CrossProduct(this.direction, this.planeSettingVector), step > 0 ? -this.angleBetweenBranches / 2 : -1);
            var rightBranchDirection = RotateVector(this.direction, Vector3D.CrossProduct(this.direction, this.planeSettingVector), -this.angleBetweenBranches / 2);

            this.rightSubTree?.Update(startPoint: this.startPoint + rightBranchDirectionForStartPoint * this.branchSize,
                    direction: rightBranchDirection,
                    length: this.branchSize / this.branchReductionRatio,
                    angle: this.angleBetweenBranches,
                    reductionRatio: this.branchReductionRatio,
                    thickness: this.branchWidthHeightRatio,
                    growDuration: this.branchGrowTime,
                    branchSmoothness: this.branchSmoothness,
                    level: level,
                    planeSettingVector: Vector3D.CrossProduct(this.direction, this.planeSettingVector),
                    step: step + 1);
        }

        public void StartGrowAnimation()
        {
            this.growTransform.BeginAnimation(ScaleTransform3D.ScaleXProperty, null);
            this.growTransform.BeginAnimation(ScaleTransform3D.ScaleYProperty, null);
            this.growTransform.BeginAnimation(ScaleTransform3D.ScaleZProperty, null);

            ((ScaleTransform3D)this.growTransform).ScaleX = 0;
            ((ScaleTransform3D)this.growTransform).ScaleY = 0;
            ((ScaleTransform3D)this.growTransform).ScaleZ = 0;
            ((ScaleTransform3D)this.growTransform).CenterX = this.startPoint.X;
            ((ScaleTransform3D)this.growTransform).CenterY = this.startPoint.Y;
            ((ScaleTransform3D)this.growTransform).CenterZ = this.startPoint.Z;

            this.growTransform.BeginAnimation(ScaleTransform3D.ScaleXProperty, this.growAnimation);
            this.growTransform.BeginAnimation(ScaleTransform3D.ScaleYProperty, this.growAnimation);
            this.growTransform.BeginAnimation(ScaleTransform3D.ScaleZProperty, this.growAnimation);

            leftSubTree?.StartGrowAnimation();
            rightSubTree?.StartGrowAnimation();
        }

        private ModelVisual3D GetBranchModel(double angle)
        {
            var branch = new ModelVisual3D();

            var branchGeometry = GetBranchGeometry();

            var transforms = new Transform3DGroup();

            transforms.Children.Add(this.treeTranslateTransform);
            transforms.Children.Add(this.branchScaleTransform);
            transforms.Children.Add(this.treeRotationTransform);
            transforms.Children.Add(angle > 0 ? this.leftBranchRotationTransform : this.rightBranchRotationTransform);
            transforms.Children.Add(this.growTransform);

            branch.Content = branchGeometry;
            branch.Transform = transforms;

            return branch;
        }

        private GeometryModel3D GetBranchGeometry()
        {
            var branchGeometry = new GeometryModel3D();
            branchGeometry.Geometry = cylinderCollection[branchSmoothness - 3];
            branchGeometry.Material = GetBranchMaterial();
            return branchGeometry;
        }
        private Material GetBranchMaterial()
        {
            double coef = this.treeHeight < 2 ? 0 : (double)this.step / (this.treeHeight - 1);

            return (this.step + 1 > this.treeHeight) ?
                null :
                new DiffuseMaterial(
                    new SolidColorBrush(
                        Color.FromRgb((byte)(100 + 100 * (1 - coef)), (byte)(100 + 100 * coef), (byte)(100))));
        }

        private TranslateTransform3D GetTreeTrasnlateTransform()
        {
            return new TranslateTransform3D(new Vector3D(this.startPoint.X, this.startPoint.Y, this.startPoint.Z));
        }

        private RotateTransform3D GetTreeRotationTransform()
        {
            return new RotateTransform3D(new AxisAngleRotation3D(Vector3D.CrossProduct(this.direction, new Vector3D(0, 1, 0)), -GetDirectionAngle()), this.startPoint);
        }

        private RotateTransform3D GetBranchRotationTransform(double angle)
        {
            return new RotateTransform3D(new AxisAngleRotation3D(Vector3D.CrossProduct(this.direction, this.planeSettingVector), angle), this.startPoint);
        }

        private ScaleTransform3D GetBranchSizeTransform()
        {
            return new ScaleTransform3D(new Vector3D(this.branchWidthHeightRatio * this.branchSize, this.branchSize, this.branchWidthHeightRatio * this.branchSize), this.startPoint);
        }

        private ScaleTransform3D GetGrowAnimatedTransform()
        {
            growAnimation = new DoubleAnimation();

            growAnimation.From = 0;
            growAnimation.To = 1;
            growAnimation.Duration = TimeSpan.FromSeconds(this.branchGrowTime);
            growAnimation.BeginTime = TimeSpan.FromSeconds(this.step * branchGrowTime);

            ScaleTransform3D scaling = new ScaleTransform3D(new Vector3D(0, 0, 0), this.startPoint);

            scaling.BeginAnimation(ScaleTransform3D.ScaleXProperty, growAnimation);
            scaling.BeginAnimation(ScaleTransform3D.ScaleYProperty, growAnimation);
            scaling.BeginAnimation(ScaleTransform3D.ScaleZProperty, growAnimation);

            return scaling;
        }

        private Vector3D RotateVector(Vector3D source, Vector3D axis, double angle)
        {
            var m = Matrix3D.Identity;
            m.Rotate(new Quaternion(axis, angle));
            return m.Transform(source);
        }

        private double GetDirectionAngle()
        {
            return Vector3D.AngleBetween(direction, new Vector3D(0, 1, 0));
        }

        static public MeshGeometry3D GenerateCylinder(Point3D center, double h, double radius, int slices, int stacks)
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
            return mesh;
        }

    }
}

