using UnityEngine;

public class InteractibleManager : MonoBehaviour
{
    public static InteractibleManager Instance { get; private set; }

    public InteractibleComponent ActiveInteractible { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance.gameObject);
        }
        else
        {
            if(Instance!=this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void OnTargetInteractible(InteractibleComponent interactible)
    {
        ActiveInteractible = interactible;

        ActiveInteractible.SetActiveInteractible(true);
    }

    public void OnUntargetInteractible(InteractibleComponent interactible)
    {
        if(ActiveInteractible == interactible)
        {
            ActiveInteractible.SetActiveInteractible(false);

            ActiveInteractible = null;
        }
    }

    public void TriggerInteraction()
    {
        if(ActiveInteractible != null)
        {
            ActiveInteractible.Interact();
        }
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            ActiveInteractible = null;
            Instance = null;
        }
    }
}
