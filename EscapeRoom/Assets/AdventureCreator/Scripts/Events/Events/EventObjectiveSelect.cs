using UnityEngine;

namespace AC
{

	public class EventObjectiveSelect : EventBase
	{

		[SerializeField] private int objectiveID = -1;


		public override string[] EditorNames { get { return new string[] { "Objective/Select" }; } }
		protected override string EventName { get { return "OnObjectiveSelect"; } }
		protected override string ConditionHelp { get { return "Whenever " + ((objectiveID >= 0) ? GetObjectiveName () : "an Objective") + " is selected."; } }


		public override void Register ()
		{
			EventManager.OnObjectiveSelect += OnObjectiveSelect;
		}


		public override void Unregister ()
		{
			EventManager.OnObjectiveSelect -= OnObjectiveSelect;
		}


		private void OnObjectiveSelect (Objective objective, ObjectiveState state)
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