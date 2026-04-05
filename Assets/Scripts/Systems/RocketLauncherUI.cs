using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public class RocketLauncherUI : MonoBehaviour
{
    [Header("Rocket Prefabs")]
    public GameObject[] rocketPrefabs;

    [Header("UI Sliders")]
    public Slider sizeSlider;
    public Slider speedSlider;
    public Slider spawnSlider;

    [Header("Spawn")]
    public Transform launchPoint;

    [Header("Camera")]
    public CameraFollow cameraFollow;

    [Header("Launch Event")]
    public UnityEvent onRocketLaunched;

    [Header("Destroy Event")]
    public UnityEvent onRocketDestroyed;

    private bool canSpawn = true;

    private GameObject selectedRocketPrefab;
    private GameObject currentRocket;

    private float currentSize = 1f;
    private float currentSpeed = 5f;
    private float currentSpawnOffset = 0f;

    public GameObject GetCurrentRocket()
    {
        return currentRocket;
    }

    public void SelectRocket(int index)
    {
        if (!canSpawn)
            return;

        selectedRocketPrefab = rocketPrefabs[index];
        CreateOrReplacePreviewRocket();
    }

    public void SetSize(float value)
    {
        currentSize = value;

        if (currentRocket != null)
        {
            currentRocket.transform.localScale = Vector3.one * currentSize;
        }
    }
    public void SetSpeed(float value)
    {
        currentSpeed = value;

        if (currentRocket != null)
        {
            Rocket rocketScript = currentRocket.GetComponent<Rocket>();
            if (rocketScript != null)
            {
                rocketScript.SetSpeed(currentSpeed);
            }
        }
    }

    public void SetSpawnOffset(float value)
    {
        currentSpawnOffset = value;

        if (currentRocket != null)
        {
            currentRocket.transform.position = launchPoint.position + new Vector3(currentSpawnOffset, 0f, 0f);
        }
    }

    void CreateOrReplacePreviewRocket()
    {
        if (currentRocket != null)
        {
            Destroy(currentRocket);
        }

        if (selectedRocketPrefab == null)
            return;

        Vector3 spawnPosition = launchPoint.position + new Vector3(currentSpawnOffset, 0f, 0f);

        currentRocket = Instantiate(selectedRocketPrefab, spawnPosition, Quaternion.identity);
        currentRocket.transform.localScale = Vector3.one * currentSize;

        Rocket rocketScript = currentRocket.GetComponent<Rocket>();
        if (rocketScript != null)
        {
            rocketScript.SetSpeed(currentSpeed);
        }
    }

    public void LaunchFromInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LaunchRocket();
        }
    }

    public void LaunchRocket()
    {
        if (!canSpawn || currentRocket == null)
            return;

        Rocket rocketScript = currentRocket.GetComponent<Rocket>();
        if (rocketScript != null)
        {
            rocketScript.Launch();
        }

        onRocketLaunched.Invoke();

        canSpawn = false;
        StartCoroutine(WaitForRocketDeath(currentRocket));
    }

    IEnumerator WaitForRocketDeath(GameObject rocket)
    {
        yield return new WaitForSeconds(5f);

        if (rocket != null)
        {
            Destroy(rocket);
        }

        onRocketDestroyed.Invoke();

        cameraFollow.StopFollowing();
        currentRocket = null;
        canSpawn = true;
    }
}