using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinLab.FileSystem
{
    [DisplayName("Platform File System Info")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileSystemInfoPage : ContentPage
    {
        public FileSystemInfoPage()
        {
            InitializeComponent();

            try
            {
                BindingContext = DependencyService.Get<IFileSystemInfo>();
            }
            catch (Exception error)
            {
                stack.Children.Clear();
                stack.Children.Add(new Label() { Text = error.Message, BackgroundColor = Color.LightPink, TextColor = Color.DarkRed });
                return;
            }
        }
    }
}