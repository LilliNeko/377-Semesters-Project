
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] float velocityMultiplier = 1f;
    [SerializeField] float throwForce = 6f;
    [SerializeField] float maxThrowVelocity = 8f;
    [SerializeField] float maxDistance = 3f;

    private bool isHolding = false;

    private TempParent tempParent;
    private Rigidbody rb;

    private Vector3 lastHeldPosition;
    private Vector3 holdVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isHolding)
            Hold();
    }

    public bool TryPickup(TempParent playerTempParent)
    {
        if (isHolding || playerTempParent == null)
            return false;

        tempParent = playerTempParent;

        float distance = Vector3.Distance(
            transform.position,
            tempParent.transform.position
        );

        if (distance > maxDistance)
            return false;

        isHolding = true;

        rb.isKinematic = true;
        rb.useGravity = false;
        rb.detectCollisions = true;

        transform.SetParent(tempParent.transform);

        lastHeldPosition = transform.position;
        holdVelocity = Vector3.zero;

        return true;
    }

    private void Hold()
    {
        float distance = Vector3.Distance(
            transform.position,
            tempParent.transform.position
        );

        if (distance > maxDistance)
            Drop();
    }

    public void Drop()
    {
        if (!isHolding)
            return;

        isHolding = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = holdVelocity * velocityMultiplier;
    }

    public void Throw(Vector3 direction)
    {
        if (!isHolding)
            return;

        Drop();

        rb.AddForce(direction * throwForce, ForceMode.Impulse);
    }

    private void LateUpdate()
    {
        if (!isHolding)
            return;

        holdVelocity = (transform.position - lastHeldPosition)
            / Time.deltaTime;

        holdVelocity = Vector3.ClampMagnitude(
            holdVelocity,
            maxThrowVelocity
        );

        lastHeldPosition = transform.position;
    }
}
