
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
    
public partial class City
{

    public City()
    {

        this.Branch = new HashSet<Branch>();

    }


    public string CountryName { get; set; }

    public string CityName { get; set; }



    public virtual ICollection<Branch> Branch { get; set; }

    public virtual Country Country { get; set; }

}

}
