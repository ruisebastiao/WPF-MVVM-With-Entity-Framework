namespace VH.Model
{
    public interface IListPaging
    {
        #region Properties
        bool IsPagingAvailable { get; }
        int CurrentPageNumber { get; set; }
        int NextPageNumber { get; }
        int NumberOfRemainingPage { get; set; }
        #endregion        
    }
}
