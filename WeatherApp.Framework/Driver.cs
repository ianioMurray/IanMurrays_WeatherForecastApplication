using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WeatherApp.Framework
{
    public class Driver
    {
        public IWebDriver instance;

        public Driver()
        {
            instance = new ChromeDriver();
            instance.Navigate().GoToUrl("localhost:3000");
            instance.Manage().Window.Maximize();
        }

        public void Dispose()
        {
            instance.Dispose();
        }
    }
}
