using System.Collections;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	public class ConversationCamera : MonoBehaviour
	{

		#region Variables

		[SerializeField] private float minShotLength = 3f;
		[SerializeField] private bool extendCutscenesToFitMinShotLength = true;
		[SerializeField] private FloatRange cutDelay = new FloatRange (0f, 2f);
		[SerializeField] private float animatedShotFrequency = 0.6f;
		[SerializeField] private float maxAnimationDuration = 5f;
		[SerializeField] private bool autoTurnHeads = true;
		[SerializeField] private bool moveWithCharacters = false;
		[SerializeField] private LayerMask collisionMask = new LayerMask ();
		[SerializeField] private float distanceFactor = 1f;
		[SerializeField] private float fovFactor = 1f;

		private CharacterData characterDataA;
		private CharacterData characterDataB;

		private Coroutine focusOnCharacterCo;
		private bool isRunning;
		private bool isSleeping;
		private _Camera ownCamera;
		public enum ShotType { Centred=0, CloseUp=1, OverTheShoulder=2 };

		private float lastShotTime;
		private int lastShotType = 1;
		private CameraShot activeCameraShot, firstShot;
		private bool characterAOnRight;

		#endregion


		#region UnityStandards

		private void OnEnable ()
		{
			ownCamera = GetComponent <_Camera>();

			EventManager.OnStartSpeech_Alt += OnStartSpeech;
			EventManager.OnSwitchCamera += OnSwitchCamera;
		}


		private void OnDisable ()
		{
			EventManager.OnStartSpeech_Alt -= OnStartSpeech;
			EventManager.OnSwitchCamera -= OnSwitchCamera;
		}


		private void OnValidate ()
		{
			cutDelay.OnValidate ();
		}


		private void LateUpdate ()
		{
			if (activeCameraShot != null)
			{
				if (moveWithCharacters)
				{
					bool isReverseShot = activeCameraShot.IsReverseShot;
					CameraShot newShot = CreateShot (activeCameraShot.ShotType, (isReverseShot) ? characterDataB : characterDataA, (isReverseShot) ? characterDataA : characterDataB, isReverseShot);
					if (newShot != null)
					{
						activeCameraShot.Apply (ownCamera, newShot);
					}
				}

				bool endAnim = (Time.time - lastShotTime) > maxAnimationDuration;
				if (!endAnim)
				{
					activeCameraShot.Update (transform);
				}
			}
		}

		#endregion


		#region PublicFunctions

		public void Begin (Char _characterA, Char _characterB)
		{
			if (_characterA == null || _characterB == null) return;

			isRunning = true;
			activeCameraShot = null;
			firstShot = null;
			lastShotType = -1;

			characterDataA = new CharacterData (_characterA);
			characterDataB = new CharacterData (_characterB);

			Vector3 characterAWorldPosition = characterDataA.Character.Transform.position;
			Vector2 characterAScreenPosition = Camera.main.WorldToScreenPoint (characterAWorldPosition);

			Vector3 characterBWorldPosition = characterDataB.Character.Transform.position;
			Vector2 characterBScreenPosition = Camera.main.WorldToScreenPoint (characterBWorldPosition);

			characterAOnRight = (characterAScreenPosition.x > characterBScreenPosition.x);

			if (autoTurnHeads)
			{
				characterDataA.LookAt (characterDataB);
				characterDataB.LookAt (characterDataA);
			}
		}


		public void Begin (Char _characterA, Char _characterB, ShotType shotType, bool isReverse, bool setImmediately)
		{
			Begin (_characterA, _characterB);

			if (!isRunning)
			{
				return;
			}
			
			CameraShot newShot = CreateShot (shotType, (isReverse) ? characterDataB : characterDataA, (isReverse) ? characterDataA : characterDataB, isReverse);
			if (newShot == null)
			{
				return;
			}

			firstShot = newShot;
			if (setImmediately)
			{
				activeCameraShot = newShot;
				activeCameraShot.Apply (ownCamera);

				lastShotTime = Time.time;
			}
		}


		public void End (bool isSkipping = false)
		{
			if (!isRunning) return;

			isRunning = false;
			
			if (focusOnCharacterCo != null) StopCoroutine (focusOnCharacterCo);

			if (autoTurnHeads)
			{
				characterDataA.StopLooking (isSkipping);
				characterDataB.StopLooking (isSkipping);
			}

			if (KickStarter.mainCamera.attachedCamera == ownCamera)
			{
				if (isSkipping)
				{
					KickStarter.mainCamera.SetGameCamera (KickStarter.mainCamera.GetLastGameplayCamera ());
				}
				else if (extendCutscenesToFitMinShotLength)
				{
					AddEndPause ();
				}
				else
				{
					KickStarter.mainCamera.SetGameCamera (KickStarter.mainCamera.GetLastGameplayCamera ());
				}
			}

			characterDataA = null;
			characterDataB = null;

			activeCameraShot = null;
		}

		#endregion


		#region CustomEvents

		private void OnStartSpeech (Speech speech)
		{
			if (isRunning && !isSleeping)
			{
				if (IsReferenced (speech.GetSpeakingCharacter ()))
				{
					if (focusOnCharacterCo != null) StopCoroutine (focusOnCharacterCo);
					focusOnCharacterCo = StartCoroutine (FocusOnCharacterCo (speech.GetSpeakingCharacter ()));
				}
			}
		}


		private void OnSwitchCamera (_Camera oldCamera, _Camera newCamera, float transitionTime)
		{
			if (isRunning)
			{
				isSleeping = (newCamera != ownCamera);
				if (!isSleeping)
				{
					lastShotTime = Time.time;
				}
			}
			else
			{
				isSleeping = false;
			}
		}

		#endregion


		#region PrivateFunctions

		private IEnumerator FocusOnCharacterCo (Char character)
		{
			if (character != null)
			{
				float delayBeforeCut = 0f;
				
				if (activeCameraShot != null)
				{
					delayBeforeCut = cutDelay.GetRandomValue ();
					delayBeforeCut = Mathf.Max (delayBeforeCut, minShotLength + lastShotTime - Time.time);
				}
				yield return new WaitForSeconds (delayBeforeCut);
				
				bool isReverseShot = (character == characterDataB.Character);
				
				CameraShot newShot = firstShot;
				if (firstShot != null)
				{
					firstShot = null;
				}
				
				bool doSkip = false;
				if (newShot == null)
				{
					if (activeCameraShot != null)
					{
						if (activeCameraShot.MainCharacter.Character == character)
						{
							bool changeCharacter = Random.Range (0, 10) >= 5;
							if (changeCharacter)
							{
								isReverseShot = !isReverseShot;
								newShot = CreateShot ((isReverseShot) ? characterDataB : characterDataA, (isReverseShot) ? characterDataA : characterDataB, isReverseShot);
							}
							else
							{
								if (activeCameraShot.ShotType == ShotType.Centred)
								{
									newShot = CreateShot (ShotType.CloseUp, (isReverseShot) ? characterDataB : characterDataA, (isReverseShot) ? characterDataA : characterDataB, isReverseShot);
								}
								else
								{
									newShot = CreateShot (ShotType.Centred, (isReverseShot) ? characterDataB : characterDataA, (isReverseShot) ? characterDataA : characterDataB, isReverseShot);
								}
							}
						}
						else
						{
							int i = 0;
							while (i < 10 && (newShot == null || newShot.ShotType == activeCameraShot.ShotType))
							{
								newShot = CreateShot ((isReverseShot) ? characterDataB : characterDataA, (isReverseShot) ? characterDataA : characterDataB, isReverseShot);
								i++;
							}
						}
					}
					else
					{
						int i=0;
						while (i < 10 && (newShot == null || newShot.ShotType == ShotType.CloseUp))
						{
							newShot = CreateShot ((isReverseShot) ? characterDataB : characterDataA, (isReverseShot) ? characterDataA : characterDataB, isReverseShot);
							i++;
						}
					}
				}
				else if (newShot == activeCameraShot)
				{
					// If first shot delayed
					doSkip = true;
				}

				if (!doSkip)
				{
					if (newShot != null)
					{
						activeCameraShot = newShot;
						activeCameraShot.Apply (ownCamera);
					}
					else
					{
						ACDebug.LogWarning ("Failed to create Conversation Camera shot - is the Collision mask set up correctly?");
					}

					lastShotTime = Time.time;
				}
			}
		}


		private ShotType GetRandomShotType ()
		{
			int rand = Random.Range (0, 3);

			if (lastShotType >= 0 && rand == lastShotType)
			{
				rand++;
				if (rand == 3) rand = 0;
			}

			switch (rand)
			{
				case 0:
					return ShotType.Centred;

				case 1:
					return ShotType.CloseUp;

				default:
					return ShotType.OverTheShoulder;
			}
		}


		private CameraShot CreateShot (CharacterData mainChar, CharacterData otherChar, bool isReverseShot)
		{
			return CreateShot (GetRandomShotType (), mainChar, otherChar, isReverseShot);
		}


		private CameraShot CreateShot (ShotType shotType, CharacterData mainChar, CharacterData otherChar, bool isReverseShot)
		{
			Vector3 forwardDirection = (mainChar.Character.Transform.position - otherChar.Character.Transform.position).normalized;

			float spinAngle = 0f;
			Vector3 centre = Vector3.zero;
			Vector3 lookAt = Vector3.zero;
			float distance = 0f;
			float fov = 0f;

			float aspectRatio = ACScreen.safeArea.size.x / ACScreen.safeArea.size.y;
			float aspectFactor = Mathf.Clamp01 ((aspectRatio - 1.78f) / (0.56f - 1.78f));

			switch (shotType)
			{
				case ShotType.Centred:
					{
						Vector3 mainCharCentre = mainChar.GetShoulderPosition ();
						Vector3 otherCharCentre = otherChar.GetShoulderPosition ();

						spinAngle = (isReverseShot) ? 270f : 90f;
						if (characterAOnRight)
						{
							spinAngle += 180f;
						}

						centre = (mainCharCentre + otherCharCentre) / 2f;

						float _distanceFactor = Mathf.Lerp (1.2f, 1.9f, aspectFactor);
						distance = Vector3.Distance (mainCharCentre, otherCharCentre) * _distanceFactor;
						if (distance < 2f)
						{
							centre += 0.1f * mainChar.Height * Vector3.up;
						}
						lookAt = centre;
						fov = Mathf.Clamp (40f + (distance * 0.67f), 40f, 45f);
					}
					break;

				case ShotType.CloseUp:
					{
						Vector3 mainCharCentre = mainChar.GetHeadPosition ();

						spinAngle = (isReverseShot != characterAOnRight) ? 215f : 145f;

						centre = mainCharCentre - (forwardDirection * mainChar.Radius);
						lookAt = centre;
						distance = mainChar.Radius + 0.6f;
						fov = 38f;
					}
					break;

				case ShotType.OverTheShoulder:
					{
						Vector3 mainCharCentre = mainChar.GetHeadPosition ();
						Vector3 otherCharCentre = otherChar.GetHeadPosition ();
						Vector3 mainCharCentreOffset = mainCharCentre - (0.1f * mainChar.Height * Vector3.up);
						float characterDistance = Vector3.Distance (mainCharCentre, otherCharCentre);

						spinAngle = 180f + (Mathf.Rad2Deg * Mathf.Atan2 (otherChar.Radius + 0.35f, characterDistance / 2f));
						if (isReverseShot == characterAOnRight) spinAngle *= -1f;

						centre = (mainCharCentre + otherCharCentre) / 2f;
						lookAt = (otherCharCentre + mainCharCentreOffset) / 2f;

						float _distanceFactor = Mathf.Lerp (0.5f, 0.7f, aspectFactor);
						distance = (otherChar.Radius + 1.5f) + (characterDistance * _distanceFactor);
						fov = 35f - (distance * 5f);
					}
					break;

				default:
					break;
			}

			distance *= distanceFactor;
			fov *= fovFactor;

			Quaternion rotation = Quaternion.AngleAxis (spinAngle, Vector3.down);
			Vector3 position = centre + rotation * forwardDirection * distance;

			CameraShot newShot = new CameraShot (shotType, mainChar, position, lookAt, fov, animatedShotFrequency, isReverseShot);

			if (newShot.IsValid (mainChar, otherChar, collisionMask))
			{
				lastShotType = (int) shotType;
				return newShot;
			}
			return null;
		}


		private void AddEndPause ()
		{
			float timeLeft = minShotLength + lastShotTime - Time.time;
			if (timeLeft > 0f)
			{
				ActionList actionList = GetComponent<ActionList> ();
				if (actionList)
				{
					if (actionList.AreActionsRunning ())
					{
						actionList.Kill ();
					}
				}
				else
				{
					actionList = gameObject.AddComponent<ActionList> ();
				}

				actionList.actions = new List<Action>
				{
					ActionPause.CreateNew (timeLeft),
					ActionCamera.CreateNew (KickStarter.mainCamera.GetLastGameplayCamera ())
				};

				actionList.Interact ();
			}
			else
			{
				KickStarter.mainCamera.SetGameCamera (KickStarter.mainCamera.GetLastGameplayCamera ());
			}
		}


		private bool IsReferenced (Char character)
		{
			if (character)
			{
				if (characterDataA != null && characterDataA.Character == character)
				{
					return true;
				}
				if (characterDataB != null && characterDataB.Character == character)
				{
					return true;
				}
			}
			return false;
		}

		#endregion


		#region PrivateClasses

		[System.Serializable]
		private class CharacterData
		{

			public readonly Char Character;
			public readonly float Height;
			public readonly float Radius;


			public CharacterData (Char _character)
			{
				Character = _character;

				CapsuleCollider capsuleCollder = Character.GetComponent<CapsuleCollider> ();
				if (capsuleCollder) 
				{
					Height = capsuleCollder.height;
					Radius = capsuleCollder.radius;
				}
				else
				{
					CharacterController characterController = Character.GetComponent<CharacterController> ();
					Height = characterController.height;
					Radius = characterController.radius;
				}
				Height *= Character.Transform.localScale.y;

				if (Character.GetAnimator () && Character.GetAnimator ().GetBoneTransform (HumanBodyBones.Head))
				{
					Transform head = Character.GetAnimator ().GetBoneTransform (HumanBodyBones.Head);
					Height = (head.position.y - Character.transform.position.y) * 1.2f;
				}
			}


			public void LookAt (CharacterData otherCharacter)
			{
				Character.SetHeadTurnTarget (otherCharacter.Character.transform, 0.94f * otherCharacter.Height * Vector3.up, false);
			}


			public void StopLooking (bool isSkipping)
			{
				Character.ClearHeadTurnTarget (isSkipping, HeadFacing.Manual);
			}


			public Vector3 GetHeadPosition ()
			{
				if (Character.GetAnimator () && Character.GetAnimator ().GetBoneTransform (HumanBodyBones.Head))
				{
					Transform head = Character.GetAnimator ().GetBoneTransform (HumanBodyBones.Head);
					return head.position;
				}
				return GetPosition (0.85f);
			}


			public Vector3 GetShoulderPosition ()
			{
				if (Character.GetAnimator () && Character.GetAnimator ().GetBoneTransform (HumanBodyBones.LeftShoulder))
				{
					Transform leftShoulder = Character.GetAnimator ().GetBoneTransform (HumanBodyBones.LeftShoulder);
					Vector3 headPosition = GetHeadPosition ();
					return new Vector3 (headPosition.x, leftShoulder.position.y, headPosition.z);
				}
				return GetPosition (0.7f);
			}


			public Vector3 GetPosition (float heightFactor)
			{
				Vector3 basePosition = Character.Transform.position;

				if (Character.GetAnimator () && Character.GetAnimator ().GetBoneTransform (HumanBodyBones.Hips))
				{
					Vector3 hipsPosition = Character.GetAnimator ().GetBoneTransform (HumanBodyBones.Hips).position;
					basePosition.x = hipsPosition.x;
					basePosition.z = hipsPosition.z;
				}

				return basePosition + (Height * heightFactor * Vector3.up);
			}

		}

		[System.Serializable]
		private class FloatRange
		{

			[SerializeField] private float minValue;
			[SerializeField] private float maxValue;

			private float minLimit;
			private float maxLimit;


			public FloatRange (float min, float max)
			{
				minValue = min;
				maxValue = max;

				minLimit = min;
				maxLimit = max;
			}


			public float GetRandomValue ()
			{
				return Random.Range (minValue, maxValue);
			}


			public void OnValidate ()
			{
				minValue = Mathf.Max (minValue, minLimit);
				maxValue = Mathf.Min (maxValue, maxLimit);
			}


			#if UNITY_EDITOR

			public void ShowGUI (string label)
			{
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField (label, GUILayout.Width (100f));
				
				string minValueLabel = minValue.ToString ();
				if (minValueLabel.Length > 4) minValueLabel = minValueLabel.Substring (0, 4);

				string maxValueLabel = maxValue.ToString ();
				if (maxValueLabel.Length > 4) maxValueLabel = maxValueLabel.Substring (0, 4);

				EditorGUILayout.LabelField ("Min: " + minValueLabel + "s", GUILayout.Width (80f));
				EditorGUILayout.LabelField ("Max: " + maxValueLabel + "s", GUILayout.Width (80f));

				EditorGUILayout.MinMaxSlider (ref minValue, ref maxValue, minLimit, maxLimit);
				EditorGUILayout.EndHorizontal ();
			}

			#endif

		}


		private class CameraShot
		{

			private ShotType shotType;
			private CharacterData mainCharacter;
			private Vector3 cameraPosition;
			private Vector3 lookAtPosition;
			private float cameraFOV;
			private RaycastHit raycastHit;
			
			private bool isAnimated;
			private AnimationStyle animationStyle;
			private enum AnimationStyle { Pivot, PanHorizontal, PanVertical, PanIn };
			private float animationSpeed;
			private float shotTime;
			private bool isReverseShot;


			public CameraShot (ShotType _shotType, CharacterData _mainCharacter, Vector3 _cameraPosition, Vector3 _lookAtPosition, float _cameraFOV, float animatedShotFrequency, bool _isReverseShot)
			{
				shotType = _shotType;
				mainCharacter = _mainCharacter;
				cameraPosition = _cameraPosition;
				lookAtPosition = _lookAtPosition;
				cameraFOV = _cameraFOV;
				isAnimated = Random.Range (0f, 1f) <= animatedShotFrequency;
				isReverseShot = _isReverseShot;
				
				if (isAnimated)
				{
					switch (shotType)
					{
						case ShotType.Centred:
							animationStyle = AnimationStyle.Pivot;
							animationSpeed = 0.1f;
							break;

						case ShotType.CloseUp:
							animationStyle = (Random.Range (0, 10) >= 5) ? AnimationStyle.PanVertical : AnimationStyle.PanIn;
							animationSpeed = (animationStyle == AnimationStyle.PanVertical) ? 0.01f : 0.04f;
							break;

						case ShotType.OverTheShoulder:
							animationStyle = AnimationStyle.PanHorizontal;
							animationSpeed = 0.04f;
							break;
					}

					if (Random.Range (0, 10) >= 5) animationSpeed *= -1f;
				}
			}


			public void Apply (_Camera camera)
			{
				camera.transform.position = cameraPosition;
				camera.transform.LookAt (lookAtPosition);
				camera.Camera.fieldOfView = cameraFOV;

				KickStarter.mainCamera.SetGameCamera (camera);
			}


			public void Apply (_Camera camera, CameraShot cameraShot)
			{
				cameraPosition = cameraShot.cameraPosition;
				lookAtPosition = cameraShot.lookAtPosition;

				camera.transform.position = cameraPosition;
				camera.transform.LookAt (lookAtPosition);
				camera.Camera.fieldOfView = cameraFOV;
			}


			public void Update (Transform transform)
			{
				if (!isAnimated)
				{
					return;
				}
				
				shotTime += Time.deltaTime;

				switch (animationStyle)
				{
					case AnimationStyle.Pivot:
						transform.position = cameraPosition + (transform.right * animationSpeed * shotTime);
						transform.LookAt (lookAtPosition);
						break;

					case AnimationStyle.PanHorizontal:
						transform.position = cameraPosition + (transform.right * animationSpeed * shotTime);
						break;

					case AnimationStyle.PanVertical:
						transform.position = cameraPosition + (transform.up * animationSpeed * shotTime);
						transform.LookAt (lookAtPosition);
						break;

					case AnimationStyle.PanIn:
						transform.position = cameraPosition + (transform.forward * animationSpeed * shotTime);
						break;
				}

			}


			public bool IsValid (CharacterData characterA, CharacterData characterB, LayerMask collisionMask)
			{
				Vector3 headPositionA = characterA.GetHeadPosition ();
				if (Physics.Raycast (cameraPosition, headPositionA - cameraPosition, out raycastHit, Vector3.Distance (cameraPosition, headPositionA), collisionMask))
				{
					if (raycastHit.collider.gameObject != characterA.Character.gameObject)
					{
						return false;
					}
				}

				if (ShowsBothCharacters ())
				{
					Vector3 headPositionB = characterB.GetHeadPosition ();
					if (Physics.Raycast (cameraPosition, headPositionB - cameraPosition, out raycastHit, Vector3.Distance (cameraPosition, headPositionB), collisionMask))
					{
						if (raycastHit.collider.gameObject != characterB.Character.gameObject)
						{
							return false;
						}
					}
				}

				return true;
			}


			private bool ShowsBothCharacters ()
			{
				switch (shotType)
				{ 
					case ShotType.CloseUp:
						return false;

					default:
						return true;
				}
			}


			public CharacterData MainCharacter
			{
				get
				{
					return mainCharacter;
				}
			}


			public ShotType ShotType
			{
				get
				{
					return shotType;
				}
			}


			public bool IsReverseShot
			{
				get
				{
					return isReverseShot;
				}
			}

		}

		#endregion


		#if UNITY_EDITOR

		public void ShowGUI ()
		{
			minShotLength = EditorGUILayout.Slider ("Min. shot duration (s):", minShotLength, 0f, 10f);
			if (minShotLength > 0f) extendCutscenesToFitMinShotLength = EditorGUILayout.ToggleLeft ("Extend cutscenes to fit min shot duration?", extendCutscenesToFitMinShotLength);
			collisionMask = AdvGame.LayerMaskField ("Collision layer mask:", collisionMask);

			EditorGUILayout.Space ();

			cutDelay.ShowGUI ("Cut delay:");

			EditorGUILayout.Space ();

			moveWithCharacters = EditorGUILayout.Toggle ("Move with characters?", moveWithCharacters);
			animatedShotFrequency = EditorGUILayout.Slider ("Animated frequency:", animatedShotFrequency, 0f, 1f);
			if (animatedShotFrequency > 0f)
			{
				maxAnimationDuration = EditorGUILayout.Slider ("Max. animation duration:", maxAnimationDuration, 0f, 10f);
			}
			autoTurnHeads = EditorGUILayout.Toggle ("Auto-turn character heads?", autoTurnHeads);

			EditorGUILayout.Space ();

			distanceFactor = EditorGUILayout.Slider ("Distance factor:", distanceFactor, 0.1f, 2f);
			fovFactor = EditorGUILayout.Slider ("FOV factor:", fovFactor, 0.1f, 2f);
		}

		#endif

	}

}