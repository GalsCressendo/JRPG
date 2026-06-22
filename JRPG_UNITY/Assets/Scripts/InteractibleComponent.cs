using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractibleComponent : MonoBehaviour
{
    [SerializeField] GameObject interactiblePrompt = null;

    public UnityEvent onEnterTrigger = new UnityEvent();

    public UnityEvent onExitTrigger = new UnityEvent();

    [SerializeField] UnityEvent onInteract = new UnityEvent();

    bool isInteractible = false; 

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

    public void SetActiveInteractible(bool active)
    {
        if(interactiblePrompt != null)
        {
            interactiblePrompt.SetActive(active);
        }

        isInteractible = active;
    }

    public void Interact()
    {
        if(!isInteractible) return;

        onInteract?.Invoke();
    }
}
