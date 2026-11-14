using UnityEngine;

public class PickUp : MonoBehaviour
{
    bool isHolding = false;
    [SerializeField]
    float throwForce = 600f;
    [SerializeField]
    float maxDistance = 3f;
    float distance;

    TempParent tempParent;
    Rigidbody rb;

    Vector3 objectPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempParent = TempParent.Instance;
        Debug.Log(tempParent);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            Hold();
        }
    }

    private void OnMouseDown()
    {
        if (tempParent != null)
        {
            isHolding = true;
            rb.useGravity = false;
            rb.detectCollisions = true;

            this.transform.SetParent(tempParent.transform);
        }
        else
        {
            Debug.Log("temp parent not found in scene");
        }
    }

    private void OnMouseUp()
    {
        // Drop
    }

    private void OnMouseExit()
    {
        // Drop
    }

    private void Hold()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (Input.GetMouseButtonDown(1))
        {
            // Throw
        }
    }
}
