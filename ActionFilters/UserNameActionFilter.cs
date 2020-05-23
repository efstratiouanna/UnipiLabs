using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnipiLabs.DataAccess;

namespace UnipiLabs.ActionFilters
{
    public class UserNameActionFilter: ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));
                var user = userManager.FindById(HttpContext.Current.User.Identity.GetUserId<int>());
                filterContext.Controller.ViewBag.UserName = user.UserName;

            } else
            {
                filterContext.Controller.ViewBag.UserName = "Unauthorized";
            }
        }
    }
}