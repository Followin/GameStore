using System;
using System.Runtime.Serialization;
using BankService.Entities;

namespace BankService.Models
{
    [DataContract]
    public class PaymentInfo
    {
        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String AccountNumber { get; set; }

        [DataMember]
        public String Cvv2 { get; set; }

        [DataMember]
        public Decimal Sum { get; set; }

        [DataMember]
        public String Owner { get; set; }

        [DataMember]
        public Int32 ExpirationMonth { get; set; }

        [DataMember]
        public Int32 ExpirationYear { get; set; }
    }
}