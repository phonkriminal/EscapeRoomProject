using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TKCharacterController2D : MonoBehaviour
{
    bool awake;
    bool active;
    bool grounded; 
    bool jumping;
    bool running;
    bool blocked;
    Transform trans;
    Rigidbody rigid;
    Animator anim;
    Vector3 dirforw;
    float deltawalk;
    public float walkspeed;
    public float runspeed;
    public float sprintspeed;
    float myspeed;
    float myturns;
    float deltafaster;
    float jumpforce;
    float faster;
    float tospeed;
    int right;
    int looking;
    int turn;
    float angleforward;
    float Kz;
    float idlecounter;
    float idletime;
    int AUX;
    bool turning;
    RaycastHit hit;
    RaycastHit hit1;
    RaycastHit hit2;



    void Start()
    {
        looking = 1;
        idletime = 5f;
        deltawalk = 0.05f;
        deltafaster = 0.05f;
        jumpforce = 3.5f;
        tospeed = walkspeed;
        trans = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        active = true;
        awake = true;
        grounded = false;
    }

    void Update()
    {
        Debug.DrawRay(trans.position + new Vector3(0f, 0.1f, 0f), dirforw, Color.green);
        anim.SetBool("active", active);
        if (Input.GetKey("p"))
        {
            trans.position = new Vector3(0f, 0.5f, 0f);
            rigid.velocity = new Vector3(0f, 0f, 0f);
            active = true;
            anim.Play("freefall");
        }

        
            Checkground();
            if (active)
            {
                if (awake)
                {
                    Setdir();
                    if (grounded)
                    {
                        Checkidles();
                        Checkinput();
                                rigid.velocity = dirforw * myspeed;
                                anim.SetInteger("right", right);
                                anim.SetFloat("faster", faster);
                                anim.SetFloat("speed", myspeed * 10f / -runspeed*looking);
                                anim.SetFloat("angle", angleforward/4f);  
                    }
                    else
                    {   //no grounded
                        Checkwalls();
                        if (blocked)
                        {
                            if (myspeed > 3f || myspeed < -3f) Hitwithforce(myspeed);
                        }                        
                            rigid.AddForce(Vector3.right * Input.GetAxisRaw("Horizontal") * 0.025f, ForceMode.VelocityChange);
                           
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
        
        
    }

    void Checkinput()
    {
        //right
        if (Input.GetKey("d") || Input.GetKey("a"))
        {
            if (Input.GetKey("d")) right = 1;
            else right = -1;
            if (looking == right && !turning) trans.rotation = Quaternion.LookRotation(Vector3.right * looking);
            if (looking != right && !turning)
            {
                if (myspeed<runspeed/2f && myspeed > runspeed / -2f && faster<5f) anim.Play("turn180");
                else anim.Play("runturn180");
                looking = right;
                StartCoroutine("Turnchar");
            }
            Checkwalls();
            if (blocked)
            {
                if (myspeed > 3f) Hitwithforce(myspeed);
                if (myspeed < -3f) Hitwithforce(myspeed);
                if (blocked) right = 0;
            }
        }
        else right = 0;
        
              
        //faster
        if (Input.GetKey(KeyCode.LeftShift)) faster = Mathf.Lerp(faster, 10f, deltafaster);
        else faster = Mathf.Lerp(faster, 0f, deltafaster);
        faster = Mathf.Clamp(faster, 0f, 10f);

        //sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (myspeed > (runspeed - 2f) || myspeed < (-runspeed + 2f)) tospeed = sprintspeed * -looking;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            tospeed = walkspeed * -looking;
        }

        //tospeed
        if (right != 0)
        {
            if (tospeed != sprintspeed * -looking)
            {
                if (faster > 5f) tospeed = runspeed*-looking;
                else tospeed = walkspeed*-looking;
            }
        }        
        if (right == 0f) tospeed = 0f;

        //SPEEDs
        Kz = 1f;
        myspeed = Mathf.Lerp(myspeed, tospeed * Kz, deltawalk);

        //jump
        if (Input.GetButtonDown("Jump") && grounded && !jumping) StartCoroutine("Jump");        

        //TEST HITS
        if (Input.GetKeyDown(KeyCode.U)) Hitwithforce(Random.Range(1f, 5f));
        if (Input.GetKeyDown(KeyCode.Y)) Hitwithforce(Random.Range(1f, 5f));
    }
    void Checkground()
    {
        //ground
        Debug.DrawRay(trans.position + new Vector3(0f, 0.05f, 0f), Vector3.down * 0.25f, Color.red);
        Debug.DrawRay(trans.position + new Vector3(0f, 0.05f, 0f) + dirforw * 0.075f, Vector3.down * 0.25f, Color.red);
        Debug.DrawRay(trans.position + new Vector3(0f, 0.05f, 0f) + dirforw * -0.075f, Vector3.down * 0.25f, Color.red);        

        if (!jumping)
        {
            float dist;
            if (grounded) dist = 0.2f; else dist = 0.075f;
            if (Physics.Raycast(trans.position + new Vector3(0f, 0.05f, 0f), Vector3.down, 0.125f) ||
              Physics.Raycast(trans.position + new Vector3(0f, 0.05f, 0f) + dirforw * 0.075f, Vector3.down, dist) ||
              Physics.Raycast(trans.position + new Vector3(0f, 0.05f, 0f) + dirforw * -0.075f, Vector3.down, dist))
                grounded = true;
            else grounded = false;
        }
        else grounded = false;

        anim.SetBool("grounded", grounded);
    }
    void Checkwalls()
    {
        //walls        
        if (Physics.Raycast(trans.position + new Vector3(0f, 0.4f, 0f) , Vector3.right * looking,0.2f))
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
                anim.Play(idles[Random.Range(0, 3)]);
            }
            else idlecounter += Time.deltaTime;
        }
    }
    void Setdir()
    {
        if (Physics.Raycast(trans.position + new Vector3(0f, 0.1f, 0f), Vector3.down * 2, 0.5f))
        {
            Physics.Raycast(trans.position + new Vector3(0f, 0.1f, 0f), Vector3.down * 2, out hit, 0.55f);
            Physics.Raycast(trans.position + new Vector3(0f, 0.1f, 0f) + Vector3.right * 0.15f, Vector3.down * 2, out hit1, 0.55f);
            Physics.Raycast(trans.position + new Vector3(0f, 0.1f, 0f) + Vector3.right * -0.15f, Vector3.down * 2, out hit2, 0.55f);
            dirforw = Vector3.Slerp(dirforw, -Vector3.Cross(hit.normal + hit1.normal + hit2.normal, Vector3.forward), 0.25f);
            dirforw.Normalize();
            angleforward = Mathf.Lerp(angleforward, Vector3.SignedAngle(trans.forward, -Vector3.Cross(hit.normal + hit1.normal + hit2.normal, trans.right), trans.right), 0.5f);
        }
        else
        {
            dirforw = Vector3.Slerp(dirforw, Vector3.right*-1f, 0.25f);
        }
    }
    void Hitwithforce(float force)
    {
        tospeed = 0f;
        if (force > 4f)
        {
                active = false;
                anim.Play("fallback");
                StartCoroutine("Waitfor", 2f);
        }
        else
        {
                active = false;
                anim.Play("hitback");
                StartCoroutine("Waitfor", 0.75f);
        }
        myspeed = 0f;
        tospeed = 0f;
        faster = 0f;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.right *looking* (-force*0.75f) + new Vector3(0f, 2f, 0f), ForceMode.Impulse);               
    }

    IEnumerator Waitfor(float itime)
    {
        active = false;
        yield return new WaitForSeconds(itime);
        active = true;
    }
    IEnumerator Jump()
    {
        jumping = true;
        if (myspeed < 1f && myspeed > -1f)
        {
            anim.Play("jump");
            yield return new WaitForSeconds(0.25f);
        }
        else
        {
            anim.Play("runjumpIN");
            yield return new WaitForSeconds(0.1f);
        }
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.right*right*(faster*0.25f) + Vector3.up * jumpforce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.3f);
        jumping = false;
    }
    IEnumerator Turnchar()
    {        
        while (AUX < 30)
        {
            turning = true;
            transform.Rotate(new Vector3(0f, -6f, 0f));
            yield return null;
            AUX ++;
        }
        trans.rotation = Quaternion.LookRotation(Vector3.right * looking);
        AUX = 0;
        turning = false;
    }
}

