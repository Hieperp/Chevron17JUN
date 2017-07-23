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
        private FillingData fillingData;

        private bool loopRoutine = false;

        private string mainStatus;

        private bool ledGreenOn;
        private bool ledAmberOn;
        private bool ledRedOn;


        #region Public Properties


        public FillingData FillingData
        {
            get
            {
                return this.fillingData;
            }

            protected set
            {
                this.fillingData = value;
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
                return this.ledGreenOn;
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


        protected void setLED() { this.setLED(false, false, false); }
        protected void setLED(bool ledGreenOn, bool ledAmberOn, bool ledRedOn)
        {
            this.LedGreenOn = ledGreenOn;
            this.LedAmberOn = ledAmberOn;
            this.LedRedOn = ledRedOn;
            this.NotifyPropertyChanged("LedStatus");
        }


        #endregion Puclic Properties

    }

}

