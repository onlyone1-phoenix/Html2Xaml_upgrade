using System;
using System.Windows;

namespace HTMLConverter
{
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            var app = new App();
            app.Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var testWindow = new TestWindow();
            testWindow.Show();
        }
    }
}
