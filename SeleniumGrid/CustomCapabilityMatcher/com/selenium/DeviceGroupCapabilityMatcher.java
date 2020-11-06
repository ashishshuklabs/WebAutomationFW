package com.selenium;
import java.util.Map;
import org.openqa.grid.internal.utils.DefaultCapabilityMatcher;
public class DeviceGroupCapabilityMatcher extends DefaultCapabilityMatcher {
	private final String groupsNodeName = "DeviceGroup";
    @Override
    public boolean matches(Map<String, Object> nodeCapabilities, Map<String, Object> requestedCapabilities) {
		boolean basicChecks = super.matches(nodeCapabilities, requestedCapabilities);
		System.out.println("Inside custom capability matcher. Basic capability match result:"+ basicChecks);
		
        if (! requestedCapabilities.containsKey(groupsNodeName)){
            // If the user didnt set the custom capability 
            // lets just return what the DefaultCapabilityMatcher
            // would return. That way we are backward compatibility and 
            // arent breaking the default behavior of the grid
			System.out.println("Custom capability:" + groupsNodeName +" not found in requested capabilities");
            return basicChecks;
        }
		System.out.println("Found custom capability:" + groupsNodeName );
        return basicChecks &&
		nodeCapabilities.containsKey(groupsNodeName) &&
		nodeCapabilities.get(groupsNodeName).equals(requestedCapabilities.get(groupsNodeName));
    }
}
