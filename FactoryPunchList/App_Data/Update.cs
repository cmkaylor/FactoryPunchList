//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FactoryPunchList.App_Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Update
    {
        public int id { get; set; }
        public string Description { get; set; }
        public Nullable<int> PunchlistID { get; set; }
        public Nullable<int> Author { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    
        public virtual Punchlist Punchlist { get; set; }
        public virtual Supervisor Supervisor { get; set; }
    }
}