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
    
    public partial class Transport
    {
        public int Id { get; set; }
        public int teamId { get; set; }
        public string originPostalCode { get; set; }
        public string destinationPostalCode { get; set; }
        public Nullable<System.DateTime> creationTime { get; set; }
        public Nullable<System.DateTime> allocationTime { get; set; }
        public Nullable<System.DateTime> assignmentTime { get; set; }
        public Nullable<int> teamMileage { get; set; }
        public int teamNumber { get; set; }
    
        public virtual Equipe Equipe { get; set; }
    }
}
