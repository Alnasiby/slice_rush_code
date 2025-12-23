using UnityEngine;

public class Bomb : MonoBehaviour
{
    private bool exploded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Blade"))
        {
            exploded = true;
            Debug.Log("Boom! Game Over!");

            // Call GameManager to handle game over
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
                gm.GameOver();

            // Optionally disable bomb object immediately
            gameObject.SetActive(false);
        }
    }
}
