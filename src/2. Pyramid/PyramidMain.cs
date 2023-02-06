using System;
using System.Windows;
using System.Threading;

namespace Pyramid
{
    public class PyramidMain : Window
    {
        [STAThread]
        public static void Main()
        {
            var domainForLowLevel = AppDomain.CreateDomain("low-level");
            var domainForHighLevel = AppDomain.CreateDomain("high-level");

            CrossAppDomainDelegate actionForLowLevel = () =>
            {
                Thread thread = new Thread(() =>
                {
                    Application app = new Application();
                    app.MainWindow = new PyramidLowLevel();
                    app.MainWindow.Width = 320;
                    app.MainWindow.Height = 320;
                    app.MainWindow.Show();
                    app.Run();
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            };
            CrossAppDomainDelegate actionForHighLevel = () =>
            {
                Thread thread = new Thread(() =>
                {
                    Application app = new Application();
                    app.MainWindow = new PyramidHighLevel();
                    app.MainWindow.Width = 320;
                    app.MainWindow.Height = 320;
                    app.MainWindow.Show();
                    app.Run();
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            };

            domainForLowLevel.DoCallBack(actionForLowLevel);
            domainForHighLevel.DoCallBack(actionForHighLevel);
        }
    }
}

