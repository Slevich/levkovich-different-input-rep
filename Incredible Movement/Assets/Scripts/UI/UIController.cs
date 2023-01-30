using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour, IInputChanger
{
    #region Fields
    [Header("A game object containing a screen displayed on the stage during the game.")]
    [SerializeField] private GameObject playableScreen;
    [Header("A game object containing a screen displayed when the game is paused.")]
    [SerializeField] private GameObject pauseScreen;
    [Header("Button list for switching the control type in the pause menu.")]
    [SerializeField] private List<GameObject> changeInputTypeButtons;
    [Header("Button list for switching the control device in the pause menu.")]
    [SerializeField] private List<GameObject> changeControlDeviceButtons;
    [Header("A sprite indicating that the button is activated.")]
    [SerializeField] private Sprite buttonActiveSprite;
    [Header("A sprite indicating that the button is disactivated.")]
    [SerializeField] private Sprite buttonDisactiveSprite;
    [Header("An instance of the InputValues class, whose component is on the player.")]
    [SerializeField] private InputValuesManager inputValues;

    //Variable that contains the currently active screen.
    private GameObject currentScreen;
    #endregion

    #region Methods
    /// <summary>
    /// In Awake, we assign the playable screen to the current game screen.
    /// </summary>
    private void Awake()
    {
        currentScreen = playableScreen;
    }

    private void Start()
    {
        UpdateButtonsSpritesOnStart();
    }

    /// <summary>
    /// The method turns off the previous screen, turns on the new screen. 
    /// At the same time, the new screen is assigned to the current screen.
    /// If the time in the game is not paused, bring it to zero. 
    /// Otherwise, do the opposite.
    /// </summary>
    /// <param name="nextScreen"></param>
    public void ChangeScreen(GameObject nextScreen)
    {
        currentScreen.SetActive(false);
        nextScreen.SetActive(true);
        currentScreen = nextScreen;

        if (Time.timeScale == 0) Time.timeScale = 1;
        else Time.timeScale = 0;
    }

    /// <summary>
    /// When you click on a button, we get its index in the buttons list.
    /// Then in the loop we change the sprites of the activated and inactive buttons.
    /// At the same time, depending on whether the buttons switch the type of control, 
    /// operations are performed with one or another list.
    /// </summary>
    public void ChangeButtonsSpritesOnClick(bool isButtonChangeInputType)
    {
        List<GameObject> buttonsList;

        if (isButtonChangeInputType)
        {
            buttonsList = changeInputTypeButtons;
        }
        else
        {
            buttonsList = changeControlDeviceButtons;
        }

        int selectedButtonIndex = buttonsList.IndexOf(EventSystem.current.currentSelectedGameObject);

        for (int i = 0; i < buttonsList.Count; i++)
        {
            if (i == selectedButtonIndex)
            {
                buttonsList[i].GetComponent<Image>().sprite = buttonActiveSprite;
            }
            else
            {
                buttonsList[i].GetComponent<Image>().sprite = buttonDisactiveSprite;
            }
        }
    }

    /// <summary>
    /// The method calls the method to change the control type when the button is clicked.
    /// </summary>
    /// <param name="newInputType"></param>
    public void ChangeInputType(int newInputType)
    {
        inputValues.ChangeCurrentInputType(newInputType);
    }

    /// <summary>
    /// The method calls the method to change the control device when the button is clicked.
    /// </summary>
    /// <param name="newInputType"></param>
    public void ChangeControlDevice(int newControlDevice)
    {
        inputValues.ChangeCurrentControlDevice(newControlDevice);
    }

    private void UpdateButtonsSpritesOnStart()
    {
        for (int i = 0; i < changeInputTypeButtons.Count; i++)
        {
            if (++i == inputValues.CurrentInputType)
            {
                changeInputTypeButtons[i].GetComponent<Image>().sprite = buttonActiveSprite;
                Debug.Log("Инпуты!");
                break;
            }
        }

        for (int q = 0; q < changeControlDeviceButtons.Count; q++)
        {
            if (++q == inputValues.CurrentControlDevice)
            {
                changeControlDeviceButtons[q].GetComponent<Image>().sprite = buttonActiveSprite;
                Debug.Log("Контролы!");
                break;
            }
        }
    }
    #endregion
}
