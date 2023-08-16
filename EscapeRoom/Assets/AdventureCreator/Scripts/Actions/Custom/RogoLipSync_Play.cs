using UnityEngine;
using System.Collections;
using RogoDigital.Lipsync;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{
    [System.Serializable]
    public class RogoLipSync_Play : Action
    {
        public bool isPlayer;
        public LipSync lipSyncTarget;
        public LipSyncData dataClip;
        public bool canInterrupt;

        public RogoLipSync_Play()
        {
            this.isDisplayed = true;
            category = ActionCategory.Custom;
            title = "Rogo LipSync - Play";
            description = "Starts playing a LipSyncData clip on a LipSync component";
            isPlayer = true;
            canInterrupt = true;
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

            if (dataClip == null)
            {
                Debug.LogWarning("RogoLipSync_Play: No LipSync clip defined.");
                return 0f;
            }

            lipSyncTarget.Play(dataClip);
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
            dataClip = (LipSyncData)EditorGUILayout.ObjectField("Clip:", dataClip, typeof(LipSyncData), true);
            canInterrupt = EditorGUILayout.Toggle("Can interrupt?", canInterrupt);
            AfterRunningOption();
        }
#endif
    }
}