using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionConversationCamera : Action
	{

		[SerializeField] private CharacterData characterDataA = new CharacterData ();
		[SerializeField] private CharacterData characterDataB = new CharacterData ();

		[SerializeField] private StopStart stopStart = StopStart.Start;
		private enum StopStart { Stop, Start };

		private ConversationCamera runtimeConversationCamera;
		[SerializeField] private ConversationCamera conversationCamera;
		[SerializeField] private int conversationCameraConstantID = 0;
		[SerializeField] private int conversationCameraParameterID = -1;

		[SerializeField] private bool setFirstshot = false;
		[SerializeField] private ConversationCamera.ShotType firstShotType;
		[SerializeField] private bool firstShotReversed = false;
		[SerializeField] private bool firstShotSetImmediately = false;


		public override ActionCategory Category { get { return ActionCategory.Camera; }}
		public override string Title { get { return "Conversation"; }}
		public override string Description { get { return "Configures the state of a conversation camera sequcence between two characters."; }}


		public override void AssignValues (List<ActionParameter> parameters)
		{
			runtimeConversationCamera = AssignFile<ConversationCamera> (parameters, conversationCameraParameterID, conversationCameraConstantID, conversationCamera);
		}


		public override float Run ()
		{
			if (runtimeConversationCamera == null)
			{
				LogWarning ("Cannot find ConversationCamera");
				return 0f;
			}

			switch (stopStart)
			{
				case StopStart.Start:
					if (setFirstshot)
					{
						runtimeConversationCamera.Begin (characterDataA.Character, characterDataB.Character, firstShotType, firstShotReversed, firstShotSetImmediately);
					}
					else
					{
						runtimeConversationCamera.Begin (characterDataA.Character, characterDataB.Character);
					}
					break;

				case StopStart.Stop:
					runtimeConversationCamera.End ();
					break;
			}

			return 0f;
		}


		public override void Skip ()
		{
			switch (stopStart)
			{
				case StopStart.Start:
					runtimeConversationCamera.Begin (characterDataA.Character, characterDataB.Character);
					break;

				case StopStart.Stop:
					runtimeConversationCamera.End (true);
					break;
			}
		}


		#if UNITY_EDITOR

		public override void ShowGUI (List<ActionParameter> parameters)
		{
			conversationCameraParameterID = Action.ChooseParameterGUI ("Conversation camera:", parameters, conversationCameraParameterID, ParameterType.GameObject);
			if (conversationCameraParameterID >= 0)
			{
				conversationCameraConstantID = 0;
				conversationCamera = null;
			}
			else
			{
				conversationCamera = (ConversationCamera) EditorGUILayout.ObjectField ("Conversation camera:", conversationCamera, typeof (ConversationCamera), true);

				conversationCameraConstantID = FieldToID<ConversationCamera> (conversationCamera, conversationCameraConstantID);
				conversationCamera = IDToField<ConversationCamera> (conversationCamera, conversationCameraConstantID, true);
			}

			stopStart = (StopStart) EditorGUILayout.EnumPopup ("Method:", stopStart);

			if (stopStart == StopStart.Start)
			{
				EditorGUILayout.LabelField ("Character A:");
				characterDataA.isPlayer = EditorGUILayout.Toggle ("Is player?", characterDataA.isPlayer);
				if (characterDataA.isPlayer)
				{
					characterDataA.playerID = ChoosePlayerGUI (characterDataA.playerID, true);
				}
				else
				{
					characterDataA.npc = (NPC) EditorGUILayout.ObjectField ("NPC:", characterDataA.npc, typeof (NPC), true);
				}

				EditorGUILayout.LabelField ("Character B:");
				characterDataB.isPlayer = EditorGUILayout.Toggle ("Is player?", characterDataB.isPlayer);
				if (characterDataB.isPlayer)
				{
					characterDataB.playerID = ChoosePlayerGUI (characterDataB.playerID, true);
				}
				else
				{
					characterDataB.npc = (NPC) EditorGUILayout.ObjectField ("NPC:", characterDataB.npc, typeof (NPC), true);
				}

				EditorGUILayout.Space ();
				setFirstshot = EditorGUILayout.Toggle ("Set first shot?", setFirstshot);
				if (setFirstshot)
				{
					firstShotType = (ConversationCamera.ShotType) EditorGUILayout.EnumPopup ("Shot type:", firstShotType);
					firstShotReversed = EditorGUILayout.Toggle ("Focus on Character B?", firstShotReversed);
					firstShotSetImmediately = EditorGUILayout.Toggle ("Apply immediately?", firstShotSetImmediately);
				}
			}
		}


		public override void AssignConstantIDs (bool saveScriptsToo, bool fromAssetFile)
		{
			if (saveScriptsToo)
			{
				AssignConstantID (conversationCamera, conversationCameraConstantID, conversationCameraParameterID);
			}
		}


		public override bool ReferencesPlayer (int _playerID = -1)
		{
			if (stopStart != StopStart.Start) return false;

			if (characterDataA.isPlayer)
			{
				if (_playerID < 0 || characterDataA.playerID < 0) return true;
				if (_playerID == characterDataA.playerID) return true;
			}

			if (characterDataB.isPlayer)
			{
				if (_playerID < 0 || characterDataB.playerID < 0) return true;
				if (_playerID == characterDataB.playerID) return true;
			}

			return false;
		}


		public override bool ReferencesObjectOrID (GameObject gameObject, int id)
		{
			if (conversationCameraParameterID < 0)
			{
				if (conversationCamera && conversationCamera.gameObject == gameObject) return true;
				if (conversationCameraConstantID == id && id != 0) return true;
			}
			return base.ReferencesObjectOrID (gameObject, id);
		}

		#endif


		[System.Serializable]
		private class CharacterData
		{

			public NPC npc = null;
			public bool isPlayer = false;
			public int playerID = -1;


			public Char Character
			{
				get
				{
					if (isPlayer)
					{
						if (KickStarter.settingsManager == null || KickStarter.settingsManager.playerSwitching == PlayerSwitching.DoNotAllow) return KickStarter.player;
						if (playerID < 0) return KickStarter.player;

						return KickStarter.settingsManager.GetPlayerPrefab (playerID).GetSceneInstance ();
					}

					return npc;
				}
			}

		}

	}

}