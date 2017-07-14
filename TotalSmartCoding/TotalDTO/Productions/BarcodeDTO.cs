﻿namespace TotalDTO.Productions
{
    public class BarcodeDTO : BaseDTO
    {
        public string PCID { get; set; }
        public int FillingLineID { get; set; }
        public int CommodityID { get; set; }
        public string Code { get; set; }
        public int EntryStatusID { get; set; }

        public int QueueID { get; set; } //JUST FOR OnlinePackDTO ONLY
    }
}
