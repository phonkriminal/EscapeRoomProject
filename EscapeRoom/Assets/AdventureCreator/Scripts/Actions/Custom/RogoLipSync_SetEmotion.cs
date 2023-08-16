using UnityEngine;
using System.Collections;
using RogoDigital.Lipsync;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{
    [System.Serializable]
    public class RogoLipSync_SetEmotion : Action
    {
        public bool isPlayer;
        public LipSync lipSyncTarget;
        public string emotion;
        public float blendTime;


        public RogoLipSync_SetEmotion()
        {
            this.isDisplayed = true;
            category = ActionCategory.Custom;
            title = "Rogo LipSync - Set Emotion";
            description = "Blends a LipSync emotion on a LipSync component";
            isPlayer = true;

        }

        override public float Run()
        {
            if (isPlayer)
            {
                lipSyncTarget = KickStarter.player.GetComponent<LipSync>();

                if (lipSyncTarget == null)
                {
                    Debug.LogWarning("RogoLipSync_Play: No LipSync component found on Player.");
                    return 0f;
                }
            }

            else if (lipSyncTarget == null)
            {
                Debug.LogWarning("RogoLipSync_Play: No LipSync component defined.");
                return 0f;
            }

            lipSyncTarget.SetEmotion(emotion, blendTime);
            return 0f;
        }


        override public void Skip()
        {

        }



#if UNITY_EDITOR
        override public void ShowGUI()
        {
            isPlayer = EditorGUILayout.Toggle("Is player?", isPlayer);

            if (!isPlayer)
            {
                lipSyncTarget = (LipSync)EditorGUILayout.ObjectField("Character:", lipSyncTarget, typeof(LipSync), true);
            }

            emotion = EditorGUILayout.TextField("Emotion:", emotion);
            blendTime = EditorGUILayout.Slider("Blend Time:", blendTime, 0f, 5f);
            AfterRunningOption();
        }
#endif
    }
}