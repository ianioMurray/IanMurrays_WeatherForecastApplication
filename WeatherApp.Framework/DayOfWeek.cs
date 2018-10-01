using System;

namespace WeatherApp.Framework
{
    public class DayOfWeekHelper
    {
        int todayAsInt;
        int currentDay = (int)DayOfWeek.Tuesday;
        bool currentDaySelected = false;

        public int ConvertDayOfWeekToNumber(string dayName)
        {
            try
            {
                DayOfWeek today = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayName, true);
                todayAsInt = (int)today;
            }
            catch
            {
                throw new Exception("Unable to convert entered day to a valid day of week");
            }
        
            if (todayAsInt == currentDay)
            {
                currentDaySelected = true;
            }
            
            return todayAsInt - currentDay;
        }

        public bool isSelectedDayTheCurrentDay()
        {
            return currentDaySelected;
        }
    }
}
