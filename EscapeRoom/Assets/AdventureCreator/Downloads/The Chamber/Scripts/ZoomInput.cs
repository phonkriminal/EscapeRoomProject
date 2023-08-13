#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
#define MOBILE_INPUT
#endif
using UnityEngine;

namespace AC.TheChamber
{

	public class ZoomInput : MonoBehaviour
	{

		#region Variables

		[Header("Pinching")]
		[SerializeField] private float pinchThreshold = 0.6f;
		[SerializeField] private float sphereCastRadius = 0.2f;
		private bool isPinching = false;
		private float startPinchDistance;

		[Header("Zooming")]
		[SerializeField] private AnimationCurve zoomCurve = new AnimationCurve (new Keyframe(-1, -1), new Keyframe(1, 1));
		[SerializeField] private ActionListAsset switchCameraActionList = null;
		[SerializeField] private float zoomFactor = 5f;
		private LerpUtils.FloatLerp zoomLerp = new LerpUtils.FloatLerp ();
		private float zoomOffset;
		private const float zoomSpeed = 5f;
		private LayerMask hotspotLayerMask;
		private Vector3 zoomScaler;
		private float relativePinchDistance;

		private ZoomHotspot activeZoomHotspot;
		private const int inventoryCameraID = 32618;

		[SerializeField] private ExitZoomMenu exitZoomMenu = null;

		#endregion


		#region UnityStandards

		private void Start ()
		{
			zoomScaler = Vector3.forward * -0.02f;
			hotspotLayerMask = 1 << LayerMask.NameToLayer (KickStarter.settingsManager.hotspotLayer);
		}


		private void OnEnable ()
		{
			EventManager.OnSwitchCamera += OnSwitchCamera;
		}


		private void OnDisable ()
		{
			EventManager.OnSwitchCamera -= OnSwitchCamera;
		}


		private void LateUpdate ()
		{
			if (!KickStarter.stateHandler.IsInGameplay () || KickStarter.stateHandler.AreCamerasDisabled ())
			{
				isPinching = false;
				SetZoomAmount (0f);
				return;
			}

			if (Input_StartPinch)
			{
				isPinching = true;
				startPinchDistance = Input_PinchDistance;
			}
			
			if (isPinching)
			{
				if (Input_EndPinch)
				{
					isPinching = false;

					if (relativePinchDistance >= 1f)
					{
						ActivateHotspot (ACScreen.safeArea.center);
					}
					else if (relativePinchDistance <= -1f)
					{
						ExitCurrentCamera ();
					}
				}
				else
				{
					float pinchDistance = (Input_PinchDistance - startPinchDistance) / Screen.dpi;
					relativePinchDistance = Mathf.Clamp (pinchDistance / pinchThreshold, -1f, 1f);
				}

				SetZoomAmount ((isPinching) ? -relativePinchDistance : 0f);
			}
			else
			{
				SetZoomAmount (0f);
			}
		}


		public void SetActiveZoomHotspot (ZoomHotspot zoomHotspot)
		{
			activeZoomHotspot = zoomHotspot;

			if (exitZoomMenu != null)
			{
				exitZoomMenu.OnSetActiveZoomHotspot (zoomHotspot);
			}
		}

		#endregion


		#region CustomEvents

		private void OnSwitchCamera (_Camera fromCamera, _Camera toCamera, float transitionTime)
		{
			if (!fromCamera)
			{
				return;
			}

			GameCameraThirdPerson fromTPCam = fromCamera as GameCameraThirdPerson;
			GameCameraThirdPerson toTPCam = toCamera as GameCameraThirdPerson;
			
			ConstantID fromID = fromCamera.GetComponent <ConstantID>();
			ConstantID toID = toCamera.GetComponent<ConstantID> ();

			bool moveFromCamera = true;
			if (toTPCam)
			{
				if (fromID == null || fromID.constantID != inventoryCameraID)
				{
					toTPCam.SetInitialDirection ();
				}
			}
			
			if ((toID && toID.constantID == inventoryCameraID) ||
				(fromID && fromID.constantID == inventoryCameraID))
			{
				moveFromCamera = false;
			}

			if (moveFromCamera && fromTPCam)
			{
				fromTPCam.BeginAutoMove (1f, fromTPCam.GenerateTargetAngles (toCamera.Camera.transform), false);
			}
		}

		#endregion


		#region PrivateFunctions

		private void ActivateHotspot (Vector2 screenPosition)
		{
			Ray ray = KickStarter.CameraMain.ScreenPointToRay (screenPosition);
			RaycastHit raycastHit;

			if (Physics.SphereCast (ray, sphereCastRadius, out raycastHit, KickStarter.settingsManager.hotspotRaycastLength, hotspotLayerMask))
			{
				Hotspot hotspot = raycastHit.collider.GetComponent<Hotspot> ();
				if (hotspot != null && hotspot.doubleClickingHotspot == DoubleClickingHotspot.IsRequiredToUse)
				{
					hotspot.RunUseInteraction ();
				}
			}
		}


		private void SetZoomAmount (float amount)
		{
			float targetFOVOffset = zoomCurve.Evaluate (amount) * zoomFactor;
			zoomOffset = zoomLerp.Update (zoomOffset, targetFOVOffset, zoomSpeed);

			GameCameraData currentFrameCameraData = KickStarter.mainCamera.CurrentFrameCameraData;
			if (currentFrameCameraData != null)
			{
				KickStarter.CameraMain.transform.position += currentFrameCameraData.rotation * zoomScaler * zoomOffset;
			}
		}


		private void ExitCurrentCamera ()
		{
			if (activeZoomHotspot)
			{
				activeZoomHotspot.ZoomOut ();
			}
		}

		#endregion


		#region GetSet

		public static ActionListAsset SwitchCameraActionList
		{
			get
			{
				return Instance.switchCameraActionList;
			}
		}


		public ZoomHotspot ActiveZoomHotspot
		{
			get
			{
				return activeZoomHotspot;
			}
		}


		private float Input_PinchDistance
		{
			get
			{
				#if MOBILE_INPUT
				return (Input.GetTouch (0).position - Input.GetTouch (1).position).magnitude;
				#else
				return Input.mousePosition.y;
				#endif
			}
		}


		private bool Input_StartPinch
		{
			get
			{
				if (isPinching) return false;

				#if MOBILE_INPUT
				return Input.touchCount == 2 && Input.GetTouch (1).phase == TouchPhase.Began;
				#else
				return Input.GetMouseButtonDown (2);
				#endif
			}
		}


		private bool Input_EndPinch
		{
			get
			{
				if (!isPinching) return false;
				
				#if MOBILE_INPUT
				return (Input.touchCount != 2);
				#else
				return !Input.GetMouseButton (2);
				#endif
			}
		}

		#endregion


		#region Singleton

		private static ZoomInput instance;
		public static ZoomInput Instance
		{
			get
			{
				if (instance == null) instance = FindObjectOfType<ZoomInput> ();
				return instance;
			}
		}

		#endregion

	}

}