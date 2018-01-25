# AchillesProject
A project to develop an Achilles injury recovery tool for android devices.

# Startup
The following are the items you need to have or configure in order to make the project work.

## Requirements
..* You need nodejs. This can be found at https://nodejs.org/en/. Linux users should use their local repository.
..* You need git. This can be found at https://git-scm.com/. Linux users should have a form already, or can install it from their local packaging software
..* You need a version of Visual Studio, 2015 will work but 2017 is preferred. This can be found at https://www.visualstudio.com/
..* You need Visual Studio Code, the latest version can be found at https://code.visualstudio.com/download
..* You need the Visual Studio GitHub Extension, this will let you link your Visual Studio instance to your Github Account and pull down the repository. This can be found here, https://visualstudio.github.com/

## Setup Steps

Once all of the software has been downloaded and installed, you need to pull down an instance of the current master branch. Via Visual Studio,this can be done by finding the Team Explorer window and clicking the Clone Button. This will pull down the latest instance of the working project.

![](/Images/Capture1.png?raw=true)

The Team Explorer tab can be found by checking the view tab, on the menu bar. It'll be one of the first entries. Along this process, you maybe prompted to sign in order to get the repository, sign in using your GitHub account. Joshua, or another administrator will add you as contributers to the project so you can pull the repository. 

![](/Images/Capture2.png)

### Running the API
Now that you have the project. You need to open up the Dot Net Core solution, called Achilles. This can be accessed via the Team Explorer tab by double clicking the name just under the Github tab and selecting the solution. 

![](/Images/Capture3.png)

The final step for running the Dot Net Core section is to run the project as it itself, do not use IISExpress, and see if it loads into chrome. If it does, and displays a blank screen. Then you've got the API Working.

### Running the Front End
The front end is provided by ionic, or more specifically, by npm. To run this, open up Visual Studio Code and navigate to your clone of the overall project. Once there, go into the Achillies Project in Visual Studio Code. It should look similar to the below.

![](/Images/Capture4.png)

Once you are at the right level, you need to run the command `npm install` to install all of the npm packages required for the project. This will take a few minutes, so let it run and finish off.

Once all the packages are present, you then need to run the command `ionic serve` to load the project into a web browser. This will trigger a web browser to popup with the front end present. Should you encounter any errors along the way. Follow the instructions or message us via slack to help resolve them.

Congratulations, you now have a complete working install. To check that you can access the API correctly. Use your terminal and check for a 200 error when loading or reloading the front end. If you see that, you can communicate to the API correctly without trouble.

## Help

Help can be found via the slack.