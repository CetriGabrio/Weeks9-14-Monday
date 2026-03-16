using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    bool canSpawn = true;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && canSpawn)
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    IEnumerator SpawnRoutine()
    {
        canSpawn = false;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;

        GameObject obj = Instantiate(prefab, worldPos, Quaternion.identity);

        growing grow = obj.GetComponent<growing>();

        yield return StartCoroutine(grow.Grow());

        canSpawn = true;
    }
}