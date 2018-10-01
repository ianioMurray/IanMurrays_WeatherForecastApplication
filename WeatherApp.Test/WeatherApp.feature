Feature: WeatherApp
	In order to plan my trips to different cities  
	As a person who is concerned about the weather
	I want to know what the weather will be like in the city I am visiting

Background: 
	Given I have the weather app open

Scenario Outline: Enter city name, get 5 day weather forecast 
	When I choose <city> city 
	Then I recieve a 5 day forecast

	Examples: 
	| city      |
	| Edinburgh |
	| Glasgow   |
	| Stirling  |
	| Dundee    |
	| Aberdeen  |
	| Perth     |
	

Scenario Outline: Select day, get 3 hourly forecast 
	When I choose <city> city 
		And select <nameOfDay>
	Then I recieve <NoOfThreeHourlyReports> Three hourly weather updates
		And the three hourly updates are visible

	Examples: 
	| city      | nameOfDay | NoOfThreeHourlyReports |
	| Glasgow   | Friday    | 8                      | 
	| Dundee    | Tuesday   | 4                      | 
	| Perth     | Saturday  | 8                      |
	| Edinburgh | Wednesday | 8                      | 
	| Aberdeen  | Tuesday   | 4                      | 
	| Stirling  | Thursday  | 8                      | 

Scenario Outline: Select day again, hide 3 hourly forecast 
	When I choose <city> city 
		And select <nameOfDay> 
		And select <nameOfDay> 
	Then the three hourly updates are not visible

	Examples: 
	| city      | nameOfDay |
	| Aberdeen  | Tuesday   |
	| Glasgow   | Wednesday |
	| Edinburgh | Saturday  |


Scenario Outline: Daily forecast should summarise the 3 hour data
					 Most dominant (or current) condition
					 Most dominant (or current) wind speed and direction
					 Aggregate rainfall
					 Minimum and maximum temperatures
	When I choose <city> city 
		And on <day> at overview
	Then the daily forecast will summerise the three hourly data for rainfall 
	    And the daily forecast Max and Min temperatures will summerise the three hourly data temperatures
		And the daily forecast weather conditions will summerise the three hourly weather conditions  
		And the daily forecast wind conditions will summerise the three hourly wind conditions 

	Examples: 
	| city      | day       |
	| Aberdeen  | Thursday  | # correctly fails - weather conditions not dominant     
	| Glasgow   | Tuesday   | # correctly fails - weather conditions not dominant 
	| Edinburgh | Friday    | # correctly fails - wind speed not dominant     
	| Dundee    | Saturday  |
	| Stirling  | Wednesday | # correctly fails - weather conditions not dominant 
	| Perth     | Tuesday   |
	| Edinburgh | Thursday  | # correctly fails - wind direction not dominant 
	| Edinburgh | Wednesday | # correctly fails - weather conditions not dominant 
	| Aberdeen  | Tuesday   |


Scenario Outline: Daily Overview/Three hourly Weather correct for a city
				     All values should be rounded down
	When I choose <city> city
	  And on <day> at <time>
	Then the weather condition will be <weatherCondition>
	  And the max temp will be <maxTemp>
	  And the min temp will be <minTemp>
	  And the windspeed will be <windspeed>
	  And the wind direction will be <windDirection>
	  And the average rainfall <rainfall> 
	  And the average pressure will be <pressure>

	  Examples: 
	  | city      | day       | time     | weatherCondition | maxTemp | minTemp | windspeed | windDirection | rainfall | pressure |
	  | Stirling  | Tuesday   | overview | Rain             | 12°     | 8°      | 1kph      | 187           | 3mm      | 992mb    | 
	  | Stirling  | Wednesday | 1900     | Rain             | 11°     | 11°     | 2kph      | 180           | 1mm      | 986mb    | 
	  | Glasgow   | Tuesday   | 1600     | Clouds           | 16°     | 14°     | 3kph      | 221           | 0mm      | 1014mb   |
	  | Edinburgh | Tuesday   | 1300     | Clouds           | 18°     | 14°     | 1kph      | 226           | 0mm      | 1008mb   |
	  | Aberdeen  | Saturday  | 0100     | Rain             | 13°     | 13°     | 10kph     | 199           | 0mm      | 1001mb   |
	  | Dundee    | Friday    | overview | Clear            | 14°     | 9°      | 7kph      | 241           | 2mm      | 1015mb   | 
	  | Perth     | Thursday  | 1000	 | Rain             | 12°     | 12°     | 3kph      | 153           | 0mm      | 1001mb   | 

	
# Stirling - Wednesday 1900 - weather condition = rain
#							- min and max temp rounded down from 11.64
#                           - windspeed rounded down from 2.66
#                           - wind direction round down from 180.001
#                           - rain round down from 1.46                         -- FAILS
#                           - pressure rounded down from 986.04

# Glasgow - Tuesday 1600    - weather condition = clouds 
#							- min and max temp rounded down from 14.04 and 16.03
#                           - windspeed rounded down from 3.42
#                           - wind direction round down from 221.02
#                           - rain 0
#                           - pressure rounded down from 1014.68

# Edinburgh - Tuesday 1300  - weather condition = clouds 
#							- min and max temp rounded down from 14.87 and 18.4
#                           - windspeed rounded down from 1.86
#                           - wind direction round down from 226
#                           - rain 0
#                           - pressure rounded down from 1008.47

# Aberdeen - Saturday 0100  - weather condition = rain 
#							- min and max temp rounded down from 13.97 and 13.97
#                           - windspeed rounded down from 10.7
#                           - wind direction round down from 199.501
#                           - rain round up from 0.72                          -- FAILS          
#                           - pressure rounded down from 1001.39

# Perth - Thursday 1000     - weather condition = rain    
#							- min and max temp rounded down from 12.66 and 12.66
#                           - windspeed rounded down from 3.86
#                           - wind direction round down from 153.502
#                           - rain round up from 0.68                          -- FAILS   
#                           - pressure rounded down from 1001.76