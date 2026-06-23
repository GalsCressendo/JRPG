using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    private readonly HashSet<PlayerMovementDisabler> movementDisablers = new HashSet<PlayerMovementDisabler>();

    public UnityEvent<bool> onSetActiveMovement = new UnityEvent<bool>();

    public static PlayerManager Instance { get; private set; }

    public bool IsMovementEnabled => movementDisablers.Count == 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        ApplyWhenPlayerIsReady();
    }

    public void SetMovementDisabled(PlayerMovementDisabler disabler, bool disabled)
    {
        if (disabler == null)
            return;

        movementDisablers.RemoveWhere(item => item == null);

        if (disabled)
            movementDisablers.Add(disabler);
        else
            movementDisablers.Remove(disabler);

        RefreshMovementState();
    }

    public void RefreshMovementState()
    {
        bool movementEnabled = IsMovementEnabled;

        if (Player.Instance != null)
            Player.Instance.SetMovementEnabled(movementEnabled);

        onSetActiveMovement?.Invoke(movementEnabled);
    }

    private async void ApplyWhenPlayerIsReady()
    {
        await UniTask.WaitUntil(() => Player.Instance != null);

        if (this != null)
            RefreshMovementState();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
