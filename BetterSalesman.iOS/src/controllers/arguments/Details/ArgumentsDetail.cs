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
                DisableRatingButtons();
            } 
            else
            {
                RestartRatingButtons();
            }
        }
        
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            // TODO - there's update view in ViewDidLoad - consider adding flag like isFirstViewDidAppear (set it to false in ViewDidLoad)
            //         that is set in the first ViewDidAppear (but without update), so that update doesn't get called twice, potentially blocking the view
            UpdateView();
        }
        
        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            
            DisableRatingButtons();
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

        void PerformRateRequest()
        {
            DisableRatingButtons();
            
            if (!IsNetworkAvailable())
            {
                ShowAlert(ServiceAccessError.ErrorHostUnreachable.LocalizedMessage);
                RestartRatingButtons();
                return;
            }
            
            ShowHud(I18n.Sending);
            
            ServiceProviderArgument.Instance.Rate(
                Argument, 
                selectedRating, 
                updatedArgument => 
                {
                    Argument = updatedArgument;
                    UpdateView();
                    HideHud();
                }, errorMessage => 
                {
                    RestartRatingButtons();
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
            
            // extra space for styling
            labelEarnXPForVote.Text = extraWhiteSpace + extraWhiteSpace + labelEarnXPForVote.Text + extraWhiteSpace;

            if (Argument.Rated || Argument.UserId == UserManager.LoggedInUser().Id)
            {
                selectedRating = Argument.MyRating;
                if (Argument.UserId != UserManager.LoggedInUser().Id)
                {
					SetRatingControlsHidden(false);
                }
				else
				{
					SetRatingControlsHidden(true);
                }
                
                DisableRatingButtons();
            }
            
            ColorRatingButtons();
        }
        
        #region Rating

		private void SetRatingControlsHidden(bool hidden) 
		{
            rateContainer.Hidden = hidden;
		}

        // not selected
        const string ratingbuttonborderleft = "rating_button_border_left";
        const string ratingbuttonborderright = "rating_button_border_right";
        const string ratingbuttonbordermiddle = "rating_button_border_middle";
        
        // selected
        const string ratingbuttonred = "rating_button_red";
        const string ratingbuttongreen = "rating_button_green";
        const string ratingbuttonorange = "rating_button_orange";
        
        UIEdgeInsets insetLeft = new UIEdgeInsets(4,4,4,1);
        UIEdgeInsets insetMiddle = new UIEdgeInsets(4,1,4,4);
        UIEdgeInsets insetRight = new UIEdgeInsets(1,1,1,1);

        void ColorRatingButtons()
        {
            var bgImageBorderLeft = UIImage.FromBundle(ratingbuttonborderleft).CreateResizableImage(insetLeft);
            var bgImageBorderRight = UIImage.FromBundle(ratingbuttonborderright).CreateResizableImage(insetMiddle);
            var bgImageBorderCenter = UIImage.FromBundle(ratingbuttonbordermiddle).CreateResizableImage(insetRight);
            
            ratedButtonLow.SetBackgroundImage(bgImageBorderLeft);
            ratedButtonMedium.SetBackgroundImage(bgImageBorderCenter);
            ratedButtonHigh.SetBackgroundImage(bgImageBorderRight);
            
            ratedButtonLow.SetTitleColor(AppDelegate.RatingLowColor);
            ratedButtonMedium.SetTitleColor(AppDelegate.RatingMediumColor);
            ratedButtonHigh.SetTitleColor(AppDelegate.RatingHighColor);
            
            if (selectedRating != -1)
            {
                var bgImageBorderLeftSelected = UIImage.FromBundle(ratingbuttonred).CreateResizableImage(insetLeft);
                var bgImageBorderRightSelected = UIImage.FromBundle(ratingbuttongreen).CreateResizableImage(insetMiddle);
                var bgImageBorderCenterSelected = UIImage.FromBundle(ratingbuttonorange).CreateResizableImage(insetRight);
                
                switch (selectedRating)
                {
                    case 1:
                        ratedButtonLow.SetBackgroundImage(bgImageBorderLeftSelected);
                        ratedButtonLow.SetTitleColor(UIColor.White);
                    break;
                        
                    case 2:
                        ratedButtonMedium.SetBackgroundImage(bgImageBorderCenterSelected);
                        ratedButtonMedium.SetTitleColor(UIColor.White);
                    break;
                        
                    case 3:
                        ratedButtonHigh.SetBackgroundImage(bgImageBorderRightSelected);
                        ratedButtonHigh.SetTitleColor(UIColor.White);
                    break;
                }
            }
        }

        void RestartRatingButtons()
        {
            DisableRatingButtons();
            
            selectedRating = -1;
            
            ratedButtonLow.TouchUpInside += OnRateButtonTouched;
            ratedButtonMedium.TouchUpInside += OnRateButtonTouched;
            ratedButtonHigh.TouchUpInside += OnRateButtonTouched;
        }
        
        void DisableRatingButtons()
        {
            ratedButtonLow.TouchUpInside -= OnRateButtonTouched;
            ratedButtonMedium.TouchUpInside -= OnRateButtonTouched;
            ratedButtonHigh.TouchUpInside -= OnRateButtonTouched;
        }
        
        void OnRateButtonTouched(object sender, EventArgs e)
        {   
            if (sender.Equals(ratedButtonLow)) selectedRating = 1;
            
            if (sender.Equals(ratedButtonMedium)) selectedRating = 2;
            
            if (sender.Equals(ratedButtonHigh)) selectedRating = 3;
            
            PerformRateRequest();
        }
        
        #endregion
    }
    
    static class UIButtonExtension
    {
        public static void SetTitleColor(this UIButton button, UIColor titleColor)
        {
            button.SetTitleColor(titleColor, UIControlState.Normal);
            button.SetTitleColor(titleColor, UIControlState.Highlighted);
        }
        
        public static void SetBackgroundImage(this UIButton button, UIImage image)
        {
            button.SetBackgroundImage(image, UIControlState.Normal);
            button.SetBackgroundImage(image, UIControlState.Highlighted);
        }
    }
}
