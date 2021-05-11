using System;
using Telerik.Windows.Controls.TimeBar;

namespace CMAPlatform.TimeLine.Converter
{
    public class DayFormat : IIntervalFormatterProvider
    {
        private Func<DateTime, string>[] formatters;
        private Func<DateTime, string>[] intervalSpanFormatters;

        public Func<DateTime, string>[] GetFormatters(IntervalBase interval)
        {
            if (formatters == null)
            {
                formatters = new Func<DateTime, string>[]
                {
                    date => string.Format("{0:MM}月{1:dd}日", date, date)
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
                        string.Format("{0:MM}月{1:dd}日 - {2:MM}月{3:dd}日", date, date,
                            interval.IncrementByCurrentInterval(date), interval.IncrementByCurrentInterval(date))
                };
            }
            return intervalSpanFormatters;
        }
    }

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

    public class MinuteFormat : IIntervalFormatterProvider
    {
        private Func<DateTime, string>[] formatters;
        private Func<DateTime, string>[] intervalSpanFormatters;

        public Func<DateTime, string>[] GetFormatters(IntervalBase interval)
        {
            if (formatters == null)
            {
                formatters = new Func<DateTime, string>[]
                {
                    (DateTime date) => string.Format("{0:HH:mm}", date)
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
                        string.Format("{0:HH:smm} - {1:HH:mm}", date, interval.IncrementByCurrentInterval(date))
                };
            }
            return intervalSpanFormatters;
        }
    }

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

    public class YearFormat : IIntervalFormatterProvider
    {
        private Func<DateTime, string>[] formatters;
        private Func<DateTime, string>[] intervalSpanFormatters;

        public Func<DateTime, string>[] GetFormatters(IntervalBase interval)
        {
            if (formatters == null)
            {
                formatters = new Func<DateTime, string>[]
                {
                    (DateTime date) => string.Format("{0:yyyy}年", date)
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
                        string.Format("{0:yyyy}年 - {1:yyyy}年", date, interval.IncrementByCurrentInterval(date))
                };
            }
            return intervalSpanFormatters;
        }
    }

    public class DecadeMinuteInterval : IntervalBase
    {
        private static readonly Func<DateTime, string>[] formatters;

        static DecadeMinuteInterval()
        {
            formatters = new Func<DateTime, string>[]
            {
                (DateTime date) => string.Format("{0:HH:mm}", date)
            };
        }

        public override Func<DateTime, string>[] Formatters
        {
            get { return formatters; }
        }

        public override TimeSpan MinimumPeriodLength
        {
            get { return TimeSpan.FromSeconds(600); }
        }

        public override DateTime ExtractIntervalStart(DateTime date)
        {
            var firstMonthOfInterval = GetFirstMonthOfInterval(date);
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, firstMonthOfInterval, 0);
        }

        public override DateTime IncrementByInterval(DateTime date, int intervalSpan)
        {
            return date.AddMinutes(intervalSpan * 10);
        }

        private int GetFirstMonthOfInterval(DateTime date)
        {
            //int quarter = GetNumberOfInterval(date);
            var firstMonthOfInterval = date.Minute / 10 * 10;
            return firstMonthOfInterval;
        }
    }
}