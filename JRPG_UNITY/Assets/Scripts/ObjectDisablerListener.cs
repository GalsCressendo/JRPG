using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectDisablerListener : MonoBehaviour
{
    private readonly HashSet<GameObject> disableObjects = new HashSet<GameObject>();
    private readonly HashSet<ObjectDisabler> activeDisablers = new HashSet<ObjectDisabler>();
    private readonly Dictionary<GameObject, bool> previousActiveStates = new Dictionary<GameObject, bool>();

    public static ObjectDisablerListener Instance { get; private set; }

    public UnityEvent<bool> onSetActive = new UnityEvent<bool>();

    public bool IsBlocked => activeDisablers.Count > 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Register(GameObject target)
    {
        if (target == null)
            return;

        disableObjects.Add(target);

        if (IsBlocked)
        {
            if (!previousActiveStates.ContainsKey(target))
                previousActiveStates[target] = target.activeSelf;

            target.SetActive(false);
        }
    }

    public void Unregister(GameObject target)
    {
        if (target == null)
            return;

        disableObjects.Remove(target);
        previousActiveStates.Remove(target);
    }

    public void SetDisablerActive(ObjectDisabler disabler, bool active)
    {
        if (disabler == null)
            return;

        bool wasBlocked = IsBlocked;
        RemoveDestroyedReferences();

        if (active)
            activeDisablers.Add(disabler);
        else
            activeDisablers.Remove(disabler);

        bool isBlocked = IsBlocked;
        if (wasBlocked == isBlocked)
            return;

        if (isBlocked)
            DisableRegisteredObjects();
        else
            RestoreRegisteredObjects();
    }

    private void DisableRegisteredObjects()
    {
        previousActiveStates.Clear();

        foreach (GameObject target in disableObjects)
        {
            if (target == null)
                continue;

            previousActiveStates[target] = target.activeSelf;
            target.SetActive(false);
        }

        onSetActive?.Invoke(false);
    }

    private void RestoreRegisteredObjects()
    {
        foreach (KeyValuePair<GameObject, bool> state in previousActiveStates)
        {
            if (state.Key != null)
                state.Key.SetActive(state.Value);
        }

        previousActiveStates.Clear();
        onSetActive?.Invoke(true);
    }

    private void RemoveDestroyedReferences()
    {
        activeDisablers.RemoveWhere(disabler => disabler == null);
        disableObjects.RemoveWhere(target => target == null);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
