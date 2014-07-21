using System;
using MonoTouch.UIKit;
using MBProgressHUD;

namespace BetterSalesman.iOS
{
    public class BaseUIViewController : UIViewController
    {
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
                    myHud = MTMBProgressHUD.ShowHUD(View, true);
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
        
        protected void ShowAlert(string msg)
        {
            ShowAlert(msg, string.Empty);
        }
        
        protected void ShowAlert(string msg, string title)
        {
            new UIAlertView (title, msg, null, I18n.OK, null).Show ();
        }
    }
}

