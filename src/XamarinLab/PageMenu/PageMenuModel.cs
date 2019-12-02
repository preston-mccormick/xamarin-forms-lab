using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace PageMenu
{
    public class Group<T> : IComparable<Group<T>>
    {
        public Group() : this(null, null)
        {
        }

        public Group(string name, IEnumerable<T> members = null)
        {
            Name = name;
            Members = new ObservableCollection<T>(members) ?? new ObservableCollection<T>();
        }

        public ObservableCollection<T> Members { get; }
        public string Name { get; set; }

        public int CompareTo(Group<T> other)
        {
            if (other == null) return -1;
            return Name.CompareTo(other.Name);
        }
    }

    public class PageMenuModel
    {
        public PageMenuModel(INavigation navigation, Assembly assembly, params Type[] exclude)
        {
            Navigation = navigation;
            Pages = new ObservableCollection<PageModel>(GetPages(assembly, exclude));
        }

        public PageMenuModel(INavigation navigation, params Type[] exclude) : this(navigation, Assembly.GetCallingAssembly(), exclude)
        {
        }

        public Action<Page> Navigate { get; set; } = delegate (Page p) { return; };

        public ObservableCollection<PageModel> Pages { get; }

        public string Title { get; } = "Page Directory";

        private INavigation Navigation { get; }

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

        public List<Group<PageModel>> GetPagesByCategories(List<Type> pageTypes)
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

    public class PageModel : IComparable<PageModel>
    {
        public PageModel(INavigation navigation, Type pageType)
        {
            if (!pageType.IsSubclassOf(typeof(Page))) throw new ArgumentException("The type must derive from Page");

            Navigation = navigation;

            PageType = pageType;
            Name = GetDisplayNameAttribute(pageType);
            Description = GetDescriptionAttribute(pageType);
            FullName = pageType.FullName;

            ShowPageCommand = new Command(() => ShowPage(), ShowPageIsEnabled);
        }

        public string Description { get; set; }

        public string FullName { get; }

        public string Name { get; set;  }

        public Type PageType { get; }

        public ICommand ShowPageCommand { get; }

        private INavigation Navigation { get; }

        /// <summary>
        /// Default sort by Name.
        /// </summary>
        public int CompareTo(PageModel other)
        {
            if (other == null) return -1;
            else return Name.CompareTo(other.Name);
        }

        protected virtual async void ShowPage()
        {
            if (Activator.CreateInstance(PageType) is Page nextPage)
            {
                await Navigation.PushAsync(nextPage);
                return;
            }
        }

        protected virtual bool ShowPageIsEnabled()
        {
            return true;
        }

        private static string GetDescriptionAttribute(Type type)
        {
            var att = type.GetCustomAttribute<DescriptionAttribute>();
            if (att == null) return string.Empty;
            else return att.Description;
        }

        private static string GetDisplayNameAttribute(Type type)
        {
            var dna = type.GetCustomAttribute<DisplayNameAttribute>();
            if (dna == null) return type.Name;
            else return dna.DisplayName;
        }

    }
}