using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinLab.FileSystem
{
    [DisplayName("File Picker Tests")]

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilePickerPage : ContentPage
    {
        public FilePickerPage()
        {
            InitializeComponent();
        }
        FileData FileData { get; set; }

        private async void PickFileButtonClicked(object sender, EventArgs e)
        {
            try
            {
                FileData = await CrossFilePicker.Current.PickFile();
                if (FileData == null)
                {

                    OpenFileButton.IsVisible = OpenFileButton.IsEnabled = false;
                    return; // user canceled file picking
                }

                SelectedPathLabel.Text = FileData.FilePath;
                SelectedPathLabel.IsVisible = true;
                OpenFileButton.IsVisible = true;
                OpenFileButton.IsEnabled = true;
                //string fileName = fileData.FileName;

                //string contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);

                //System.Console.WriteLine("File name chosen: " + fileName);
                //System.Console.WriteLine("File data: " + contents);
            }
            catch (Exception ex)
            {
                OpenFileButton.IsVisible = OpenFileButton.IsEnabled = false;
                ErrorLabel.Text = ex.ToString();
                ErrorLabel.IsVisible = true;
            }
        }
        private async void OpenFileButtonClicked(object sender, EventArgs e)
        {
            try
            {
                string filePath = SelectedPathLabel.Text;

                var fi = new FileInfo(filePath);
                if (fi.Extension == ".txt")
                {
                    string contents = Encoding.UTF8.GetString(FileData.DataArray);
                    TextContentLabel.Text = contents;

                    //TextContentLabel.Text = File.ReadAllText(filePath);
                    TextContentLabel.IsVisible = true;
                    return;
                }
                if (fi.IsReadOnly)
                {
                    ErrorLabel.Text = "The file is read-only.";
                    ErrorLabel.IsVisible = true;
                    return;
                }

                SQLiteConnection sqlite = new SQLiteConnection(filePath);

            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
                ErrorLabel.IsVisible = true;
            }
        }

        private void CreateDatabaseButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var conn = new SQLiteConnection("foo.sqlite");
            }            
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
                ErrorLabel.IsVisible = true;
            }

}
    }
}