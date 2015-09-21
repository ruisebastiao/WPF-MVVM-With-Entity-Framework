using GalaSoft.MvvmLight.Messaging;
using VH.Model;
using VH.Resources;
using VH.SimpleUI.Entities;

namespace VH.ViewModel
{
    public class PrintViewModel : BaseViewModel
    {
        private IVHEntity _entity;

        #region Properties
        public IVHEntity Entity
        {
            get { return _entity; }
            set
            {
                _entity = value;
                this.RaisePropertyChanged(() => this.Entity);
            }
        }

        #region Window Properties
        public override string Title
        {
            get { return TitleResources.Print; }
        }


        public override double DialogStartupCustomHeight
        {
            get
            {
                return 750;
            }
        }

        public override double DialogStartupCustomWidth
        {
            get
            {
                return 950;
            }
        }

        public override DialogType DialogType
        {
            get
            {
                return DialogType.BySizeInPixel;
            }
        }
        #endregion
        #endregion
       

        public PrintViewModel(IMessenger messenger, UserLogin userLogin, IVHEntity entity)
            : base(messenger, userLogin)
        {
            this.Entity = entity;
        }
    }
}