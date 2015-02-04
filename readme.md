## README 

### Setup

1. Locate a system running a modern version of Windows (Windows 7, 32/64bit, Windows 8, 32/64bit). 
2. Download and install [Microsoft Visual Studio 2013 Community Edition]( http://www.visualstudio.com/en-us/news/vs2013-community-vs.aspx)
3. Download and (*Typical*) install [MongoDB](https://www.mongodb.org/download) for your Windows system.
4. Get your MongoDB enivornment ready by creating the following directories: *"C:/data"* and *"C:/data/db"*. Once the MongoDB server is running this is where it expects the database files to be located.
5. Open a Windows command console and navigate to your MongoDB installation directory. (eg. *"C:/%ProgramFiles%/MongoDB 2.X Standard"*).
6. Locate the  *bin/* directory found within your MongoDB installation directory. Start the mongo database server by running *mongod.exe*.
7. Download the source code for the application from the following GitHub repository: https://github.com/drewan1984/intouch_insight. 
8. Open a new Windows command console and navigate to the same bin/ directory within the Mongo directory and run the following command: *"mongoimport --db oymdb --collection medal_winners --type csv --headerline --file %PROJECT_SOURCE_PATH%/medals.csv*".  Where **oymdb** is the name of the database. Where **medal_winners** is the name of a collection within the database. **%PROJECT_SOURCE_PATH% is the path where the application source files from github reside on the local system. And finally **medals.csv** which is the .csv file which is being imported into MongoDB.
9. Open Visual Studio 2013 Community Edition as Administrator. Right-Click Visual Studio shortcut and select "Run as Administrator".
10. From Visual Studio select *"FILE -> OPEN -> Project/Solution"* and locate the recently downloaded source files you download from GitHub. Once located select: *MedalService.sln*
11. Once the solution has opened in Visual Studio,  right-click the solution node "Solution 'MedalService'" from the Solution Explorer and select *Enable Nuget Package Restore*."
12. Build the solution. While building the nuget package dependencies should automatically be downloaded and referenced by the projects needing them.

### Application Start Up
1. Build and run the application within Visual Studio Community 2013. This will start the self-hosting nancy web service which will place a listen on TCP port 3579.
2. Ensure the mongodb server, *mongod* is running. (See Setup Step #6)
3. Load a web browser and navigate to *http://localhost:3579*

### Application Testing
* From within Visual Studio 2013 community go the "TEST" menu and select "Run => All Tests"

### Technical Environment

* **Web Front-end:** JavaScript, HTML
* **Web Framework:** NancyFx (C#/.NET)
	* **Testing:** *NUnit (.NET)*
* **Data Storage:** MongoDB

### Development Notes / Pitfalls / Known Issues
* The installation process should have been automated. Batch scripts may have been helpful.
* The MongoDB server should have been configured as a Windows service.
* The intent was to start development within windows and then migrate to Linux (Mono(.NET Runtime) + MonoDevelop(IDE)). This would have provided for easier automation scripting.
* The OlympicAthletes.xlsx was manually downloaded and then converted using Excel. The intent was to write a script and then use 'wget' to download .xlxs and then hopefully use an opensource tool or custom tool to convert from .xlsx to .csv. The .csv file would then be imported into mongodb using the mongoimport (linux equivalent) utility. 
* In hindsight I should have developed the Front-end with Silverlight instead of Javascript and HTML. I spent much of my time  googling syntax and the JavaScript library (eg. JQuery,Google Charts) apis. 
* Unfortunately I was only able to provide unit testing for my back-end data service interface. I did not have time to learn the Nancy Testing framework.
* Overall working on this application has been positve learning experience. 

