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
    
    public partial class Certification
    {
        public int CertificateID { get; set; }
        public int CertificateTypeID { get; set; }
        public int InventoryID { get; set; }
        public string ProductKey { get; set; }
        public int BranchID { get; set; }
        public int EmployeeID { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Branch Branch { get; set; }
        public virtual CertificateType CertificateType { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}