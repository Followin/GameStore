using System;
using System.IO;
namespace GameStore.BLL.Services
{
    public class PdfService
    {
        public static Stream GenerateInvoiceFile()
        {
            return new MemoryStream(new byte[0]);
        }
    }
}
