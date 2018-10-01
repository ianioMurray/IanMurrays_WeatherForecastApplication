using System;

namespace WeatherApp.Framework
{
    class TimeOfDayHelper
    {
        DayOfWeekHelper dayHelper;
        string currentTime = "1300";

        public TimeOfDayHelper(DayOfWeekHelper dayHelper)
        {
            this.dayHelper = dayHelper;
        }
        
        public int ConvertTimeToIndex(string time)
        {
            int currentTimeIndex = 0;

            if (dayHelper.isSelectedDayTheCurrentDay())
            {
                currentTimeIndex = GetTimeIndex(currentTime);
            }

            int indexOfSelectedTime = GetTimeIndex(time) - currentTimeIndex;

            if (indexOfSelectedTime >= 0)
            {
                return indexOfSelectedTime;
            }
            else
            {
                throw new Exception("It is not possible to select a time in the past");
            }
        }

        private int GetTimeIndex(string time)
        {
            switch (time)
            {
                case "0100":
                    return 0;
                case "0400":
                    return 1;
                case "0700":
                    return 2;
                case "1000":
                    return 3;
                case "1300":
                    return 4;
                case "1600":
                    return 5;
                case "1900":
                    return 6;
                case "2200":
                    return 7;
                default:
                    throw new Exception("Unknown time specified");
            }
        }
    }
}
