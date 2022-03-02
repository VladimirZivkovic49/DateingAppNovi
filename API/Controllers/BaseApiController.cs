using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
//(L162)
[ServiceFilter(typeof (LogUserActivity))]
//(162)
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController:ControllerBase
    {
        
    }
}