using System.Text;
using System.Windows.Documents;
using System.IO;
using System.Windows;

namespace VH.UI.UserControls.Controls
{
    /// <summary>
    /// Formats the RichTextBox text as RTF
    /// </summary>
    public class RtfFormatter : ITextFormatter
    {
        public string GetText(FlowDocument document)
        {
            TextRange tr = new TextRange(document.ContentStart, document.ContentEnd);
            if (document.Blocks.Count == 0)
                return string.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                tr.Save(ms, DataFormats.Rtf);
                return ASCIIEncoding.Default.GetString(ms.ToArray());;
            }
        }

        public void SetText(FlowDocument document, string text)
        {
            if (string.IsNullOrEmpty(text))
                text = @"{\rtf1\ansi}";

                TextRange tr = new TextRange(document.ContentStart, document.ContentEnd);


                using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(text)))
                {
                    tr.Load(ms, DataFormats.Rtf);
                }
                
            
        }
    }
}
