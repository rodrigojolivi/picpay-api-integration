using Microsoft.Extensions.Configuration;
using PicPayService.Model;
using RestSharp;
using System;
using System.Text.Json;

namespace PicPayService.Service
{
    public class PaymentService : IPaymentService
    {
        public string BaseUrl { get; private set; }
        public string Token { get; private set; }
        public RestRequest Request { get; private set; }

        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;

            GetCredentials();
            AddHeaders();           
        }

        public IRestResponse RequestPayment(Payment payment)
        {
            Request.Method = Method.POST;

            AddReferenceId(payment);

            var client = GetCompleteUrl("public/payments");

            var json = SerializeObject(payment);

            Request.AddJsonBody(json);

            var response = ExecuteRequest(client, Request);

            return response;
        }        

        public IRestResponse RequestCancel(string referenceId, string authorizationId)
        {
            Request.Method = Method.POST;

            var client = GetCompleteUrl($"public/payments/{referenceId}/cancellations");

            var json = SerializeObject(authorizationId);

            Request.AddJsonBody(json);

            var response = ExecuteRequest(client, Request);

            return response;
        }

        public IRestResponse RequestStatus(string referenceId)
        {
            Request.Method = Method.GET;

            var client = GetCompleteUrl($"public/payments/{referenceId}/status");

            var response = ExecuteRequest(client, Request);

            return response;
        }

        private void GetCredentials()
        {
            BaseUrl = _configuration.GetSection("PicPay:BaseUrl").Value;
            Token = _configuration.GetSection("PicPay:XPicPayToken").Value;
        }

        private void AddHeaders()
        {
            Request = new RestRequest();
            Request.AddHeader("Content-Type", "application/json");
            Request.AddHeader("x-picpay-token", Token);
        }

        private void AddReferenceId(Payment payment)
        {
            if (string.IsNullOrEmpty(payment.ReferenceId))
            {
                payment.ReferenceId = Guid.NewGuid().ToString();
            }
        }

        private RestClient GetCompleteUrl(string url)
        {
            return new RestClient($"{BaseUrl}{url}");
        }

        private string SerializeObject(object @object)
        {
            return JsonSerializer.Serialize(@object);
        }

        private IRestResponse ExecuteRequest(RestClient client, RestRequest request)
        {
            return client.Execute(request);
        }
    }
}
