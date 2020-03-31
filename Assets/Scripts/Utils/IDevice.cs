using UnityEngine;
using System.Collections;

/**
 * <summary>Interface for the PLUX devices events</summary>
 */
public interface IDevice {
    void onConnectionStateChanged(string state);
    void onDataAvailable (string frame);
    void onDeviceReady(string info);
}
