# PLUX Android/Unity API | Sample APP
The Sample APP contained inside this repository provides a practical and intuitive resource to explore the integration of some signal acquisition devices
commercialized by **PLUX** ([**biosignalsplux**](https://www.pluxbiosignals.com/collections/biosignalsplux/products/researcher-kit), [**biosignalsplux Solo**](https://www.pluxbiosignals.com/products/solo-kit), [**muscleBAN**](https://www.pluxbiosignals.com/products/muscleban-kit) and [**cardioBAN**](https://www.pluxbiosignals.com/products/cardioban)) in an **Android** application created through **Unity** environment.

Some questions can be enlightened, such as:

+	How PLUX acquisition systems can be searched and found on the Android device?
+	How to pair an Android tablet/mobile phone with a PLUX device?
+	How to start a real-time acquisition?
+	How to stop the real-time acquisition?

## Recommended SDK and Unity Editor

+	SDK Platform 27 \[*Android 8.1*]
+	Unity 2022.1.16f1

## Limitations

+	**PLUX** devices from **BITalino** product line are not currently supported. 
+	Graphical User Interface is optimized for tablets.

## How to Use It

The provided project folder is a ready to use **Unity** solution, you simply need to access the cloned folder through **Unity** interface.

On the following animation we can quickly show the main functionalities of our Sample APP:

![sample-app-animation.gif](https://i.ibb.co/gMTy3QM/android-unity-sample-app.gif)

## How to Easily Integrate the API in your Unity Project?
1.	Inside the **Plugins** folder of our Sample APP, copy the **Android/hw-api.dll** file to the **Plugins** folder of your **Unity** project. This **.dll** contains the **PLUX API Plugin**, acting as an interface between your application and **PLUX** hardware
2.	Copy **Scripts/Utils** folder to the **Scripts** directory of you **Unity** project. Classes contained inside this folder will be responsible for a high-level abstraction in the interaction between your **C#** scripts and **PLUX API Plugin**

After the previous two steps, you will be fully prepared to expand your **Unity** project and include some interesting functionalities through the use of **PLUX** signal acquisition devices.

In order to trigger the execution of a specific operation, such as **a) PLUX Devices Scan**, **b) Android/PLUX Device Connection**, **c) Start/Stop of a Real-Time Acquisition** or **d) Android/PLUX Device Disconnection**, it will only be necessary to call one of the available methods of the **DeviceController** class ([Methods](#Methods)).

# Android/Unity API

## Methods

-	[DeviceController.Scan](#Scan)
-	[DeviceController.Connect](#Connect)
-	[DeviceController.Disconnect](#Disconnect)
-	[DeviceController.StartAcquisition](#StartAcquisition)
-	[DeviceController.Stop](#Stop)
-	[DeviceController.IsSensorConnected](#IsSensorConnected)
-	[DeviceController.Close](#Close)

---

## Scan

Method that is used to trigger the start of a **Bluetooth** device scan, searching for **PLUX** data acquisition systems.
This method is invoked when the **"Scan"** button of the Sample APP is pressed.

```csharp
public void Scan(bool enable)
```

### Description

The input argument defines if the scanning process will be executed with a timeout limit, i.e., if **enable** is **true** then the search for **Bluetooth** devices will be conducted during 10 seconds.

Otherwise, if this parameter is **false**, a previously started scan can be securely interrupted/stopped.

### Parameters

+	**enable** `bool`: A flag that defines if the scan should be started (**true**) or stopped (**false**) before the standard 10 seconds period ends.

---

## Connect

This method is intended to establish a **Bluetooth** connection with a **PLUX** device identifiable through the respective MAC-Address.

```csharp
public void Connect(string id)
```

### Description

With `Connect` method it is possible to establish a Bluetooth connection between an **Android** system and a **PLUX** Device. To ensure a successful pairing, the user only needs to specify the device mac-address (attached to **biosignalsplux**, **biosignalsplux Solo** or **MuscleBAN** hub...).
This mac-address should be inserted in a string format with the following structure: `"00:07:80:4D:2E:AD"` (6 pairs of characters separated by ":").

### Parameters

+ **id** `string`: The mac-address of the **PLUX** device with which a **Bluetooth** connection will be established.

---

## Disconnect

Opposing to [Connect](#Connect), this method is very useful when you want to safely cut the connection between **PLUX** devices and an **Android** system.
```csharp
public void Disconnect(string id)
```

### Description

Through `Disconnect` method the established connection between a **PLUX** device and an **Android** system can be securely closed. If a real-time acquisition is yet being executed, a stop command is automatically sent, ensuring the end of communication loop.

### Parameters

+ **id** `string`: The mac-address of the **PLUX** device whose connection will be broken.

---

## StartAcquisition

Class method used to start a Real-Time acquisition at the **PLUX** device paired with the **Android** system through [Connect](#Connect) method.
```csharp
public void StartAcquisition(String id, int frequency, int nChannels, int freqDivisor)
```

### Description

In order to communicate with a digital system (as an **Android** tablet or mobile phone) the **PLUX** device needs to execute an **Analogical to Digital Conversion** (ADC), through data sampling.
Taking ADC stage into consideration, while starting a real-time acquisition, selecting a specific sampling rate or downsampling ratio  is extremely easy through the inputs of `StartAcquisition`.

### Parameters

+	**id** `string`: mac-address of a previously connected **PLUX** device, where the real-time acquisition should take place.
+	**frequency** `int`: Desired sampling rate that will be used during the data acquisition stage. The used units are in Hz (samples/s).
+	**numberOfChannels** `int`: Number of the active channels that will be used during data acquisition. With **biosignalsplux** it is possible to collect data from up to 8 channels (simultaneously).
+	**freqDivisor** `int`: Frequency divisor, i.e., acquired data will be subsampled accordingly to this parameter. If **freqDivisor = 10**, it means that each set of 10 acquired samples will trigger the communication of a single sample (through the communication loop).

---

## Stop

Class method used to interrupt the real-time communication loop triggered by `StartAcquisition()` method.
```csharp
public void Stop(String id)
```

### Description

After starting a real-time acquisition a communication loop between the **PLUX** device and the **Android** system starts running on a parallel thread.
With `Stop` function, first of all, the communication loop is interrupted, stopping securely the real-time acquisition.

### Parameters

+	**id** `string`: mac-address of a **PLUX** device where a real-time data acquisition is <ins>currently</ins> taking place.

---

## IsSensorConnected

A simple getter method that retrieves the state of an internal flag intended to identify when a connection between a **PLUX** device and an **Android** system is currently active or not.
```csharp
public bool IsSensorConnected()
```

### Description

This method is extremely useful to control the start of a real-time acquisition in a secure way, i.e., it is advisable to verify if a **Bluetooth** connection is active before triggering the start of a real-time data acquisition.

### Parameters

*Method without input parameters*

### Returned Parameters

A Boolean flag is returned, stating that a **Bluetooth** connection between a **PLUX** device and an **Android** system is currently active (**true**) or inactive (**false**).

---

## Close

A quick but very reliable way to destroy in a securely the **DeviceController** instance.

```csharp
public void Close()
```

### Description

This method acts as a destroyer of a **DeviceController** instance unregistering the **BroadcastReceivers** used by **PLUX API Plugin** in the **Android** environment.

### Parameters

*Method without input parameters*

---


## Quick-Notes

There are some relevant callbacks that should be taken into consideration for a solid and secure implementation of all API functionalities.

The following instructions must be included in the **Start()** and **OnDisable()** callbacks of **Unity** APPs:

```csharp
void Start () {
    // Link the callbacks to the DeviceController object.
    DeviceController.OnDataEvent += OnNewDataEvent;  
    DeviceController.OnDeviceConnectionChange += OnDeviceConnection;  
    DeviceController.OnNewDeviceFound += OnNewDeviceFound;
}

void OnDisable(){
    // Remove callbacks from the scope of DeviceController class.
    DeviceController.OnDataEvent -= OnNewDataEvent;  
    DeviceController.OnDeviceConnectionChange -= OnDeviceConnection; 
    DeviceController.OnNewDeviceFound -= OnNewDeviceFound; 

    // Destroy the DeviceController instance.
    DeviceController.Instance.Close();
}
```

A practical example of application of each callback (**a) OnNewDataEvent** | **b) OnDeviceConnection** | **c) OnNewDeviceSelected**) is available on the **Scripts/MainScreenManager.cs** file of the **Sample APP**.

**OnNewDataEvent** is used to receive the data packages communicated by the **PLUX** device, while **OnNewDeviceFound** is useful to receive the mac-address of each device detected during the **Bluetooth** scan.

Regarding **OnDeviceConnection**, this callback is a powerful resource to understand if the **PLUX** device is connected/disconnected or if a real-time acquisition is under execution.

---

## Support
If you find any problem during your experience, please, feel free to create a new issue track on our repository.
We will be very glad to guide you in this amazing journey and listen your suggestions!
