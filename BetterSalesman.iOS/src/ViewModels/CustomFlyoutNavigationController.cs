using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
	public delegate void MenuWasShown();

    public class CustomFlyoutNavigationController : FlyoutNavigationController
    {
		public event MenuWasShown MenuWasShown;
        
        UIView OverlayView;

        public CustomFlyoutNavigationController(UITableViewStyle navigationStyle = UITableViewStyle.Plain) : base(navigationStyle)
        {
            OverlayView = new UIView(UIScreen.MainScreen.Bounds);
            OverlayView.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0.5f);
            OverlayView.Alpha = 0;
        }
        

		public override void ShowMenu()
		{
			base.ShowMenu();
            
            mainView.AddSubview(OverlayView);
            
            UIView.Animate(0.5f, () => { OverlayView.Alpha = 1; }, null);

			OnMenuWasShown();
		}
        
        public override void HideMenu()
        {
            mainView.BringSubviewToFront(OverlayView);
            
            base.HideMenu();
            
            UIView.Animate(0.5f, () => { OverlayView.Alpha = 0; }, OverlayView.RemoveFromSuperview);
        }

		private void OnMenuWasShown()
		{
			if (MenuWasShown != null)
			{
				MenuWasShown();
			}
		}
    }
}