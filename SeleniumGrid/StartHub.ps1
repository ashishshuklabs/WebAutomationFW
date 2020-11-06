$source = pwd;
cd $source.Path;
Remove-Item -Recurse *.log;
New-Item -Name hub.log;
java -cp *`;`. org.openqa.grid.selenium.GridLauncherV3 -role hub -log hub.log -hubConfig hubConfig.json