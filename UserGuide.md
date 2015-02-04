### **In-Touch Insight Systems Inc.**
#### *Developer technical interview*
-------------------------------

**Option 2**: Proof-Of-Concept Dashboard
<br>*By:Andrew Langlais*

##User Guide

### Description

This application provides a web-based user interface to query and analyze Olympic medal winnings data for all athletes from 2000 to 2012. 

### Usage Requirements
* Modern Web Browser (eg. [Google Chrome](http://www.google.com/chrome/ "Google Chrome"), [Mozilla FireFox](https://www.mozilla.org/en-US/firefox/desktop/ "Mozilla FireFox")).
* Network or web access.

----------

### Overview
Currently this application resides on a single web page. This single web page consists of two charts, two data tables and a drop down menu. 

#### Country Data Table
The first data table, **the country table**, drives the application. It provides the Olympic Medal counts by Country for a specified year.

* *The year may be specified using the drop-down in the upper left-hand corner of the web page.*
* *Columns may be sorted by clicking the desired column headings*
* *Table page sizes may be adjusted using the "Countries Per page" drop down menu above the table.*

#### Athlete Data Table
The second data table, **the athlete table**, provides the athletes for the country selected within the country data table.
	
* *This table will update as the country table selection changes.*

#### Charts 
The charts above the country data table provide graphical representations of the Olympic Medals for the country selected within the country data table.

* The **Bar Chart** shows the medal breakdown *(eg. Gold,Silver,Bronze)* against the Total medal count.

* The **Pie Chart** shows the medal distribution by sport *(eg. Badminton, Fencing, Athletics)*. 

----------

### Missing Features

* Upon selection items in the charts (eg. Medal breakdown bar chart, Medal distribution pie chart) the athletes table could filter based upon chart selection.
	* *eg. If the "Gold" bar of the bar chart is selected, all the athletes who won gold for the current country should be shown in the athletes table.*
	*  *eg. If the "Badminton" pie slice is selected in the pie chart, all the athletes who won in badminton for the current the country should be shown.*
* A drill-down versus stacked approach may have been better to represent the data. 
* The application could use more charts and data perspectives.
* Columns filters and search features could have been provided for the tabular data.

### Known Issues
* Athletes table should be paged and sorted in the same fashion as the country table.
* The page number indicator is displayed for the country table although it's not required.
* Users may page past the end of the country data set.






