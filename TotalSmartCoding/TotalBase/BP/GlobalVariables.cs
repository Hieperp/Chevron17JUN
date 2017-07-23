using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalBase
{
    public class GlobalVariables
    {

        public static bool MyTest = false;

        public static bool shouldRestoreProcedure = false;


        public enum ENMVNTaskID
        {
            EEmployeeCategory = 1,
            EEmployeeType = 2,
            EEmployeeName = 3
        }


        public const char charESC = (char)27;//char.ConvertFromUtf32(27);//1BH
        public const char charEOT = (char)4;//char.ConvertFromUtf32(4);//04H
        public const char charACK = (char)6;//char.ConvertFromUtf32(6);//06H
        public const char charNACK = (char)21;//char.ConvertFromUtf32(21);//15H

        public const char charPrintingACK = (char)28;//char.ConvertFromUtf32(21);//1CH

        public const char charSTX = (char)02;//char.ConvertFromUtf32(02);//02H
        public const char charETX = (char)03;//char.ConvertFromUtf32(03);//03H


        public const char doubleTabChar = (char)09;


        public const char charLF = (char)10;//char.ConvertFromUtf32( );//
        public const char charCR = (char)13;//char.ConvertFromUtf32( );//


        public static int[] DominoProductCounter = new int[4] { 0, 0, 0, 0 };   //This array use in associated with [enum DominoPrinterName] => Important Note: element 0: not use!!!





        public enum FillingLine
        {
            Ocme = 1,
            Smallpack = 2,
            Drum = 3,
            CM = 4,
            Pail = 5
        }


        public enum PrinterName
        {
            DegitInkjet = 1,
            PackInkjet = 2,
            CartonInkjet = 3,
            PalletLabel = 6
        }


        public enum BarcodeScannerName
        {
            QualityScanner = 1,
            MatchingScanner = 2,
            CartonScanner = 3
        }



        public enum BarcodeStatus
        {
            Normal = 0,
            ReadyToCarton = 2,
            BlankBarcode = 9,
            EmptyCarton = 10,
            HasSent = 99,
            Deleted = 199
        }
        
        public enum ZebraStatus
        {
            Freshnew = 0,
            Reprint = -1,


            Successfully = 1,
            
            
            //WAIT FOR 3 TIMES TO ENSURE RECEIVE ACK/ NACK FROM ZEBRA PRINTER
            Printing1 = 90,
            Printing2 = 91,
            Printing3 = 92,
            
            Reprinting1 = -90,
            Reprinting2 = -91,
            Reprinting3 = -92
        }


        public const string BlankBarcode = "[Blank]";

        public static bool IgnoreEmptyCarton = true;
        public static bool IgnoreEmptyPallet = true;

        public static bool DuplicateCartonBarcodeFound = false;

        public static int LocationID = -1;



        public static FillingLine FillingLineID = FillingLine.Ocme;
        public static string FillingLineCode = "0";
        public static string FillingLineName = FillingLine.Ocme.ToString();

        public static int noItemPerCartonSetByProductID = 0;

        public static int PortNumber = 7000;
        public static string PortName = "COM 4";


        public const int ServerLineID = 99;


        public static string IpAddress(PrinterName dominoPrinterNameID)
        {
            switch (GlobalVariables.FillingLineID)
            {
                case FillingLine.Ocme:
                    switch (dominoPrinterNameID)
                    {
                        case PrinterName.DegitInkjet:
                            return "192.168.1.101";
                        case PrinterName.PackInkjet:
                            return "192.168.1.102";
                        case PrinterName.CartonInkjet:
                            return "192.168.1.163";
                        default:
                            return "127.0.0.1";
                    }
                case FillingLine.Smallpack:
                    switch (dominoPrinterNameID)
                    {
                        case PrinterName.DegitInkjet:
                            return "192.168.1.104";
                        case PrinterName.PackInkjet:
                            return "192.168.1.105";
                        case PrinterName.CartonInkjet:
                            return "192.168.1.106";
                        default:
                            return "127.0.0.1";
                    }

                case FillingLine.CM:
                    switch (dominoPrinterNameID)
                    {
                        case PrinterName.DegitInkjet:
                            return "192.168.1.107";
                        case PrinterName.PackInkjet:
                            return "192.168.1.108";
                        case PrinterName.CartonInkjet:
                            return "192.168.1.109";
                        default:
                            return "127.0.0.1";
                    }

                case FillingLine.Drum:
                    switch (dominoPrinterNameID)
                    {
                        case PrinterName.DegitInkjet:
                            return "192.168.1.110";
                        case PrinterName.PackInkjet:
                            return "192.168.1.111";
                        case PrinterName.CartonInkjet:
                            return "192.168.1.112";
                        default:
                            return "127.0.0.1";
                    }

                case FillingLine.Pail:
                    switch (dominoPrinterNameID)
                    {
                        case PrinterName.DegitInkjet:
                            return "192.168.1.113";
                        case PrinterName.PackInkjet:
                            return "192.168.1.114";
                        case PrinterName.CartonInkjet:
                            return "127.0.0.1";
                        default:
                            return "127.0.0.1";
                    }
                default:
                    return "127.0.0.1";
            }

        }




        public static string IpAddress(BarcodeScannerName barcodeScannerName)
        {
            switch (GlobalVariables.FillingLineID)
            {
                case FillingLine.Ocme:
                    switch (barcodeScannerName)
                    {
                        case BarcodeScannerName.QualityScanner:
                            return "127.0.0.1";
                        case BarcodeScannerName.MatchingScanner:
                            return "192.168.1.51";
                        case BarcodeScannerName.CartonScanner:
                            return "127.0.0.1";
                        default:
                            return "127.0.0.1";
                    }
                case FillingLine.Smallpack:
                    switch (barcodeScannerName)
                    {
                        case BarcodeScannerName.QualityScanner:
                            return "127.0.0.1";
                        case BarcodeScannerName.MatchingScanner:
                            return "192.168.1.55"; //192.168.1.52
                        case BarcodeScannerName.CartonScanner:
                            return "127.0.0.1";
                        default:
                            return "127.0.0.1";
                    }

                case FillingLine.CM:
                    switch (barcodeScannerName)
                    {
                        case BarcodeScannerName.QualityScanner:
                            return "127.0.0.1";
                        case BarcodeScannerName.MatchingScanner:
                            return "192.168.1.53";
                        case BarcodeScannerName.CartonScanner:
                            return "127.0.0.1";
                        default:
                            return "127.0.0.1";
                    }

                case FillingLine.Drum:
                    switch (barcodeScannerName)
                    {
                        case BarcodeScannerName.QualityScanner:
                            return "127.0.0.1";
                        case BarcodeScannerName.MatchingScanner:
                            return "192.168.1.54";
                        case BarcodeScannerName.CartonScanner:
                            return "127.0.0.1";
                        default:
                            return "127.0.0.1";
                    }

                case FillingLine.Pail:
                    switch (barcodeScannerName)
                    {
                        case BarcodeScannerName.QualityScanner:
                            return "127.0.0.1";
                        case BarcodeScannerName.MatchingScanner:
                            return "192.168.1.55";
                        case BarcodeScannerName.CartonScanner:
                            return "127.0.0.1";
                        default:
                            return "127.0.0.1";
                    }
                default:
                    return "127.0.0.1";
            }

        }





        public static int NoItemDiverter()
        {
            switch (GlobalVariables.FillingLineID)
            {
                case FillingLine.Ocme:
                    return 8;
                case FillingLine.Smallpack:
                    return 6;
                case FillingLine.Drum:
                    return 1;
                case FillingLine.CM:
                    return 1;
                case FillingLine.Pail:
                    return 1;
                default:
                    return 1;
            }
        }


        public static int NoSubQueue()
        {
            //IMPORTANT:
            //NoItemPerCarton must be a multiples of NoSubQueue
            //This means: (GlobalVariables.NoItemPerCarton % GlobalVariables.NoSubQueue == 0)

            switch (GlobalVariables.FillingLineID)
            {
                case FillingLine.Ocme:
                    return 2;
                case FillingLine.Smallpack:
                    return 6;
                case FillingLine.Drum:
                    return 1;
                case FillingLine.CM:
                    return 1;
                case FillingLine.Pail:
                    return 1;
                default:
                    return 1;
            }
        }


        public static int NoItemPerCarton()
        {
            //IMPORTANT:
            //------NoItemPerCarton must be a multiples of NoSubQueue
            //------This means: (GlobalVariables.NoItemPerCarton % GlobalVariables.NoSubQueue == 0)


            //It is free to change NoItemPerCarton
            //with one thing should be considered:
            //----If NoItemPerCarton <= 24: There is nothing must modify
            //----BUT: If NoItemPerCarton > 24: MUST MODIFY THE CODES IN: BarcodeScannerBLL.cs: When copy Data FROM packInOneCarton TO cartonDataTable; AND viseversa

            switch (GlobalVariables.FillingLineID)
            {
                case FillingLine.Ocme:
                    return 12;
                case FillingLine.Smallpack:
                    return 24;
                case FillingLine.Drum:
                    return GlobalVariables.noItemPerCartonSetByProductID;
                case FillingLine.CM:
                    return GlobalVariables.noItemPerCartonSetByProductID;
                case FillingLine.Pail:
                    return 1;
                default:
                    return 1;
            }
        }


        public static bool RepeatedSubQueueIndex()
        {
            switch (GlobalVariables.FillingLineID)
            {
                case FillingLine.Ocme:
                    return false;
                case FillingLine.Smallpack:
                    return true;
                case FillingLine.Drum:
                    return false;
                case FillingLine.CM:
                    return false;
                case FillingLine.Pail:
                    return false;
                default:
                    return false;
            }
        }



        public static string MCUAddress(BarcodeScannerName barcodeScannerNameID)
        {
            switch (GlobalVariables.FillingLineID)
            {
                case FillingLine.Ocme:
                    switch (barcodeScannerNameID)
                    {
                        case BarcodeScannerName.QualityScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("30", 16));
                        case BarcodeScannerName.MatchingScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("31", 16));
                        case BarcodeScannerName.CartonScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("32", 16));
                        default:
                            return char.ConvertFromUtf32(Convert.ToInt32("3F", 16)); //3FH
                    }
                case FillingLine.Smallpack:
                    switch (barcodeScannerNameID)
                    {
                        case BarcodeScannerName.QualityScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("30", 16));
                        case BarcodeScannerName.MatchingScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("31", 16));
                        case BarcodeScannerName.CartonScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("32", 16));
                        default:
                            return char.ConvertFromUtf32(Convert.ToInt32("3F", 16));
                    }
                case FillingLine.Drum:
                    switch (barcodeScannerNameID)
                    {
                        case BarcodeScannerName.QualityScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("30", 16));
                        case BarcodeScannerName.MatchingScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("31", 16));
                        case BarcodeScannerName.CartonScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("32", 16));
                        default:
                            return char.ConvertFromUtf32(Convert.ToInt32("3F", 16));
                    }
                case FillingLine.CM:
                    switch (barcodeScannerNameID)
                    {
                        case BarcodeScannerName.QualityScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("30", 16));
                        case BarcodeScannerName.MatchingScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("31", 16));
                        case BarcodeScannerName.CartonScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("32", 16));
                        default:
                            return char.ConvertFromUtf32(Convert.ToInt32("3F", 16));
                    }
                case FillingLine.Pail:
                    switch (barcodeScannerNameID)
                    {
                        case BarcodeScannerName.QualityScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("30", 16));
                        case BarcodeScannerName.MatchingScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("31", 16));
                        case BarcodeScannerName.CartonScanner:
                            return char.ConvertFromUtf32(Convert.ToInt32("32", 16));
                        default:
                            return char.ConvertFromUtf32(Convert.ToInt32("3F", 16));
                    }
                default:
                    return char.ConvertFromUtf32(Convert.ToInt32("3F", 16));
            }

        }






    }






}
