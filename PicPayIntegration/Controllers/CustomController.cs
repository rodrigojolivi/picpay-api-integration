using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net;

namespace PicPayIntegration.Controllers
{
    public class CustomController : ControllerBase
    {
        protected new IActionResult Response(IRestResponse restResponse)
        {
            return restResponse.StatusCode switch
            {
                HttpStatusCode.OK => Ok(restResponse.Content),
                HttpStatusCode.Unauthorized => Unauthorized(restResponse.Content),
                HttpStatusCode.UnprocessableEntity => UnprocessableEntity(restResponse.Content),
                _ => BadRequest(),
            };
        }
    }
}