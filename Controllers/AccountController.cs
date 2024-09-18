using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SahiHisabAPI.Model;
using SahiHisabAPI.SahiHisabDA;

namespace SahiHisabAPI.Controllers
{
   
    [RoutePrefix("api/[controller]")]
    public class AccountController : ApiController
    {
        private readonly IAccountDA _accountDA;

        // Constructor with dependency injection
        public AccountController(IAccountDA accountDA)
        {
            _accountDA = accountDA ?? throw new ArgumentNullException(nameof(accountDA));
        }

        [AllowAnonymous]
        [Route("registerAccount")]
       
        [HttpPost]
        public async Task<IHttpActionResult> RegisterAccount(Register val)
        {
            if (val == null)
                return BadRequest("Please fill all the details.");

            try
            {
                int result = await Task.Run(() => _accountDA.RegisterAccount(val));

                if (result > 0)
                    return Ok(result);
                else
                    return BadRequest("Registration failed. Please try again.");
            }
            catch (Exception ex)
            {
                // Consider logging the exception here
                return InternalServerError(new Exception("An error occurred while processing your request.", ex));
            }
        }
    }
}



//using Microsoft.Identity.Client;
//using SahiHisabAPI.Model;
//using SahiHisabAPI.SahiHisabDA;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;



//namespace SahiHisabAPI.Controllers
//{
//    [Authorize]
//    [RoutePrefix("api/[controller]")]

//    public class AccountController : ApiController


//    {
//        public AccountController()
//        {

//        }

//        [AllowAnonymous]
//        [Route("registerAccount")]
//        [HttpPost]
//        public async Task<HttpResponseMessage> RegisterAccount(Register val)
//        {
//            try
//            {

//                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
//                if (val == null)
//                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please fill all the Details");
//                AccountDA oda = new AccountDA();
//                int msg = oda.RegisterAccount(val);
//                if (msg != 0)
//                    return Request.CreateResponse(HttpStatusCode.OK, msg);
//                else
//                    return Request.CreateResponse(HttpStatusCode.BadRequest, msg);
//            }
//            catch (Exception ex)
//            {
//                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message + " ------" + ex.StackTrace);
//            }

//        }
//    }
//}

