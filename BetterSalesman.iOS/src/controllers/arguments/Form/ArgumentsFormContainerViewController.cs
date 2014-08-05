using System;
using BetterSalesman.Core.BusinessLayer;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
    public partial class ArgumentsFormContainerViewController : BaseProductCategoryPickerUIViewController
	{   
        private const string SegueIdArgumentsListEmbeded = "segueIdArgumentFormContainer";
        
        public Argument Argument = new Argument();
        
        ArgumentFormViewController formController;
        
		public ArgumentsFormContainerViewController (IntPtr handle) : base (handle)
		{
		}
        
        #region Lifecycle
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            var saveButton = new UIBarButtonItem(I18n.Save, UIBarButtonItemStyle.Plain, SaveArgument);

            NavigationItem.SetRightBarButtonItem(saveButton, false);
        }
        
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            
            // TODO update product gruoup in picker (in case of edit)
            formController.Feature = Argument.Feature;
            formController.Benefit = Argument.Benefit;
        }
        
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier.Equals(SegueIdArgumentsListEmbeded))
            {
                formController = segue.DestinationViewController as ArgumentFormViewController;
            }
        }
        
        #endregion
        
        void SaveArgument(object o, EventArgs e)
        {
            View.EndEditing(true);

            if (formController.Validator.Validate())
            {
                if (!IsNetworkAvailable())
                {
                    ShowAlert(ServiceAccessError.ErrorHostUnreachable.LocalizedMessage);
                    return;
                }
                
                Argument.ProductGroupId = ProductGroupsDataSource.SelectedProductGroup.Id;
                Argument.Feature = formController.Feature;
                Argument.Benefit = formController.Benefit;

                ShowHud(I18n.Sending);
                
                ServiceProviderArgument.Instance.CreateOrUpdate(
                    Argument,
                    result =>
                    {       
                        HideHud();
                        DismissViewController(true,null);
                    },
                    errorMessage =>
                    {
                        HideHud();
                        ShowAlert(errorMessage);
                    }
                );
            }
            else
            {
                ShowAlert(string.Join("\n", formController.Validator.Errors));
            }
        }
	}
}
