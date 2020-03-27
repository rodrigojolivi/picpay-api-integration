using Microsoft.AspNetCore.Mvc;
using PicPayService.Model;
using PicPayService.Service;

namespace PicPayIntegration.Controllers
{
    [Route("api/picpay")]
    [ApiController]
    public class PicPayController : CustomController
    {
        private readonly IPaymentService _paymentService;

        public PicPayController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("payments")]
        public IActionResult RequestPayment([FromBody]Payment payment)
        {
            var result = _paymentService.RequestPayment(payment);
            return Response(result);
        }

        [HttpPost("{referenceId}/cancellations")]
        public IActionResult RequestCancel([FromRoute]string referenceId, [FromBody]string authorizationId)
        {
            var result = _paymentService.RequestCancel(referenceId, authorizationId);
            return Response(result);
        }

        [HttpGet("{referenceId}/status")]
        public IActionResult RequestStatus([FromRoute]string referenceId)
        {
            var result = _paymentService.RequestStatus(referenceId);
            return Response(result);
        }
    }
}