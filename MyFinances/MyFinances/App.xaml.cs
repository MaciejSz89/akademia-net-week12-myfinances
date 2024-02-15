using MyFinances.Services;
using MyFinances.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyFinances
{
    public partial class App : Application
    {
        public static string BackendUrl = "http://10.0.2.2:80/api/";
        public App()
        {
            InitializeComponent();

            DependencyService.Register<IOperationService, OperationSqliteService>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
