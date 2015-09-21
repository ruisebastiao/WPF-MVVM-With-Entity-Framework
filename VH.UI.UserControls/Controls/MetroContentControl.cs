using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.Bases;
using VH.UI.UserControls.Helper;

namespace VH.UI.UserControls
{
    /// <summary>
    /// Originally from http://xamlcoder.com/blog/2010/11/04/creating-a-metro-ui-style-control/
    /// </summary>
    public class MetroContentControl : ContentControl
    {
        #region Fileds
        NavigationHelper _navigationHelper = new NavigationHelper();
        private IBaseViewModel _viewModelToNavTo;
        private UIElement _view;
        private UIElement _currentView;
        #endregion

        #region Properties
        public IBaseViewModel CurrentViewModel { get; protected set; }

        public List<UIElement> ViewList
        {
            get
            {
                if (_navigationHelper != null)
                    return _navigationHelper.ViewList;
                return null;
            }
        }

        public UIElement CurrentView
        {
            get
            {
                if (_navigationHelper != null)
                    return _navigationHelper.CurrentView;
                return null;
            }
            set
            {
                NavigateToExisting(value);
            }
        }
        #endregion

        #region Commands
        private ICommand _navBackCommand;
        public ICommand NavBackCommand
        {
            get { return _navBackCommand ?? (_navBackCommand = new RelayCommand(OnNavBack, CanNavBack)); }
        }

       

        private ICommand _navForwardCommand;
        public ICommand NavForwardCommand
        {
            get { return _navForwardCommand ?? (_navForwardCommand = new RelayCommand(OnNavForward, CanNavForward)); }
        }

       #endregion

        #region Dependence Property
        public static readonly DependencyProperty ReverseTransitionProperty = DependencyProperty.Register("ReverseTransition", typeof(bool), typeof(MetroContentControl), new FrameworkPropertyMetadata(false));
        #endregion

        #region Publice Methods
        public bool ReverseTransition
        {
            get { return (bool)GetValue(ReverseTransitionProperty); }
            set { SetValue(ReverseTransitionProperty, value); }
        }


        public MetroContentControl()
        {
            DefaultStyleKey = typeof(MetroContentControl);

            Loaded += MetroContentControlLoaded;
            Unloaded += MetroContentControlUnloaded;

            IsVisibleChanged += MetroContentControlIsVisibleChanged;
            RegisterMessages();
        }

        public void Reload()
        {
            if (ReverseTransition)
            {
                VisualStateManager.GoToState(this, "BeforeLoaded", true);
                VisualStateManager.GoToState(this, "AfterUnLoadedReverse", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "BeforeLoaded", true);
                VisualStateManager.GoToState(this, "AfterLoaded", true);
            }

        }
        #endregion

        #region Private Methods
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            // NavigateToView((IBaseViewModel) newContent);

            if (!IsVisible)
                VisualStateManager.GoToState(this, ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded", false);
            else
                VisualStateManager.GoToState(this, ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded", true);
        }

        void MetroContentControlIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!IsVisible)
                VisualStateManager.GoToState(this, ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded", false);
            else
                VisualStateManager.GoToState(this, ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded", true);
        }

        private void MetroContentControlUnloaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded", false);
        }

        private void MetroContentControlLoaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded", true);
        }
        #endregion

        #region Command Methods
        private bool CanNavBack()
        {
            return _navigationHelper.CanNavigateBack;
        }

        private void OnNavBack()
        {
            NavigateBack(true);
        }
        private bool CanNavForward()
        {
            return _navigationHelper.CanNavigateForward;
        }

        private void OnNavForward()
        {
            NavigateForward(true);
        }
        #endregion

        #region Private Methods

        private void RegisterMessages()
        {
            Messenger.Default.Register<IBaseViewModel>(this, NavigateToView);
        }

        public void NavigateToView(IBaseViewModel viewModelToNavTo)
        {
            if (viewModelToNavTo == null)
                throw new ArgumentNullException();

            _viewModelToNavTo = viewModelToNavTo;

            NavigateToView(ConfirmNavigation(NavigateToView));
        }

        private void NavigateToView(bool confirmed)
        {
            if (!confirmed)
                return;

            ContentControl view = new ContentControl();
            view.SetValue(JournalEntry.NameProperty, _viewModelToNavTo.Title);
            view.Content = _viewModelToNavTo;
            this._navigationHelper.NavigateToNewView(view);
            this.CurrentViewModel = _viewModelToNavTo;
            this.SetView(view);
        }

        private void NavigateToExisting(UIElement view)
        {
            _view = view;
            NavigateToExisting(ConfirmNavigation(NavigateToExisting));
        }

        private void NavigateToExisting(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            _view = (UIElement)e.Parameter;
            NavigateToExisting(ConfirmNavigation(NavigateToExisting));
        }

        private void NavigateToExisting(bool confirmed)
        {
            if (!confirmed)
            {
                return;
            }

            this._navigationHelper.NavigateToExisting(_view);
            this.SetView(_view);
        }

        private void NavigateBack(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            NavigateBack(ConfirmNavigation(NavigateBack));
        }

        private void NavigateBack(bool confirmed)
        {
            if (!confirmed)
                return;

            UIElement uiElement = _navigationHelper.NavigateBack();
            this.SetView(uiElement);
        }

        private void NavigateForward(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            NavigateForward(ConfirmNavigation(NavigateForward));
        }

        private void NavigateForward(bool confirmed)
        {
            if (!confirmed)
                return;

            UIElement uiElement = _navigationHelper.NavigateForward();
            this.SetView(uiElement);
        }

        private void SetView(UIElement view)
        {
            if(view == null)
                return;

            var newViewModel = ((view as ContentControl).Content as IBaseViewModel);
            if (newViewModel != null)
            {
                _viewModelToNavTo = newViewModel;

                this.Content = newViewModel;
                newViewModel.HandleViewModeChanges(null);
                _currentView = view;
            }
        }

        private bool ConfirmNavigation(Action<bool> callback)
        {
            return true;
        }
        #endregion
    }
}
