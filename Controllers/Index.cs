using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace net_assignment.Index
{
    [Route("/")]
    public class IndexController: Controller{
        [HttpGet]
        public string Get()
        {
            return "";
        }
    }
}