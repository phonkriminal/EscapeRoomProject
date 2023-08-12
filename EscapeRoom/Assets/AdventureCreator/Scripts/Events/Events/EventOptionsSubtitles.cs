namespace AC
{

	public class EventOptionsSubtitles : EventBase
	{


		public override string[] EditorNames { get { return new string[] { "Options/Change subtitles" }; } }
		protected override string EventName { get { return "OnChangeSubtitles"; } }
		protected override string ConditionHelp { get { return "Whenever Subtitles are toggled."; } }


		public override void Register ()
		{
			EventManager.OnChangeSubtitles += OnChangeSubtitles;
		}


		public override void Unregister ()
		{
			EventManager.OnChangeSubtitles -= OnChangeSubtitles;
		}


		private void OnChangeSubtitles (bool showSubtitles)
		{
			Run (new object[] { showSubtitles });
		}


		protected override ParameterReference[] GetParameterReferences ()
		{
			return new ParameterReference[]
			{
				new ParameterReference (ParameterType.Boolean, "Subtitles on"),
			};
		}


#if UNITY_EDITOR

		protected override bool HasConditions (bool isAssetFile) { return false; }

#endif

	}

}