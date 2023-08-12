using UnityEngine;

namespace AC
{

	public class EventObjectiveUpdate : EventBase
	{

		[SerializeField] private int objectiveID = -1;


		public override string[] EditorNames { get { return new string[] { "Objective/Update" }; } }
		protected override string EventName { get { return "OnObjectiveUpdate"; } }
		protected override string ConditionHelp { get { return "Whenever " + ((objectiveID >= 0) ? GetObjectiveName () : "an Objective") + " is updated."; } }


		public override void Register ()
		{
			EventManager.OnObjectiveUpdate += OnObjectiveUpdate;
		}


		public override void Unregister ()
		{
			EventManager.OnObjectiveUpdate -= OnObjectiveUpdate;
		}


		private void OnObjectiveUpdate (Objective objective, ObjectiveState state)
		{
			if (objectiveID < 0 || objectiveID == objective.ID)
			{
				Run ();
			}
		}


		private string GetObjectiveName ()
		{
			if (KickStarter.inventoryManager)
			{
				Objective objective = KickStarter.inventoryManager.GetObjective (objectiveID);
				if (objective != null) return "objective '" + objective.Title + "'";
			}
			return "objective " + objectiveID;
		}


#if UNITY_EDITOR

		protected override bool HasConditions (bool isAssetFile) { return true; }


		protected override void ShowConditionGUI (bool isAssetFile)
		{
			if (KickStarter.inventoryManager)
			{
				objectiveID = ActionRunActionList.ShowObjectiveSelectorGUI ("Objective:", KickStarter.inventoryManager.objectives, objectiveID);
			}
			else
			{
				objectiveID = CustomGUILayout.IntField ("Objective ID:", objectiveID);
			}
		}

#endif

	}

}