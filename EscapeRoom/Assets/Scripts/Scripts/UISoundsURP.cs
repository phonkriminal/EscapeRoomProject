using UnityEngine;
using UnityEngine.EventSystems;

namespace AC
{

	public class UISoundsURP : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
	{

		[SerializeField] private AudioClip hoverSound, clickSound = null;

		public void OnPointerEnter (PointerEventData eventData) { PlayHoverSound (); }
		public void OnPointerDown (PointerEventData eventData) { PlayClickSound (); }

		public void PlayHoverSound ()
		{
			PlaySound (hoverSound);
		}

		public void PlayClickSound ()
		{
			PlaySound (clickSound);
		}

		private void PlaySound (AudioClip clip)
		{
			if (clip && KickStarter.sceneSettings.defaultSound) KickStarter.sceneSettings.defaultSound.Play (clip, false);
		}

	}

}