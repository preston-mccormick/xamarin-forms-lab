using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinLab.Essentials
{
    [DisplayName("Environment: TableView")]
    public class TableViewPage : ContentPage
    {
        public TableViewPage()
        {
            Title = "Environment: TableView";
            TableRoot root = new TableRoot("Xamarin.Essentials")
            {
                new TableSection("App Info")
                {
                    new EntryCell { Label = "Package", Text = AppInfo.PackageName, IsEnabled = false },
                    new EntryCell { Label = "Name", Text = AppInfo.Name, IsEnabled = false },
                    new EntryCell { Label = "Version", Text = AppInfo.VersionString, IsEnabled = false },
                    new EntryCell { Label = "Build", Text = AppInfo.BuildString, IsEnabled = false },
                    new ViewCell { View = new Frame { Padding=new Thickness(10, 1), BackgroundColor=Color.Transparent,
                        Content = new Button { Text = "Show App Settings",
                            Command = new Command(AppInfo.ShowSettingsUI),
                            HorizontalOptions =LayoutOptions.Center } }
                    }
                },
                new TableSection("Device Info")
                {
                    new EntryCell { Label = "Type", Text = DeviceInfo.DeviceType.ToString() },
                    new EntryCell { Label = "Idiom", Text = DeviceInfo.Idiom.ToString() },
                    new EntryCell { Label = "Manufacturer", Text = DeviceInfo.Manufacturer },
                    new EntryCell { Label = "Model", Text = DeviceInfo.Model },
                    new EntryCell { Label = "Name", Text = DeviceInfo.Name, IsEnabled = false },
                    new EntryCell { Label = "Platform", Text = DeviceInfo.Platform.ToString() },
                    new EntryCell { Label = "Version", Text = DeviceInfo.Version.ToString() },
                    new EntryCell { Label = "VersionString", Text = DeviceInfo.VersionString }
                }
            };
            Content = new TableView
            {
                Intent = TableIntent.Data,
                Root = root
            };
        }
    }
}