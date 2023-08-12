using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCharacterController3D : MonoBehaviour
{
    bool alive;
    bool awake;
    bool active;
    bool grounded;
    bool sitdown;
    bool flying;
    bool climbing;
    bool pushing;
    bool jumping;
    bool running;
    bool blocked;
    Transform trans;
    Rigidbody rigid;
    Animator anim;
    Vector3 dirforw;
    Vector3 dirside;
    float deltawalk;
    float deltaturn;
    float deltastrafe;
    public float walkspeed;
    public float runspeed;
    public float sprintspeed;
    public float turnspeed;
    public float strafespeed;
    float myspeed;
    float mystrafes;
    float myturns;
    float deltafaster;
    public float jumpforce;
    float faster;
    float tospeed;
    int forward;
    int turn;
    int strafe;
    float angleforward;
    float angleright;
    float Kz;
    float Kx;
    float idlecounter;
    float idletime;

    RaycastHit hit;
    RaycastHit hit1;
    RaycastHit hit2;



    void Start()
    {
        idletime = 5f;
        deltawalk = 0.05f;
        deltastrafe = 0.125f;
        deltaturn = 0.125f;
        deltafaster = 0.05f;
        tospeed = walkspeed;
        trans = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        alive = true;
        active = true;
        awake = true;
        grounded = false;
        awake = true;
        flying = false;
        pushing = false;
        climbing = false;
    }

    void Update()
    {
        Debug.DrawRay(trans.position + new Vector3(0f, 0.1f, 0f), dirforw, Color.green);
        Debug.DrawRay(trans.position + new Vector3(0f, 0.1f, 0f), dirside, Color.green);
        anim.SetBool("active", active);
        if (Input.GetKey("p"))
        {
            trans.position = new Vector3(0f, 0.5f, 0f);
            rigid.velocity = new Vector3(0f, 0f, 0f);
            active = true;
            anim.Play("freefall");
        }

        if (alive)
        {
            Checkground();
            if (active)
            {
                if (awake)
                {
                    Setdir();
                    if (grounded)
                    {
                        if (!sitdown)
                        {
                            if (!pushing) //walking
                            {
                                Checkidles();
                                Checkwalls();
                                Checkinput();
                                rigid.velocity = dirforw * myspeed + dirside * mystrafes;
                                trans.Rotate(0f, myturns, 0f);
                                anim.SetInteger("forward", forward);
                                anim.SetInteger("turn", turn);
                                anim.SetInteger("strafe", strafe);
                                anim.SetFloat("faster", faster);
                                anim.SetFloat("speed", myspeed * 10f / runspeed);
                                anim.SetFloat("strafespeed", mystrafes * 5f / strafespeed);
                                anim.SetFloat("angle", angleforward);
                                anim.SetFloat("turnspeed", myturns *5f/turnspeed);
                            }
                            else
                            {
                                //pusing
                            }
                        }
                        else
                        {   //sit down
                            if (Input.anyKeyDown)
                            {
                                sitdown = false;
                                anim.SetBool("sitdown", false);
                                StartCoroutine("Waitfor", 1f);
                            }
                        }
                    }
                    else
                    {   //no grounded
                        if (flying) { }
                        if (climbing) { }
                        if (!flying && !climbing)
                        {
                            rigid.AddForce(trans.forward * Input.GetAxisRaw("Vertical") * 0.0125f, ForceMode.VelocityChange);
                        }
                    }
                }
                else
                {
                    //inconscious
                    if (Input.anyKeyDown)
                    {
                        anim.SetBool("active", true);
                        StartCoroutine("Waitfor", 1f);
                    }
                }
            }
            else 
            {
                //inactive
            }
        }
        else
        {
            //dead
        }
    }

    void Checkinput()
    {
        //forward
        if (Input.GetKey("w") || Input.GetKey("s"))
        {
            if (Input.GetKey("w")) forward = 1;
            else if (Input.GetKey("s")) forward = -1;
            Checkwalls();
            if (blocked && myspeed >  3f) Hitwithforce( 1, Random.Range(1f, myspeed));
            if (blocked && myspeed < -2f) Hitwithforce(-1, Random.Range(1f, myspeed));
            if (blocked) forward = 0;
        }
        else forward = 0;

        //turn
        if (Input.GetKey("d") || Input.GetKey("a"))
        {
            if (Input.GetKey("d")) turn = 1;
            else if (Input.GetKey("a")) turn = -1;
        }
        else turn = 0;

        //strafe
        if (Input.GetKey("e") || Input.GetKey("q"))
        {
            if (Input.GetKey("e")) strafe = 1;
            else if (Input.GetKey("q")) strafe = -1;
            Checkwalls();
            if (blocked) forward = 0;
        }
        else strafe = 0;          

        //faster
        if (Input.GetKey(KeyCode.LeftShift)) faster = Mathf.Lerp(faster, 10f, deltafaster);
        else faster = Mathf.Lerp(faster, 0f, deltafaster);
        faster = Mathf.Clamp(faster, 0f, 10f);

        //sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (myspeed > (runspeed-2f)) tospeed = sprintspeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            tospeed = walkspeed;
        }

        //tospeed
        if (forward == 1)
        {
            if (tospeed != sprintspeed)
            {
                if (faster > 5f) tospeed = runspeed;
                else tospeed = walkspeed;
            }
        }
        if (forward == -1)
        {
            if (faster > 5f) tospeed = -runspeed;
            else tospeed = -walkspeed;
        }
        if (forward == 0f) tospeed = 0f;

        //SPEEDs
        Kz = 1f;
        if (strafe != 0) Kz *= 0.9f;
        Kz *= 1f+(angleforward / 450f*forward);
        if (forward == -1) Kz *= 0.75f;
        Kx = 1f;
        Kx *= 1f + (angleright / 450f * -strafe);
        myspeed = Mathf.Lerp(myspeed, tospeed * Kz, deltawalk);
        mystrafes = Mathf.Lerp(mystrafes, strafespeed*strafe* Kx* ((faster * 0.1f) + 1f), deltastrafe);
        myturns = Mathf.Lerp(myturns, turnspeed*turn* ((faster * 0.1f) + 1f), deltaturn);

        //jump
        if (Input.GetButtonDown("Jump") && grounded && !jumping) StartCoroutine("Jump");

        //SITDOWN      
        if (Input.GetKeyDown(KeyCode.C) && forward == 0 )
        {
            if (Physics.Raycast(trans.position + new Vector3(0.0f, 0.3f, 0f) - trans.forward * 0.1f, -trans.forward, 0.17f)
                && !Physics.Raycast(trans.position + new Vector3(0.0f, 0.42f, 0f) - trans.forward * 0.1f, -trans.forward, 0.4f))
            {
                StartCoroutine("Waitfor", 1f);
                sitdown = true;
                anim.SetBool("sitdown", true);
                anim.Play("sitdown");
            }
            else 
            {
                anim.Play("lookback");
                StartCoroutine("Waitfor", 1.25f);
            }
        }

        //TEST HITS
        if (Input.GetKeyDown(KeyCode.U)) Hitwithforce (1,Random.Range(1f,5f));
        if (Input.GetKeyDown(KeyCode.Y)) Hitwithforce( -1, Random.Range(1f, 5f));
    }
    void Checkground()
    {
        //ground
        Debug.DrawRay(trans.position + new Vector3(0f, 0.05f, 0f), Vector3.down * 0.25f, Color.red);
        Debug.DrawRay(trans.position + new Vector3(0f, 0.05f, 0f) + dirforw * 0.15f, Vector3.down * 0.25f, Color.red);
        Debug.DrawRay(trans.position + new Vector3(0f, 0.05f, 0f) + dirforw * -0.15f, Vector3.down * 0.25f, Color.red);
        Debug.DrawRay(trans.position + new Vector3(0f, 0.05f, 0f) + dirside * 0.15f, Vector3.down * 0.25f, Color.red);
        Debug.DrawRay(trans.position + new Vector3(0f, 0.05f, 0f) + dirside * -0.15f, Vector3.down * 0.25f, Color.red);

        if (!jumping)
        {
            float dist;
            if (grounded) dist = 0.2f; else dist = 0.055f;
            if (Physics.Raycast(trans.position + new Vector3(0f, 0.05f, 0f), Vector3.down, 0.125f) ||
              Physics.Raycast(trans.position + new Vector3(0f, 0.05f, 0f) + dirforw * 0.15f, Vector3.down, dist) ||
              Physics.Raycast(trans.position + new Vector3(0f, 0.05f, 0f) + dirforw * -0.15f, Vector3.down, dist) ||
              Physics.Raycast(trans.position + new Vector3(0f, 0.05f, 0f) + dirside * 0.15f, Vector3.down, dist) ||
              Physics.Raycast(trans.position + new Vector3(0f, 0.05f, 0f) + dirside * -0.15f, Vector3.down, dist))
                grounded = true;
            else grounded = false;
        }
        else grounded = false;

        anim.SetBool("grounded", grounded);
    }
    void Checkwalls()
    {
        //walls        
        if (Physics.Raycast(trans.position + new Vector3(0f, 0.4f, 0f), dirforw * forward + dirside * strafe, 0.25f))
        {
            blocked = true;
        }
        else blocked = false;
        anim.SetBool("blocked", blocked);
    }
    void Checkidles()
    {
        if (Input.anyKey) idlecounter = 0f;
        else
        {
            if (idlecounter > idletime)
            {
                string[] idles = { "idle2", "idle3", "idle4" };
                idlecounter = 0f;
                idletime = Random.Range(8f, 16f);
                anim.CrossFade(idles[Random.Range(0, 3)],0.2f);
            }
            else idlecounter += Time.deltaTime;
        }
    }
    void Setdir()
    {
        if (Physics.Raycast(trans.position + new Vector3(0f, 0.1f, 0f), Vector3.down*2, 0.5f))
        {
            Physics.Raycast(trans.position + new Vector3(0f, 0.1f, 0f), Vector3.down * 2, out hit, 0.55f);
            Physics.Raycast(trans.position + new Vector3(0f, 0.1f, 0f) + trans.forward *  0.15f, Vector3.down * 2, out hit1 , 0.55f);
            Physics.Raycast(trans.position + new Vector3(0f, 0.1f, 0f) + trans.forward * -0.15f, Vector3.down * 2, out hit2, 0.55f);
            dirforw = Vector3.Slerp(dirforw,-Vector3.Cross(hit.normal + hit1.normal + hit2.normal, trans.right),0.25f);
            dirforw.Normalize();
            angleforward = Mathf.Lerp(angleforward, Vector3.SignedAngle(trans.forward, -Vector3.Cross(hit.normal + hit1.normal + hit2.normal, trans.right), trans.right),0.25f);
            angleright = Mathf.Lerp(angleright, Vector3.SignedAngle(trans.right, -Vector3.Cross(hit.normal + hit1.normal + hit2.normal, -trans.forward), trans.forward), 0.25f);
            Physics.Raycast(trans.position + new Vector3(0f, 0.1f, 0f) + trans.right * 0.15f, Vector3.down * 2, out hit1, 0.55f);
            Physics.Raycast(trans.position + new Vector3(0f, 0.1f, 0f) + trans.right * -0.15f, Vector3.down * 2, out hit2, 0.55f);
            dirside = Vector3.Slerp(dirside,Vector3.Cross(hit.normal + hit1.normal + hit2.normal, trans.forward),0.25f);
            dirside.Normalize();
        }
        else
        {
            dirforw = Vector3.Slerp(dirforw, trans.forward, 0.25f);
            dirside = Vector3.Slerp(dirside, trans.right, 0.25f);
        }
    } 
    void Hitwithforce(int direction,float force)
    {
        tospeed = 0f;
        if (direction == 1)
        {
            if (force > 4f)
            {
                active = false;
                anim.Play("fallback");
                StartCoroutine("Waitfor", 5f);
                forward = 0;turn = 0;strafe = 0;
            }
            else
            {
                active = false;
                anim.Play("hitback");
                StartCoroutine("Waitfor", 0.75f);
            }
            myspeed = 0f;
            myturns = 0f;
            mystrafes = 0f;
            faster = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(dirforw * -force + new Vector3(0f,2f,0f), ForceMode.Impulse);

        }
        if (direction == -1)
        {
            if (force > 2.5f)
            {
                active = false;
                anim.Play("fallforward");
                StartCoroutine("Waitfor", 5f);
            }
            else
            {
                active = false;
                anim.Play("hitforward");
                StartCoroutine("Waitfor", 0.75f);
            }
            tospeed = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(dirforw * force * 2f + new Vector3(0f, 2f, 0f), ForceMode.Impulse);
        }
    }

    IEnumerator Waitfor(float itime)
    {
        active = false;
        yield return new WaitForSeconds(itime);
        active = true;        
    }
    IEnumerator Jump()
    {
        if (forward < 0.2f) anim.Play("jump");
        else anim.Play("runjumpin");
        jumping = true;
        yield return new WaitForSeconds(0.16f);
        rigid.AddForce(Vector3.up * jumpforce , ForceMode.Impulse);
        yield return new WaitForSeconds(0.3f);
        jumping = false;
    }
}
