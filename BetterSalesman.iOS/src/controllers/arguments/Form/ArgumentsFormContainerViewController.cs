using System;

using BetterSalesman.Core.BusinessLayer;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace BetterSalesman.iOS
{
    public partial class ArgumentsFormContainerViewController : BaseProductCategoryPickerUIViewController
	{   
        private const string SegueIdArgumentsListEmbeded = "segueIdArgumentFormContainer";
        
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
