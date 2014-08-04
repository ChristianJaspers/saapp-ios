using System;

using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.iOS
{
    public partial class ArgumentsForm : BaseProductCategoryPickerUIViewController
	{   
        public Argument Argument;
        
		public ArgumentsForm (IntPtr handle) : base (handle)
		{
		}
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            // init validator here?
            
            // load argument if any (edit mode)
        }
	}
}
