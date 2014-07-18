using XValidator;
using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
    public class XUITextFieldValidate : XFieldValidate<UITextField>
    {
        public override string Value()
        {
            return FieldView.Text;
        }
        
        public override void ViewStateError()
        {
            FieldView.Layer.BorderWidth = 1;
            FieldView.Layer.BorderColor = UIColor.Red.CGColor;
        }
        
        public override void ViewStateNormal()
        {
            FieldView.Layer.BorderWidth = 1;
            FieldView.Layer.BorderColor = UIColor.LightGray.CGColor;
        }
    }
}

