## Technical Details

### Setup

1. Locate a system running a modern version of Windows. (Windows 7, Windows 8). The application was developed on system with a Windows 7 64-bit OS. The installation procedures were confirmed with on a system Windows 8.1 32-built OS.
2. Download and install Microsoft Visual Studio 2013 Community Edition: http://www.visualstudio.com/en-us/news/vs2013-community-vs.aspx
3. Download and install MongoDB for your target Windows OS: https://www.mongodb.org/downloads. The typical installation is fine.
4. Create the following directories "C:/data/" and "C:/data/db"." C:/data/db" is where mongoDB will look to put your database once it has been created. You can always change the directory to something else but you must ensure you update your mongo db configuration. This is not the same directory as the mongo installation directory.
5. Open a Windows command console and navigate to your mongoDB installation directory. If you did not specify the directory during installation the likely default is "C:/<PROGRAM FILES>/MongoDB 2.X Standard".
6. Locate the bin/ directory found within your MongoDB directory and start the mongo database server by running *mongod*.
7. Open another Windows command console to the same bin/ directory within the Mongo directory and run the following command:
8. Download source from the following GitHub repository: https://github.com/drewan1984/intouch_insight
9. Open the windows command line console and navigate to your MongoDB installation directory. Navigate to the /bin directory within installation directory and run the following Command: **mongoimport --db oymdb --collection medal_winners --type csv --headerline --file <THE PATH of your medals.csv found within the project source root>**. This command will import the medals.csv file into your mongo database **oymdb** within the collection **medal_winners**. If any of these names are changed changes will also have to be made to the MedalStatServiceMongoDB class found within (MedalService\Services\Impl\MedalStatService.cs) of the solution source files.
10. Open Visual Studio 2013 Community Edition as Administrator. Right-Click Visual Studio shortcut and select "Run as Administrator".
11. From Visual Studio select "FILE -> OPEN -> Project/Solution" and locate the recently downloaded source files you download from GitHub. Once located select: **MedalService.sln**
12. Once the solution has opened in Visual Studio,  right-click the solution node "Solution 'MedalService'" from the Solution Explorer and select "Enable Nuget Package Restore."
13. Build the solution. While building the nuget package dependecies should automatically be downloaded and referenced by the projects needing them.


### Application Start Up
1. Build the application and run. This will start the self-hosting nancy web service.
2. Ensure the mongodb server, *mongod* is running (See Setup Step#6)
3. Load browser and navigate to http://localhost:3579

### Application Testing
* From within Visual Studio go the "TEST" menu and select "Run => All Tests"

### Technical Environment

* **Web Front-end:** JavaScript, HTML
* **Web Framework:** NancyFx (C#/.NET)
	* **Testing:** *NUnit (.NET)*
* **Data Storage:** MongoDB

### Known Issues

### Future Enhancements

### Developer Notes

