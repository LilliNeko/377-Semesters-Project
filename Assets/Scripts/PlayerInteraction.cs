using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TempParent tempParent;

    [SerializeField] private float interactionDistance = 3f;

    private InputAction interactAction;
    private InputAction throwAction;

    private PickUp heldObject;

    private void Awake()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        interactAction = playerInput.actions.FindAction("Interact");
        throwAction = playerInput.actions.FindAction("Throw");

        interactAction.started += OnInteractStarted;
        interactAction.canceled += OnInteractCanceled;
        throwAction.performed += OnThrow;
    }

    private void OnDisable()
    {
        interactAction.started -= OnInteractStarted;
        interactAction.canceled -= OnInteractCanceled;
        throwAction.performed -= OnThrow;
    }

    private void OnInteractStarted(InputAction.CallbackContext context)
    {
        if (heldObject != null)
            return;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            PickUp pickup = hit.collider.GetComponentInParent<PickUp>();

            if (pickup != null && pickup.TryPickup(tempParent))
            {
                heldObject = pickup;
            }
        }
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        if (heldObject != null)
        {
            heldObject.Drop();
            heldObject = null;
        }
    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        if (heldObject != null)
        {
            heldObject.Throw(playerCamera.transform.forward);
            heldObject = null;
        }
    }
}
