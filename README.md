# Hololens

# Setup instructions (starting from scratch):

git clone https://github.com/Team-ART-Gemstone/Hololens.git

Use HTTPS unless you have an SSH key set up already.

Open folder / project "integrate2" using Unity 2017.4.x (Unity Hub recommended for this)

Open specific scene (abcdefg for now)

# Build instructions

## EDITING ONE OF THE CLASS LIBRARIES

When you edit one of the class libraries. Either The Cognitive Helper or the Spell Checker, you have to rebuild the project.
1) Open the library you updated
2) At the top menu go to Debug -> Properties. Then on the left you should be at build. In that place find output path at the bottom
3) Click Browse and select the path to the project's asset folder (Should be Documents/ART/Integrate3/Assets/Plugins ...), and it should generate some files. If the files were there before, they should have been overwitten but you can delete them to be safe
4) Build the class library project (Not the unity project)
5) In unity, the project should have reloaded. Find the place where you saved the generated files and gind the dll file (The puzzle piece)
6) Select it and to the right in select platforms make sure only WSAPlayer is selected. In platform settings make UWP the SDK. Check the box that says Don't process and apply. Follow steps on Common build. Don't delete old app folder if you dont have to.
## FRESH BUILD

Menu to Edit -> Project Settings -> Player. Under Publishing Settings make sure that InternetClient, InternetClientServer, WebCam, Microphone, and SpatialPerception are all enabled. Under XR Settings make sure Virtual Reality Supported is enabled and add Windows Mixed Reality under Virtual Reality SDKs if not there.

Menu to File -> Build Settings. Make sure the main scene is included in the build. Set Platform to Universal Windows Platform and Target Device to HoloLens.

Build to "App" folder.

Then follow Common Build Steps below.

## Common Build Steps

Open the .sln file created in App using Visual Studio. Menu to Project -> Manage NuGet Packages.

In in installed tab, select Windows UWP and update to version 6.1.7. Do this for both assembly and project
In the browse/find tab, look up cognitive services and find Azure.CognitiveServices.Vision. Add the 3.2.0 version to both assembly and the project

On thr right side you will see the solutions manager. Open it up until you find refrences. Right click -> add Reference -> Browse.
In the box that opens up there should be a file called Newtonsoft.Json. Make sure it is version 10.1 (10.0.1???). Copy this file.

Open up the actual app folder again in windows and search "newtonsoft". Now you have to manually go into each file separately and inspect the version of newtonsoft. If it is not 10.1 (10.0.1???) or 10.3 replace it with the version you just copied. Do not replace any of the pdp or pll (forgot) files. Only newtonsoft.json

Set the build toolbar to Release / x86 / Remote Machine. When prompted enter IPv4 address of HoloLens (obtained on device through Settings -> Network near the bottom of the page). If not prompted menu to Debug -> (PROJECT NAME) Properties and overwrite the IP address stored with that of the HoloLens. Press green arrow and wait for deployment.

You may need to get the latest version of Newtonsoft and search and replace all instances of Newtonsoft in the project for it to build correctly

## Minor Changes

If you make a change in unity and want to rebuild the project, we want to AVOID deleting the old solution. So insted of deleting the app folder and going through the entire process again, just build in unity like you normally would, and it will overide the only solution. You still have to go through the process with newtonsoft again. If you are getting build errors and think deleting the old solution will help, that is fine, you just have to go through the entire build process again.
