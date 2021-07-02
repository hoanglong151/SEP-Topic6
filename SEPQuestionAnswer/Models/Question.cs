//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SEPQuestionAnswer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Question
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public Nullable<int> Category_ID { get; set; }
        public string Questioner { get; set; }
        public string Respondent { get; set; }
        [Required]
        public string AskQuestion { get; set; }
        [Required]
        public string Answer { get; set; }
        public Nullable<int> CountView { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual Category Category { get; set; }
    }
}
