# Hololens

# Setup instructions (starting from scratch):

git clone https://github.com/Team-ART-Gemstone/Hololens.git

Use HTTPS unless you have an SSH key set up already.

Open folder / project "integrate2" using Unity 2017.4.x (Unity Hub recommended for this)

Open specific scene (abcdefg for now)

# Build instructions

(need to verify w/ Abdul)

## FRESH BUILD

Menu to Edit -> Project Settings -> Player. Under Publishing Settings make sure that InternetClient, InternetClientServer, WebCam, Microphone, and SpatialPerception are all enabled. Under XR Settings make sure Virtual Reality Supported is enabled and add Windows Mixed Reality under Virtual Reality SDKs if not there.

Menu to File -> Build Settings. Make sure the main scene is included in the build. Set Platform to Universal Windows Platform and Target Device to HoloLens.

Build to "App" folder.

Then follow Common Build Steps below.

## Common Build Steps

Open the .sln file created in App using Visual Studio. Menu to Project -> Manage NuGet Packages.

In in installed tab, select Windows UWP and updat to version 6.1.7. Do this for both assembly and project
In the browse/find tap, look up conitive services and find Azure.ConitiveServices.Vision. Add the 3.2.0 version to both assembly and the project

On thr right side you will see the solutions manager. Open it up until you find refrences. Right click -> add Refrence -> Browse.
In the box that opens up there should be a file called Newtonsoft.Json. Make sure i is version 10.1. Copy this file.

Open uo the actual app folder again in windows and search "newtonsoft". Now you have to manually go into each file sepeatley and inspect the version of newtonsoft. If it is not 10.1 or 10.3 replace it with the version you just copied. Do not replace any of the pdp or pll (forgot) files. Only newtonsoft.json

Set the build toolbar to Release / x86 / Remote Machine. When prompted enter IPv4 address of HoloLens (obtained on device through Settings -> Network near the bottom of the page). Press green arrow and wait for deployment.

You may need to get the latest version of Newtonsoft and search and replace all instances of Newtonsoft in the project for it to build correctly

## Minor Changes

If you make a change in unity and want to rebuild the project, we want to AVOID deleting the old solution. So insted of deleting the app folder and going through the entire process again, just build in unity like you normally would, and it will overide teh only solution. You still have to go through the process with newtonsft agaiin. If you are getting build errors and think deleting the old solution will help, that is fine, you just have o go through the entire build process again.
