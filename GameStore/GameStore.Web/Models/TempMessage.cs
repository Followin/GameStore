using System;

namespace GameStore.Web.Models
{
    public class TempMessage
    {
        public TempMessageType Type { get; set; }
        public string Message { get; set; }

        public TempMessage(TempMessageType type, string message)
        {
            Type = type;
            Message = message;
        }
    }

    public class LinkTempMessage : TempMessage
    {
        public string LinkText { get; set; }
        public string LinkHref { get; set; }

        public LinkTempMessage(TempMessageType type, string message, string linkText, string linkHref)
            : base(type, message)
        {
            LinkText = linkText;
            LinkHref = linkHref;
        }
    }

    public enum TempMessageType
    {
        Info,
        Error,
        Success
    }
}