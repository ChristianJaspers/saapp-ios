using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
	public delegate void MenuWasShown();

    public class CustomFlyoutNavigationController : FlyoutNavigationController
    {
		public event MenuWasShown MenuWasShown;

        public CustomFlyoutNavigationController(UITableViewStyle navigationStyle = UITableViewStyle.Plain) : base(navigationStyle)
        {
        }

		public override void ShowMenu()
		{
			base.ShowMenu();

			OnMenuWasShown();
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