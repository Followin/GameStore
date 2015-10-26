using System;
using SelectPdf;

namespace GameStore.BLL.Services
{
    public class PdfService
    {
        public static byte[] GenerateInvoiceFile(String userName)
        {
            PdfDocument doc = new PdfDocument();
            var page = doc.AddPage();
            var font = doc.AddFont(PdfStandardFont.Helvetica);
            var text = new PdfTextElement(50, 50, "Your session id: " + userName, font);
            page.Add(text);
            return doc.Save();
        }
    }
}
