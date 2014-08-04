using System;

using BetterSalesman.Core.BusinessLayer;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

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
            
            // init validator here?
            
            // load argument if any (edit mode)
            
            var saveButton = new UIBarButtonItem(UIImage.FromBundle(ic_save_button), UIBarButtonItemStyle.Plain, delegate
            {
                // TODO save here
            });

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
	}
}
