using TechTalk.SpecFlow;
using WeatherApp.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherApp.Test
{
    [Binding]
    public class WeatherAppSteps
    {
        private Homepage homepage;

        [Given(@"I have the weather app open")]
        public void GivenIHaveTheWeatherAppOpen()
        {
            homepage = new Homepage();
        }
        
        [When(@"I choose (.*) city")]
        public void WhenIChooseACity(string cityName)
        {
            homepage.EnterCityName(cityName);
        }
        
        [Then(@"I recieve a (.*) day forecast")]
        public void ThenIRecieveADayForecast(int noOfDays)
        {
            Assert.AreEqual(noOfDays, homepage.checkNumberOfDailyOverviews(),  "Incorrect number of days displayed");
            Assert.AreEqual("Tue", homepage.getDayName("Tuesday"), "Incorrect day name displayed");
            Assert.AreEqual("Wed", homepage.getDayName( "Wednesday"), "Incorrect day name displayed");
            Assert.AreEqual("Thu", homepage.getDayName("Thursday"), "Incorrect day name displayed");
            Assert.AreEqual("Fri", homepage.getDayName("Friday"), "Incorrect day name displayed");
            Assert.AreEqual("Sat", homepage.getDayName("Saturday"), "Incorrect day name displayed");
        }

        [When(@"select (.*)")]
        public void WhenSelectDay(string day)
        {
            homepage.displayThreeHourlyUpdatesForDay(day);
        }

        [Then(@"I recieve (.*) Three hourly weather updates")]
        public void ThenIRecieveThreeHourlyWeatherUpdates(int noOfThreeHourlyWeatherUpdates)
        {
            Assert.AreEqual(noOfThreeHourlyWeatherUpdates, homepage.getNumberOfThreeHourlyWeatherUpdates(), "Incorrect number of three hourly weather updates displayed");
        }

        [Then(@"the three hourly updates are visible")]
        public void ThenTheThreeHourlyUpdatesAreVisible()
        {
            Assert.IsTrue(homepage.isThreeHourelyUpdatesVisible(), "Three hourly updates should be visible ");
        }


        [Then(@"the three hourly updates are not visible")]
        public void ThenTheThreeHourlyUpdatesAreNotVisible()
        {
            Assert.IsFalse(homepage.isThreeHourelyUpdatesNotVisible(), "Three hourly updates should not be visible ");
        }

        [When(@"on (.*) at (.*)")]
        public void WhenOnDayAtTime(string day, string time)
        {
            homepage.setDayTime(day, time);
        }

        [Then(@"the weather condition will be (.*)")]
        public void ThenTheWeatherConditionWillBe(string weathercondition)
        {
            Assert.AreEqual(weathercondition, homepage.getWeatherConditions(), "Incorrect Weather Conditions displayed");
        }

        [Then(@"the max temp will be (.*)")]
        public void ThenTheMaxTempWillBe(string temperature)
        {
            Assert.AreEqual(temperature, homepage.getMaxTemp(), "Incorrect Max temp displayed");
        }

        [Then(@"the min temp will be (.*)")]
        public void ThenTheMinTempWillBeDegrees(string temperature)
        {
            Assert.AreEqual(temperature, homepage.getMinTemp(), "Incorrect Min temp displayed");
        }

        [Then(@"the windspeed will be (.*)")]
        public void ThenTheWindspeedWillBe(string windSpeed)
        {
            Assert.AreEqual(windSpeed, homepage.getWindSpeed(), "Incorrect windspeed displayed");
        }

        [Then(@"the wind direction will be (.*)")]
        public void ThenTheWindDirectionWillBe(string windDirection)
        {
            Assert.AreEqual(windDirection, homepage.getWindDirection(), "Incorrect wind direction displayed");
        }

        [Then(@"the average rainfall (.*)")]
        public void ThenTheAverageRainfall(string rainfall)
        {
            Assert.AreEqual(rainfall, homepage.getRainfall(), "Incorrect rainfall displayed");
        }

        [Then(@"the average pressure will be (.*)")]
        public void ThenTheAveragePressureWillBe(string pressure)
        {
            Assert.AreEqual(pressure, homepage.getPressure(), "Incorrect Pressure displayed");
        }

        [Then(@"the daily forecast will summerise the three hourly data for rainfall")]
        public void ThenTheDailyForecastWillSummeriseTheThreeHourlyDataForRainfall()
        {
            Assert.IsTrue(homepage.DailyForecastRainfallIsCorrect(), "The Daily forecasted rainfall is not an average of the 3 hourly updates");
        }

        [Then(@"the daily forecast Max and Min temperatures will summerise the three hourly data temperatures")]
        public void ThenTheDailyForecastMaxAndMinTemperaturesWillBeSummeriseTheThreeHourlyDataTemperatures()
        {
            Assert.IsTrue(homepage.DailyForecastMaxTemperatureIsCorrect(), "The Daily forecasted max temperature is not the greatest 3 hourly temperature");
            Assert.IsTrue(homepage.DailyForecastMinTemperatureIsCorrect(), "The Daily forecasted min temperature is not the lowest 3 hourly temperature");
        }

        [Then(@"the daily forecast weather conditions will summerise the three hourly weather conditions")]
        public void ThenTheDailyForecastWeatherConditionsWillSummeriseTheThreeHourlyWeatherConditions()
        {
            Assert.IsTrue(homepage.DailyForecastWeatherConditionsIsCorrect(), "The Daily forecast weather conditions arent the dominant 3 hourly weather conditions");
        }

        [Then(@"the daily forecast wind conditions will summerise the three hourly wind conditions")]
        public void ThenTheDailyForecastWindConditionsWillSummeriseTheThreeHourlyWindConditions()
        {
            Assert.IsTrue(homepage.DailyForecastWindSpeedIsCorrect(), "The Daily forecast wind speed isnt the dominant 3 hourly wind speed"); 
            Assert.IsTrue(homepage.DailyForecastWindDirectionIsCorrect(), "The Daily forecast wind direction isnt the dominant 3 hourly wind direction");
        }










        //[When(@"I read the file")]
        //public void WhenIReadTheFile()
        //{
        //    homepage.test();
        //}






        [AfterScenario]
        public void testCleanUp()
        {
            homepage.dispose();
        }
    }
}
