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
    
    public partial class Question
    {
        public int ID { get; set; }
        public int Status_ID { get; set; }
        public int Category_ID { get; set; }
        public string Questioner { get; set; }
        public string Respondent { get; set; }
        public string AskQuestion { get; set; }
        public string Answer { get; set; }
        public Nullable<int> CountView { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual StatusQuestion StatusQuestion { get; set; }
    }
}