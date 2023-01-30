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
    /// Метод передает по ключу в класс пользовательских настроек
    /// значение переменных с позициями слайдеров, хранящихся в глобальном классе.
    /// Затем сохраняет их.
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("InputType", inputTypeNumber);
        PlayerPrefs.SetInt("ControlDevice", controlDeviceNumber);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Метод передает в переменные с позициями слайдеров из глобального класса, значения
    /// по ключам. При этом, проверяется, имеется ли там соответствующий ключ.
    /// Если же нет, тогда задается дефолтное значение в единицу.
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
