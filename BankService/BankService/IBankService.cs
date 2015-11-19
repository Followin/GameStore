using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using BankService.Models;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBankService" in both code and config file together.
    [ServiceContract]
    public interface IBankService
    {
        [OperationContract]
        Task<PaymentResult> PayByVisaAsync(PaymentInfo info);

        [OperationContract]
        Task<PaymentResult> PayByMastercardAsync(PaymentInfo info);

        [OperationContract]
        Task<Boolean> ConfirmAsync(String code);
    }
}
