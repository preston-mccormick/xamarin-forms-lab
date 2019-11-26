using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinLab.PageList
{
    public class ModalAttribute : Attribute
    {
        public ModalAttribute(bool isModal = true)
        {
            IsModal = isModal;
        }

        public bool IsModal { get; }
    }
}
