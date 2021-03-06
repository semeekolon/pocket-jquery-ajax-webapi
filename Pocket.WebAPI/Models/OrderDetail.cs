//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pocket.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public long Id { get; set; }
        public Nullable<long> OrderMasterId { get; set; }
        public Nullable<long> ProductId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Total { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual OrderMaster OrderMaster { get; set; }
        public virtual Product Product { get; set; }
    }
}
