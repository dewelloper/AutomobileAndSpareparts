//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dal
{
    using Dal.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AutoServices")]
    public partial class AutoServices: Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AutoServices()
        {
            this.ProductPlaces = new HashSet<ProductPlaces>();
        }
    
        public string ServiceName { get; set; }
        public string AuthorizedName { get; set; }
        public string AuthorizedSurname { get; set; }
        public string AuthorizedEmail { get; set; }
        public string AuthorizedPassword { get; set; }
        public string AuthorizedPhone { get; set; }
        public string Website { get; set; }
        public string AddressDefinition { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> RegionId { get; set; }
        public Nullable<long> RegistererId { get; set; }
        public Nullable<long> ModifierId { get; set; }
        public string MapURL { get; set; }
        public Nullable<int> FeedCount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPlaces> ProductPlaces { get; set; }
    }
}