using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

/** <summary>
* Sensors controller class.
* This class handles all the sensor connection
* Register your class to receive sensorEvent and onConnectionChange objects
* </summary>
*/
public class DeviceController : MonoBehaviour, IDevice {
    
    // Instance of DeviceController class.
    public static DeviceController Instance;

    // Variable responsible for storing the state of the current device.
	private DeviceState[] deviceStatus = new DeviceState[1];
    
    // These classes handle unity-android plugin integration
    private AndroidJavaObject androidSensorsClass = null;
	private AndroidJavaObject activityContext = null;
	private AndroidJavaClass activityClass = null;
	private AndroidJavaClass pluginClass = null;

	// Delegate for connection changes
	public delegate void onConnectionChange (String device, DeviceState.States state);
	public static event onConnectionChange OnDeviceConnectionChange;

	// Delegate for new sensor events 
	public delegate void dataEvent(BiopluxFrame data);
	public static event dataEvent OnDataEvent;

    // Delegate for new device selected 
    public delegate void onDeviceSelected(string identifier);
    public static event onDeviceSelected OnNewDeviceFound;
    
    //mac address of the currently connected device.
    public string identifier = "00:00:00:00:00:00";

    // Variable that stores the current device state.
    private DeviceState.States currState;

    /**
     * ==================================================== Unity Callbacks =============================================================
     */

    /**
     * <summary>Awake is called when the script instance is being loaded.
     * Awake is used to initialize any variables or game state before the game starts. Awake is called only once during the lifetime of the script instance,
     * being always invoked before Start().</summary>
     * <see>https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html</see>
     */
    void Awake(){
		//Check if instance already exists
		if (Instance == null)
			//if not, set instance to this
			Instance = this;
    }

    /**
     * <summary>Unity callback that is invoked when the script is enabled (is only executed one time).</summary>
     * <see>https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html</see>
     */
    void Start()
    {
        Init();
    }

    /**
     * Callback invoked when the application is closed.
     */
    void OnApplicationQuit()
    {
        // Destruction of previously created objects.
        if (androidSensorsClass != null)
            androidSensorsClass.Dispose();
        if (activityClass != null)
            activityClass.Dispose();

        Close();
    }

    /**
     * ============================================= Initializers =====================================================
     */

    /**
     * <summary>Initializer of handlers that ensure effectiveness of Android/Unity integration/communication.</summary>
     */
    private void Init(){

        // Attach current thread to a Java virtual machine.
		AndroidJNI.AttachCurrentThread ();

        // Creation of an instance of the Android-Java connection class.
		activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        // Establishing the context of the Android activity.
        activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
		
        // Definition of the interface between Unity and Android, i.e., an instance to the PLUXApiPlugin
        // class.
		AndroidJavaClass pluginClass = new AndroidJavaClass ("info.plux.api.PLUXApiPlugin");

        // Initializing device.
        InitDevice();

        // Check if an instance of the pluginClass was previously created.
		if (pluginClass != null) {
			androidSensorsClass = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
                
                // Start Unity/Android communication through sequential calls.
				if (androidSensorsClass != null) {
					androidSensorsClass.Call ("setContext", activityContext);
					androidSensorsClass.Call ("setGameObjectName", this.gameObject.name);
					androidSensorsClass.Call ("restart");       
                    androidSensorsClass.Call ("init");
				}
		}

        // Object destruction.
		if (activityClass != null)
			activityClass.Dispose ();

		if (activityContext != null)
			activityContext.Dispose ();

	}

    /**
     * <summary>Auxiliary method used to initialise a device.</summary>
     */
	void InitDevice()
	{
        // Invoking the callback.
        if (OnNewDeviceFound != null)
            OnNewDeviceFound(identifier);
    }

    /**
     * ================================================== Communication with Android Plugin ===========================================================
     */

    /**
     * <summary>Method that is used to trigger the start of a Bluetooth device scan, searching for PLUX data acquisition systems.
     * This method is invoked when the "Scan" button is pressed.</summary>
     *
     * <param name="enable">If <c>true</c> the scan is started and if <c>false</c> the scan will be stopped.</param>
     */
    public void Scan(bool enable)
    {
        if (androidSensorsClass == null)
        {
            return;
        }

        androidSensorsClass.Call("scan", enable);
    }

    /**
     * <summary>During the Bluetooth scan this method is invoked with the aim of adding all found devices to a list,
     * containing all valid devices that user can choose to pair.</summary>
     *
     * <param name="id">Identifier/MAC-Address of the device that was found during the device scan.</param>
     */
    private void AddDevice(string id)
    {
        androidSensorsClass.Call("addDevice", id);
    }
		
    /**
     * <summary> Connects to a specific device.</summary>
     *
     * <param name="string">Id of the device.</param>
     */
	public void Connect(string id)
    {
        if (androidSensorsClass == null)
        {
            return;
        }

        // Add device to the internal list of the Plugin.
        AddDevice(id);

        // Update current connection.
        identifier = id;

		androidSensorsClass.Call ("connect", id);
	}

    /**
	 * <summary>Disconnects from a specific device.</summary>
     *
     * <param name="id">Id of the device.</param>
	 */
    public void Disconnect(string id){
        if (androidSensorsClass == null)
        {
            return;
        }

        // Stop real-time acquisition.
        if (currState.Equals(DeviceState.States.ACQUISITION_OK))
        {
            Stop(id);
        }
    }

    /**
     * <summary>Starts the sensors. It only works if sensors are already connected</summary>
     *
     * <param name="id">PLUX Device id/MAC-Address</param>
     * <param name="frequency">Data acquisition sampling rate to be used in the Analog-to-Digital Converter (ADC)</param>
     * <param name="nChannels">Number of active channels.</param>
     * <param name="freqDivisor">Frequency divisor to be used in order to downsample the acquired signal.</param>
     */
    /// <param name="freqDivisor">Freq divisor.</param>
    public void StartAcquisition(String id, int frequency, int nChannels, int freqDivisor){
        if (androidSensorsClass == null)
        {
            return;
        }

        androidSensorsClass.Call ("start", id, frequency, nChannels, freqDivisor);
    }

    /**
     * <summary>Starts the sensors. It only works if sensors are already connected</summary>
     *
     * <param name="id">PLUX Device id/MAC-Address</param>
     */
    public void StartAcquisition(String id){
        StartAcquisition(id, 1000, 1, 100); // Only 10 samples per second (frequency / freqDivisor = 1000 / 100 = 10) will be communicated.
    }

    /**
	 * <summary>Stops the data collection from the PLUX device.</summary>
     *
     * <param name="id">MuscleBAN id</param>
	 */
    public void Stop(String id){
        if (androidSensorsClass == null)
        {
            return;
        }

        // Send command to the PLUX API, through the Android/Unity Plugin.
        androidSensorsClass.Call("stop", id);
	}

    /**
     * =================================================== Getters =====================================================
     */

    /** 
    * <summary>Check if the sensor is connected</summary>
    * <returns><c>true</c>, if sensor connected was used, <c>false</c> otherwise.</returns>
    */
    public bool IsSensorConnected()
    {
        bool connection = false;
        if (androidSensorsClass != null)
            connection = androidSensorsClass.Call<bool>("isSensorConnected", identifier);

        return connection;
    }

    /**
     * =================================================== Callbacks ====================================================
     */

    /**
     * <summary>When a new device is found, it is received here as a json</summary>
     *
     * <param name="foundDevice">New device that was found during the device scan.</param>
     */
    public void onDeviceFound(string foundDevice)
    {
        // Convert json object to an object format.
        BluetoothDevice device = JsonUtility.FromJson<BluetoothDevice>(foundDevice);

        // Redirect the found device to the callback.
        if(OnNewDeviceFound != null)
        {
            OnNewDeviceFound(device.Address);
        }
    }

    /**
     * <summary>The device connection state changed, it is received here as a json</summary>
     *
     * <param name="state">Current connection state.</param>
     */
    public void onConnectionStateChanged(string state)
    {
        try
        {
            // Convert json object to an object format.
            DeviceState s = JsonUtility.FromJson<DeviceState>(state);

            // Check if the callback was previously initialized.
            if (OnDeviceConnectionChange != null)
            {
                currState = (DeviceState.States) s.state;
                OnDeviceConnectionChange(s.identifier, (DeviceState.States) s.state);

                // Disconnect device is the acquisition stops.
                if (currState.Equals(DeviceState.States.ACQUISITION_STOPPING))
                {
                    // Disconnect device.
                    androidSensorsClass.Call("disconnect", identifier);
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /**
     * <summary> A new BiopluxFrame is received here as a json</summary>
     *
     * <param name="frame">Frame containing data acquired from the multiple active sensors/channels
     * of the PLUX device under use.</param>
     */
    public void onDataAvailable(string frame){
		BiopluxFrame bFrame = JsonUtility.FromJson<BiopluxFrame>(frame);

        if (OnDataEvent != null)
            OnDataEvent (bFrame);
    }

    /**
     * <summary>If the device connection state changed, it is received here as a json</summary>
     *
     * <param name="state">Current connection state.</param>
     */
    public void onDeviceReady(string info)
    {
        Debug.Log("On device ready: " + info);

        StartAcquisition(JsonUtility.FromJson<DeviceInfo>(info).identifier);
    }

    /**
     * <summary>Cleans up this class and all the others</summary>
     */
    public void Close()
    {
        Instance = null;

        if (androidSensorsClass == null)
        {
            return;
        }
		
        androidSensorsClass.Call ("close");
	}
}