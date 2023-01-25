using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Button = UnityEngine.UI.Button;

#if PLATFORM_ANDROID

using UnityEngine.Android;

#endif

public class MainScreenManager : MonoBehaviour
{
    // Global ReactiveProperty variables.
    private CompositeDisposable disposables = new CompositeDisposable();

    private IReactiveProperty<bool> sensorsConnectedProperty;
    private IReactiveProperty<string> valueTextProperty;
    private IReactiveProperty<string> statusTextProperty;

    // UI components
    public Button connectButton;

    public Button scanButton;
    public Text statusText;
    public Text selectedDeviceText;
    public Text valueText;
    public LevelManager levManager;
    public Button DeviceOption1;
    public Button DeviceOption2;
    public Button DeviceOption3;
    public Button DeviceOption4;
    public Button DeviceOption5;
    public Button DeviceOption6;
    public Button DeviceOption7;
    public Button DeviceOption8;
    public Button DeviceOption9;
    public Button DeviceOption10;
    public Button DeviceOption11;
    protected GameObject dialog = null;

    // Auxiliary variables
    private List<string> deviceList = new List<string>();

    private Button[] deviceListButtons;
    private string selectedDevice;
    private bool permissionGranted = false;

    // Auxiliary Constants
    private const string ANDROID_BLUETOOTH_SCAN_PERMISSION = "android.permission.BLUETOOTH_SCAN";

    private const string ANDROID_BLUETOOTH_CONNECT_PERMISSION = "android.permission.BLUETOOTH_CONNECT";

    /**
     * <summary>Unity callback that is invoked when the script is enabled (is only executed one time).</summary>
     * <see>https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html</see>
     */

    private void Start()
    {
        // Request permission to use location/Bluetooth communication.
#if PLATFORM_ANDROID
        // >>> Coarse Location
        if (getSDKInt() < 31 && !Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
        {
            AndroidRuntimePermissions.RequestPermission(Permission.CoarseLocation);

            // Present an informative message to the user, informing that in devices with Android >= 10 it will be necessary to
            // navigate to the APP settings and provide the "Allow All the Time" location permission to use the Bluetooth features.
            Debug.Log("SDK Version: " + getSDKInt());
            if (getSDKInt() >= 29)
            {
                ShowAlert("Permission Alert",
                    "In order to use the Bluetooth features, in devices with Android 10 or higher, the user should 1) manually navigate to the APP settings; 2) access the Permissions tab and 3) change the 'Location' permission to 'Allow All Time'.");
            }
        }

        // >>> Bluetooth Scan (Android >= 12)
        if (getSDKInt() >= 31 && !Permission.HasUserAuthorizedPermission(ANDROID_BLUETOOTH_SCAN_PERMISSION))
        {
            AndroidRuntimePermissions.RequestPermission(ANDROID_BLUETOOTH_SCAN_PERMISSION);
        }

        // >>> Bluetooth Connect (Android >= 12)
        if (getSDKInt() >= 31 && !Permission.HasUserAuthorizedPermission(ANDROID_BLUETOOTH_CONNECT_PERMISSION))
        {
            AndroidRuntimePermissions.RequestPermission(ANDROID_BLUETOOTH_CONNECT_PERMISSION);
        }

        // Confirm if the permission was granted.
        permissionGranted = (getSDKInt() < 31 && Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)) || (getSDKInt() >= 31 && Permission.HasUserAuthorizedPermission(ANDROID_BLUETOOTH_SCAN_PERMISSION) && Permission.HasUserAuthorizedPermission(ANDROID_BLUETOOTH_CONNECT_PERMISSION));
#endif

        // Initialise interface text fields.
        Init();

        // Define events.
        AddObservables();

        // Link the callbacks to the DeviceController object.
        DeviceController.OnDataEvent += OnNewDataEvent;
        DeviceController.OnDeviceConnectionChange += OnDeviceConnection;
        DeviceController.OnNewDeviceFound += OnNewDeviceFound;
    }

    /**
     * <summary>Unity callback called once per frame.</summary>
     * <see>https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html</see>
     */

    private void Update()
    {
        if (!permissionGranted)
        {
            // Quit application if permission was not granted.
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }

    /**
     * <summary>This function is called when the behaviour becomes disabled.
     * This is also called when the object is destroyed and can be used for any cleanup code.</summary>
     * <see>https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDisable.html</see>
     */

    private void OnDisable()
    {
        //clear disposables
        disposables.Clear();

        // Remove callbacks from the scope of DeviceController class.
        DeviceController.OnDataEvent -= OnNewDataEvent;
        DeviceController.OnDeviceConnectionChange -= OnDeviceConnection;
        DeviceController.OnNewDeviceFound -= OnNewDeviceFound;

        // Destroy the DeviceController instance.
        DeviceController.Instance.Close();
    }

    /**
     * <summary>Method used to initialise the GUI with default values in the text fields.</summary>
     */

    private void Init()
    {
        // Update the property stating if an active Bluetooth connection exists or not.
        sensorsConnectedProperty = new ReactiveProperty<bool>();
        sensorsConnectedProperty.Value = false;

        // Define the initial connection state.
        statusTextProperty = new ReactiveProperty<string>();
        statusTextProperty.Value = "no connection";

        // The initial value of the horizontal bar widget is an empty string.
        valueTextProperty = new ReactiveProperty<string>();
        valueTextProperty.Value = "";

        // Define the list of buttons inside the ScrollView.
        deviceListButtons = new Button[] { DeviceOption1, DeviceOption2, DeviceOption3, DeviceOption4, DeviceOption5, DeviceOption6, DeviceOption7, DeviceOption8, DeviceOption9, DeviceOption10, DeviceOption11 };
    }

    /**
     * <summary>Through this method, it is possible to define multiple behaviors, like for example the
     * onClick event/function that should be triggered when a button is pressed.</summary>
     */

    private void AddObservables()
    {
        connectButton.OnClickAsObservable().Subscribe(connect => ConnectSensors()).AddTo(disposables);
        DeviceOption1.OnClickAsObservable().Subscribe(click => SelectedDevice(DeviceOption1)).AddTo(disposables);
        DeviceOption2.OnClickAsObservable().Subscribe(click => SelectedDevice(DeviceOption2)).AddTo(disposables);
        DeviceOption3.OnClickAsObservable().Subscribe(click => SelectedDevice(DeviceOption3)).AddTo(disposables);
        DeviceOption4.OnClickAsObservable().Subscribe(click => SelectedDevice(DeviceOption4)).AddTo(disposables);
        DeviceOption5.OnClickAsObservable().Subscribe(click => SelectedDevice(DeviceOption5)).AddTo(disposables);
        DeviceOption6.OnClickAsObservable().Subscribe(click => SelectedDevice(DeviceOption6)).AddTo(disposables);
        DeviceOption7.OnClickAsObservable().Subscribe(click => SelectedDevice(DeviceOption7)).AddTo(disposables);
        DeviceOption8.OnClickAsObservable().Subscribe(click => SelectedDevice(DeviceOption8)).AddTo(disposables);
        DeviceOption9.OnClickAsObservable().Subscribe(click => SelectedDevice(DeviceOption9)).AddTo(disposables);
        DeviceOption10.OnClickAsObservable().Subscribe(click => SelectedDevice(DeviceOption10)).AddTo(disposables);
        DeviceOption11.OnClickAsObservable().Subscribe(click => SelectedDevice(DeviceOption11)).AddTo(disposables);
        sensorsConnectedProperty.Subscribe(sensorsConnected => SensorsConnectionChange(sensorsConnected)).AddTo(disposables);
        scanButton.OnClickAsObservable().Subscribe(click => Scan()).AddTo(disposables);
        statusTextProperty.SubscribeToText(statusText).AddTo(disposables);
        valueTextProperty.SubscribeToText(valueText).AddTo(disposables);
    }

    /**
     * =================================================== Methods linked to Events ====================================================
     */

    /**
     * <summary>Scan method triggers the beginning of a Bluetooth device scan, which standard duration is 10 seconds.
     * Function linked to the onClick event of scanButton</summary>
     */

    private void Scan()
    {
        // Clear the list of devices.
        deviceList.Clear();

        // Start a new scan.
        DeviceController.Instance.Scan(true);
    }

    /**
     * <summary>This method is invoked when connectButton is clicked and updates the informative text in the GUI
     * (regarding the current connection state).</summary>
     */

    private void ConnectSensors()
    {
        if (DeviceController.Instance.IsSensorConnected())
        {
            // Trigger the disconnection of the device.
            DeviceController.Instance.Disconnect(selectedDevice);
        }
        else
        {
            // Establish a connection with the device.
            DeviceController.Instance.Connect(selectedDevice);

            //set text
            statusTextProperty.Value = "Connecting...";
        }
    }

    /**
     * <summary>Callback invoked when the connection state changes.</summary>
     * <param name="sensorsConnected">A boolean flag that defines if a Bluetooth connection between the Android System and
     * PLUX devices is currently active (true) or not (false).</param>
     */

    private void SensorsConnectionChange(bool sensorsConnected)
    {
        if (sensorsConnected)
        {
            connectButton.GetComponentInChildren<Text>().text = "Disconnect";
        }
        else
        {
            connectButton.GetComponentInChildren<Text>().text = "Connect";
            valueTextProperty.Value = "";
            BarScript.Instance.Value = -1;
        }
    }

    /*
     * ====================================================== Callbacks ============================================================
     */

    /**
     * <summary>Callback invoked when a new package of data is communicated by PLUX device and reaches the Android system.</summary>
     * <param name="frame">Package of data containing the RAW values returned by each active channel.</param>
     */

    private void OnNewDataEvent(BiopluxFrame frame)
    {
        //show the values for sensor/channel 1 - Examples
        valueTextProperty.Value = frame.analogData[0].ToString();

        if (BarScript.Instance == null)
        {
            return;
        }

        // Update widget value/state.
        BarScript.Instance.Value = frame.analogData[0];
    }

    /**
     * <summary>Callback invoked every time the connection state changes.</summary>
     * <param name="device">Device identifier to which the current state is linked.</param>
     * <param name="state">New state currently being communicated.</param>
     */

    private void OnDeviceConnection(string device, DeviceState.States state)
    {
        // Check if a PLUX device is currently connected.
        sensorsConnectedProperty.Value = DeviceController.Instance.IsSensorConnected();

        string message = "";

        switch (state)
        {
            case DeviceState.States.LISTEN:
                message = "listening";
                break;

            case DeviceState.States.CONNECTING:
                message = "connecting";
                break;

            case DeviceState.States.CONNECTED:
                message = "connected";
                break;

            case DeviceState.States.ACQUISITION_TRYING:
                message = "starting";
                break;

            case DeviceState.States.ACQUISITION_OK:
                message = "in acquisition";
                break;

            case DeviceState.States.ACQUISITION_STOPPING:
                message = "stopping";
                break;

            case DeviceState.States.DISCONNECTED:
                message = "disconnected";
                break;

            default:
                message = "no connection";
                break;
        }

        // Update GUI informative text regarding the connection state.
        statusTextProperty.Value = message;
    }

    /**
     * <summary>Callback invoked during a Bluetooth scan, when a new PLUX device is detected.</summary>
     * <param name="identifier">MAC-Address of the PLUX device that was detected.</param>
     */

    private void OnNewDeviceFound(string identifier)
    {
        // Check if the device is already in the list.
        if (!deviceList.Contains(identifier))
        {
            // Store the current mac-address.
            deviceList.Add(identifier);
        }

        // Update GUI with the list of available devices.
        UpdateDeviceList();
    }

    /**
     * ============================================= Auxiliary Functions =============================================
     */

    /**
     * <summary>Method intended to update the content of GUI device list, containing the mac-addresses of all detected devices.</summary>
     */

    private void UpdateDeviceList()
    {
        // Update content of the GUI list.
        for (int i = 0; i < deviceListButtons.Length; i++)
        {
            // Activate a button for each available device.
            if (i < deviceList.Count)
            {
                deviceListButtons[i].interactable = true;
                deviceListButtons[i].GetComponentInChildren<Text>().text = deviceList[i];
            }
            // Disable the remaining buttons.
            else
            {
                deviceListButtons[i].interactable = false;
                deviceListButtons[i].GetComponentInChildren<Text>().text = "-";
            }
        }
    }

    /**
     * <summary>Method that stores in selectDevice global variable the MAC-Address of the chosen device (clicked button in the device list).</summary>
     * <param name="clickedButton">GUI element, corresponding to the interactive button that was clicked and that contains the desired MAC-Address.</param>
     */

    private void SelectedDevice(Button clickedButton)
    {
        // Store the selected device mac-address.
        selectedDevice = clickedButton.GetComponentInChildren<Text>().text;

        // Update informative text.
        selectedDeviceText.text = selectedDevice;
    }

    /**
     * <summary>Method that retrieves the Android SDK version of the device where the APP is running.</summary>
     */

    private int getSDKInt()
    {
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return version.GetStatic<int>("SDK_INT");
        }
    }

    /**
     * <summary>Class intended to deal with the actions triggered by the user while interacting with an Alert Dialog.</summary>
     * <link>https://weesals.wordpress.com/2019/12/20/minimum-code-for-android-alert-dialog-box-in-unity/</link>
     */

    private class OnClickListener : AndroidJavaProxy
    {
        public readonly Action Callback;

        /**
         * <summary>Listener monitoring when the OK button of the Alert Dialog is pressed.</summary>
         * <param name="callback">Callback triggered when the OK button of the Alert Dialog is pressed.</param>
         */

        public OnClickListener(Action callback) : base("android.content.DialogInterface$OnClickListener")
        {
            Callback = callback;
        }

        /**
         * <summary>Method linked to the OnClick event triggered by the user after clicking the OK button in the Alert Dialog.</summary>
         * <param name="dialog">Dialog object that gave origin to the event.</param>
         * <param name="id">Unique identifier of the event.</param>
         */

        public void OnClick(AndroidJavaObject dialog, int id)
        {
            Callback();
        }
    }

    /**
     * <summary>Method that when called will generate an Alert Dialog to inform the user about a specific condition.</summary>
     * <param name="title">Title of the Alert Dialog.</param>
     * <param name="content">Alert message to be presented to the user.</param>
     */

    private void ShowAlert(string title, string content)
    {
        AndroidJavaObject activity = null;
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
        activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            AndroidJavaObject dialog = null;
            using (AndroidJavaObject builder = new AndroidJavaObject("android.app.AlertDialog$Builder", activity))
            {
                builder.Call<AndroidJavaObject>("setTitle", title).Dispose();
                builder.Call<AndroidJavaObject>("setMessage", content).Dispose();
                builder.Call<AndroidJavaObject>("setPositiveButton", "OK", new OnClickListener(() =>
                {
                    Debug.Log("Button pressed");
                })).Dispose();
                dialog = builder.Call<AndroidJavaObject>("create");
            }
            dialog.Call("show");
            dialog.Dispose();
            activity.Dispose();
        }));
    }
}