using UnityEngine;

namespace AC
{

	public class EventPointClick : EventBase
	{

		public override string[] EditorNames { get { return new string[] { "Character/Point and click" }; } }
		protected override string EventName { get { return "OnPointAndClick"; } }
		protected override string ConditionHelp { get { return "Whenever the Player moves via Point and Click."; } }


		public override void Register ()
		{
			EventManager.OnPointAndClick += OnPointAndClick;
		}


		public override void Unregister ()
		{
			EventManager.OnPointAndClick -= OnPointAndClick;
		}


		private void OnPointAndClick (Vector3[] pointArray, bool isRunning)
		{
			if (pointArray.Length > 1)
				Run (new object[] { pointArray[pointArray.Length - 1], isRunning }); ;
		}


		protected override ParameterReference[] GetParameterReferences ()
		{
			return new ParameterReference[]
			{
				new ParameterReference (ParameterType.Vector3, "Destination"),
				new ParameterReference (ParameterType.Boolean, "Is running"),
			};
		}


#if UNITY_EDITOR

		protected override bool HasConditions (bool isAssetFile) { return false; }

#endif

	}

}