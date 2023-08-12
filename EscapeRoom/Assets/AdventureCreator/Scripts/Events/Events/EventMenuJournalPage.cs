using UnityEngine;

namespace AC
{

	public class EventMenuJournalPage : EventBase
	{

		[SerializeField] private AddRemove addRemove;
		private enum AddRemove { Add, Remove };


		public override string[] EditorNames { get { return new string[] { "Menu/Journal/Add page", "Menu/Journal/Remove page" }; } }
		protected override string EventName { get { return addRemove == AddRemove.Add ? "OnJournalPageAdd" : "OnJournalPageRemove"; } }
		protected override string ConditionHelp { get { return "Whenever a Journal page is " + ((addRemove == AddRemove.Add) ? "added." : "removed."); } }


		public override void Register ()
		{
			EventManager.OnJournalPageAdd += OnJournalPageAdd;
			EventManager.OnJournalPageRemove += OnJournalPageRemove;
		}


		public override void Unregister ()
		{
			EventManager.OnJournalPageAdd -= OnJournalPageAdd;
			EventManager.OnJournalPageRemove -= OnJournalPageRemove;
		}


		private void OnJournalPageAdd (MenuJournal journal, JournalPage page, int index)
		{
			if (addRemove == AddRemove.Add)
			{
				Run ();
			}
		}


		private void OnJournalPageRemove (MenuJournal journal, JournalPage page, int index)
		{
			if (addRemove == AddRemove.Remove)
			{
				Run ();
			}
		}


#if UNITY_EDITOR
		
		protected override bool HasConditions (bool isAssetFile) { return false; }


		public override void AssignVariant (int variantIndex)
		{
			addRemove = (AddRemove) variantIndex;
		}

#endif

	}

}