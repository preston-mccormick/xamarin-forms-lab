using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace XamarinLab
{
    [Serializable]
    public class ObservableList<T> : ObservableCollection<T>
    {
        public ObservableList(IEnumerable<T> collection) : base(collection)
        {

        }

        public ObservableList() : base()
        {

        }

        #region "Sort"

        public void Sort()
        {
            this.Sort(0, Count, null);
        }

        public void Sort(IComparer<T> comparer)
        {
            this.Sort(0, Count, comparer);
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            List<T> internalList = (List<T>)Items;
            internalList.Sort(index, count, comparer);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Sort(Comparison<T> comparison)
        {
            List<T> internalList = (List<T>)Items;
            internalList.Sort(comparison);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion


        #region "AddRange"

        public void AddRange(IEnumerable<T> collection)
        {
            List<T> internalList = (List<T>)Items;
            internalList.AddRange(collection);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion



    }

}
