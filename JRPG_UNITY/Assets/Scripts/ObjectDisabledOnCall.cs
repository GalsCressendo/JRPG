using Cysharp.Threading.Tasks;
using UnityEngine;


[DisallowMultipleComponent]
public class ObjectDisabledOnCall : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    private ObjectDisablerListener disablerListener;
    private GameObject registeredTarget;
    private bool isConnecting;

    private void Awake()
    {
        ConnectToListener();
    }

    private async void ConnectToListener()
    {
        if (disablerListener != null || isConnecting)
            return;

        isConnecting = true;
        await UniTask.WaitUntil(() => ObjectDisablerListener.Instance != null);

        if (this == null)
            return;

        disablerListener = ObjectDisablerListener.Instance;
        registeredTarget = obj != null ? obj : gameObject;
        isConnecting = false;
        disablerListener.Register(registeredTarget);
    }

    private void OnDestroy()
    {
        if (disablerListener != null && registeredTarget != null)
            disablerListener.Unregister(registeredTarget);
    }
}
