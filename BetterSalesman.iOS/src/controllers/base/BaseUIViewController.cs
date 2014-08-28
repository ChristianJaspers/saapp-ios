using System;
using MonoTouch.UIKit;
using MBProgressHUD;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
    public class BaseUIViewController : UIViewController
    {
        const string ic_back = "ic_back";
        
        public BaseUIViewController(IntPtr handle) : base(handle)
        {
        }
        
        protected void PresentViewControllerWithStoryboardId(string storyboardId, bool animate = true)
        {
            UIViewController vc = (UIViewController)Storyboard.InstantiateViewController(storyboardId);

            PresentViewController(vc, animate, null);   
        }
        
        #region Progress HUD
        
        private MTMBProgressHUD myHud;

        protected void ShowHud(string titleLabel = "", MBProgressHUDMode mode = MBProgressHUDMode.Indeterminate)
        {
            InvokeOnMainThread(() =>
                {
                    myHud = MTMBProgressHUD.ShowHUD(UIApplication.SharedApplication.KeyWindow.RootViewController.View, true);
                    myHud.LabelText = titleLabel;
                    myHud.Mode = mode;
                });
        }

        protected void SetHudTitleLabel(string titleLabel)
        {
            myHud.LabelText = titleLabel;
        }

        protected void SetHudDetailsLabel(string detailsLabel)
        {
            myHud.DetailsLabelText = detailsLabel;
        }

        protected void SetHudMode(MBProgressHUDMode mode)
        {
            myHud.Mode = mode;
        }

        protected void SetHudProgress(float progress)
        {
            myHud.Progress = progress;
        }

        protected void HideHud()
        {
            if (myHud == null)
            {
                return;
            }

            InvokeOnMainThread(() => myHud.Hide(true));
        }
        
        #endregion
        
        #region Alerts
        
        protected void ShowAlert(string msg)
        {
            ShowAlert(msg, string.Empty);
        }
        
        protected void ShowAlert(string msg, string title)
        {
			InvokeOnMainThread(() => new UIAlertView(title, msg, null, I18n.OK, null).Show());
        }
        
        AlertDismissDelegate alertDelegate;
        
        protected void ShowAlert(string msg, Action dismissCallback)
        {
            InvokeOnMainThread(() => {
                alertDelegate = new AlertDismissDelegate(dismissCallback);
                new UIAlertView(string.Empty, msg, alertDelegate, I18n.OK, null).Show();
            });
        }
        
        class AlertDismissDelegate : UIAlertViewDelegate
        {
            public Action DismissCallback;
            
            public AlertDismissDelegate(Action dismissCallback)
            {
                DismissCallback = dismissCallback;
            }
            
            public override void Dismissed(UIAlertView alertView, int buttonIndex)
            {
                if (DismissCallback != null)
                {
                    DismissCallback();
                }
            }
        }
        
        #endregion
        
        protected bool IsNetworkAvailable()
        {
            return Reachability.IsHostReachable(HttpConfig.Host);
        }
        
        #region Synchronization delegates
        
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            SynchronizationManager.Instance.StartedSynchronization += OnSynchronizationStart;
            SynchronizationManager.Instance.FinishedSynchronization += OnSynchronizationFinished;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            SynchronizationManager.Instance.StartedSynchronization -= OnSynchronizationStart;
            SynchronizationManager.Instance.FinishedSynchronization -= OnSynchronizationFinished;
        }
        
        protected virtual void OnSynchronizationStart()
        {
        }

        protected virtual void OnSynchronizationFinished()
        {
        }
        
        #endregion
        
        protected void LeftBarButtonAsArrowIconOnly()
        {
            NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(
                UIImage.FromBundle(ic_back),
                UIBarButtonItemStyle.Plain, 
                (o, s) => NavigationController.PopViewControllerAnimated(true))
                ,false);
        }  
    }
}

