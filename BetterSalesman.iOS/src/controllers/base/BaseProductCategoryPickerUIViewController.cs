using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace BetterSalesman.iOS
{
    public class BaseProductCategoryPickerUIViewController : BaseUIViewController
    {
        public ProductGroupsDataSource ProductGroupsDataSource { get; private set; }
        
        private const string SegueIdProductGroupPickerEmbeded = "segueIdProductGroupPickerSubview";
        
        public BaseProductCategoryPickerUIViewController(IntPtr handle) : base (handle)
        {
            ProductGroupsDataSource = new ProductGroupsDataSource();
        }
        
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (ProductGroupsDataSource != null)
            {
                ProductGroupsDataSource.ReloadProductGroups();
            }
        }
        
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier.Equals(SegueIdProductGroupPickerEmbeded))
            {
                var destinationController = segue.DestinationViewController as ProductGroupPickerButtonViewController;
                if (destinationController != null)
                {
                    destinationController.ProductGroupsDataSource = ProductGroupsDataSource;
                }
            }
        }
    }
}

