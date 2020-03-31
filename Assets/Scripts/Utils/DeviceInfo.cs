using System;

/**
 * <summary>Device Info object
 * Contains some information regarding the connected device (MAC-Address...) [Serializable]
 * </summary>
 */
[Serializable]
public class DeviceInfo
{
    // Class variables.
    public String identifier;

    /**
	 * <summary>Gets or sets the device identifier.</summary>
     *
     * <value>The device's MACS Address</value>
	 */
    public String DeviceId
    {
        // Assessing the device MAC-Address.
        get
        {
            return this.identifier;
        }
        // Storing the device MAC-Address.
        set
        {
            identifier = value;
        }
    }

    /**
     * <summary>Method that overrides the conventional toString function, converting an object of DeviceInfo class into
     * simple string format.</summary>
     * <returns>A string with the textual representations of the current state of the PLUX device.</returns>
     */
    public override string ToString()
    {
        return string.Format("[Connected Device ID: deviceId={0}]", identifier);
    }

}
