using System;

/**
 * <summary>Device State object
 * It tells us the current status of a device [Serializable]
 * </summary>
 */
[Serializable]
public class DeviceState
{
    // Class variables.
	public String identifier;
	public int state;

	//sensor states
	public enum States {
		NO_CONNECTION,
		LISTEN,
		CONNECTING,
		CONNECTED,
		ACQUISITION_TRYING,
		ACQUISITION_OK,
		ACQUISITION_STOPPING,
		DISCONNECTED,
		ENDED
	}

    /**
     * <summary>Class constructor</summary>
     */
	public DeviceState ()
	{
	}

    /**
	 * <summary>Gets or sets the device identifier.</summary>
     *
     * <value>The device's MACS Address</value>
	 */
    public String DeviceId {
        // Assessing the device MAC-Address.
		get {
			return this.identifier;
		}
        // Storing the device MAC-Address.
		set {
			identifier = value;
		}
	}

    /**
	 * <summary>Gets or sets the state.</summary>
     *
     * <value>The state of the PLUX device</value>
	 */
    public int State {
        // Assessing the current state of the PLUX device.
        get {
			return this.state;
		}
        // Storing the new state of the PLUX device.
		set {
			state = value;
		}
	}

    /**
     * <summary>Method that overrides the conventional toString function, converting an object of DeviceState class into
     * simple string format.</summary>
     * <returns>A string with the textual representations of the current state of the PLUX device.</returns>
     */
    public override string ToString ()
	{
		return string.Format ("[SensorStatus: deviceId={0}, state={1}]", identifier, state);
	}
	
}


