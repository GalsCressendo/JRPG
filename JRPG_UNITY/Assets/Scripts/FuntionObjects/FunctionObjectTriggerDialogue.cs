using UnityEngine;
using GalsCressendo.JRPG;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "FunctionObjectTriggerDialogue", menuName = "GalsCressendo/FunctionObject/TriggerDialogue")]
public class FunctionObjectTriggerDialogue : FunctionObjects
{
    [SerializeField] DialogueConversationData conversationData;

    public override void DoAction()
    {
        Debug.Log($"{this.name} Triggering conversation ");
        TriggerConversation();
    }

    private async void TriggerConversation()
    {
        await UniTask.WaitUntil(() => DialogueSystemManager.Instance != null);

        if (conversationData == null)
            return;

        DialogueSystemManager.Instance.TriggerDialogue(conversationData);
    }
}
