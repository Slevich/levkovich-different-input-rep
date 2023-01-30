using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Fields
    [Header("Float variable reflects player's movement speed.")]
    [SerializeField] private float movementSpeed;

    //A variable that contains an instance of an object with user input values.
    private InputValuesManager inputValues;
    //A point in space indicating the target of the player's movement.
    private Vector3 movementTarget;
    //A number denoting the possible displacement in Uni-Meters of the player, relative to the world center of coordinates.
    private float axisMaxDifference = 6f;
    #endregion

    #region Methods
    #region Default Unity Methods
    /// <summary>
    /// OnValidate allows you to track the change in speed of the player, when you change it in the inspector.
    /// </summary>
    private void OnValidate()
    {
        if (movementSpeed < 0) movementSpeed = 0;
    }

    /// <summary>
    /// In Awake, we get a component with an object instance with player input values.
    /// </summary>
    private void Awake()
    {
        inputValues = GetComponent<InputValuesManager>();
    }

    /// <summary>
    /// In FixedUpdate, depending on the type of input, 
    /// move the player by setting a new transform.position.
    /// </summary>
    private void FixedUpdate()
    {
        switch(inputValues.CurrentInputType)
        {
            case 1:
                MoveCharacter(CalculateMovementDirectionByKeyboard(inputValues.InputAxisDirection));
                break;

            case 2:
                MoveCharacter(CalculateMovementDirectionByMouse(inputValues.HitAxisPosition));
                break;

            case 3:
                MoveCharacter(CalculateMovementDirectionBySwipe(inputValues.SwipeAxisDistance));
                break;
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// The method sets a new transform.position of the player.
    /// </summary>
    /// <param name="movementTarget"></param>
    private void MoveCharacter(Vector3 movementTarget)
    {
        transform.position = movementTarget;
    }

    /// <summary>
    /// The method calculates the point the player must reach depending 
    /// on the direction of his movement (left -1, right 1, in place 0).
    /// In this case, when the player has reached the movement boundary, 
    /// he can only move in the opposite direction.
    /// </summary>
    /// <param name="InputAxisValue"></param>
    /// <returns></returns>
    private Vector3 CalculateMovementDirectionByKeyboard(float InputAxisValue)
    {
        switch(InputAxisValue)
        {
            case 0:
                movementTarget = transform.position;
                break;

            case -1:
                if (transform.position.x > -axisMaxDifference)
                {
                    movementTarget = Vector3.Lerp(transform.position, transform.position + Vector3.left, movementSpeed * Time.deltaTime);
                }
                else
                {
                    movementTarget = new Vector3(-axisMaxDifference, transform.position.y, transform.position.z);
                }
                break;

            case 1:
                if (transform.position.x < axisMaxDifference)
                {
                    movementTarget = Vector3.Lerp(transform.position, transform.position + Vector3.right, movementSpeed * Time.deltaTime);
                }
                else
                {
                    movementTarget = new Vector3(axisMaxDifference, transform.position.y, transform.position.z);
                }
                break;
        }

        return movementTarget;
    }

    /// <summary>
    /// The method calculates a new point to which the player seeks when moving, 
    /// according to the new point on the X-axis where the ray hit.
    /// Here too, depending on whether the player has reached the extremes, 
    /// it is possible to move in the opposite direction.
    /// </summary>
    /// <param name="HitAxisPosition"></param>
    /// <returns></returns>
    private Vector3 CalculateMovementDirectionByMouse(float HitAxisPosition)
    {
        //A terrifying test, I know.
        if ((transform.position.x < axisMaxDifference && transform.position.x > -axisMaxDifference) 
            || (transform.position.x >= axisMaxDifference && HitAxisPosition < transform.position.x) 
            || (transform.position.x <= -axisMaxDifference && HitAxisPosition > transform.position.x))
        {
            movementTarget = Vector3.Lerp(transform.position, new Vector3(HitAxisPosition, transform.position.y, transform.position.z), movementSpeed * Time.deltaTime);
        }
        else if (transform.position.x >= axisMaxDifference && HitAxisPosition > transform.position.x)
        {
            movementTarget = new Vector3(axisMaxDifference, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -axisMaxDifference && HitAxisPosition < transform.position.x)
        {
            movementTarget = new Vector3(-axisMaxDifference, transform.position.y, transform.position.z);
        }

        return movementTarget;
    }

    /// <summary>
    /// The method calculates a new point on the X-axis, 
    /// depending on the length of the swipe.
    /// </summary>
    /// <param name="SwipeDistance"></param>
    /// <returns></returns>
    private Vector3 CalculateMovementDirectionBySwipe(float SwipeDistance)
    {
        if (inputValues.SwipeStarted == false)
        {
            //A terrifying test again.
            if ((transform.position.x < axisMaxDifference && transform.position.x > -axisMaxDifference) 
                || (transform.position.x >= axisMaxDifference && inputValues.SwipeAxisDirection < 0) 
                || (transform.position.x <= -axisMaxDifference && inputValues.SwipeAxisDirection > 0))
            {
                movementTarget = Vector3.Lerp(transform.position,
                                              new Vector3(SwipeDistance, transform.position.y, transform.position.z),
                                              movementSpeed * Time.deltaTime);
            }
            else if (transform.position.x >= axisMaxDifference)
            {
                movementTarget = new Vector3(axisMaxDifference, transform.position.y, transform.position.z);
            }
            else if (transform.position.x <= -axisMaxDifference)
            {
                movementTarget = new Vector3(-axisMaxDifference, transform.position.y, transform.position.z);
            }
        }

        return movementTarget;
    }
    #endregion
    #endregion
}
