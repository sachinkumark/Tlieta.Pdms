//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tlieta.Pdms.Web
{
    using System;
    using System.Collections.Generic;
    
    public partial class ErrorLog
    {
        public int ErrorId { get; set; }
        public string Source { get; set; }
        public Nullable<System.DateTime> LogDate { get; set; }
        public string Message { get; set; }
        public string QueryString { get; set; }
        public string TargetSite { get; set; }
        public string StackTrace { get; set; }
        public string ServerName { get; set; }
        public string RequestUrl { get; set; }
        public string UserAgent { get; set; }
        public string UserIP { get; set; }
        public string UserAuthentication { get; set; }
        public string UserName { get; set; }
        public Nullable<int> EventId { get; set; }
    }
}
