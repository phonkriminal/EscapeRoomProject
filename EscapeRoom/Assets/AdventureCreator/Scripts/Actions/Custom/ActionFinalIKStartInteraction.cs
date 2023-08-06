using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{
    [System.Serializable]
    public class ActionFinalIKStartInteraction : Action
    {
        // Declare properties here
        public override ActionCategory Category { get { return ActionCategory.Custom; } }
        public override string Title { get { return "Final IK Start Interaction"; } }
        public override string Description { get { return "This calls Final IK Start Interaction function"; } }


        public GameObject interactionSystem;
        public bool isPlayer;
        public int playerID = -1;
        public int playerParameterID = -1;
        public FullBodyBipedEffector effector;
        public GameObject interactionObject;
        public bool interrupt;
        public int constantID_is, constantID_io;
        public int parameterID_is, parameterID_io = -1;

       
        override public void AssignValues(List<ActionParameter> parameters)
        {
            if (isPlayer)
            {
                Player _player = AssignPlayer(playerID, parameters, playerParameterID);
                if (_player) { interactionSystem = _player.gameObject; }
                else { interactionSystem = null; }

            }
            else
            {
                interactionSystem = AssignFile(parameters, parameterID_is, constantID_is, interactionSystem);
            }
            interactionObject = AssignFile(parameters, parameterID_io, constantID_io, interactionObject);
        }

        override public float Run()
        {
            if (interactionSystem && interactionObject)
            {
                InteractionSystem interactionSystemScript = interactionSystem.GetComponent<InteractionSystem>();
                InteractionObject interactionObjectScript = interactionObject.GetComponent<InteractionObject>();
                if (interactionSystemScript && interactionObjectScript)
                {
                    interactionSystemScript.StartInteraction(effector, interactionObjectScript, interrupt);
                }
            }
            return 0f;
        }

#if UNITY_EDITOR
        override public void ShowGUI(List<ActionParameter> parameters)
        {
            isPlayer = EditorGUILayout.Toggle("Interaction System on Player:", isPlayer);
            if (isPlayer)
            {
                if (KickStarter.settingsManager != null && KickStarter.settingsManager.playerSwitching == PlayerSwitching.Allow)
                {
                    playerParameterID = ChooseParameterGUI("Player ID:", parameters, playerParameterID, ParameterType.Integer);
                    if (playerParameterID < 0)
                        playerID = ChoosePlayerGUI(playerID, true);
                }

                Player _player = null;

                if (playerParameterID < 0 || KickStarter.settingsManager == null || KickStarter.settingsManager.playerSwitching == PlayerSwitching.DoNotAllow)
                {
                    if (playerID >= 0)
                    {
                        PlayerPrefab playerPrefab = KickStarter.settingsManager.GetPlayerPrefab(playerID);
                        if (playerPrefab != null)
                        {
                            _player = (Application.isPlaying) ? playerPrefab.GetSceneInstance() : playerPrefab.EditorPrefab;
                        }
                    }
                    else
                    {
                        _player = (Application.isPlaying) ? KickStarter.player : AdvGame.GetReferences().settingsManager.PlayerPrefab.EditorPrefab;
                    }
                }
                if (_player)
                {
                    interactionSystem = _player.gameObject;
                    parameterID_is = -1;
                }
                else interactionSystem = null;
            }
            else
            {
                parameterID_is = Action.ChooseParameterGUI("Interaction System:", parameters, parameterID_is, ParameterType.GameObject);
                if (parameterID_is >= 0)
                {
                    constantID_is = 0;
                    interactionSystem = null;
                }
                else
                {
                    interactionSystem = (GameObject)EditorGUILayout.ObjectField("Interaction System:", interactionSystem, typeof(GameObject), true);
                    constantID_is = FieldToID(interactionSystem, constantID_is);
                    interactionSystem = IDToField(interactionSystem, constantID_is, true);
                }
            }
            effector = (FullBodyBipedEffector)EditorGUILayout.EnumPopup("Full Body Biped Effector:", effector);
            parameterID_io = Action.ChooseParameterGUI("Interaction Object:", parameters, parameterID_io, ParameterType.GameObject);
            if (parameterID_io >= 0)
            {
                constantID_io = 0;
                interactionObject = null;
            }
            else
            {
                interactionObject = (GameObject)EditorGUILayout.ObjectField("Interaction Object:", interactionObject, typeof(GameObject), true);
                constantID_io = FieldToID(interactionObject, constantID_io);
                interactionObject = IDToField(interactionObject, constantID_io, true);
            }
            interrupt = EditorGUILayout.Toggle("Interrupt:", interrupt);
            AfterRunningOption();
        }

        public override string SetLabel()
        {
            string labelAdd = "";
            if (interactionObject)
            {
                labelAdd = " " + effector + " on" + interactionObject.name;
            }
            return labelAdd;
        }
#endif
    }
}