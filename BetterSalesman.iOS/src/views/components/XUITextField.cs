using XValidator;
using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
    public class XUITextField : XField<UITextField>
    {
        public override string Value()
        {
            return FieldView.Text;
        }
        
        public override void ViewStateError()
        {
        }
        
        public override void ViewStateNormal()
        {
        }
    }
}

