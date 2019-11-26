using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinLab.PageList
{
    public class PageViewModel
    {
        public PageViewModel(INavigation navigation, Type pageType)
        {
            if (!pageType.IsSubclassOf(typeof(Page))) throw new ArgumentException("The type must derive from Page");

            Navigation = navigation;

            PageType = pageType;
            DisplayName = GetDisplayName(pageType);
            Description = GetDescriptionName(pageType);
            FullName = pageType.FullName;

            ShowPageCommand = new Command(() => ShowPage());
        }

        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public bool IsModal { get; set; }
        public Type PageType { get; set; }
        public ICommand ShowPageCommand { get; set; }
        private INavigation Navigation { get; }

        private static string GetDescriptionName(Type type)
        {
            var att = type.GetCustomAttribute<DescriptionAttribute>();
            if (att == null) return string.Empty;
            else return att.Description;
        }

        private static string GetDisplayName(Type type)
        {
            var dna = type.GetCustomAttribute<DisplayNameAttribute>();
            if (dna == null) return type.Name;
            else return dna.DisplayName;
        }

        private async void ShowPage()
        {
            if (Activator.CreateInstance(PageType) is Page nextPage)
            {
                await Navigation.PushAsync(nextPage);
                return;
            }
        }
    }
}