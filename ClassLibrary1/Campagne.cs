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
    
    public partial class Campagne
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Campagne()
        {
            this.Soirees = new HashSet<Soiree>();
        }
    
        public int Id { get; set; }
        public int Year { get; set; }
        public int CentraleID { get; set; }
    
        public virtual Centrale Centrale { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Soiree> Soirees { get; set; }
    }
}