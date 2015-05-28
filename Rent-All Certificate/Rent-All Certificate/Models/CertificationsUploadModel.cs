using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Rent_All_Certificate.Models
{
    public class CertificationsUploadModel
    {
        public Certification Certification { get; set; }

        public IEnumerable<HttpPostedFileBase> Certificates { get; set; }
    }
}