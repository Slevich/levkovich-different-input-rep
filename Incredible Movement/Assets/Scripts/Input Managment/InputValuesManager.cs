using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputValuesManager : InputTypes
{
    #region Fields
    [Header("Layer mask 'RayCollider'.")]
    [SerializeField] private LayerMask rayCollidedMask;
    [Header("Game settings manipulator component.")]
    [SerializeField] private GameSettingsManipulator settingManipulator;

    //A variable that contains an instance of an object with a description of user input.
    private PlayerInput playerInput;
    //A Vector2 point that describes the location of a point on the screen (mouse cursor or finger).
    private Vector2 pointPosition;
    //Vector3 variable with the coordinates of the main camera.
    private Vector3 mainCameraPosition;
    //Vector3 variable with the coordinates of position of the screen point in coordinates.
    private Vector3 screenPointScreenPosition;
    //Vector3 variable with the global coordinates of the screen point in world coodrinates.
    private Vector3 screenPointWorldPosition;
    //Variable that describes the X direction when entered from the keyboard.
    private float inputAxisDirection;
    //A float variable that contains the X-axis value of the beginning of the swipe movement.
    private float swipeStartAxisPosition;
    //A float variable that contains the X-axis value of the ending of the swipe movement.
    private float swipeEndAxisPosition;
    //The length of the segment between the start and end points of the swipe on X-axis.
    private float swipeAxisDistance;
    //Variable that describes the direction of the swipe along the X-axis (positive to the right, negative to the left).
    private float swipeAxisDirection;
    //Distance of the ray.
    private float rayCastDistance = 50f;
    //A variable that contains a number describing the X-coordinate of the hit ray point.
    private float hitAxisPosition;
    //Variable that toggles the state of whether a swipe is started or not.
    private bool swipeStarted;
    #endregion

    #region Properties
    //Properties on getting the value of some variables from other classes.
    public float InputAxisDirection { get { return inputAxisDirection; } }
    public float HitAxisPosition { get { return hitAxisPosition; } }
    public float SwipeAxisDistance { get { return swipeAxisDistance; } }
    public float SwipeAxisDirection { get { return swipeAxisDirection; } }
    public bool SwipeStarted { get { return swipeStarted; } }
    #endregion

    #region Methods
    #region Base Unity Methods
    /// <summary>
    /// On Awake we get an instance of the class with player input.
    /// We also get the current position of the camera.
    /// We subscribe to start and cancel events by pressing the mouse or tapping on the screen.
    /// </summary>
    private void Awake()
    {
        playerInput = new PlayerInput();
        mainCameraPosition = Camera.main.transform.position;
        settingManipulator.LoadSettings();
        ChangeCurrentInputType(settingManipulator.InputTypeNumber);
        ChangeCurrentControlDevice(settingManipulator.ControlDeviceNumber);
        playerInput.PlayerMap.MouseButtonPress.started += SwipeMovement_started;
        playerInput.PlayerMap.MouseButtonPress.canceled += SwipeMovement_canceled;
        playerInput.PlayerMap.TouchPressMovement.started += TouchPressMovement_started;
        playerInput.PlayerMap.TouchPressMovement.canceled += TouchPressMovement_canceled;
    }

    /// <summary>
    /// In Update, depending on the type of input, switch the called methods.
    /// </summary>
    private void Update()
    {
        switch((InputType)CurrentInputType)
        {
            case InputType.Keyboard:
                GetKeyboardAxisDirection();
                break;

            case InputType.Drag:
                GetRaycastHitPositionOnX(screenPointWorldPosition);
                break;

            case InputType.Swipe:
                GetRaycastHitPositionOnX(screenPointWorldPosition);
                break;
        }
    }
    #endregion

    #region InputEvents
    /// <summary>
    /// These are the events we signed up for at Awake. In them, we invoke methods of starting a swipe or ending a swipe.
    /// </summary>
    /// <param name="obj"></param>
    private void TouchPressMovement_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StartSwipe();
    }

    private void TouchPressMovement_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        EndSwipe();
    }

    private void SwipeMovement_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StartSwipe();
    }

    private void SwipeMovement_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        EndSwipe();
    }
    #endregion

    #region OnEnable/OnDisable Methods
    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// The method gets a value reflecting the input direction from the keyboard along the X-axis.
    /// </summary>
    private void GetKeyboardAxisDirection()
    {
        inputAxisDirection = playerInput.PlayerMap.ButtonMovement.ReadValue<float>();
    }

    /// <summary>
    /// The method, depending on whether there is keyboard and mouse input or not, 
    /// gets the position of the cursor point of the mouse or finger.
    /// </summary>
    private void GetScreenPointPosition()
    {
        switch((ControlDevice)CurrentControlDevice)
        {
             case ControlDevice.KeyboardAndMouse:
                  pointPosition = playerInput.PlayerMap.MouseMovement.ReadValue<Vector2>();
                  break;
        
             case ControlDevice.Touchscreen:
                  pointPosition = playerInput.PlayerMap.TouchMovement.ReadValue<Vector2>();
                  break;
        }

        screenPointScreenPosition = new Vector3(pointPosition.x, pointPosition.y, mainCameraPosition.y);
        screenPointWorldPosition = Camera.main.ScreenToWorldPoint(screenPointScreenPosition);
    }

    /// <summary>
    /// The method calculates the length of the swipe as well as its direction. 
    /// After that, to the length of the swipe is added the current position of the player on the X-axis, 
    /// to which the player must move.
    /// </summary>
    private void CalculateSwipeAxisDistance()
    {
        swipeAxisDistance = Mathf.Abs(swipeEndAxisPosition - swipeStartAxisPosition);
        Vector3 heading = new Vector3(swipeEndAxisPosition, transform.position.y, transform.position.x) 
                          - new Vector3(swipeStartAxisPosition, transform.position.y, transform.position.x);
        float pointsDistance = heading.magnitude;
        Vector3 swipeDirection = heading / pointsDistance;
        swipeAxisDirection = swipeDirection.x;

        Debug.Log(swipeAxisDirection);

        if (swipeAxisDirection > 0)
        {
            swipeAxisDistance = transform.position.x + swipeAxisDistance;
        }
        else if (swipeAxisDirection < 0)
        {
            swipeAxisDistance = transform.position.x - swipeAxisDistance;
        }
    }

    /// <summary>
    /// The method at the start of the swipe, translates the position of the variable to the true. 
    /// Nulls the swipe length and swipe direction variables, 
    /// and the point on the X axis where the ray was hit is assigned to the swipe start position.
    /// </summary>
    private void StartSwipe()
    {
        swipeStarted = true;
        swipeAxisDirection = 0;
        swipeAxisDistance = 0;
        swipeStartAxisPosition = hitAxisPosition;
    }

    /// <summary>
    /// The method at the end of the swipe switches the position to false. 
    /// At the end of the swipe, the current X-coordinate of the point where the ray hits is transmitted.
    ///The direction and length of the swipe is calculated.
    /// </summary>
    private void EndSwipe()
    {
        swipeStarted = false;
        swipeEndAxisPosition = hitAxisPosition;
        CalculateSwipeAxisDistance();
    }

    /// <summary>
    /// The method sends a beam from the center of the camera to the mouse cursor point or points, 
    /// but at global coordinates.
    /// </summary>
    /// <param name="PointPosition"></param>
    private void GetRaycastHitPositionOnX(Vector3 PointPosition)
    {
        GetScreenPointPosition();
        Vector3 rayDirection = (PointPosition - mainCameraPosition);
        Ray cameraRay = new Ray(mainCameraPosition, rayDirection);
        RaycastHit cameraHit;

        if (Physics.Raycast(cameraRay, out cameraHit, rayCastDistance, rayCollidedMask))
        {
            hitAxisPosition = cameraHit.point.x;
        }
    }
    #endregion
    #endregion
}
