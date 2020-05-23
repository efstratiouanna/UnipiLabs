using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;


namespace UnipiLabs.Hubs
{
    public class NotificationHub : Hub
    {
        public void SendNotification(string notification)
        {
            Clients.All.addNewMessageToPage("ReceiveMessage", notification);
        }

    }
}