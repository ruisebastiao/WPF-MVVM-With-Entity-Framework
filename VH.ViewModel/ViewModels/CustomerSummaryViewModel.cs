using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using VH.DataAccess;
using VH.Model;

namespace VH.ViewModel
{
    public class CustomerSummaryViewModel : BaseViewModel<Customer>
    {
        #region Fields
        
        #endregion

        #region Properties
        
        #endregion

        #region Command Properties
        
        #endregion

        #region Constructors
        public CustomerSummaryViewModel(IMessenger messenger, UserLogin userLogin, Customer customer)
            : base(messenger, userLogin)
        {
            this.Entity = customer;
            this.RefreshCustomerHearingAidOrderCollection();
            this.RefreshCustomerEarMoldOrderCollection();
            this.GetRefreshCustomerRepairCollection();
        }
        #endregion

        #region Override Methods
        
        #endregion

        #region Command Methods
        
        #endregion

        #region Private Methods
        
        #endregion

        #region Public Methods
        private void GetRefreshCustomerRepairCollection()
        {

            Task.Factory.StartNew(() =>
            {
                var items = CustomerAction.GetCustomerRepairList(this.DBConnectionString, this.Entity);
                if (items != null && items.InternalList.Any())
                {
                    this.Entity.CustomerRepairCollection = null;
                    this.Entity.CustomerRepairCollection = items.InternalList;
                }
            });
        }

        private void RefreshCustomerEarMoldOrderCollection()
        {
            Task.Factory.StartNew(() =>
            {
                var items = CustomerAction.GetCustomerEarMoldOrderList(this.DBConnectionString, this.Entity);
                if (items != null && items.InternalList.Any())
                    this.Entity.CustomerEarMoldOrderCollection = items.InternalList;
            });
        }

        private void RefreshCustomerHearingAidOrderCollection()
        {
            Task.Factory.StartNew(() =>
            {
                var items = CustomerAction.GetCustomerHearingAidOrderList(this.DBConnectionString, this.Entity);
                if (items != null && items.InternalList.Any())
                    this.Entity.CustomerHearingAidOrderCollection = items.InternalList;
            });
        }
        #endregion



    }
}