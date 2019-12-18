# BiksBudget - 3. Semester Project AAU
An application with the focus on saving money and minimizing food waste.
The website is hosted on: (https://biksbudget.azurewebsites.net/)
## Prerequisites
### MySql Setup
To run the web application, it is required to install MySql 8.0, which can be found on:
https://dev.mysql.com/downloads/installer/
- Select the MSI installer of your choice and click download.
- Select 'No thanks, just start my download' to start the download without an account.
- Run the installer when downloaded.

- At first page select 'Add' on the left side.
- The products to choose is:
                  Applications -> MySql Workbench 8.0 
                  Connector -> C# for MySql 8.0

- Click next and afterward execute.
- This directs you to the 'Choosing Setup Type' Where you have to select 'Developer default'
- The next two times and execute the installer.
- At Type and Networking, you click next.
- When you get to Accounts and Roles, the password used on the application is 'BiksBudget123'
- After this, you click next and execute the installer.
- Insert your username: Root and Password: 'BiksBudget123' to confirm the information.

### ConnectionSettings
The ConnectionSettings Class can be found in the project 'BBCollection' under DBConnection, in this class you will have to change the onlineDB and onlineAPI booleans to either true or false, depending on how you want the front end to connect to the backend.
There are 3 ways to make the web application's database work:
1. Use the online database by changing the ConnectSettings
2. Pre-Process the database by running the B3-BiksBudget.Exe file.
3. Import the Database with the attached CreateSqlTables.sql file.

### Pre-Processing
The front end requires a database to run correctly, one of the ways to do so, is running the b3-biksbudget.exe file in the project folder, this will start the gathering of products and recipes.
The default option will run all the gathering tools in the code that will make create and populate the database.
If you want to use the default options you will have to write (Y) at the first option encountered in the .exe file.
Otherwise, you will have to write a sequence of 1's or 0's, functioning as a way to toggle the different parts on and off.
The list of functions is as follows:
1. Getting products from the coop API.
2. Crawling Alletiderskogebog and generating recipes ingredient, and product refs. (Will take a long time)
3. Populate links to cheapest products to ingredients
4. Deletes the generated product tags. (should not be on by default
5. Autocorrect suggestions in product search
### Import Database
The last way is by importing the CreateSqlTable.sql file.
1. First, open the MySql Workbench 8.0 launcher.
2. Click on the "Local instance MySQL80' and insert the login information from 'MySql Setup'
3. In the toolbar click on 'Server' and select 'Data import'
4. In 'Import Options' select 'Import from Self-Contained File' and select the 'CreateSqlTable.sql' file
5. Click on 'Start Import' to execute the creation of the tables.
## Deployment of the Project
To run the application you will have to start up the project uncompiled. Afterward, you will have to right-click on 'Solution 'B3-BiksBudget' and select the 'Properties'. Then turn everything but 'Backend' and 'FrontEnd2' to 'None' and those two to 'Start'.
Then you can quite the properties window and click 'F5' to run the application. 
## Built with
The application is build with:
- C#
- HTML
- CSS
- Javascript
## Authors
* **Baldwin, Jamie** [Jamiejbp](https://github.com/Jamiejbp)
* **Baltzersen, Bjarke** [AirealAble](https://github.com/AirealAble)
* **Holt, Jeppe** [JeppeJonHolt](https://github.com/JeppeJonHolt)
* **Horo, Roni** [GreatestHoro](https://github.com/GreatestHoro)
* **Kusk, Mads** [KuskIV](https://github.com/KuskIV)
* **Rasmussen, Emilie** [emilierasmussen](https://github.com/emilierasmussen)
* **Samim, Milad** [ms99uni](https://github.com/ms99uni)
