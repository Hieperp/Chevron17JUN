using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel.Helpers;

namespace TotalModel
{
    public interface IBaseModel : IValidatableObject
    {
        DateTime? EntryDate { get; set; }
        int LocationID { get; set; }


        bool Approved { get; set; }
        Nullable<System.DateTime> ApprovedDate { get; set; }

        bool InActive { get; set; }
        Nullable<System.DateTime> InActiveDate { get; set; }

        bool InActivePartial { get; set; }
        Nullable<System.DateTime> InActivePartialDate { get; set; }

        Nullable<int> VoidTypeID { get; set; }

    }

    public abstract class BaseModel : NotifyPropertyChangeObject, IBaseModel
    {
        protected BaseModel() { this.EntryDate = DateTime.Now; }
        public virtual void Init() { this.EntryDate = DateTime.Now; }

        private DateTime? entryDate;
        [UIHint("DateTimeReadonly")]
        [Display(Name = "Ngày lập")]
        [Required(ErrorMessage = "Vui lòng nhập ngày lập")]
        public DateTime? EntryDate {
            get { return this.entryDate; }
            set { ApplyPropertyChange<BaseModel, DateTime?>(ref this.entryDate, o => o.EntryDate, value); }
        }



        public int LocationID { get; set; }

        private string remarks;
        [Display(Name = "Ghi chú")]
        [DefaultValue(null)]
        public string Remarks
        {
            get { return this.remarks; }
            set { ApplyPropertyChange<BaseModel, string>(ref this.remarks, o => o.Remarks, value); }
        }

        public virtual bool Approved { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        
        public virtual bool InActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActivePartialDate { get; set; }

        public virtual Nullable<int> VoidTypeID { get; set; }

        #region Implementation of IValidatableObject

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (false) yield return new ValidationResult("", new[] { "" });
        }

        #endregion
    }
}
