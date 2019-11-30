using System;
using System.Collections.Generic;

namespace XamarinLab.PageMenu
{
    public class Group<T> : IComparable<Group<T>>
    {
        public Group() : this(null, null)
        {
        }

        public Group(string name, IEnumerable<T> members = null)
        {
            Name = name;
            Members = new ObservableList<T>(members) ?? new ObservableList<T>();
        }

        public ObservableList<T> Members { get; }
        public string Name { get; set; }

        public int CompareTo(Group<T> other)
        {
            if (other == null) return -1;
            return Name.CompareTo(other.Name);
        }
    }
}