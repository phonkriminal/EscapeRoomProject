using UnityEngine;
using System.Collections;
using RogoDigital.Lipsync;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{
    [System.Serializable]
    public class RogoLipSync_Stop : Action
    {
        public bool isPlayer;
        public LipSync lipSyncTarget;
        private LipSyncData dataClip = null;

        public RogoLipSync_Stop()
        {
            this.isDisplayed = true;
            category = ActionCategory.Custom;
            title = "Rogo LipSync - Stop";
            description = "Stops a LipSyncData clip on a LipSync component when a LipSync animation is already playing.";
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

            lipSyncTarget.Stop(dataClip);
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
            AfterRunningOption();
        }
#endif
    }
}