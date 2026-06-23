using Cysharp.Threading.Tasks;
using UnityEngine;


[DisallowMultipleComponent]
public class ObjectDisabler : MonoBehaviour
{
    private ObjectDisablerListener listener;
    private bool isConnecting;

    private void OnEnable()
    {
        ConnectToListener();
    }

    private void OnDisable()
    {
        listener?.SetDisablerActive(this, false);
    }

    private async void ConnectToListener()
    {
        if (listener != null)
        {
            listener.SetDisablerActive(this, true);
            return;
        }

        if (isConnecting)
            return;

        isConnecting = true;
        await UniTask.WaitUntil(() => ObjectDisablerListener.Instance != null);

        // UniTask continues when a component is disabled, so use its current
        // state rather than the state it had when this method started.
        if (this == null)
            return;

        listener = ObjectDisablerListener.Instance;
        isConnecting = false;
        listener.SetDisablerActive(this, isActiveAndEnabled);
    }

    private void OnDestroy()
    {
        listener?.SetDisablerActive(this, false);
    }
}
