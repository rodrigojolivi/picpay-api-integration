using PicPayService.Model;
using RestSharp;

namespace PicPayService.Service
{
    public interface IPaymentService
    {
        IRestResponse RequestPayment(Payment payment);
        IRestResponse RequestCancel(string referenceId, string authorizationId);
        IRestResponse RequestStatus(string referenceId);
    }
}
