using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalBase
{
    public class OptionSetting
    {
        private DateTime lowerFillterDate;
        private DateTime upperFillterDate;

        public OptionSetting() : this(DateTime.Today.AddDays(-45), new DateTime(2013, 12, 31)) { }

        public OptionSetting(DateTime lowerFillterDate, DateTime upperFillterDate)
        {
            this.LowerFillterDate = lowerFillterDate;
            this.UpperFillterDate = upperFillterDate;
        }

        public DateTime LowerFillterDate
        {
            get { return this.lowerFillterDate; }
            set { this.lowerFillterDate = value; }
        }

        public DateTime UpperFillterDate
        {
            get { return this.upperFillterDate; }
            set { this.upperFillterDate = value; }
        }

    }
}
