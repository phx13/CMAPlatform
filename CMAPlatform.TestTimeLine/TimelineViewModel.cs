using System;
using System.Collections.Generic;
using System.ComponentModel;
using Telerik.Windows.Controls.TimeBar;

namespace IntervalSpecificItems
{
    public class TimelineViewModel : INotifyPropertyChanged
    {
        private IntervalBase currentInterval;
        private List<TimelineDataItem> timelineItemsSource;

        public TimelineViewModel()
        {
            GenerateItems();
        }

        public IntervalBase CurrentInterval
        {
            get { return currentInterval; }
            set
            {
                currentInterval = value;
                UpdateTimelineItemsSource();
            }
        }

        public List<TimelineDataItem> TimelineItemsSource
        {
            get { return timelineItemsSource; }
            set
            {
                timelineItemsSource = value;
                OnPropertyChanged("TimelineItemsSource");
            }
        }

        public List<TimelineDataItem> MinuteItems { get; set; }

        public List<TimelineDataItem> HourItems { get; set; }

        public List<TimelineDataItem> DayItems { get; set; }

        public List<TimelineDataItem> MonthItems { get; set; }

        public List<TimelineDataItem> YearItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateTimelineItemsSource()
        {
            var intervalType = CurrentInterval.GetType();

            if (intervalType == typeof (DecadeMinuteInterval))
            {
                TimelineItemsSource = MinuteItems;
            }
            else if (intervalType == typeof (HourInterval))
            {
                TimelineItemsSource = HourItems;
            }
            else if (intervalType == typeof (DayInterval))
            {
                TimelineItemsSource = DayItems;
            }
            else if (intervalType == typeof (MonthInterval))
            {
                TimelineItemsSource = MonthItems;
            }
            else if (intervalType == typeof (YearInterval))
            {
                TimelineItemsSource = YearItems;
            }
        }

        private void GenerateItems()
        {
            var startDate = new DateTime(2000, 1, 1);
            var endDate = new DateTime(2010, 1, 1);

            MinuteItems = new List<TimelineDataItem>();
            HourItems = new List<TimelineDataItem>();
            DayItems = new List<TimelineDataItem>();
            MonthItems = new List<TimelineDataItem>();
            YearItems = new List<TimelineDataItem>();

            for (var date = startDate; date < endDate; date = date.AddMinutes(10))
            {
                //this.MinuteItems.Add(new TimelineDataItem() { Date = date, Duration = TimeSpan.FromMinutes(10) });
            }

            for (var date = startDate; date < endDate; date = date.AddHours(1))
            {
                HourItems.Add(new TimelineDataItem {Date = date, Duration = TimeSpan.FromHours(1)});
            }

            for (var date = startDate; date < endDate; date = date.AddDays(1))
            {
                DayItems.Add(new TimelineDataItem {Date = date, Duration = TimeSpan.FromDays(1)});
            }

            for (var date = startDate; date < endDate; date = date.AddMonths(1))
            {
                MonthItems.Add(new TimelineDataItem {Date = date, Duration = date.AddMonths(1) - date});
            }

            for (var date = startDate; date < endDate; date = date.AddYears(1))
            {
                YearItems.Add(new TimelineDataItem {Date = date, Duration = date.AddYears(1) - date});
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}