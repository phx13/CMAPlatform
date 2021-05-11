using System;
using Telerik.Windows.Controls.TimeBar;

namespace IntervalSpecificItems
{
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
            return date.AddMinutes(intervalSpan*10);
        }

        private int GetFirstMonthOfInterval(DateTime date)
        {
            //int quarter = GetNumberOfInterval(date);
            var firstMonthOfInterval = date.Minute/10*10;
            return firstMonthOfInterval;
        }
    }
}