using System;
using System.Collections.Generic;
using System.ComponentModel;

using Xamarin.Forms;
using XamarinLab.FileSystem;

namespace XamarinLab
{
    [DisplayName("Special Folder List")]
    public class SpecialFoldersPage : ContentPage
    {
        public SpecialFoldersPage()
        {
            Dictionary<Environment.SpecialFolder, string> specials = new Dictionary<Environment.SpecialFolder, string>();
            foreach (Environment.SpecialFolder special in (Environment.SpecialFolder[])Enum.GetValues(typeof(Environment.SpecialFolder)))
            {
                try
                {
                    if (!specials.ContainsKey(special))
                    {
                        string specialPath = Environment.GetFolderPath(special);
                        if (!String.IsNullOrWhiteSpace(specialPath))
                        {
                            specials.Add(special, specialPath);
                        }
                    }
                }
                catch (Exception error)
                {
                    specials.Add(special, error.Message);
                }
            }

            StackLayout stack = new StackLayout() { Padding = new Thickness(15, 0, 15, 30) };
            stack.Children.Add(new Label() { Text = "System.Environment.SpecialFolders", FontAttributes=FontAttributes.Bold, Margin=new Thickness(0, 10, 0, 0) });
            foreach (var pair in specials)
            {
                stack.Children.Add(new Label() { Text = $"{pair.Key} = {pair.Value}" });
            }

            stack.Children.Add(new Label() { Text = "IPlatformFileSystem.SpecialFolders", FontAttributes = FontAttributes.Bold, Margin=new Thickness(0,10,0,0) });
            IPlatformSpecialFolders sf = DependencyService.Get<IPlatformSpecialFolders>();
            foreach (var pair in sf.SpecialFolders)
            {
                stack.Children.Add(new Label() { Text = $"{pair.Key} = {pair.Value}" });
            }

            var scroll = new ScrollView();
            scroll.Content = stack;
            Content = scroll;

        }
    }
}