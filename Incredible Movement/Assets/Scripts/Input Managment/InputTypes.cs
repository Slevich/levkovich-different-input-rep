using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputTypes : MonoBehaviour
{
    #region Fields
    [Header("The number of the current control type (integer, 1 to 3).")]
    [SerializeField] private int currentInputType;
    [Header("The number of the current control device (integer, 1 to 2).")]
    [SerializeField] private int currentControlDevice;
    #endregion

    #region Properties
    //A property that returns and set the number of the current control type.
    public int CurrentInputType { get { return currentInputType; }}
    //A property that returns and set the number of the current control device.
    public int CurrentControlDevice { get { return currentControlDevice; }}
    #endregion

    #region Enums
    //An enumeration containing the names of all control types and their ordinal numbers.
    public enum InputType
    {
        Keyboard = 1,
        Drag = 2,
        Swipe = 3
    }

    //An enumeration containing the names of all control devices and their ordinal numbers.
    protected private enum ControlDevice
    {
        KeyboardAndMouse = 1,
        Touchscreen = 2
    }
    #endregion

    #region Methods
    public void ChangeCurrentInputType(int newInputTypeNumber) => currentInputType = newInputTypeNumber;

    public void ChangeCurrentControlDevice(int newControlDeviceNumber) => currentControlDevice = newControlDeviceNumber;
    #endregion
}
