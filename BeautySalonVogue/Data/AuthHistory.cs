//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BeautySalonVogue.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class AuthHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public System.DateTime DateTime { get; set; }
        public bool Status { get; set; }
    
        public virtual User User { get; set; }
    }
}
