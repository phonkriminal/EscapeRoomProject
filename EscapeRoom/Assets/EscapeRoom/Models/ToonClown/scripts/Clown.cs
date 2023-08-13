using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown : MonoBehaviour {

    Rigidbody rigidbo;
    Transform trans;
    Animator anim;
    bool grounded;
    float run;
    public float jumpforce;

    void Start ()
    {
        rigidbo = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }	
	
	void Update ()
    {
        //GROUNDED
        if (Physics.Raycast(trans.position + new Vector3(0.1f, 0.1f, 0.1f), Vector3.down, 0.15f)
          || Physics.Raycast(trans.position + new Vector3(0.1f, 0.1f, -0.1f), Vector3.down, 0.15f)
          || Physics.Raycast(trans.position + new Vector3(0f, 0.1f, 0f), Vector3.down, 0.15f)
          || Physics.Raycast(trans.position + new Vector3(-0.1f, 0.1f, -0.1f), Vector3.down, 0.15f)
          || Physics.Raycast(trans.position + new Vector3(-0.1f, 0.1f, 0.1f), Vector3.down, 0.15f))
        {
            grounded = true;
            anim.SetBool("gr", true);
        }
        else
        {
            grounded = false;
            anim.SetBool("gr", false);
        }

        //WALK
        anim.SetFloat("walk", Input.GetAxis("Vertical"));
        if (grounded)rigidbo.velocity=(trans.forward * anim.GetFloat("speed"));

        //TURN
        anim.SetFloat("turn",Input.GetAxis("Horizontal"));
        trans.Rotate(Vector3.up * anim.GetFloat("angle"));
        
        //RUN
        if ( Input.GetKey(KeyCode.LeftShift)) run += 0.025f;
        if (!Input.GetKey(KeyCode.LeftShift)) run -= 0.025f;
        run = Mathf.Clamp(run, 0f, 1f);
        anim.SetFloat("run",run);

        //JUMP
        if (Input.GetButtonDown("Jump") && grounded)
        {            
            anim.SetBool("jump", true);
        }
        else anim.SetBool("jump", false);
    }

    void jump()
    {
        rigidbo.AddForce(Vector3.up * jumpforce + (trans.forward * (run*2f)), ForceMode.VelocityChange);
    }
}
