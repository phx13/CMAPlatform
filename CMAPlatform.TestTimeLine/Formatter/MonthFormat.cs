using System;
using Telerik.Windows.Controls.TimeBar;

namespace IntervalSpecificItems.Formatter
{
    public class MonthFormat : IIntervalFormatterProvider
    {
        private Func<DateTime, string>[] formatters;
        private Func<DateTime, string>[] intervalSpanFormatters;

        public Func<DateTime, string>[] GetFormatters(IntervalBase interval)
        {
            if (formatters == null)
            {
                formatters = new Func<DateTime, string>[]
                {
                    (DateTime date) => string.Format("{0:MM}月", date)
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
                        string.Format("{0:MM}月 - {1:MM}月", date, interval.IncrementByCurrentInterval(date))
                };
            }
            return intervalSpanFormatters;
        }
    }
}