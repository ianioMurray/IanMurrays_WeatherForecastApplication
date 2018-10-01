5 day weather forecast application test


The solution I have provided was written using Selenium, SpecFlow and C# 2017.
   
To run the tests open the solution file "WeatherApp.sln" in Visual Studio.  Ensure the test site is running (localhost:3000). Select the 'Test' dropdown and choose 'Run'->'All Tests'

This will run all tests (there are 31 tests in all - 22 pass and 9 fail )  


- IMPLEMENTED TESTS (requirement is identified by the 'Scenario Outline')

all tests are run using a number of sets of cities/days/times - see WeatherApp.feature to see the data tables used by each test

* Scenario Outline: Enter city name, get 5 day weather forecast 
	Given I have the weather app open
	When I choose <city> city 
	Then I recieve a 5 day forecast

* Scenario Outline: Select day, get 3 hourly forecast 
	Given I have the weather app open
	When I choose <city> city 
		And select <nameOfDay>
	Then I recieve <NoOfThreeHourlyReports> Three hourly weather updates
		And the three hourly updates are visible

-- Edgecase - the current day has been checked to ensure that only 4 rather than 8 three hourly updates were returned  

* Scenario Outline: Select day again, hide 3 hourly forecast 
	Given I have the weather app open
	When I choose <city> city 
		And select <nameOfDay> 
		And select <nameOfDay> 
	Then the three hourly updates are not visible


* Scenario Outline: Daily forecast should summarise the 3 hour data
					 Most dominant (or current) condition
					 Most dominant (or current) wind speed and direction
					 Aggregate rainfall
					 Minimum and maximum temperatures
	Given I have the weather app open
	When I choose <city> city 
		And on <day> at overview
	Then the daily forecast will summerise the three hourly data for rainfall 
	    	And the daily forecast Max and Min temperatures will summerise the three hourly data temperatures
		And the daily forecast weather conditions will summerise the three hourly weather conditions  
		And the daily forecast wind conditions will summerise the three hourly wind conditions 

-- Assumption - summary of weather conditions, wind speed & wind direction - the specification say 'Most dominant (or current)' which I took to mean the most often recurring entry but where there are more than 1 entry that 
recurs the most times, select the earliest occuring entry i.e. if we are looking at wind speed and the three hourly updates are ( 5, 3, 6, 7, 3, 6) then there are 2 most common recurring entries 3 and 6 but the earliest 
is 3 so return 3 as the result.  
-- Assumption - summary of Max and Min temperatures - I took max temperature summary to mean the greatest three hourly max temperature.  I took min temperature to mean the lowest three hourly min tempature. 
-- Assumption - summary of rainfall - I took rainfall summary to be the total of all hourly rainfall values. 
-- Results - based on the assumptions I have made 6 tests fail due to incorrect values being returned (for failure reasons see comments next to the tests in WeatherApp.feature)

* Scenario Outline: Daily Overview/Three hourly Weather correct for a city
				     All values should be rounded down
	Given I have the weather app open
	When I choose <city> city
	  And on <day> at <time>
	Then the weather condition will be <weatherCondition>
	  And the max temp will be <maxTemp>
	  And the min temp will be <minTemp>
	  And the windspeed will be <windspeed>
	  And the wind direction will be <windDirection>
	  And the average rainfall <rainfall> 
	  And the average pressure will be <pressure>

-- Assumption - it is assumed that manually finding values that required rounding in the JSON files and then checking they appear as the correct value is good enough.  I would have like to implement a JSON file reader
 to read the contents from the files and then compare based on the returned results but I did this manually to save time.
-- Results - 3 tests fail due to rainfall being rounded up not down


- ADDITIONAL TESTS (I would have liked to implement if I have the time) 

* Scenario Outline: Enter an invalid city, no 5 day weather forecast displayed 
	Given I have the weather app open
	When I choose <invalidCity> city    # include empty string in test
	Then I recieve a 0 day forecast
	   And a warning message is displayed

* Scenario Outline: Enter an city and then change it to a different valid city, get 5 day weather forecast displayed for the current city
	Given I have the weather app open
	When I choose <city> city 
           And I choose <city> city
	Then I recieve a 5 day forecast
	  And the weather condition will be <weatherCondition>
	  And the max temp will be <maxTemp>
	  And the min temp will be <minTemp>
	  And the windspeed will be <windspeed>
	  And the wind direction will be <windDirection>
	  And the average rainfall <rainfall> 
	  And the average pressure will be <pressure>

* Scenario Outline: Enter an city and then change it to a invalid city, no 5 day weather forecast displayed
	Given I have the weather app open
	When I choose <validCity> city 
           And I choose <invalidCity> city
	Then I recieve a 0 day forecast
	   And a warning message is displayed

Additional test - no test was done to compare the daily overview pressure value to the 3 hourly updates
Additional test - no test was done to look at what happens if the current days overview was openned and the time was after the last 3 hourly update i.e. after 22:00 
Additional test - no test was done to look at what happens when a city is selected and the date would mean the 5 day forecast would include days from a new month or new year i.e. current date was 
28th Feb (especially on a leap year) or 31st Dec
Additional test - no test was done to look at what happens if this app was run in a different time zone
Additional test - no test was done to look at what happens for localization issues (i.e. countries that use Fahrenheit, will the site work with different languages etc)
Additional test - no non funcitional tests were looked at (i.e. load testing, Performance testing etc)
 




