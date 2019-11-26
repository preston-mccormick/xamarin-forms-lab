using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using Xamarin.Forms;

namespace XamarinLab.PageList
{
    public class PageListViewModel
    {
        INavigation Navigation { get; }
        public PageListViewModel(INavigation navigation, Assembly assembly, params Type[] exclude)
        {
            Navigation = navigation;
            Pages = new ObservableList<PageViewModel>(GetPages(assembly, exclude));
        }

        public PageListViewModel(INavigation navigation, params Type[] exclude) : this(navigation, Assembly.GetCallingAssembly(), exclude) { }

        public string Title { get; } = "Page Directory";

        public Action<Page> Navigate { get; set; } = delegate (Page p) { return; };


        public ObservableList<PageViewModel> Pages { get; }



        public List<PageCategory> GetPagesInCategories(List<Type> pageTypes)
        {
            var cats = new Dictionary<string, List<PageViewModel>>();
            foreach (var pageType in pageTypes)
            {
                string category = GetCategory(pageType);
                if (!cats.ContainsKey(category))
                {
                    cats[category] = new List<PageViewModel>();
                }
                cats[category].Add(new PageViewModel(Navigation, pageType));
            }

            // Repackage as a list.
            List<PageCategory> list = new List<PageCategory>();
            foreach (string key in cats.Keys)
            {
                // Sort the pages by display name.
                cats[key].Sort((p1, p2) => (p1.DisplayName.CompareTo(p2.DisplayName)));

                list.Add(new PageCategory(key, cats[key]));
            }

            // Sort the categories by display name.
            list.Sort((c1, c2) => c1.Text.CompareTo(c2.Text));

            return list;
        }

        public List<PageViewModel> GetPages(Assembly assembly, params Type[] exclude)
        {
            List<PageViewModel> pageList = new List<PageViewModel>();

            // Get the pages and group them by category.
            var pageTypes = GetPageTypes(assembly, exclude);
            foreach (var pageType in pageTypes)
            {
                pageList.Add(new PageViewModel(Navigation, pageType));
            }

            // Sort the pages by display name.
            pageList.Sort((p1, p2) => (p1.DisplayName.CompareTo(p2.DisplayName)));


            return pageList;
        }

        /// <summary>
        /// If the page class has a Category attribute, returns that category.
        /// Otherwise, the namespace is used as the category.
        /// </summary>
        private static string GetCategory(Type type)
        {
            var ca = type.GetCustomAttribute<CategoryAttribute>();
            if (ca == null) return type.Namespace;
            else return ca.Category;
        }

        /// <summary>
        /// Searches the assembly for classes deriving from Page.
        /// </summary>
        /// 
        private static List<Type> GetPageTypes(Assembly assembly, params Type[] exclude)
        {
            var excluded = new HashSet<Type>(exclude);

            List<Type> pageTypes = new List<Type>();

            var types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (excluded.Contains(type)) continue;

                if (type.IsSubclassOf(typeof(Page)))
                {
                    pageTypes.Add(type);
                }
            }

            return pageTypes;
        }


    }
}
