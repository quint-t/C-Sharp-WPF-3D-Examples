using System.Windows.Media.Media3D;

namespace Pipe3D
{
    public interface IPipe
    {
        ModelVisual3D ModelVisual();

        void Rotate(double x, double y, double z, double angle);

        void UndoRotate();
    }
}

