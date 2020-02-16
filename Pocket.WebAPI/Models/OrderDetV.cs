using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pocket.WebAPI.Models
{
    public class OrderDetV : OrderDetail
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int GrantTotal { get; set; }
    }
}