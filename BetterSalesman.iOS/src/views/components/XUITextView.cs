using XValidator;
using MonoTouch.UIKit;
using System;

namespace BetterSalesman.iOS
{
    public class XUITextViewField : XField<UITextView>
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

