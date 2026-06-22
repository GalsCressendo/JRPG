using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractibleComponent : MonoBehaviour
{
    public UnityEvent onEnterTrigger = new UnityEvent();

    public UnityEvent onExitTrigger = new UnityEvent();

    [SerializeField] UnityEvent onInteract = new UnityEvent();

    private void OnDisable()
    {
        InteractibleManager.Instance?.OnUntargetInteractible(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Can trigger Interactions");

            InteractibleManager.Instance?.OnTargetInteractible(this);
            onEnterTrigger?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exit trigger Interactions");

            InteractibleManager.Instance?.OnUntargetInteractible(this);
            onExitTrigger?.Invoke();
        }
    }
}
