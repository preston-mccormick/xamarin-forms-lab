using System.Collections.Generic;

namespace XamarinLab.PageList
{
    public class PageCategory
    {
        public PageCategory() : this(null, null)
        {

        }

        public PageCategory(string text, List<PageViewModel> pages = null)
        {
            Text = text;
            _pages = pages ?? new List<PageViewModel>();
        }

        public string Text { get; set; }

        public IReadOnlyList<PageViewModel> Pages { get { return _pages; } }

        readonly List<PageViewModel> _pages = new List<PageViewModel>();
    }
}
