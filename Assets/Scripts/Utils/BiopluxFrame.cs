using System;

/// <summary>
/// Bioplux frame.
/// Copy of the java object to be serialized
/// Serializable
/// </summary>
[Serializable]
public class BiopluxFrame
{
    // Class variables.
	public int[] analogData; // Package of received data.
	public string identifier; // Device mac-address.
	public int sequence; // Number of the current package of data.
	public int digitalInput; // Value linked with the digital flag of the device.
	public string comments; // Comments regarding the acquisition.

    /**
     * <summary>Class constructor</summary>
     */
    public BiopluxFrame ()
	{
	}

	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>The identifier of the MuscleBAN as mac address</value>
	public string Identifier {
        // Assessing the identifier value.
        get {
			return this.identifier;
		}
        // Storage of PLUX device identifier/mac-address inside a class variable.
		set {
			identifier = value;
		}
	}

	/// <summary>
	/// Gets or sets the sequence.
	/// </summary>
	/// <value>The sequence value of the device - timestamp</value>
	public int Sequence {
        // Assessing the sequence number value.
        get
        {
			return this.sequence;
		}
        // Storage of the sequence number inside a class variable.
        set
        {
			sequence = value;
		}
	}

	/// <summary>
	/// Gets or sets the analog data.
	/// </summary>
	/// <value>The array of raw data - each value represents a channel</value>
	public int[] AnalogData {
        // Assessing the package of data values.
		get {
			return this.analogData;
		}
        // Storage of the content of the package of analog data into a class variable.
		set {
			analogData = value;
		}
	}

	/// <summary>
	/// Gets or sets the digital input.
	/// </summary>
	/// <value>The digital input for specific channels - not used</value>
	public int DigitalInput {
        // Assessing the digital flag state.
		get {
			return this.digitalInput;
		}
        // Storage of the digital flag state in a class variable.
		set {
			digitalInput = value;
		}
	}

	/// <summary>
	/// Gets or sets the comments.
	/// </summary>
	/// <value>Additional info for a sensor</value>
	public string Comments {
        // Assessing the data acquisition comments.
        get {
			return this.comments;
		}
        // Storage of the data acquisition comments in a class variable.
		set {
			comments = value;
		}
	}

    /**
     * <summary>Method that overrides the conventional toString function, converting an object of BiopluxFramce class into
     * simple string format.</summary>
     * <returns>A string with the textual representations of the BiopluxFrame object.</returns>
     */
	public override string ToString ()
	{
		return string.Format ("[BiopluxFrame: identifier={0}, sequence={1}, analogData={2}, digitalInput={3}, comments={4}]",
			identifier, sequence, analogData.ToString(), digitalInput, comments);
	}
	
}


