Node Setup/Configuration:
--------------------------------
1. Update default capabilities based on the device to be connected on StartNodexxx.bat 
2. Each device should a separate StartNodexxx.bat and NodeConfigxxx.json config file.
3. Each NodeConfigxxx.json file must have capabilities defined as per needs along with the mandatory "DeviceGroup" parameter
4. "DeviceGroup" parameter must atleast contain platformName and device type string  separated by a ";". You can provide other keys as well if needed.
5. You can define as many capabilities you want with same/different "DeviceGroup" parameters to target different application types and configurations.
6. All the port numbers can be updated as needed in StartNodexxx.bad file as needed.
7. Copy over the sample config and powershell script files from here to your local or designated node machine. Do not modify and check in the sample values provided.
8. Ensure the following files are copied in the same folder and the batch files are run from the same location as well:
nodeConfigxxx.json
StartNodexxx.ps1

*where xxx- is a number 

Hub Setup/Configuration:
--------------------------------
1. Ensure capability matcher parameter is setup as below in the hubConfig.json file
"capabilityMatcher": "com.selenium.DeviceGroupCapabilityMatcher"
2. Ensure the following files are in the same folder and you run the powershell script from the same folder:
deviceGroupCapabilityMatcher.jar
selenium-server-standalone-3.9.1.jar
hubConfig.json
StartHub.ps1
3. Update Hub and Node config files with appropriate host/ip addresses when planning to connect to them remotely. By default its setup to run locally.
General Information
--------------------------------
1. You can have HUB and NODE(s) on the same / different machines.
2. You can check Hub/Node status by checking out or updating the below url for a remote setup:
http://localhost:4444/grid/console
3. Log files are generated for both hub and nodexxx servers in the same folder.

Setting up Environment variables (Windows)
-------------------------------------------
Go to https://github.com/simnova/webdevdocs/wiki/Installing-PhoneGap-and-Android-Studio-on-Windows and follow instructions for Java and Android Studio