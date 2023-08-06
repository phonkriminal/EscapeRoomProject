#if UNITY_EDITOR

using UnityEditor;

namespace AC
{

	[CustomEditor (typeof (RememberSound), true)]
	public class RememberSoundEditor : ConstantIDEditor
	{

		public override void OnInspectorGUI ()
		{
			RememberSound _target = (RememberSound) target;

			_target.saveClip = CustomGUILayout.ToggleLeft ("Save change in AudioClip asset?", _target.saveClip, "If True, the currently-playing clip asset will be saved and restored.");

			SharedGUI ();
		}

	}

}

#endif