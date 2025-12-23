using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Header("Whole & Sliced Prefabs")]
    public GameObject wholePrefab;
    public GameObject slicedPrefab;

    [Header("Juice Effect")]
    public ParticleSystem juiceEffect;

    [Header("Slice Settings")]
    public float explosionForce = 5f;
    public float explosionRadius = 1f;
    public float sliceLifetime = 2f;

    [Header("Score Settings")]
    public int pointValue = 1;   // ⭐ Different points per fruit

    private bool isSliced = false;

    private void Start()
    {
        if (juiceEffect != null)
        {
            juiceEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isSliced && other.CompareTag("Blade"))
        {
            SliceFruit();
        }
    }

    private void SliceFruit()
    {
        isSliced = true;

        // Disable whole fruit and enable sliced version
        if (wholePrefab != null)
            wholePrefab.SetActive(false);

        if (slicedPrefab != null)
            slicedPrefab.SetActive(true);

        // ⭐ Increase score based on fruit-specific value
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
            gm.IncreaseScore(pointValue);

        // Play juice effect
        if (juiceEffect != null)
        {
            juiceEffect.transform.position = transform.position;
            juiceEffect.Play();
        }

        // Add explosion force to sliced parts
        if (slicedPrefab != null)
        {
            Rigidbody[] slices = slicedPrefab.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in slices)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // Destroy after slice lifetime
        Destroy(gameObject, sliceLifetime);
    }
}
