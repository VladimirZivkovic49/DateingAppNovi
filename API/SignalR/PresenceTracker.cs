using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string,List<string>>OnlineUsers=
        new Dictionary<string, List<string>>();
    
       /*  public  Task UserConnected(string username, string connectionId) (L234) */
        public Task<bool> UserConnected(string username, string connectionId)
        
        {
           //(L234)
            bool isOnline=false;
           //(L234)
           
           
            lock(OnlineUsers)
            {
                if(OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers[username].Add(connectionId);
                   
                }
                else
                {
                    OnlineUsers.Add(username,new List<string>{connectionId});
                    isOnline=true;
                }
            }
               /*  return Task.CompletedTask; */

               //(L234)
                    return Task.FromResult(isOnline);

               //(L234)
        }
               /* public  Task UserDisConnected(string username, string connectionId)(L234) */
        public  Task <bool> UserDisConnected(string username, string connectionId)
        
        {
             //(L234)
            bool isOffline=false;
           //(L234)
            
            lock(OnlineUsers)
            {
                if(!OnlineUsers.ContainsKey(username))
                {
                    /* return Task.CompletedTask; (L234)*/
                    return Task.FromResult(isOffline);
                }
               OnlineUsers[username].Remove(connectionId);
                if(OnlineUsers[username].Count==0)
                {
                    OnlineUsers.Remove(username);
                    //(L234)
                        isOffline=true;
                    //(L234)
                }
           
            }
                /* return Task.CompletedTask; (L234) */
                     return Task.FromResult(isOffline);
        
        } 
                public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;

            lock (OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();

                

            }
                return Task.FromResult(onlineUsers);
       }
        //(L233)

    public Task <List<string>> GetConnectionsForUser(string username)
    {
            List<string> connectionIds;
            lock(OnlineUsers)
            {
                connectionIds=OnlineUsers.GetValueOrDefault(username);


            }
                return Task.FromResult(connectionIds);

    }



        //(L233)
   
   
    }
    
    }