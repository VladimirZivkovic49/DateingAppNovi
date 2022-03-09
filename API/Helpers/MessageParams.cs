using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class MessageParams:PaginationParams
    {
        public string UserName { get; set; }
        public string Container { get; set; }="Unread";
        /* public int minAge { get; set; }=18;
        public int maxAge { get; set; }=150; */
       /*  public string OrderBy { get; set; }="lastActive"; */
    }
}