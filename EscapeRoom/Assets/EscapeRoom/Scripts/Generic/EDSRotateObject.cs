using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class EDSRotateObject : MonoBehaviour
	{

	[Header("Rotation Velocity", order = 0)]
    [Space(10, order = 1)]

	public float xRotateSpeed;
	public float yRotateSpeed;
	public float zRotateSpeed;
	
    public Space space;
    
    public bool isClamp;
    
    public Axis axis;
	
    [Range(1f, 180f)]
	public float clampAngle = 1f;
     
    void Update()
	{
        if (isClamp)
        {
            switch (axis)
            {
                case Axis.X:
                    transform.localRotation = Quaternion.Euler(Mathf.PingPong(Time.time * xRotateSpeed, clampAngle * 2) - clampAngle, 0.0f, 0.0f);
                    break;

                case Axis.Y:
                    transform.localRotation = Quaternion.Euler(0.0f, Mathf.PingPong(Time.time * yRotateSpeed, clampAngle * 2) - clampAngle, 0.0f);
                    break;

                case Axis.Z:
                    transform.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.PingPong(Time.time * zRotateSpeed, clampAngle * 2) - clampAngle);
                    break;

                case Axis.All:
                    transform.localRotation = Quaternion.Euler(Mathf.PingPong(Time.time * xRotateSpeed, clampAngle * 2) - clampAngle,
                                                                Mathf.PingPong(Time.time * yRotateSpeed, clampAngle * 2) - clampAngle,
                                                                Mathf.PingPong(Time.time * zRotateSpeed, clampAngle * 2) - clampAngle);
                    break;

            }
        }
        else
        {
            transform.Rotate(xRotateSpeed * Time.deltaTime, yRotateSpeed * Time.deltaTime, zRotateSpeed * Time.deltaTime , space);
        }
	}
}
	[Serializable]
    public enum Axis
    {
        X, Y, Z, All
    }

