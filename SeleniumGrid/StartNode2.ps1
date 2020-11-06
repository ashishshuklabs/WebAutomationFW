$source = pwd;
cd $source.Path;
Remove-Item -Recurse Node2.log;
New-Item -Name Node2.log;
appium --port 4726 --nodeconfig nodeConfig2.json --log node2.log --default-capabilities '{ \"udid\": \"192.168.1.104:5555\", \"deviceName\": \"RedmiX\", \"systemPort\": \"8202\", \"platformVersion\":\"10\", \"platformName\":\"Android\" }' --session-override --relaxed-security