using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TotalBase.Enums;
using TotalModel;

namespace TotalDTO.Helpers
{
    public interface IQuantityDetailDTO : IBaseModel
    {
        int CommodityID { get; set; }
        string CommodityCode { get; set; }
        string CommodityName { get; set; }
        int CommodityTypeID { get; set; }

        decimal Quantity { get; set; }
        decimal Volume { get; set; }
    }

    public abstract class QuantityDetailDTO : BaseModel, IQuantityDetailDTO, IBaseModel
    {
        public virtual int CommodityTypeID { get { return 1; } set { } } //NOT USED NOW. KEEP HERE FOR USE LATER

        private int commodityID;
        [DefaultValue(null)]
        public int CommodityID
        {
            get { return this.commodityID; }
            set { ApplyPropertyChange<QuantityDetailDTO, int>(ref this.commodityID, o => o.CommodityID, value); }
        }

        private string commodityCode;
        [DefaultValue("")]
        public virtual string CommodityCode
        {
            get { return this.commodityCode; }
            set { ApplyPropertyChange<QuantityDetailDTO, string>(ref this.commodityCode, o => o.CommodityCode, value); }
        }

        private string commodityName;
        [DefaultValue("")]
        public virtual string CommodityName
        {
            get { return this.commodityName; }
            set { ApplyPropertyChange<QuantityDetailDTO, string>(ref this.commodityName, o => o.CommodityName, value); }
        }

        private decimal quantity;
        [DefaultValue(0)]
        [Range(0, 99999999999, ErrorMessage = "Số lượng không hợp lệ")]
        public virtual decimal Quantity
        {
            get { return this.quantity; }
            set { ApplyPropertyChange<QuantityDetailDTO, decimal>(ref this.quantity, o => o.Quantity, Math.Round(value, (int)GlobalEnums.rndQuantity)); }
        }

        private decimal volume;
        [DefaultValue(0)]
        [Range(0, 99999999999, ErrorMessage = "Volume không hợp lệ")]
        public virtual decimal Volume
        {
            get { return this.volume; }
            set { ApplyPropertyChange<QuantityDetailDTO, decimal>(ref this.volume, o => o.Volume, Math.Round(value, (int)GlobalEnums.rndVolume)); }
        }
    }
}
