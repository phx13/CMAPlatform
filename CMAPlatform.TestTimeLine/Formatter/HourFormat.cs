using System;
using Telerik.Windows.Controls.TimeBar;

namespace IntervalSpecificItems.Formatter
{
    public class HourFormat : IIntervalFormatterProvider
    {
        private Func<DateTime, string>[] formatters;
        private Func<DateTime, string>[] intervalSpanFormatters;

        public Func<DateTime, string>[] GetFormatters(IntervalBase interval)
        {
            if (formatters == null)
            {
                formatters = new Func<DateTime, string>[]
                {
                    (DateTime date) => string.Format("{0:HH:00}", date)
                };
            }
            return formatters;
        }

        public Func<DateTime, string>[] GetIntervalSpanFormatters(IntervalBase interval)
        {
            if (intervalSpanFormatters == null)
            {
                intervalSpanFormatters = new Func<DateTime, string>[]
                {
                    (DateTime date) =>
                        string.Format("{0:HH:00} - {1:HH:00}", date, interval.IncrementByCurrentInterval(date))
                };
            }
            return intervalSpanFormatters;
        }
    }
}