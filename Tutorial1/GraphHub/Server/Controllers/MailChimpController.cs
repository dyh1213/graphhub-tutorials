using System;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;

namespace GraphHub.Server.Controllers
{

    [ApiController]
    [Route("mailchimp")]
    public class MailChimpController : ControllerBase
    {
        private readonly MailchimpAPI mailchimp;

        public MailChimpController(MailchimpAPI mailchimp)
        {
            this.mailchimp = mailchimp;
        }


        [HttpGet("addnewemail/{email}")]
        public async Task<bool> AddNewEmail(string email)
        {
            var subscriber = new MailchimpSubscriber
            {
                Email = email,
                FirstName = "TBD",
            };

            var success = await mailchimp.AddSubscriber(subscriber);

            if (success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}



