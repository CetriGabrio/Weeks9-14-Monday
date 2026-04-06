using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

//This script manages the rocket launcher UI and the overall launch system.
//It handles pretty much most of the functionalities of this system project, such as rocket selection, slider values, player input and events, etc.

public class RocketLauncherUI : MonoBehaviour
{
    //I'm using headers to keep my inspector more organized and easier to navigate
    [Header("Rocket Prefabs")]
    public GameObject[] rocketPrefabs; //List of all the selectable rocket prefabs

    [Header("UI Sliders")]
    public Slider sizeSlider;   //Slider used to control the rocket size
    public Slider speedSlider;  //Slider used to control the rocket speed
    public Slider spawnSlider;  //Slider used to control the rocket spawn position on the X-axis

    [Header("Spawn")]
    public Transform launchPoint; //Base position where rockets appear

    [Header("Camera")]
    public CameraFollow cameraFollow; //Reference to the camera follow system

    [Header("Launch Event")]
    public UnityEvent onRocketLaunched; //Event triggered when a rocket is launched

    [Header("Destroy Event")]
    public UnityEvent onRocketDestroyed; //Event triggered when a rocket is destroyed

    private bool canSpawn = true; //Controls whether a new rocket can be launched

    private GameObject selectedRocketPrefab; //Stores the currently selected rocket prefab
    private GameObject currentRocket;        //Stores the current preview/launched rocket
    private Color currentColor = Color.white; //Stores the currently selected rocket color

    private float currentSize = 1f;         //Current rocket size value
    private float currentSpeed = 5f;        //Current rocket speed value
    private float currentSpawnOffset = 0f;  //Current horizontal spawn offset

    //Returns the current rocket so other scripts can access it
    public GameObject GetCurrentRocket()
    {
        return currentRocket;
    }

    //Selects a rocket prefab and creates a preview rocket if launching is allowed
    public void SelectRocket(int index)
    {
        if (!canSpawn)
            return; //Prevent changing a rocket while one is flying

        selectedRocketPrefab = rocketPrefabs[index]; //Store the selected prefab
        CreateOrReplacePreviewRocket();              //Update preview rocket by "creating" a rocket if none is visible, or replacing it if player chooses a different model
    }

    //Updates the current rocket size from the slider
    public void SetSize(float value)
    {
        currentSize = value; //Store the new rocket size

        if (currentRocket != null)
        {
            currentRocket.transform.localScale = Vector3.one * currentSize; //Apply the chosen scale to the preview rocket
        }
    }

    //Updates the current rocket speed from the slider
    public void SetSpeed(float value)
    {
        currentSpeed = value; //Store the new rocket speed

        if (currentRocket != null)
        {
            Rocket rocketScript = currentRocket.GetComponent<Rocket>(); //Get the reference to the rocket movement script
            if (rocketScript != null)
            {
                rocketScript.SetSpeed(currentSpeed); //Apply the chosen speed to the preview rocket
            }
        }
    }

    //Updates the current rocket color
    public void SetColor(Color color)
    {
        currentColor = color; //Store the chosen rocket color

        if (currentRocket != null)
        {
            ApplyColor(currentRocket); //Apply the chosen color to the preview rocket
        }
    }

    //These are the individual functions for the 4 possible colors the player can assign to teh rocket

    //Sets the rocket color to purple
    public void SetColorPurple()
    {
        SetColor(Color.purple);
    }

    //Sets the rocket color to white (or back to neutral)
    public void SetColorWhite()
    {
        SetColor(Color.white);
    }

    //Sets the rocket color to green
    public void SetColorGreen()
    {
        SetColor(Color.green);
    }

    //Sets the rocket color to yellow
    public void SetColorYellow()
    {
        SetColor(Color.yellow);
    }

    //Applies the currently selected color to a rocket
    void ApplyColor(GameObject rocket)
    {
        SpriteRenderer sr = rocket.GetComponent<SpriteRenderer>(); //Get a reference to the chosen rocket sprite renderer

        if (sr != null)
        {
            sr.color = currentColor; //Change the sprite color based on the chosen option
        }
    }

    //Updates the rocket spawn offset from the slider
    public void SetSpawnOffset(float value)
    {
        currentSpawnOffset = value; //Store the new rocket offset

        if (currentRocket != null)
        {
            //Move the preview rocket to the updated spawn position
            currentRocket.transform.position = launchPoint.position + new Vector3(currentSpawnOffset, 0f, 0f);
        }
    }

    //as mentioned before, this functions creates a new preview rocket and removes the old one to only have one on screen and avoid overlappings of sprites
    void CreateOrReplacePreviewRocket()
    {
        if (currentRocket != null)
        {
            Destroy(currentRocket); //Remove the existing preview rocket
        }

        if (selectedRocketPrefab == null)
            return; //Do nothing if no prefab is selected

        Vector3 spawnPosition = launchPoint.position + new Vector3(currentSpawnOffset, 0f, 0f); //Calculate the rocket spawn position

        currentRocket = Instantiate(selectedRocketPrefab, spawnPosition, Quaternion.identity); //Instantiate the new preview rocket
        currentRocket.transform.localScale = Vector3.one * currentSize; //Apply the chosen size

        Rocket rocketScript = currentRocket.GetComponent<Rocket>(); //Get the reference to the rocket movement script once again
        if (rocketScript != null)
        {
            rocketScript.SetSpeed(currentSpeed); //Apply the chosen speed
        }

        ApplyColor(currentRocket); //Apply the chosen color
    }

    //Launches the current rocket if launching is allowed when pressing the red launch button
    public void LaunchRocket()
    {
        if (!canSpawn || currentRocket == null)
            return; //Prevent launch if no rocket exists or one is already active/flying

        Rocket rocketScript = currentRocket.GetComponent<Rocket>(); //usual reference to rocket movement script
        if (rocketScript != null)
        {
            rocketScript.Launch(); //Start the rocket upward movement
        }

        onRocketLaunched.Invoke(); //Trigger all launch event listeners

        canSpawn = false; //Prevent new launches while current rocket is active by debilitating the possibility to spawn a new rocket
        StartCoroutine(WaitForRocketDeath(currentRocket)); //Start the rocket lifetime timer using a coroutine

        ApplyColor(currentRocket); //A safe check that reapplies the chosen color to ensure no mismatch between the preview rocket and the launched one
    }

    //This function simply calls the launching function to launch a rocket, but using the Input System instead
    public void LaunchFromInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LaunchRocket(); //Launch only once when the input (pressing spacebar) is performed
        }
    }

    //This is the coroutine that after a set time destroys the rocket and resets the system to allow a new rocket spawn
    IEnumerator WaitForRocketDeath(GameObject rocket)
    {
        yield return new WaitForSeconds(5f); //Wait 5 seconds before destroying the rocket

        if (rocket != null)
        {
            Destroy(rocket); //Remove the current rocket from the scene
        }

        onRocketDestroyed.Invoke(); //Trigger all the destroy event listeners

        cameraFollow.StopFollowing(); //Reset the camera position to its original position
        currentRocket = null;         //Clear the current rocket reference
        canSpawn = true;              //Allow a new rocket to be selected, instantiated and launched
    }
}