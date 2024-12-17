//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Agency.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Request
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Request()
        {
            this.Trade = new HashSet<Trade>();
        }
    
        public int RequestId { get; set; }
        public int RequestClientId { get; set; }
        public System.DateTime RequestDate { get; set; }
        public int RequestAgentId { get; set; }
        public float RequestMinCost { get; set; }
        public float RequestMaxCost { get; set; }
        public int RequestRooms { get; set; }
        public int RequestRegionId { get; set; }
        public int RequestStatusId { get; set; }
        public string RequestDescription { get; set; }
    
        public virtual Agent Agent { get; set; }
        public virtual Client Client { get; set; }
        public virtual Region Region { get; set; }
        public virtual StatusRequest StatusRequest { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Trade> Trade { get; set; }
    }
}
