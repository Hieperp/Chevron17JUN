using TotalBase;

namespace TotalDTO.Productions
{
    public class BarcodeDTO : BaseDTO
    {
        public BarcodeDTO()
        { }
        public BarcodeDTO(FillingData fillingData)
        {
            if (fillingData != null)
            {
                this.FillingLineID = (int)fillingData.FillingLineID;
                this.CommodityID = fillingData.CommodityID;
                this.PCID = fillingData.PCID;
                this.EntryStatusID = (int)GlobalVariables.BarcodeStatus.Normal;
            }
        }

        public string PCID { get; set; }
        public int FillingLineID { get; set; }
        public int CommodityID { get; set; }
        public string Code { get; set; }
        public int EntryStatusID { get; set; }

        public int QueueID { get; set; } //JUST FOR FillingPackDTO ONLY

        public override int PreparedPersonID { get { return 1; } }
    }
}
