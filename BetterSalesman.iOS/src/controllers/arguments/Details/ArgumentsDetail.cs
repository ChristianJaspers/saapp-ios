using System;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.ServiceAccessLayer;
using BetterSalesman.Core.BusinessLayer.Managers;
using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
    public partial class ArgumentsDetail : BaseUIViewController
	{
        const string segueIdArgumentEdit = "segueIdArgumentEdit";
        
        public Argument Argument;
        
		public ArgumentsDetail (IntPtr handle) : base (handle)
		{
		}
        
        #region Lifecycle
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            UpdateView();
            
            LeftBarButtonAsArrowIconOnly();

            if (Argument.UserId == UserManager.LoggedInUser().Id)
            {
                var buttonPasswordChange = new UIBarButtonItem(I18n.Edit, UIBarButtonItemStyle.Plain, delegate
                {
                    PerformSegue(segueIdArgumentEdit, this);
                });

                NavigationItem.SetRightBarButtonItem(buttonPasswordChange, true);
            }
            
            if ( Argument.MyRating > 0 && Argument.UserId == UserManager.LoggedInUser().Id )
            {
                chooseRating.Enabled = false;
            } 
            else
            {
                chooseRating.ValueChanged += (sender, e) =>
                {
                    if (!IsNetworkAvailable())
                    {
                        ShowAlert(ServiceAccessError.ErrorHostUnreachable.LocalizedMessage);
                        return;
                    }
                        
                    ShowHud(I18n.Sending);
                
                    ServiceProviderArgument.Instance.Rate(
                        Argument,
                        chooseRating.SelectedSegment + 1,
                        updatedArgument =>
                        {
                            Argument = updatedArgument;
                            UpdateView();
                            HideHud();
                        },
                        errorMessage =>
                        {
                            chooseRating.SelectedSegment = -1;
                            HideHud();
                            ShowAlert(errorMessage);
                        }
                    );
                };
            }
        }
        
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			// TODO - there's update view in ViewDidLoad - consider adding flag like isFirstViewDidAppear (set it to false in ViewDidLoad)
			// 		   that is set in the first ViewDidAppear (but without update), so that update doesn't get called twice, potentially blocking the view
			UpdateView();
		}

        public override void PrepareForSegue(UIStoryboardSegue segue, MonoTouch.Foundation.NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            
            if (segue.Identifier.Equals(segueIdArgumentEdit))
            {
                var vc = (ArgumentsFormContainerViewController)segue.DestinationViewController;
                
                vc.Argument = Argument;
            }
        }
        
        void UpdateView()
        {
			labelEarnXPForVote.Text = Argument.Rated ? I18n.ArgumentThanksForVoting : I18n.ArgumentEarnXpByVoting;
			labelHowRelevant.Text = I18n.ArgumentRelevanceTitle;

            labelFeature.Text = Argument.Feature;
            labelBenefit.Text = Argument.Benefit;

			labelEarnXPForVote.Text = Argument.Rated ? I18n.ArgumentThanksForVoting : I18n.ArgumentEarnXpByVoting;

            if (Argument.Rated || Argument.UserId == UserManager.LoggedInUser().Id)
            {
                if (Argument.UserId != UserManager.LoggedInUser().Id)
                {
                    chooseRating.SelectedSegment = Argument.MyRating - 1;
					SetRatingControlsHidden(false);
                }
				else
				{
					SetRatingControlsHidden(true);
				}
                
                chooseRating.Enabled = false;
            }
        }

		private void SetRatingControlsHidden(bool hidden)
		{
			labelHowRelevant.Hidden = hidden;
			labelEarnXPForVote.Hidden = hidden;
			chooseRating.Hidden = hidden;
		}
        
        #endregion
	}
}
