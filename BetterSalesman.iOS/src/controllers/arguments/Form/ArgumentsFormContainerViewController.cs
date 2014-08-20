using System;
using BetterSalesman.Core.BusinessLayer;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using BetterSalesman.Core.ServiceAccessLayer;
using BetterSalesman.Core.BusinessLayer.Managers;

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
            
            Title = Argument.Id > 0 ? I18n.ArgumentEdit : I18n.ArgumentAdd;
            
            var saveButton = new UIBarButtonItem(I18n.Save, UIBarButtonItemStyle.Plain, SaveArgument);

            NavigationItem.SetRightBarButtonItem(saveButton, false);
            
            if (Argument.ProductGroupId > 0)
            {
                var productGroup = ProductGroupManager.GetProductGroup(Argument.ProductGroupId);
            
                ProductGroupsDataSource.SelectedProductGroup = productGroup;
            }
            
            LeftBarButtonAsArrowIconOnly();
        }
        
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier.Equals(SegueIdArgumentsListEmbeded))
            {
                formController = segue.DestinationViewController as ArgumentFormViewController;
                
                formController.Argument = Argument;
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
                
                Argument = formController.Argument;
                Argument.ProductGroupId = ProductGroupsDataSource.SelectedProductGroup.Id;

                ShowHud(I18n.Sending);
                
                ServiceProviderArgument.Instance.CreateOrUpdate(
                    Argument,
                    result =>
                    {       
                        HideHud();
						InvokeOnMainThread(() => NavigationController.PopViewControllerAnimated(true));
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
