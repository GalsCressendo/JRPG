using Cysharp.Threading.Tasks;
using UnityEngine;


[DisallowMultipleComponent]
public class PlayerMovementDisabler : MonoBehaviour
{
    private PlayerManager playerManager;
    private bool isConnecting;

    private void OnEnable()
    {
        ConnectToManager();
    }

    private void OnDisable()
    {
        playerManager?.SetMovementDisabled(this, false);
    }

    private async void ConnectToManager()
    {
        if (playerManager != null)
        {
            playerManager.SetMovementDisabled(this, true);
            return;
        }

        if (isConnecting)
            return;

        isConnecting = true;
        await UniTask.WaitUntil(() => PlayerManager.Instance != null);

        if (this == null)
            return;

        playerManager = PlayerManager.Instance;
        isConnecting = false;
        playerManager.SetMovementDisabled(this, isActiveAndEnabled);
    }

    private void OnDestroy()
    {
        playerManager?.SetMovementDisabled(this, false);
    }
}
