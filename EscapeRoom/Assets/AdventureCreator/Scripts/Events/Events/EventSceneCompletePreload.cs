namespace AC
{

	public class EventSceneCompletePreload : EventBase
	{

		public override string[] EditorNames { get { return new string[] { "Scene/Complete preload" }; } }
		protected override string EventName { get { return "OnCompleteScenePreload"; } }
		protected override string ConditionHelp { get { return "Whenever a scene has completed preloading."; } }


		public override void Register ()
		{
			EventManager.OnCompleteScenePreload += OnCompleteScenePreload;
		}


		public override void Unregister ()
		{
			EventManager.OnCompleteScenePreload -= OnCompleteScenePreload;
		}


		private void OnCompleteScenePreload (string nextSceneName)
		{
			Run (new object[] { nextSceneName });
		}


		protected override ParameterReference[] GetParameterReferences ()
		{
			return new ParameterReference[]
			{
				new ParameterReference (ParameterType.String, "Scene name"),
			};
		}


#if UNITY_EDITOR

		protected override bool HasConditions (bool isAssetFile) { return false; }

#endif

	}

}