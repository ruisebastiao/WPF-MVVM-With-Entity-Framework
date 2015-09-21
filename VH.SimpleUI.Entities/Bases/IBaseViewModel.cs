using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace VH.Bases
{
    public interface IBaseViewModel : IWindowElement
    {
        bool ShowProgressBar { get; set; }
        dynamic Messenger { get; set; }
        
        IBaseViewModel ParentViewModel { get; set; }
        IBaseViewModel ChildViewModel { get; set; }
        IBaseViewModel ContentViewModel { get; set; }

        Action CloseWindow { get; set; }

        void UpdateUserLogin(dynamic userLogin);
        void HandleViewModeChanges(dynamic data);
        void Initialize();
        void CloseChild();
        void Unload();
    }
}
