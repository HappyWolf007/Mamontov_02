//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mamontov_02
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ads
    {
        public int ID { get; set; }
        public System.DateTime PublicationDate { get; set; }
        public int CityID { get; set; }
        public string Seller { get; set; }
        public int CategoryID { get; set; }
        public string AdsType { get; set; }
        public bool IsOpen { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string Photo { get; set; }
    
        public virtual AdsType AdsType1 { get; set; }
        public virtual Category Category { get; set; }
        public virtual City City { get; set; }
        public virtual User User { get; set; }
    }
}
