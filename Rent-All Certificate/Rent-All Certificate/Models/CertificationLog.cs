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
    
    public partial class CertificationLog
    {
        public int CertificateID { get; set; }
        public string CertificateTypeName { get; set; }
        public int CertificateTypeID { get; set; }
        public int InventoryID { get; set; }
        public string ProductKey { get; set; }
        public string BranchName { get; set; }
        public string EmployeeName { get; set; }
        public System.DateTime Date { get; set; }
    }
}
