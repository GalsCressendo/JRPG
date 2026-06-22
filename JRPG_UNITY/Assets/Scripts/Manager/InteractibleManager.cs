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
    }

    public void OnUntargetInteractible(InteractibleComponent interactible)
    {
        if(ActiveInteractible == interactible)
        {
            ActiveInteractible = null;
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
