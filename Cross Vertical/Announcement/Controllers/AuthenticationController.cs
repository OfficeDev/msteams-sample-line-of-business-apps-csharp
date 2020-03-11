using CrossVertical.Announcement.Helper;
using CrossVertical.Announcement.Helpers;
using CrossVertical.Announcement.Models;
using CrossVertical.Announcement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CrossVertical.Announcement.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        [Route("adminconsent")]
        public async Task<ActionResult> ConsentPage(string tenant, string admin_consent, string state)
        {
            if (string.IsNullOrEmpty(tenant))
            {
                return HttpNotFound();
            }

            var tenantDetails = await Cache.Tenants.GetItemAsync(tenant);
            tenantDetails.IsAdminConsented = true;
            await Cache.Tenants.AddOrUpdateItemAsync(tenantDetails.Id, tenantDetails);
            return View();
        }

        // GET: Authentication
        [Route("test")]
        public async Task<ActionResult> Test()
        {
            var token = await GraphHelper.GetAccessToken("0d9b645f-597b-41f0-a2a3-ef103fbd91bb", ApplicationSettings.AppId, ApplicationSettings.AppSecret);
            GraphHelper helper = new GraphHelper(token);
            var photo = await helper.GetUserProfilePhoto("0d9b645f-597b-41f0-a2a3-ef103fbd91bb", "mungo@blrdev.onmicrosoft.com");
            return View();
        }
    }
}