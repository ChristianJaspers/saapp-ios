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
            
            updateView();
            
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
                            updateView();
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
        
        void updateView()
        {
            labelFeature.Text = Argument.Feature;
            labelBenefit.Text = Argument.Benefit;
            
            if (Argument.Rated || Argument.UserId == UserManager.LoggedInUser().Id)
            {
                if (Argument.UserId != UserManager.LoggedInUser().Id)
                {
                    chooseRating.SelectedSegment = Argument.MyRating - 1;
                } 
                
                chooseRating.Enabled = false;
            }
        }
        
        #endregion
	}
}
