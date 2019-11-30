using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using Xamarin.Forms;

namespace XamarinLab.PageMenu
{
    public class PageMenuModel
    {
        INavigation Navigation { get; }
        public PageMenuModel(INavigation navigation, Assembly assembly, params Type[] exclude)
        {
            Navigation = navigation;
            Pages = new ObservableList<PageModel>(GetPages(assembly, exclude));
        }

        public PageMenuModel(INavigation navigation, params Type[] exclude) : this(navigation, Assembly.GetCallingAssembly(), exclude) { }

        public string Title { get; } = "Page Directory";

        public Action<Page> Navigate { get; set; } = delegate (Page p) { return; };

        public ObservableList<PageModel> Pages { get; }

        public List<Group<PageModel>> GroupPagesByNamespace(List<Type> pageTypes)
        {
            var namespaces = new Dictionary<string, List<PageModel>>();
            foreach (var type in pageTypes)
            {
                string ns = type.Namespace;
                if (!namespaces.ContainsKey(type.Namespace))
                {
                    namespaces[type.Namespace] = new List<PageModel>();
                }
                namespaces[type.Namespace].Add(new PageModel(Navigation, type));
            }

            // Repackage as a list.
            List<Group<PageModel>> groups = new List<Group<PageModel>>();
            foreach (string ns in namespaces.Keys)
            {
                // Sort the pages by display name.
                namespaces[ns].Sort();

                groups.Add(new Group<PageModel>(ns, namespaces[ns]));
            }
            // Sort by Name
            groups.Sort();

            return groups;
        }


        public List<Group<PageModel>> GetPagesInCategories(List<Type> pageTypes)
        {
            var cats = new Dictionary<string, List<PageModel>>();
            foreach (var pageType in pageTypes)
            {
                string category = GetCategory(pageType);
                if (!cats.ContainsKey(category))
                {
                    cats[category] = new List<PageModel>();
                }
                cats[category].Add(new PageModel(Navigation, pageType));
            }

            // Repackage as a list.
            List<Group<PageModel>> list = new List<Group<PageModel>>();
            foreach (string key in cats.Keys)
            {
                // Sort the pages by display name.
                cats[key].Sort((p1, p2) => (p1.Name.CompareTo(p2.Name)));

                list.Add(new Group<PageModel>(key, cats[key]));
            }

            list.Sort();

            return list;
        }

        public List<PageModel> GetPages(Assembly assembly, params Type[] exclude)
        {
            List<PageModel> pageList = new List<PageModel>();

            // Get the pages and group them by category.
            var pageTypes = GetPageTypes(assembly, exclude);
            foreach (var pageType in pageTypes)
            {
                pageList.Add(new PageModel(Navigation, pageType));
            }

            // Sort the pages by display name.
            pageList.Sort((p1, p2) => (p1.Name.CompareTo(p2.Name)));


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
