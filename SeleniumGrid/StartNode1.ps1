$source = pwd;
cd $source.Path;
Remove-Item -Recurse Node1.log;
New-Item -Name Node1.log;
appium --port 4725 --nodeconfig nodeConfig1.json --log node1.log --webkit-debug-proxy-port 9220 --default-capabilities '{ \"udid\": \"513a8cc3113d26b35b7da84870bce12582ca9ff9\", \"deviceName\": \"ipad\", \"systemPort\": \"8201\", \"platformVersion\":\"13.6\", \"platformName\":\"iOS\" }' --session-override --relaxed-security;