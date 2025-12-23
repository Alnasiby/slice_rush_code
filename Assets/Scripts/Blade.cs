using UnityEngine;

public class Blade : MonoBehaviour
{
    private bool slicing;
    private SphereCollider bladeCollider;
    private Camera mainCamera;
    private Vector3 previousPosition;
    private GameObject bladeVisual;
    private TrailRenderer trail;

    [Header("Blade Settings")]
    public float minSliceVelocity = 0.01f;
    public float zOffset = 10f;
    public Color bladeColor = Color.cyan;      
    public float trailTime = 0.25f;            
    public float bladeThickness = 0.05f;       
    public float bladeLength = 0.7f; 
              

    private void Awake()
    {
        mainCamera = Camera.main;

        bladeCollider = GetComponent<SphereCollider>();
        if (bladeCollider == null)
            bladeCollider = gameObject.AddComponent<SphereCollider>();
        bladeCollider.isTrigger = true;
        bladeCollider.enabled = false;

        bladeVisual = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        bladeVisual.transform.parent = transform;
        bladeVisual.transform.localPosition = Vector3.zero;
        bladeVisual.transform.localScale = new Vector3(bladeThickness, bladeLength / 2, bladeThickness);
        bladeVisual.GetComponent<Collider>().enabled = false;

        Material bladeMat = new Material(Shader.Find("Standard"));
        bladeMat.color = bladeColor;
        bladeVisual.GetComponent<MeshRenderer>().material = bladeMat;

        trail = gameObject.AddComponent<TrailRenderer>();
        trail.time = trailTime;
        trail.startWidth = bladeThickness * 2;
        trail.endWidth = 0f;
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.material.color = bladeColor;
        trail.enabled = false;
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Debug.Log($"Mouse Position: {mousePos}");
        // 🔍 DEBUG FOR ITCH.IO INPUT ISSUES
        if (Input.GetMouseButtonDown(0))
            Debug.Log("MouseDown at: " + Input.mousePosition);

        // Normal game logic
        if (Input.GetMouseButtonDown(0))
            StartSlicing();
        else if (Input.GetMouseButtonUp(0))
            StopSlicing();

        if (slicing)
            UpdateBlade();
    }

    private void StartSlicing()
    {
        slicing = true;
        bladeCollider.enabled = true;
        trail.Clear();
        trail.enabled = true;

        Vector3 mp = Input.mousePosition;
        mp.z = zOffset;                                
        previousPosition = mainCamera.ScreenToWorldPoint(mp);
    }

    private void StopSlicing()
    {
        slicing = false;
        bladeCollider.enabled = false;
        trail.enabled = false;
    }

    private void UpdateBlade()
    {
        Vector3 mp = Input.mousePosition;
        mp.z = zOffset;                                
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(mp);

        newPosition.z = 0; 
        transform.position = newPosition;

        float velocity = (newPosition - previousPosition).magnitude / Time.deltaTime;
        float scaleFactor = Mathf.Clamp(1 + velocity * 0.01f, 1f, 2f);

        bladeVisual.transform.localScale = new Vector3(bladeThickness * scaleFactor, bladeLength / 2, bladeThickness * scaleFactor);

        bladeCollider.enabled = velocity > minSliceVelocity;
        previousPosition = newPosition;
    }
}
