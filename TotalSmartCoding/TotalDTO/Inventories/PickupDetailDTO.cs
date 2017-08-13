using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;

namespace TotalDTO.Inventories
{
    public class PickupDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.PickupDetailID; }

        //IMPORTANT: IMPLEMENT PropertyChanged!!!!
        //NOW: AFTER ADD PickupDetailDTO TO COLLECTION, WE DON'T CHANGE THESE PROPERTIES FROM BINDING DataGridView ALSO FROM BACKEND PickupDetailDTO OBJECT. SO: WE DON'T IMPLEMENT PropertyChanged FOR THESE PROPERTIES
        //LATER: IF WE RECEIPT FORM OTHER SOURCE THAN FROM PICKUP ONLY, WE SHOULD CONSIDER THIS => AND IMPLEMENT PropertyChanged FOR THESE PROPERTIES WHEN NECCESSARY


        public int PickupDetailID { get; set; }
        public int PickupID { get; set; }

        public int WarehouseID { get; set; }

        public int BinLocationID { get; set; }
        public string BinLocationCode { get; set; }

        public Nullable<int> PackID { get; set; }
        public Nullable<System.DateTime> PackEntryDate { get; set; }
        public string PackCode { get; set; }

        public Nullable<int> CartonID { get; set; }
        public Nullable<System.DateTime> CartonEntryDate { get; set; }
        public string CartonCode { get; set; }

        public Nullable<int> PalletID { get; set; }
        public Nullable<System.DateTime> PalletEntryDate { get; set; }
        public string PalletCode { get; set; }
    }





}
