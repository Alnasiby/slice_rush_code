using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] fruitPrefabs;   // All fruits
    public GameObject bombPrefab;       // Single bomb prefab

    [Range(0f, 1f)]
    public float bombSpawnChance = 0.15f;  // 15% chance to spawn bomb

    private Collider spawnArea;

    [Header("Spawn Timing")]
    public float minSpawnDelay = 0.25f;
    public float maxSpawnDelay = 1f;

    [Header("Rotation")]
    public float minAngle = -15f;
    public float maxAngle = 15f;

    [Header("Force")]
    public float minForce = 18f;
    public float maxForce = 22f;

    public float maxLifetime = 5f;

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            // Random delay
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            GameObject prefab;

            // 🎯 Decide if it should spawn bomb or fruit
            float chance = Random.value;

            if (chance < bombSpawnChance)
            {
                prefab = bombPrefab; // spawn bomb
            }
            else
            {
                prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
            }

            // Random position
            Vector3 position = new Vector3(
                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
            );

            // Instantiate object
            GameObject obj = Instantiate(prefab, position, Quaternion.identity);

            // Random rotation
            float angle = Random.Range(minAngle, maxAngle);
            obj.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Apply upward force
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float force = Random.Range(minForce, maxForce);
                rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            }

            // Destroy after lifetime
            Destroy(obj, maxLifetime);
        }
    }
}
