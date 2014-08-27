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
        
        const string extraWhiteSpace = "   ";
        
        const string FontHelveticaBold = "HelveticaNeue-Bold";
        const string FontHelveticaLight = "HelveticaNeue-Light";
        
        int selectedRating = -1;
        
		public ArgumentsDetail (IntPtr handle) : base (handle)
		{
		}
        
        #region Lifecycle
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            labelDescriptionFeature.Text = I18n.FieldFeature;
            labelDescriptionBenefit.Text = I18n.FieldBenefit;
            labelHowRelevant.Text = I18n.ArgumentRelevanceTitle;
            
            labelFeature.Editable = true;
            labelFeature.Font = UIFont.FromName(FontHelveticaBold, 19);
            labelFeature.Editable = false;

            labelBenefit.Editable = true;
            labelBenefit.Font = UIFont.FromName(FontHelveticaLight, 15);
            labelBenefit.Editable = false;
            
            ratedButtonLow.SetTitle(I18n.VoteLow, UIControlState.Normal);
            ratedButtonMedium.SetTitle(I18n.VoteMedium, UIControlState.Normal);
            ratedButtonHigh.SetTitle(I18n.VoteHigh, UIControlState.Normal);
            
            Title = ProductGroupManager.GetProductGroup(Argument.ProductGroupId).Name;
            
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
                DisableRating();
            } 
            else
            {
                RestartRatingViews();
            }
        }
        
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            // TODO - there's update view in ViewDidLoad - consider adding flag like isFirstViewDidAppear (set it to false in ViewDidLoad)
            //         that is set in the first ViewDidAppear (but without update), so that update doesn't get called twice, potentially blocking the view
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
        
        #endregion

        void RateElementRequest()
        {
            if (!IsNetworkAvailable())
            {
                ShowAlert(ServiceAccessError.ErrorHostUnreachable.LocalizedMessage);
                RestartRatingViews();
                return;
            }
            
            ShowHud(I18n.Sending);
            
            ServiceProviderArgument.Instance.Rate(
                Argument, 
                selectedRating, 
                updatedArgument => 
                {
                    ColorSelectedRating();
                    Argument = updatedArgument;
                    UpdateView();
                    HideHud();
                }, errorMessage => 
                {
                    RestartRatingViews();
                    HideHud();
                    ShowAlert(errorMessage);
                }
            );
        }
        
        void UpdateView()
        {
			labelEarnXPForVote.Text = Argument.Rated ? I18n.ArgumentThanksForVoting : I18n.ArgumentEarnXpByVoting;

            labelFeature.Text = Argument.Feature;
            labelBenefit.Text = Argument.Benefit;

			labelEarnXPForVote.Text = Argument.Rated ? I18n.ArgumentThanksForVoting : I18n.ArgumentEarnXpByVoting;
                    
            ColorSelectedRating();
            
            // extra space for styling
            labelEarnXPForVote.Text = extraWhiteSpace + extraWhiteSpace + labelEarnXPForVote.Text + extraWhiteSpace;

            if (Argument.Rated || Argument.UserId == UserManager.LoggedInUser().Id)
            {
                if (Argument.UserId != UserManager.LoggedInUser().Id)
                {
                    selectedRating = Argument.MyRating;
					SetRatingControlsHidden(false);
                }
				else
				{
					SetRatingControlsHidden(true);
				}
                
                DisableRating();
            }
        }
        
        #region Rating

		private void SetRatingControlsHidden(bool hidden) 
		{
            rateContainer.Hidden = hidden;
		}

        void ColorSelectedRating()
        {
            if (selectedRating != -1)
            {
                switch (selectedRating)
                {
                    case 1:
                    
                        break;
                    case 2:
                    
                        break;
                    case 3:
                        
                        break;
                }
            }
        }

        const string ratingbuttonborderleft = "rating_button_border_left";
        const string ratingbuttonborderright = "rating_button_border_right";
        const string ratingbuttonbordermiddle = "rating_button_border_middle";

        void RestartRatingViews()
        {
            DisableRating();
           
            // borders
            var bgImageBorderLeft = UIImage.FromBundle(ratingbuttonborderleft).CreateResizableImage(new UIEdgeInsets(4, 4, 4, 1));
            var bgImageBorderRight = UIImage.FromBundle(ratingbuttonborderright).CreateResizableImage(new UIEdgeInsets(4, 1, 4, 4));
            var bgImageBorderCenter = UIImage.FromBundle(ratingbuttonbordermiddle).CreateResizableImage(new UIEdgeInsets(1, 1, 1, 1));
            
            ratedButtonLow.SetBackgroundImage(bgImageBorderLeft, UIControlState.Normal);
            ratedButtonMedium.SetBackgroundImage(bgImageBorderCenter, UIControlState.Normal);
            ratedButtonHigh.SetBackgroundImage(bgImageBorderRight, UIControlState.Normal);
            
            ratedButtonLow.SetTitleColor(AppDelegate.RatingLowColor, UIControlState.Normal);
            ratedButtonMedium.SetTitleColor(AppDelegate.RatingMediumColor, UIControlState.Normal);
            ratedButtonHigh.SetTitleColor(AppDelegate.RatingHighColor,UIControlState.Normal);
            
            selectedRating = -1;
            
            ratedButtonLow.TouchUpInside += RateButtonTouched;
            ratedButtonMedium.TouchUpInside += RateButtonTouched;
            ratedButtonHigh.TouchUpInside += RateButtonTouched;
        }
        
        void DisableRating()
        {
            ratedButtonLow.TouchUpInside -= RateButtonTouched;
            ratedButtonMedium.TouchUpInside -= RateButtonTouched;
            ratedButtonHigh.TouchUpInside -= RateButtonTouched;
        }
        
        void RateButtonTouched(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Button touched!");
            
            if (sender.Equals(ratedButtonLow))
            {
                ratedButtonLow.SetTitleColor(UIColor.White, UIControlState.Normal);
                selectedRating = 1;
            }
            
            if (sender.Equals(ratedButtonMedium))
            {
                ratedButtonMedium.SetTitleColor(UIColor.White, UIControlState.Normal);
                selectedRating = 2;
            }
            
            if (sender.Equals(ratedButtonHigh))
            {
                ratedButtonHigh.SetTitleColor(UIColor.White, UIControlState.Normal);
                selectedRating = 3;
            }
            
            RateElementRequest();
        }
        
        #endregion
	}
}
