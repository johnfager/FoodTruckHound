# Food Truck Hound Documentation

This application has a simple API endpoint designed to search San Fransisco food trucks by location. It demonstrates some basic architectural principals, separation of concernes, dependcy injection / IOC, logging, and a basic project setup.

This code is not meant to be comprehensive, but rather a starting point of basic features.  Additions of a security layer, authentication, unit tests, a companion responsive front-end UI that automatically gets the user's location, and dynamic maps could all be considered when extending the application.

This application has been deployed to Azure and can be accessed here:
**[https://foodtruckhound.azurewebsites.net/sniff](https://foodtruckhound.azurewebsites.net/sniff)**

Parameters are:
**?latitude={{latitude}}&longitude={{longitude}}**

A few example URLs:
https://foodtruckhound.azurewebsites.net/sniff?latitude=37.7568774515357&longitude=-122.418579889476
https://foodtruckhound.azurewebsites.net/sniff?latitude=37.786904919457&longitude=-122.390920262962
https://foodtruckhound.azurewebsites.net/sniff?latitude=37.7892489084137&longitude=-122.398892959346
https://foodtruckhound.azurewebsites.net/sniff?latitude=37.8045778690901&longitude=-122.433010774343
https://foodtruckhound.azurewebsites.net/sniff?latitude=37.7272340074959&longitude=-122.38181551187
https://foodtruckhound.azurewebsites.net/sniff?latitude=37.795051430826&longitude=-122.446134422674
https://foodtruckhound.azurewebsites.net/sniff?latitude=37.726470716246&longitude=-122.381541219158
https://foodtruckhound.azurewebsites.net/sniff?latitude=37.7908384194517&longitude=-122.401160370299
https://foodtruckhound.azurewebsites.net/sniff?latitude=37.7936733815047&longitude=-122.398463509486
https://foodtruckhound.azurewebsites.net/sniff?latitude=37.7862367744654&longitude=-122.389066205605
https://foodtruckhound.azurewebsites.net/sniff?latitude=37.7875651653788&longitude=-122.40959283348
https://foodtruckhound.azurewebsites.net/sniff?latitude=37.7685570713121&longitude=-122.409610236298

## Solution Organization

The code is organized into separate projects with references that help keep the separation of concerns intact.

### FoodTruckHound.Models

This is the lowest level project that contains POCO models to represent and hold data. This is isolated so that it can be referenced by others more easily. An example would be a client application that consumes the API which could share the models but would have no need of any of the other projects.

### FoodTruckHound.Core

The core project contains interfaces that define services and repositories for the solution. This project carefully ensures that no implementation code is present to help support successful DI and IOC patterns.

#### Services
Services are bound with an IService to help programatically work with service interfaces and implementations. Programatic DI registrations are possible and this inheritence helps identify immediately that something is a service. Services can potentially sync and transform data from multiple sources. Services can also perform calculations and more complex routines against larger datasets that go beyond simple CRUD operations.

#### Repositories
Repositories are a storage medium bound to an IRepository. Programatic DI registrations are possible and this inheritence helps identify immediately that something is a repository. They are responsible for the CRUD operations in this pattern.  There are more rigid data repository patterns that would extend query capabilities and homogenize CRUD operations on a collection using complex state tracking but that level of complexity is not always necessary. 

### FoodTruckHound.Data

The data project handles the implementation code for this solution. The task was to work with the San Fransisco CSV dataset for Food Trucks ([located here](https://data.sfgov.org/Economy-and-Community/Mobile-Food-Facility-Permit/rqzj-sfat/data) and there is an endpoint with a [CSV dump of the latest data here](https://data.sfgov.org/api/views/rqzj-sfat/rows.csv).

The implementation solves several problems using different classes.

#### _baseService and _baseRepository
As our project grows and adds features, we want to keep things DRY (don't repeat yourself). Starting with a base abstract class for these elements gives us a common way to handle logging in this example. Other functions can be added that would be common to multiple repositories.  This could include per function + parameter level caching, common cache invalidation, extra authorization checks to ensure multiple levels of security are some examples of code that could be added and shared across various service or repository implementations.

#### SfGovMobileFoodScheduleService
This service handles the remote CSV data. It aquires it from the remote endpoint and transforms the CSV into our desired model. It implements some basic logging and logic checks.
- In a more complete application, integrity tests and various expectations could be added to the application to detect and notify administrators of schema changes, timeouts, an usually small or large amount of data.  
- It's important to ensure that general expectations are being met when dealing with a third-party data source. These features would need to be balanced against other requirements based on business value and risk.
- We saved time handling the parsing using a Nuget package. There are heavier pacakages for parsing CSV but some research confirmed that this package could work for this basic CSV task. More complex parsing and data validation is always possible based on the value to the application.
- Uses the FoodTruckCsvDtoMapping class to map data from the CSV to the POCO model.

#### FoodTruckLookupInMemoryRepository
This repository isn't fully necessary for this project but was included as an example pattern. Storing a copy of the remote data in some medium, whether SQL or NoSQL is an appropriate choice. If the city's data is offline, our copy is still available and the application will not be tied to a third-party system's uptime.

In this example, the repository is used to activate the IFoodTruckDataService to freshen data. It's not always a perfect pattern to have a repository kick off a service, but it was efficient and simple for our purposes. Other patterns for refreshing and syncing data are possible that would not be tied to the repository's implementation that could have benefits. 

#### FoodTruckSomeOtherStoreRepository
This is an unimplemented repository to show that the data storage medium can be implemented differently. Also, I love the pattern while coding of thowing exceptions that make sense to the developer when they start programming the next day. The **NotImplementedException** with appropritate informaion can jog the memory. The use of "TODO:" patterns integrate with the IDE help organize lists and make tasks easier to work through.

#### FoodTruckSpatialService
This service is responsible for taking in the latitude and longitude of a request and finding the closest distance. It is solely focused finding nearby food trucks.

- The **GeoCoordinate** type and its method **GetDistanceTo** is not available in .NET Core. When complex but common operations need to be done, community sourced solutions such as the one from StackOverflow can be a timesaver. Placing a **NOTE:** in the code with the link from the reseach and comments gives credit and adds transparancy. They also help other devs go back and review the source if something turns up later as a bug or a new pattern is being considered. 
- In testing the code we used from the StackOverflow post, coordinates that were very close were returning a NaN result. It's an important lesson never to just accept code as correct. Unit tests, code reviews, and manually running an application are all important practices. The implemented solution of checking for a NaN result and using 0 as the distance in miles might not be a perfectly correct solution, but given budget and risk tolerance worked.

### FoodTruckHound.Api
This is our API project. 
- Wire up of .NET Core API program
- DI configuration
- Settings files (can be differentiated by envioronment)
- A single controller with a single GET method
    - The GET method was selected to allow for a simple querystring and URL can be sent as a hyperlink.

## Taking the application further

Several items that I would generally include as a foundation are not present in this proof of concept solution.

### Exceptions, Logging and Feedback in Responses
- Proper REST based status codes with a common structure to the error messages such as Error Detail
- Custom exceptions that automatically translate to a defined pattern of 4xx or 5xx error responses with sensible information for developers and users.
    - Potential log correlation ID's for debugging with support
- Error handling filters to translate standard exceptions as a pattern that minimize try/catch and developers coding around errors.
- Logging solution that handles complex logic on notifying administrators when a problem is going on with appropriate emails or texts (including safeguards not allow flooding of messages or logs)
- API Authorization pipelines or use of an application gateway to handle rate limits as necessary
- Test coverage to verify that expected exceptions occur under certain circumstances

### More user features
- Feedback on a food truck (thumbs up or down, or rating)
    - Optionally search for most popular food trucks only
- Schedule information and only returning currently open food trucks 
- Search for specific food items and keywords