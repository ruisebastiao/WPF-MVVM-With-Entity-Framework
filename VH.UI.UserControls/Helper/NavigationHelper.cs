using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VH.UI.UserControls.Helper
{
    public class NavigationHelper
    {
        private UIElement _currentView = null;
        public bool CanNavigateBack { get; private set; }
        public bool CanNavigateForward { get; private set; }
        private List<UIElement> _viewList = new List<UIElement>();

        public List<UIElement> ViewList
        {
            get { return _viewList; }
        }

        public UIElement CurrentView
        {
            get { return _currentView; }
        }

        public NavigationHelper()
        {
            CanNavigateBack = false;
            CanNavigateForward = true;
            _viewList = new List<UIElement>();
        }


        public UIElement NavigateForward()
        {
            UIElement forward = null;
            IEnumerable<UIElement> tempList = _viewList.Reverse<UIElement>();
            foreach (UIElement uiElement in tempList)
            {
                if (_currentView == uiElement)
                {
                    break;
                }
                else
                    forward = uiElement;
            }
            _currentView = forward;
            SetBackForwardAvailability();
            return forward;
        }

        public UIElement NavigateBack()
        {
            UIElement prev = null;
            foreach (UIElement uiElement in _viewList)
            {
                if (_currentView == uiElement)
                {
                    break;
                }
                else
                    prev = uiElement;
            }
            _currentView = prev;
            SetBackForwardAvailability();
            return prev;
        }

        public void NavigateToExisting(UIElement viewToNavTo)
        {
            foreach (UIElement uiElement in _viewList)
            {
                if (uiElement == viewToNavTo)
                    _currentView = uiElement;
            }
            SetBackForwardAvailability();
        }

        public void NavigateToNewView(UIElement currentView)
        {
            List<UIElement> tempList = new List<UIElement>();
            bool found = false;
            foreach (UIElement uiElement in _viewList)
            {
                if (!found)
                    tempList.Add(uiElement);
                
                if (uiElement == _currentView)
                {
                    found = true;
                    _currentView = currentView;
                    tempList.Add(currentView);
                    //break;
                }
            }

            if (!found)
            {
                tempList.Add(currentView);
                _currentView = currentView;
            }

            _viewList = tempList;
            SetBackForwardAvailability();
        }

        private void SetBackForwardAvailability()
        {
            CanNavigateBack = false;
            CanNavigateForward = false;

            if (_viewList.Count > 1 && _viewList[0] != _currentView)
                CanNavigateBack = true;
            if (_viewList.Count > 1 && _viewList[_viewList.Count - 1] != _currentView)
                CanNavigateForward = true;
        }
    }
}