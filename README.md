#  Microsoft Teams App Platform - Demo Apps
Along with enhancements around our extensibility platform, we are simultaneously targeting efforts to integrate our customer’s Line of Business (LoB) apps in Microsoft Teams. To realize the full potential of the platform, we have built a set of industry specific LoB apps to highlight the platform capabilities and provide a skeletal working code. Look at what these apps can do:


## Airlines

For the Airline industry, here are a collection of five bots that are crucial for everyday airline operations:

1. **Flight Information Bot**<br>
    Flight Information Bot helps employees in searching alternative flight schedules their availability between locations and rebooking a passenger to another flight.    

2. **Passenger Information Bot**<br>
    This bot allows you to search for passengers using a name, PNR number, zone, or seat number. It can also help identify passengers with specific considerations, such as those who need special assistance  , as well as loyalty program members.

3. **Baggage Information Bot**<br>
    This bot can help you track the status of baggage, as well as report missing baggage. Additionally, Airline employees can report missing baggage and rebook them   onto another flight in case the passenger is bumped off or missed the flight.

4. **Flight Teams Creation Bot**<br>
    It’s as simple as uploading an Excel file with a list of members who need to be turned into a team – the bot does the dynamic team creation for you!

5. **Fleet Information Bot**<br>
    This bot allows you to search for aircrafts at a given base location from the fleet owned by the airlines. You can also assign an aircraft for a specific flight or mark it as grounded for maintenance.<br>

## Cross-industry Applications 

These are applications that can be used by employees, regardless of industry:

1. **Notification Bot**<br>
    The Notification Bot will let the admin notify specific members on Teams across several functions. Examples include sending out notifications about weather changes (i.e Airline operations), operational delays in a manufacturing set up, and notifications for company events. 

2. **Employee Survey / Polling bot**<br>
    The bot lets the admin create and publish a survey to specific members within an organization. It also enables sending out reminders to select users who have not completed the survey. The survey results will be available for download in the form of a .csv file that can be used for reporting purposes. 

3. **Company Communicator**<br>
    Reach your employees where they collaborate! The app arms your Microsoft Teams environment with corporate communication and group broadcast capabilities. Get messages out to an employee or a set of employees in one or more team channels or individually. The creators can define user groups, compose a rich message, send or schedule messages, deliver to one or more employees, track read acknowledgement and view engagement analytics for each message sent seamlessly in a single app.

## Manufacturing

1. **Inventory Bot**<br>
    The inventory bot provides a view into available inventory across multiple products and locations. Additionally, it lets users view, block, and request for new inventory as needed.

## Professional Services 

1. **Employee Leave App**<br>
    The app has an employee and manager workflow with bot and tab capabilities. It enables an employee to raise a new vacation request, check leave balance status and public holidays. In addition, when an employee raises a new vacation request, the manager gets a notification and the approval / reject workflow can be completed from within the app on Teams.  

## Experience the Apps in your Microsoft Teams client

All these samples are deployed on Microsoft Azure and you can try it yourself by uploading respective app packages (.zip files links below) to one of your teams and/or as a personal app. (Sideloading must be enabled for your tenant; see [step 6 here](https://docs.microsoft.com/en-us/microsoftteams/platform/get-started/get-started-tenant#turn-on-microsoft-teams-for-your-organization).) These apps are running on the free Azure tier, so it may take a while to load (warm up) if you haven't used it recently and it goes back to sleep quickly if it's not being used. Rest assured, once it's loaded it's pretty snappy.

**Note** Database Cleanup service cleans up user details from the database every Monday for the below apps. Hence user details should be provided every time to use the app after the database Cleanup.

[Flight Info Bot](Airline/FlightInfo/Manifest/Flight%20Info%20App%20Manifest.zip)<br>
[Passenger Info Bot](Airline/PassengerInfo/Manifest/Passenger%20Information%20App%20Manifest.zip)<br>
[Baggage Info Bot](Airline/BaggageInfo/Manifest/Baggage%20Info%20App%20Manifest.zip)<br>
[Flight Team Creation Bot](Airline/FlightTeamCreation/Manifest/Flight%20Team%20Creation%20App%20Manifest.zip)<br>
[Fleet Bot](Airline/FleetInfoBot/Manifest/FleetInfo%20App%20Manifest.zip)<br>
[Cabin Crew Roaster Bot](Airline/XAirlines/Manifest/Airlines%20Manifest.zip)<br>
[Notification Bot](Cross%20Vertical/NotificationBot/Manifest/Notification%20App.zip)<br>
[Employee survey / Polling bot](Cross%20Vertical/PollingBot/Manifest/Polling%20App%20Manifest.zip)<br>
[Inventory Bot](Manufacturing/InventoryBot/Manifest/Inventory%20App%20Manifest.zip)<br>
[Employee Leave](Professional%20Services/LeaveBot/Manifest/Leave%20App%20Manifest.zip)<br>
[Company Communicator](Cross%20Vertical/CompanyCommunicator/Manifest/Company%20Communicator%20App%20Manifest.zip)
<br>
# Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
