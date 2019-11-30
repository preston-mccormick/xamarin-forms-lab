using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinLab.PageMenu
{
    public class PageModel : IComparable<PageModel>
    {
        public PageModel(INavigation navigation, Type pageType)
        {
            if (!pageType.IsSubclassOf(typeof(Page))) throw new ArgumentException("The type must derive from Page");

            Navigation = navigation;

            PageType = pageType;
            Name = GetDisplayName(pageType);
            Description = GetDescriptionName(pageType);
            FullName = pageType.FullName;

            ShowPageCommand = new Command(() => ShowPage());
        }

        public string Description { get; set; }

        public string Name { get; set; }

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

        /// <summary>
        /// Default sort by Name.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(PageModel other)
        {
            if (other == null) return -1;
            else return Name.CompareTo(other.Name);
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