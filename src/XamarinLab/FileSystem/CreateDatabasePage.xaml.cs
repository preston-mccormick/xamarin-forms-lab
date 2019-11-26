using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinLab.FileSystem
{
    [DisplayName("Try Create Database")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateDatabasePage : ContentPage
    {
        public CreateDatabasePage()
        {
            InitializeComponent();
        }

        private async void CreateDatabaseButtonClickedAsync(object sender, EventArgs e)
        {
            CreateDatabaseButton.IsVisible = false;

            IPlatformSpecialFolders psf = DependencyService.Get<IPlatformSpecialFolders>();

            var dirs = new List<string>();
            dirs.AddRange(psf.SpecialFolders.Values);
            dirs.AddRange(GetEnvironmentSpecialFolders());

            var cans = new List<string>();
            var cants = new List<string>();
            foreach (string dir in dirs)
            {
                try
                {
                    string dbpath = Path.Combine(dir, "test.db3");
                    var dbPath = await Task<string>.Run(() =>
                    {
                        var sqlite = new SQLiteAsyncConnection(dbpath);
                        string path = sqlite.DatabasePath;
                        File.Delete(path);
                        return path;
                    }
                    );
                    cans.Add(dbPath);
                }
                catch (Exception error)
                {
                    cants.Add(error.Message);
                }
            }

            AddCanControls(cans);
            AddCantControls(cants);
        }

        private void AddCanControls(IEnumerable<string> cans)
        {
            foreach (string can in cans)
            {
                ResultsStack.Children.Add(
                    new Label()
                    {
                        Text = can,
                        BackgroundColor = Color.LightGreen,
                        TextColor = Color.DarkGreen,
                        Padding = new Thickness(10, 10)
                    }
                    );
            }
        }
        private void AddCantControls(IEnumerable<string> cants)
        {
            foreach (string cant in cants)
            {
                ResultsStack.Children.Add(
                    new Label()
                    {
                        Text = cant,
                        BackgroundColor = Color.LightPink,
                        TextColor = Color.DarkRed,
                        Padding = new Thickness(10, 10)
                    }
                    );
            }
        }
        private List<string> GetEnvironmentSpecialFolders()
        {
            var specials = new List<string>();
            foreach (Environment.SpecialFolder special in (Environment.SpecialFolder[])Enum.GetValues(typeof(Environment.SpecialFolder)))
            {
                string specialPath = Environment.GetFolderPath(special);
                if (!String.IsNullOrWhiteSpace(specialPath))
                {
                    specials.Add(specialPath);
                }
            }
            return specials;
        }
    }
}