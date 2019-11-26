using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinLab.FileSystem
{
    public interface IPlatformSpecialFolders
    {
        Dictionary<string, string> SpecialFolders { get; }
    }
}
