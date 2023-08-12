﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2023
 *	
 *	"ActionRaycast.cs"
 * 
 *	This action performs a Raycast using Unity's physics system.
 * 
 */

using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionRaycast : ActionCheck
	{

		public Transform originTransform;
		public int originConstantID = 0;
		public int originParameterID = -1;
		protected Vector3 runtimeOrigin;
		protected Vector3 runtimeDirection;

		public Vector3 direction = new Vector3 (1f, 0f, 0f);
		public int directionParameterID = -1;

		public float distance = 10f;
		public int distanceParameterID = -1;

		public float radius = 0f;
		public int radiusParameterID = -1;

		public LayerMask layerMask = new LayerMask ();

		public int detectedGameObjectParameterID = -1;
		public int detectedPositionParameterID = -1;
		protected ActionParameter detectedGameObjectParameter;
		protected ActionParameter detectedPositionParameter;


		public override ActionCategory Category { get { return ActionCategory.Physics; }}
		public override string Title { get { return "Raycast"; }}
		public override string Description { get { return "Performs a physics raycast"; }}


		public override void AssignValues (List<ActionParameter> parameters)
		{
			ActionParameter originParameter = GetParameterWithID (parameters, originParameterID);
			if (originParameter != null && originParameter.parameterType == ParameterType.Vector3)
			{
				runtimeOrigin = originParameter.vector3Value;
				runtimeDirection = AssignVector3 (parameters, directionParameterID, direction).normalized;
			}
			else
			{
				Transform runtimeOriginTransform = AssignFile (parameters, originParameterID, originConstantID, originTransform);
				runtimeOrigin = runtimeOriginTransform ? runtimeOriginTransform.position : Vector3.zero;
				runtimeDirection = runtimeOriginTransform ? runtimeOriginTransform.forward : Vector3.forward;

				Marker marker = runtimeOriginTransform.GetComponent<Marker> ();
				if (marker && SceneSettings.IsUnity2D ())
				{
					runtimeDirection = marker.transform.up;
				}
			}

			distance = AssignFloat (parameters, distanceParameterID, distance);
			radius = AssignFloat (parameters, radiusParameterID, radius);

			detectedGameObjectParameter = GetParameterWithID (parameters, detectedGameObjectParameterID);
			if (detectedGameObjectParameter != null && detectedGameObjectParameter.parameterType != ParameterType.GameObject)
			{
				detectedGameObjectParameter = null;
			}

			detectedPositionParameter = GetParameterWithID (parameters, detectedPositionParameterID);
			if (detectedPositionParameter != null && detectedPositionParameter.parameterType != ParameterType.Vector3)
			{
				detectedPositionParameter = null;
			}
		}
		
		
		public override bool CheckCondition ()
		{
			if (SceneSettings.IsUnity2D ())
			{
				RaycastHit2D hitInfo2D = UnityVersionHandler.Perform2DRaycast (runtimeOrigin, runtimeDirection, distance, layerMask);
				if (hitInfo2D.collider)
				{
					if (detectedGameObjectParameter != null)
					{
						detectedGameObjectParameter.SetValue (hitInfo2D.collider.gameObject);
					}

					if (detectedPositionParameter != null)
					{
						detectedPositionParameter.SetValue (hitInfo2D.point);
					}
					return true;
				}
				return false;
			}

			RaycastHit hitInfo;
			if ((radius <= 0f && Physics.Raycast (runtimeOrigin, runtimeDirection, out hitInfo, distance, layerMask)) ||
				(radius > 0f && Physics.SphereCast (runtimeOrigin, radius, runtimeDirection, out hitInfo, distance, layerMask)))
			{
				if (detectedGameObjectParameter != null)
				{
					detectedGameObjectParameter.SetValue (hitInfo.collider.gameObject);
				}

				if (detectedPositionParameter != null)
				{
					detectedPositionParameter.SetValue (hitInfo.point);
				}
				return true;
			}
			return false;
		}
		
		
		#if UNITY_EDITOR
		
		public override void ShowGUI (List<ActionParameter> parameters)
		{
			originParameterID = Action.ChooseParameterGUI ("Origin:", parameters, originParameterID, new ParameterType[] { ParameterType.GameObject, ParameterType.Vector3 });
			if (originParameterID >= 0)
			{
				originConstantID = 0;
				originTransform = null;

				if (GetParameterWithID (parameters, originParameterID) != null && GetParameterWithID (parameters, originParameterID).parameterType == ParameterType.Vector3)
				{
					directionParameterID = ChooseParameterGUI ("Direction:", parameters, directionParameterID, ParameterType.Vector3);
					if (directionParameterID < 0)
					{
						direction = EditorGUILayout.Vector3Field ("Direction:", direction);
					}
				}
			}
			else
			{
				originTransform = (Transform) EditorGUILayout.ObjectField ("Origin:", originTransform, typeof (Transform), true);

				originConstantID = FieldToID (originTransform, originConstantID);
				originTransform = IDToField (originTransform, originConstantID, false);
			}

			distanceParameterID = ChooseParameterGUI ("Distance:", parameters, distanceParameterID, ParameterType.Float);
			if (distanceParameterID < 0)
			{
				distance = EditorGUILayout.FloatField ("Distance:", distance);
			}

			if (!SceneSettings.IsUnity2D ())
			{
				radiusParameterID = ChooseParameterGUI ("Radius:", parameters, radiusParameterID, ParameterType.Float);
				if (radiusParameterID < 0)
				{
					radius = EditorGUILayout.FloatField ("Radius:", radius);
				}
			}

			layerMask = AdvGame.LayerMaskField ("Layer mask:", layerMask);
			detectedGameObjectParameterID = ChooseParameterGUI ("Hit GameObject:", parameters, detectedGameObjectParameterID, ParameterType.GameObject);
			detectedPositionParameterID = ChooseParameterGUI ("Detection point:", parameters, detectedPositionParameterID, ParameterType.Vector3);
		}


		protected override string GetSocketLabel (int index)
		{
			if (index == 0)
			{
				return "If object detected:";
			}
			return "If no object detected:";
		}


		public override void AssignConstantIDs (bool saveScriptsToo, bool fromAssetFile)
		{
			AssignConstantID (originTransform, originConstantID, originParameterID);
		}
		
		
		public override string SetLabel ()
		{
			if (originTransform)
			{
				return "From " + originTransform.gameObject.name;
			}
			return string.Empty;
		}


		public override bool ReferencesObjectOrID (GameObject gameObject, int id)
		{
			if (originParameterID < 0)
			{
				if (originTransform && originTransform.gameObject == gameObject) return true;
				if (originConstantID == id && id != 0) return true;
			}
			return base.ReferencesObjectOrID (gameObject, id);
		}
		
		#endif

	}

}