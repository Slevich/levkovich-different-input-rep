using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManipulator : GlobalSettings
{
    #region Fields
    [SerializeField] private InputValuesManager inputValues;
    #endregion

    #region Properties
    public int InputTypeNumber { get { return inputTypeNumber; } }
    public int ControlDeviceNumber { get { return controlDeviceNumber; } }
    #endregion

    #region Methods
    /// <summary>
    /// ����� �������� �� ����� � ����� ���������������� ��������
    /// �������� ���������� � ��������� ���������, ���������� � ���������� ������.
    /// ����� ��������� ��.
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("InputType", inputTypeNumber);
        PlayerPrefs.SetInt("ControlDevice", controlDeviceNumber);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ����� �������� � ���������� � ��������� ��������� �� ����������� ������, ��������
    /// �� ������. ��� ����, �����������, ������� �� ��� ��������������� ����.
    /// ���� �� ���, ����� �������� ��������� �������� � �������.
    /// </summary>
    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("InputType") && PlayerPrefs.HasKey("ControlDevice"))
        {
            inputTypeNumber = PlayerPrefs.GetInt("InputType");
            controlDeviceNumber = PlayerPrefs.GetInt("ControlDevice");
        }
        else
        {
            PlayerPrefs.SetInt("InputType", 1);
            PlayerPrefs.SetInt("ControlDevice", 1);
        }
        Debug.Log($"Input Type: {inputTypeNumber}");
        Debug.Log($"Control device: {controlDeviceNumber}");
    }

    private void OnApplicationQuit()
    {
        inputTypeNumber = inputValues.CurrentInputType;
        controlDeviceNumber = inputValues.CurrentControlDevice;
        SaveSettings();
    }
    #endregion
}
