using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using UnipiLabs.Models;

namespace UnipiLabs.Hubs
{
    public class UserIDProvider : IUserIdProvider
    {

        public string GetUserId(IRequest request)
        {
            return request.User.Identity.Name; // το UserName του χρήστη
        }
    }
}