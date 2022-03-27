using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
   
   //(L221)
       [Authorize] 
   //(L221)
    public class PresenceHub :Hub
    {
 private readonly PresenceTracker _tracker;

        //(L223)
         public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

       /*  public PresenceTracker Tracker => _tracker; */
 
        //(L223)


        public override async Task OnConnectedAsync()
        {
             //(L223)
           /*  await _tracker.UserConnected(Context.User.GetUsername(),Context.ConnectionId); (L234) */
          
            //(L234)
                var isOnline = await _tracker.UserConnected(Context.User.GetUsername(),Context.ConnectionId);

                if(isOnline)
                {
                await Clients.Others.SendAsync("UserIsOnLine", Context.User.GetUsername());

                }

           //(L234)
            //(L213)
            /* await Clients.Others.SendAsync("UserIsOnLine", Context.User.GetUsername()); (L234) */
           
            //(L223)
            var currentUsers= await _tracker.GetOnlineUsers();
            /* await Clients.All.SendAsync("GetOnlineUsers",currentUsers);  (L234)*/

            //(L234)
                await Clients.Caller.SendAsync("GetOnlineUsers",currentUsers);

            //(L234)
            //(L223)
        
        }
   
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        //(L223)
        /* await _tracker.UserDisConnected(Context.User.GetUsername(),Context.ConnectionId); (L234) */
       
       //(L234)
             var isOffLine = await _tracker.UserDisConnected(Context.User.GetUsername(),Context.ConnectionId);
                if(isOffLine)
                {
                await Clients.Others.SendAsync("UserIsOffLine", Context.User.GetUsername());
               

                }
                 await base.OnDisconnectedAsync(exception);
       //(L234)
       //(L223)
            //(L223)
           /*  var currentUsers= await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers",currentUsers);  (L234)*/
            //(L223)

       /*  await Clients.Others.SendAsync("UserIsOffLine", Context.User.GetUsername());
        await base.OnDisconnectedAsync(exception); (L234) */
    }
   
   
    }



    }

