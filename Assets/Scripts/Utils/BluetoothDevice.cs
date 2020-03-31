using System;

/// <summary>
/// Bluetooth Device object
/// Serializable
/// </summary>
[Serializable]
public class BluetoothDevice
{
    // Class variables.
	public String address; // Device MAC-address.
	public String name; // Device name.

    /**
     * <summary>Class constructor.</summary>
     */
    public BluetoothDevice()
	{
	}

	/// <summary>
	/// Gets or sets the device identifier.
	/// </summary>
	/// <value>The device's MACS Address</value>
	public String Address {
        // Assessing the device MAC-address.
        get {
			return this.address;
		}
        // Storage of the device MAC-address into a class variable.
		set {
            address = value;
		}
	}

	/// <summary>
	/// Gets or sets the device's name.
	/// </summary>
	/// <value>The device's name</value>
	public String Name {
        // Assessing the PLUX device name.
        get {
			return this.name;
		}
        // Storage of the PLUX device name inside a class variable.
		set {
			name = value;
		}
	}
	
}


