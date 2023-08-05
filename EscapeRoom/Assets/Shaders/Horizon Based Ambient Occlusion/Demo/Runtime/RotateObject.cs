using UnityEngine;

namespace HorizonBasedAmbientOcclusion
{
    public class RotateObject : MonoBehaviour
    {
        [Range(-50.0f, 50.0f)]
        public float speed = 15.0f;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(speed * Time.deltaTime * Vector3.up, Space.World);
        }
    }
}
