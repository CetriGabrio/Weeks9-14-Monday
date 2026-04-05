using UnityEngine;
using UnityEngine.UI;
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
    private bool canSpawn = true;

    private GameObject selectedRocketPrefab;
    private float currentSize = 1f;
    private float currentSpeed = 5f;
    private float currentSpawnOffset = 0f;

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

        if (!canSpawn)
        {
            Debug.Log("Rocket already active!");
            return;
        }

        if (selectedRocketPrefab == null)
        {
            Debug.Log("No rocket selected.");
            return;
        }

        Vector3 spawnPosition = launchPoint.position + new Vector3(currentSpawnOffset, 0f, 0f);

        GameObject rocket = Instantiate(selectedRocketPrefab, spawnPosition, Quaternion.identity);

        rocket.transform.localScale = Vector3.one * currentSize;

        Rocket rocketScript = rocket.GetComponent<Rocket>();
        if (rocketScript != null)
        {
            rocketScript.SetSpeed(currentSpeed);
        }

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

        canSpawn = true;
    }
}