using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

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
        selectedRocketPrefab = rocketPrefabs[index];
    }

    public void SetSize(float value)
    {
        currentSize = value;
    }

    public void SetSpeed(float value)
    {
        currentSpeed = value;
    }

    public void SetSpawnOffset(float value)
    {
        currentSpawnOffset = value;
    }

    public void LaunchRocket()
    {
        if (!canSpawn || selectedRocketPrefab == null)
            return;

        Vector3 spawnPosition = launchPoint.position + new Vector3(currentSpawnOffset, 0f, 0f);

        GameObject rocket = Instantiate(selectedRocketPrefab, spawnPosition, Quaternion.identity);

        currentRocket = rocket;

        rocket.transform.localScale = Vector3.one * currentSize;

        Rocket rocketScript = rocket.GetComponent<Rocket>();
        if (rocketScript != null)
        {
            rocketScript.SetSpeed(currentSpeed);
        }

        onRocketLaunched.Invoke();

        canSpawn = false;
        StartCoroutine(WaitForRocketDeath(rocket));
    }

    IEnumerator WaitForRocketDeath(GameObject rocket)
    {
        yield return new WaitForSeconds(5f);

        if (rocket != null)
        {
            Destroy(rocket);
        }

        cameraFollow.StopFollowing();
        currentRocket = null;
        canSpawn = true;
    }
}