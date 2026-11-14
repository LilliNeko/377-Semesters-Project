using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] float velocityMultiplier = 1.0f;
    bool isHolding = false;
    [SerializeField]
    float throwForce = 600f;
    [SerializeField]
    float maxThrowVelocity = 8f;
    [SerializeField]
    float maxDistance = 3f;
    float distance;
    Vector3 lastHeldPosition;
    Vector3 holdVelocity;

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
            distance = Vector3.Distance(this.transform.position, tempParent.transform.position);

            if (distance <= maxDistance)
            {
                isHolding = true;
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.detectCollisions = true;

                this.transform.SetParent(tempParent.transform);

                lastHeldPosition = transform.position;
                holdVelocity = Vector3.zero;
            }
        }
        else
        {
            Debug.Log("temp parent not found in scene");
        }
    }

    private void OnMouseUp()
    {
        Drop();
    }

    private void Hold()
    {
        distance = Vector3.Distance(this.transform.position, tempParent.transform.position);

        if (distance > maxDistance)
        {
            Drop();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Drop();
            rb.AddForce(tempParent.transform.forward * throwForce, ForceMode.Impulse);
        }
    }

    private void Drop()
    {

        if (isHolding)
        {
            isHolding = false;
            this.transform.SetParent(null);
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = holdVelocity * velocityMultiplier;
        }
    }

    private void LateUpdate()
    {
        if (isHolding)
        {
            holdVelocity = (transform.position - lastHeldPosition) / Time.deltaTime;
            holdVelocity = Vector3.ClampMagnitude(holdVelocity, maxThrowVelocity);

            lastHeldPosition = transform.position;
        }
    }
}
