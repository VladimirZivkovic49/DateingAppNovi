using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class UserParams:PaginationParams
    {/* 
        private const int MaxPageSize=50;
        public int PageNumber{get;set;}=1;

        private int _pageSize=10;

        
        public int PageSize
        {
            get =>  _pageSize;
            set => _pageSize=(value>MaxPageSize)?MaxPageSize:value;
        } (L177)*/
        public string CurrentUserName { get; set; }
        public string Gender { get; set; }
        public int minAge { get; set; }=18;
        public int maxAge { get; set; }=150;
        public string OrderBy { get; set; }="lastActive";
    }
}