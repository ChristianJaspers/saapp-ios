using System;
using System.Diagnostics;
using BetterSalesman.Core.BusinessLayer;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
    public partial class ArgumentsFormContainerViewController : BaseProductCategoryPickerUIViewController
	{   
        private const string SegueIdArgumentsListEmbeded = "segueIdArgumentFormContainer";
        
        const string ic_save_button = "ic_menu"; // TODO update save icon
        
        public Argument Argument;
        
        ArgumentFormViewController formController;
        
		public ArgumentsFormContainerViewController (IntPtr handle) : base (handle)
		{
		}
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            // TODO load argument if any (edit mode)
            
            var saveButton = new UIBarButtonItem(UIImage.FromBundle(ic_save_button), UIBarButtonItemStyle.Plain, SaveArgument);

            NavigationItem.SetRightBarButtonItem(saveButton, false);
        }
        
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier.Equals(SegueIdArgumentsListEmbeded))
            {
                formController = segue.DestinationViewController as ArgumentFormViewController;
            }
        }
        
        void SaveArgument(object o, EventArgs e)
        {
            // TODO save here
            Debug.WriteLine("Save/Edit argument");

            View.EndEditing(true);

            if (formController.Validator.Validate())
            {
                if (!IsNetworkAvailable())
                {
                    ShowAlert(ServiceAccessError.ErrorHostUnreachable.LocalizedMessage);
                    return;
                }

                // TODO request with HUD

//                    ShowHud(I18n.AuthenticationInProgress);
//                    ServiceProviderUser.Instance.Authentication(
//                        inputEmail.Text, 
//                        inputPassword.Text, 
//                        result =>
//                        {       
//                            HideHud();
//                            Login();
//                        },
//                        errorMessage =>
//                        {
//                            HideHud();
//                            ShowAlert(errorMessage);
//                        }
//                    );
            }
            else
            {
                ShowAlert(string.Join("\n", formController.Validator.Errors));
            }
        }
	}
}
