using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInputChanger
{
    void ChangeInputType(int CurrentInputType, int NewInputType)
    {
        CurrentInputType = NewInputType;
    }

    void ChangeControlDevice (bool isUsingMouseAndKeyBoard, bool newValue)
    {
        isUsingMouseAndKeyBoard = newValue;
    }
}
