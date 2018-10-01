using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace WeatherApp.Framework
{
    static class WaitForElement
    {
        public static void WaitForElementToDisplay(Driver driver, IWebElement element, TimeSpan timeToWait)
        {
            var wait = new WebDriverWait(driver.instance, TimeSpan.FromSeconds(2));
            wait.Until(myDriver => element.Displayed);
        }

        public static void WaitForElementToNotDisplay(Driver driver, IWebElement element, TimeSpan timeToWait)
        {
            var wait = new WebDriverWait(driver.instance, TimeSpan.FromSeconds(2));
            wait.Until(myDriver => !element.Displayed);
        }
    }
}
