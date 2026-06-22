using PixelCrushers.DialogueSystem;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace GalsCressendo.JRPG
{
    public class DialogueSystemManager : MonoBehaviour
    {
        public static DialogueSystemManager Instance { get; private set; }

        private DialogueSystemController subscribedDialogueManager;
        private TransformDelegate conversationStartedHandler;
        private TransformDelegate conversationEndedHandler;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            else
            {
                if(Instance != this)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            UnregisterDialogueEvents();

            if(Instance == this)
            {
                Instance = null;
            }
        }

        public void TriggerDialogue(DialogueConversationData data)
        {
            if(DialogueManager.Instance == null)
            {
                Debug.LogError($"[Dialogue System Manager] Fail to play dialogue: Pixel Crusher Dialog Manager is not initiated");
                return;
            }

            if(data == null)
            {
                Debug.LogError($"[Dialogue System Manager] Fail to play dialogue: Conversation data is null");
                return;
            }

            UnregisterDialogueEvents();
            subscribedDialogueManager = DialogueManager.Instance;

            conversationStartedHandler = (transform) =>
            {
                subscribedDialogueManager.conversationStarted -= conversationStartedHandler;
                conversationStartedHandler = null;
                data.ConversationStart();
            };

            conversationEndedHandler = (transform) =>
            {
                UnregisterDialogueEvents();
                data.ConversationFinish();
            };

            subscribedDialogueManager.conversationStarted += conversationStartedHandler;
            subscribedDialogueManager.conversationEnded += conversationEndedHandler;
            subscribedDialogueManager.StartConversation(data.ConversationName);
        }

        private void UnregisterDialogueEvents()
        {
            if(subscribedDialogueManager != null)
            {
                if(conversationStartedHandler != null)
                {
                    subscribedDialogueManager.conversationStarted -= conversationStartedHandler;
                }

                if(conversationEndedHandler != null)
                {
                    subscribedDialogueManager.conversationEnded -= conversationEndedHandler;
                }
            }

            subscribedDialogueManager = null;
            conversationStartedHandler = null;
            conversationEndedHandler = null;
        }
    }

    [Serializable]
    public class DialogueConversationData
    {
        [SerializeField] string conversationName;
        [SerializeField] UnityEvent onConversationStart;
        [SerializeField] UnityEvent onConversationFinish;

        public string ConversationName => conversationName;

        public void ConversationStart()
        {
            onConversationStart?.Invoke();
        }

        public void ConversationFinish()
        {
            onConversationFinish?.Invoke();
        }
    }

}

