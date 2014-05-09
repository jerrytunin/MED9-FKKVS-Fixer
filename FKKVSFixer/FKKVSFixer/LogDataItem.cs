using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FKKVSFixer
{
    public class LogDataItem
    {
        public double rpm { get; set; }
        public double pw { get; set; }
        public double correction { get; set; }

        public LogDataItem(double rpm, double pw, double correction)
        {
            this.rpm = rpm;
            this.pw = pw;
            this.correction = correction;
        }
    }
}
