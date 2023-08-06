/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2023
 *	
 *	"RememberContainer.cs"
 * 
 *	This script is attached to container objects in the scene
 * 
 */

using UnityEngine;
using System.Collections.Generic;

namespace AC
{

	/** This script is attached to Container objects in the scene you wish to save. */
	[AddComponentMenu("Adventure Creator/Save system/Remember Container")]
	[HelpURL("https://www.adventurecreator.org/scripting-guide/class_a_c_1_1_remember_container.html")]
	public class RememberContainer : Remember
	{

		/**
		 * <summary>Serialises appropriate GameObject values into a string.</summary>
		 * <returns>The data, serialised as a string</returns>
		 */
		public override string SaveData ()
		{
			ContainerData containerData = new ContainerData ();
			containerData.objectID = constantID;
			containerData.savePrevented = savePrevented;
			
			if (_Container)
			{
				containerData.collectionData = _Container.InvCollection.GetSaveData ();

				containerData._linkedIDs = string.Empty; // Now deprecated
				containerData._counts = string.Empty; // Now deprecated
				containerData._IDs = string.Empty; // Now deprecated
			}
			
			return Serializer.SaveScriptData <ContainerData> (containerData);
		}
		

		/**
		 * <summary>Deserialises a string of data, and restores the GameObject to its previous state.</summary>
		 * <param name = "stringData">The data, serialised as a string</param>
		 */
		public override void LoadData (string stringData)
		{
			ContainerData data = Serializer.LoadScriptData <ContainerData> (stringData);
			if (data == null) return;
			SavePrevented = data.savePrevented; if (savePrevented) return;

			if (_Container)
			{
				_Container.InvCollection = new InvCollection (_Container);

				if (!string.IsNullOrEmpty (data._linkedIDs))
				{
					_Container.InvCollection.DeleteAll ();

					int[] linkedIDs = StringToIntArray (data._linkedIDs);
					int[] counts = StringToIntArray (data._counts);
				
					if (linkedIDs != null)
					{
						for (int i=0; i<linkedIDs.Length; i++)
						{
							InvInstance invInstance = new InvInstance (linkedIDs[i], counts[i]);
							_Container.InvCollection.Add (invInstance);
						}
					}
				}
				else if (!string.IsNullOrEmpty (data.collectionData))
				{
					_Container.InvCollection.DeleteAll ();
					
					List<InvInstance> invInstances = InvCollection.DataToInstances (data.collectionData);
					foreach (InvInstance invInstance in invInstances)
					{
						_Container.InvCollection.Add (invInstance);
					}
				}
			}
		}


		private Container container;
		private Container _Container
		{
			get
			{
				if (container == null)
				{
					container = GetComponent <Container>();
				}
				return container;
			}
		}
		
	}
	

	/** A data container used by the RememberContainer script. */
	[System.Serializable]
	public class ContainerData : RememberData
	{

		/** (Deprecated) */
		public string _linkedIDs;
		/** (Deprecated) */
		public string _counts;
		/** (Deprecated) */
		public string _IDs;
		/** The contents of the container's InvCollection. */
		public string collectionData;

		/** The default Constructor. */
		public ContainerData () { }

	}
	
}