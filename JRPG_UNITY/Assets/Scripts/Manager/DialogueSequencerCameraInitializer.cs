using UnityEngine;

namespace GalsCressendo.JRPG
{
    [DisallowMultipleComponent]
    public sealed class DialogueSequencerCameraInitializer : MonoBehaviour
    {
        private void Awake()
        {
            Camera gameplayCamera = Camera.main;

            if(gameplayCamera == null || gameplayCamera.gameObject == gameObject)
            {
                return;
            }

            transform.SetPositionAndRotation(gameplayCamera.transform.position,gameplayCamera.transform.rotation);
        }
    }
}
