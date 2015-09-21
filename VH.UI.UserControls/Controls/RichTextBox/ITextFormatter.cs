using System.Windows.Documents;

namespace VH.UI.UserControls.Controls
{
    public interface ITextFormatter
    {
        string GetText(FlowDocument document);
        void SetText(FlowDocument document, string text);
    }
}
