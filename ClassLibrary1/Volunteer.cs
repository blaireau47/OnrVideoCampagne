//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ONRVideo
{
    using System;
    using System.Collections.Generic;
    
    public partial class Volunteer
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string role { get; set; }
        public string email { get; set; }
        public int teamID { get; set; }
        public string organizationName { get; set; }
        public int id { get; set; }
        public int teamNumber { get; set; }
        public string personCode { get; set; }
    
        public virtual Equipe Equipe { get; set; }
    }
}
