//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rent_All_Certificate.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CertificateType
    {
        public CertificateType()
        {
            this.Certification = new HashSet<Certification>();
        }
    
        public int CertificateTypeID { get; set; }
        public string CertificateTypeName { get; set; }
    
        public virtual ICollection<Certification> Certification { get; set; }
    }
}
