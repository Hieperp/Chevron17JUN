using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO
{
    public class BaseDTO : BaseModel, IAccessControlAttribute
    {
        public BaseDTO()
        {
            // apply any DefaultValueAttribute settings to their properties
            var propertyInfos = this.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                if (attributes.Any())
                {
                    var attribute = (DefaultValueAttribute)attributes[0];
                    propertyInfo.SetValue(this, attribute.Value, null);
                }
            }
        }

        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.UnKnown; } }

        public virtual int GetID() { return 0; }
        public int LastID { get; set; } //THIS KEEP THE LAST ID WHEN CREATE NEW Entity. HOWEVER, EACH TIME TO EDIT THE CURRENT Entity, THIS ALSO KEEP CURRENT EDITED ID

        private string reference;
        [Display(Name = "Số phiếu")]
        [DefaultValue(null)]
        public string Reference
        {
            get { return this.reference; }
            set { ApplyPropertyChange<BaseDTO, string>(ref this.reference, o => o.Reference, value); }
        }



        public int UserID { get; set; }
        [Required]
        [Display(Name = "Người lập")]
        public virtual int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }


        [Display(Name = "Người duyệt")]
        public int ApproverID { get; set; }


        [Display(Name = "Diễn giải")]
        public string Description { get; set; }


        public bool GlobalLocked { get; set; }


        public bool Newable { get; set; }
        public bool Editable { get; set; }

        //private bool editable;
        //[Display(Name = "Số phiếu")]
        //public bool Editable
        //{
        //    get { return this.editable; }
        //    set { ApplyPropertyChange<BaseDTO, bool>(ref this.editable, o => o.Editable, value); }
        //}




        public bool Approvable { get; set; }
        public bool UnApprovable { get; set; }
        public bool Deletable { get; set; }
        public bool Voidable { get; set; }
        public bool UnVoidable { get; set; }

        public bool ShowDiscount { get; set; }


        //These properties are used as an implementation preservation of ISimpleViewModel for these ________ViewModel class (Those class ________ViewModel which is BOTH inheritance from this BaseDTO AND implementation of ISimpleViewModel)
        public virtual bool PrintAfterClosedSubmit { get; set; }
        public GlobalEnums.SubmitTypeOption SubmitTypeOption { get; set; }

        public virtual int PrintOptionID { get; set; }



        public virtual void PerformPresaveRule() { }

        public virtual void PrepareVoidDetail(int? detailID) { }
    }
}

