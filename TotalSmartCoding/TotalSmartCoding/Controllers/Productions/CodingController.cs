using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalDTO.Productions;
using TotalModel.Helpers;

namespace TotalSmartCoding.Controllers.Productions
{
    public class CodingController : NotifyPropertyChangeObject
    {
        private FillingLineData fillingLineData;

        private bool loopRoutine = false;

        private string mainStatus;

        private bool ledGreenOn;
        private bool ledAmberOn;
        private bool ledRedOn;


        #region Public Properties


        public FillingLineData FillingLineData
        {
            get
            {
                return this.fillingLineData;
            }

            protected set
            {
                this.fillingLineData = value;
            }
        }


        public bool LoopRoutine
        {
            get
            {
                return this.loopRoutine;
            }

            set
            {
                if (this.loopRoutine != value)
                {
                    this.loopRoutine = value;
                }
            }
        }


        public string MainStatus
        {
            get
            {
                return this.mainStatus;
            }

            protected set
            {
                if (this.mainStatus != value)
                {
                    this.mainStatus = value;
                    this.NotifyPropertyChanged("MainStatus");
                }
            }
        }


        public bool LedGreenOn
        {
            get
            {
                return true; // this.ledGreenOn;
            }
            protected set
            {
                this.ledGreenOn = value;
            }
        }

        public bool LedAmberOn
        {
            get
            {
                return this.ledAmberOn;
            }
            protected set
            {
                this.ledAmberOn = value;
            }

        }

        public bool LedRedOn
        {
            get
            {
                return this.ledRedOn;
            }
            protected set
            {
                this.ledRedOn = value;
            }


        }

        #endregion Puclic Properties

    }

}

