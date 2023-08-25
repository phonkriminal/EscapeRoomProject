using UnityEngine;



namespace AC

{



    public class RememberToVariable : MonoBehaviour

    {



        [SerializeField] private int linkedVariableID = 0;

        [SerializeField] private Remember remember = null;





        private void OnEnable()

        {

            GVar linkedVariable = GlobalVariables.GetVariable(linkedVariableID);

            if (linkedVariable != null && !string.IsNullOrEmpty(linkedVariable.TextValue) && remember)

            {

                remember.LoadData(linkedVariable.TextValue);

            }

        }





        private void OnDisable()

        {

            GVar linkedVariable = GlobalVariables.GetVariable(linkedVariableID);

            if (linkedVariable != null && remember)

            {

                linkedVariable.TextValue = remember.SaveData();

            }

        }



    }



}