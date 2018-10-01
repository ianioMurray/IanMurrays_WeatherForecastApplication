using OpenQA.Selenium;
using System.Collections.Generic;
using System;
using System.Linq;

namespace WeatherApp.Framework
{
    public class Homepage
    {
        private Driver driver; 
        private DayOfWeekHelper dayHelper;
        private TimeOfDayHelper timeHelper;

        private int indexOfDay;
        private int indexOfTime;

        private IList<IWebElement> dailyOverviewCollection;
        private IList<IWebElement> dailyOverviewAndThreeHourlyUpdateForASpecificDayCollection;
        private IList<IWebElement> threeHourlyUpdatesCollection;

        private IWebElement weatherInfoLine;

        public delegate int myMethod();

        public Homepage()
        {
            driver = new Driver();
            dayHelper = new DayOfWeekHelper();
            timeHelper = new TimeOfDayHelper(dayHelper);
        }

        public void dispose()
        {
            driver.Dispose();
        }

        private void setDailyOverviewCollection()
        {
            dailyOverviewCollection = driver.instance.FindElements(By.ClassName("summary"));
        }

        private void setDailyOverviewAndThreeHourlyUpdateForASpecificDayCollection()
        {
            dailyOverviewAndThreeHourlyUpdateForASpecificDayCollection = driver.instance.FindElements(By.ClassName("details"));
        }

        private void setThreeHourlyUpdatesCollection(IWebElement dayInformation)
        {
            threeHourlyUpdatesCollection = dayInformation.FindElements(By.ClassName("detail"));
        }

        private void setWeatherInfoLine(IWebElement selectedLine)
        {
            weatherInfoLine = selectedLine;
        }

        private void setIndexForADailyOverview(string day)
        {
            indexOfDay = dayHelper.ConvertDayOfWeekToNumber(day);
        }

        public void setDayTime(string day, string time)
        {
            displayThreeHourlyUpdatesForDay(day);

            if (time != "overview")
            {
                indexOfTime = timeHelper.ConvertTimeToIndex(time);
                WaitForElement.WaitForElementToDisplay(driver, threeHourlyUpdatesCollection[indexOfTime], TimeSpan.FromSeconds(2));
                setWeatherInfoLine(threeHourlyUpdatesCollection[indexOfTime]);
            }
            else
            {
                setIndexForADailyOverview(day);
                setWeatherInfoLine(dailyOverviewCollection[indexOfDay]);
            }
        }

        public void EnterCityName(string cityName)
        {
            IWebElement cityNameTextField = driver.instance.FindElement(By.Id("city"));
            cityNameTextField.Clear();
            cityNameTextField.SendKeys(cityName + "\n");
        }
        
        public int checkNumberOfDailyOverviews()
        {
            setDailyOverviewCollection();
            return dailyOverviewCollection.Count;
        }
        
        public void displayThreeHourlyUpdatesForDay(string day)
        {
            //sets daily overview and opens the specified day
            setDailyOverviewCollection();
            setIndexForADailyOverview(day);
            dailyOverviewCollection[indexOfDay].Click();

            //sets the three hourly updates 
            setDailyOverviewAndThreeHourlyUpdateForASpecificDayCollection();
            setThreeHourlyUpdatesCollection(dailyOverviewAndThreeHourlyUpdateForASpecificDayCollection[indexOfDay]);
        }
        
        public string getDayName(string day)
        {
            setIndexForADailyOverview(day);
            return dailyOverviewCollection[indexOfDay].FindElement(By.ClassName("name")).Text;
        }

        public string getWeatherConditions()
        {
            return weatherInfoLine.FindElement(By.ClassName("icon")).GetAttribute("aria-label");
        }

        public string getMaxTemp()
        {
            return weatherInfoLine.FindElement(By.ClassName("max")).Text;
        }

        public string getMinTemp()
        {
            return weatherInfoLine.FindElement(By.XPath("span[3]/span[2]")).Text;
        }

        public string getWindSpeed()
        {
            return weatherInfoLine.FindElement(By.ClassName("speed")).Text;
        }

        public string getWindDirection()
        {
            IWebElement item = weatherInfoLine.FindElements(By.XPath("span[4]/span[2]/*")).First();
            string imageStyles = item.GetAttribute("style");
            string windDirection = imageStyles.Substring(imageStyles.IndexOf("rotate("));
            int startOfDegrees = windDirection.IndexOf("(") + 1;
            int endOfDegrees = windDirection.IndexOf("deg)");
            return windDirection.Substring(startOfDegrees, endOfDegrees - startOfDegrees);
        }

        public string getRainfall()
        {
            return weatherInfoLine.FindElement(By.ClassName("rainfall")).Text;
        }

        public string getPressure()
        {
            return weatherInfoLine.FindElement(By.XPath("span[5]/span[2]")).Text;
        }


        public int getNumberOfThreeHourlyWeatherUpdates()
        {
            return threeHourlyUpdatesCollection.Count;
        }

        public bool isThreeHourelyUpdatesVisible()
        {
            WaitForElement.WaitForElementToDisplay(driver, dailyOverviewAndThreeHourlyUpdateForASpecificDayCollection[indexOfDay], TimeSpan.FromSeconds(2));
            return dailyOverviewAndThreeHourlyUpdateForASpecificDayCollection[indexOfDay].Displayed;
        }

        public bool isThreeHourelyUpdatesNotVisible()
        {
            WaitForElement.WaitForElementToNotDisplay(driver, dailyOverviewAndThreeHourlyUpdateForASpecificDayCollection[indexOfDay], TimeSpan.FromSeconds(2));
            return dailyOverviewAndThreeHourlyUpdateForASpecificDayCollection[indexOfDay].Displayed;
        }

        private int convertTemperatureStringValueToaNumber(string temperature)  // converts both max and min tempatures
        {
            string TemperatureWithoutDegrees = temperature.Substring(0, temperature.IndexOf("°"));
            int TemperatureVal;
            int.TryParse(TemperatureWithoutDegrees, out TemperatureVal);
            return TemperatureVal;
        }

        private int convertMaxTemperatureStringValueToaNumber()  
        {
            return convertTemperatureStringValueToaNumber(getMaxTemp());
        }

        private int convertMinTemperatureStringValueToaNumber()  
        {
            return convertTemperatureStringValueToaNumber(getMinTemp());
        }

        public int convertWindSpeedStringValueToaNumber()
        {
            string windSpeedFromPage = getWindSpeed();
            string windSpeedWithoutkph = windSpeedFromPage.Substring(0, windSpeedFromPage.IndexOf("kph"));
            int windSpeedVal;
            int.TryParse(windSpeedWithoutkph, out windSpeedVal);
            return windSpeedVal;
        }

        public int convertWindDirectionStringValueToaNumber()
        {
            string windDirection = getWindDirection();
            int degressAsInt;
            int.TryParse(windDirection, out degressAsInt);
            return degressAsInt;
        }

        public int convertRainfallStringValueToaNumber()
        {
            string RainfallFromPage = getRainfall();
            string RainfallWithoutMillimetres = RainfallFromPage.Substring(0, RainfallFromPage.IndexOf("m"));
            int RainfallVal;
            int.TryParse(RainfallWithoutMillimetres, out RainfallVal);
            return RainfallVal;
        }

        public bool DailyForecastWeatherConditionsIsCorrect()
        {
            setWeatherInfoLine(dailyOverviewCollection[indexOfDay]);
            string dailyOverviewWeatherConditions = getWeatherConditions();

            var threeHourlyWeatherConditions = new List<string>();

            foreach (IWebElement threeHourlyUpdate in threeHourlyUpdatesCollection)
            {
                setWeatherInfoLine(threeHourlyUpdate);
                threeHourlyWeatherConditions.Add(getWeatherConditions());
            }

            var query = (from item in threeHourlyWeatherConditions
                         group item by item into orderedCollection
                         orderby orderedCollection.Count() descending
                         select orderedCollection.Key).First();

            return dailyOverviewWeatherConditions == query;
        }

        public bool DailyForecastMaxTemperatureIsCorrect()
        {
            setWeatherInfoLine(dailyOverviewCollection[indexOfDay]);
            int dailyOverviewMaxTemperature = convertMaxTemperatureStringValueToaNumber();

            myMethod method = new myMethod(convertMaxTemperatureStringValueToaNumber);
            IList<int> ThreeHourlyMaxTemperature = creatCollectionOfThreeHourlyUpdateValues(method);

            return dailyOverviewMaxTemperature == ThreeHourlyMaxTemperature.Max();
        }

        public bool DailyForecastMinTemperatureIsCorrect()
        {
            setWeatherInfoLine(dailyOverviewCollection[indexOfDay]);
            int dailyOverviewMinTemperature = convertMinTemperatureStringValueToaNumber();

            myMethod method = new myMethod(convertMinTemperatureStringValueToaNumber);
            IList<int> ThreeHourlyMinTemperature = creatCollectionOfThreeHourlyUpdateValues(method);

            return dailyOverviewMinTemperature == ThreeHourlyMinTemperature.Min();
        }

        public bool DailyForecastWindSpeedIsCorrect()
        {
            setWeatherInfoLine(dailyOverviewCollection[indexOfDay]);
            int dailyOverviewWindSpeed = convertWindSpeedStringValueToaNumber();

            myMethod method = new myMethod(convertWindSpeedStringValueToaNumber);
            List<int> ThreeHourlyWindSpeed = creatCollectionOfThreeHourlyUpdateValues(method);

            return dailyOverviewWindSpeed == getMostDominantOrFirstVal(ThreeHourlyWindSpeed);
        }

        public bool DailyForecastWindDirectionIsCorrect()
        {
            setWeatherInfoLine(dailyOverviewCollection[indexOfDay]);
            int dailyOvervieWindDirection = convertWindDirectionStringValueToaNumber();

            myMethod method = new myMethod(convertWindDirectionStringValueToaNumber);
            List<int> ThreeHourlyWindDirection = creatCollectionOfThreeHourlyUpdateValues(method);

            return dailyOvervieWindDirection == getMostDominantOrFirstVal(ThreeHourlyWindDirection);
        }

        public bool DailyForecastRainfallIsCorrect()
        {
            setWeatherInfoLine(dailyOverviewCollection[indexOfDay]);
            int dailyOverviewRainfall = convertRainfallStringValueToaNumber();

            int TotalOfThreeHourlyRainfallValues = 0;
            
            foreach (IWebElement threeHourlyUpdate in threeHourlyUpdatesCollection)
            {
                setWeatherInfoLine(threeHourlyUpdate);
                TotalOfThreeHourlyRainfallValues = TotalOfThreeHourlyRainfallValues + convertRainfallStringValueToaNumber();
            }

            return dailyOverviewRainfall == TotalOfThreeHourlyRainfallValues;
        }
              
        private List<int> creatCollectionOfThreeHourlyUpdateValues(myMethod method)
        {
            var collection = new List<int>();

            foreach (IWebElement threeHourlyUpdate in threeHourlyUpdatesCollection)
            {
                setWeatherInfoLine(threeHourlyUpdate);
                collection.Add(method());
            }

            return collection;
        }

        private int getMostDominantOrFirstVal(List<int> collection)
        {
            var query = (from item in collection
                         group item by item into orderedCollection
                         orderby orderedCollection.Count() descending
                         select orderedCollection.Key).First();
            return query;
        }

    }
}
