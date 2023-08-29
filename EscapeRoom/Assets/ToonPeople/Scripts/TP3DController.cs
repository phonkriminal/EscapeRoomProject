using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonPeople
{
    public class TP3DController : MonoBehaviour
    {
        public GameObject[] Males;
        public GameObject[] Females;
        public GameObject[] ElderMales;
        public GameObject[] ElderFemales;
        public bool Male;
        public bool Female;
        public bool Elder;
        public float walkspeed;
        public float runspeed;
        public float sprintspeed;
        public float jumpforce;
        public RuntimeAnimatorController MaleController;
        public RuntimeAnimatorController FemaleController;
        public RuntimeAnimatorController ElderController;
        GameObject Character;
        GameObject Model;
        GameObject Helper; GameObject Helper2;
        GameObject Target;
        Transform trans;
        Rigidbody rigid;
        Animator anim;
        Vector3 InputMoveDir;
        float divergence;
        float tospeed;
        float toAspeed;
        float speed;
        float Aspeed;
        float express;
        float grtime;
        bool jumping;
        bool grounded;
        Vector3 dirforw;
        Vector3 dirslide;
        float angleforward;
        float angleright;
        bool active;
        bool idles;
        float idletime;
        float blocked;
        int stand; 
        float zoom;


        void Start()
        {
            int AuxChar = 0;
            active = true;
            stand = 0;
            if (transform.childCount > 0) while (transform.childCount > 0) DestroyImmediate(transform.GetChild(0).gameObject);
            idles = false;
            if (!Male && !Female) { Male = true; AuxChar = 0; }
            if (!Elder)
            {
                if (Male && !Female) { Character = Males[Random.Range(0, 4)]; AuxChar = 0; }
                if (Female && !Male) { Character = Females[Random.Range(0, 4)]; AuxChar = 1; }
                if (Female && Male)
                {
                    int AUX = Random.Range(0, 8);
                    if (AUX < 4) { Character = Males[Random.Range(0, AUX)]; AuxChar = 0; }
                    else { Character = Females[Random.Range(0, AUX - 4)]; AuxChar = 1; }
                }
            }
            else
            {
                if (Male && !Female) { Character = ElderMales[Random.Range(0, 4)]; AuxChar = 2; }
                if (Female && !Male) { Character = ElderFemales[Random.Range(0, 4)]; AuxChar = 3; }
                if (Female && Male)
                {
                    int AUX = Random.Range(0, 8);
                    if (AUX < 4) { Character = ElderMales[Random.Range(0, AUX)]; AuxChar = 2; }
                    else { Character = ElderFemales[Random.Range(0, AUX - 4)]; AuxChar = 3; }
                }
                walkspeed = walkspeed * 0.65f;
                runspeed = runspeed * 0.6f; sprintspeed = sprintspeed * 0.75f; jumpforce = jumpforce * 0.5f;
            }

            Model = Instantiate(Character, transform.position, transform.rotation, transform);

            if (Model.GetComponent<TPMalePrefabMaker>() != null)
            {
                Model.GetComponent<TPMalePrefabMaker>().Getready();
                Model.GetComponent<TPMalePrefabMaker>().Randomize();
                if (Elder) Model.GetComponent<TPMalePrefabMaker>().ElderOn();
            }
            if (Model.GetComponent<TPFemalePrefabMaker>() != null)
            {
                Model.GetComponent<TPFemalePrefabMaker>().Getready();
                Model.GetComponent<TPFemalePrefabMaker>().Randomize();
                if (Elder) Model.GetComponent<TPFemalePrefabMaker>().ElderOn();
            }
            Helper = new GameObject("Helper"); Helper2 = new GameObject("Helper2");
            Target = new GameObject("Target");
            Helper.transform.position = transform.position;
            Helper2.transform.position = Helper.transform.position;
            Helper.transform.parent = transform;
            Helper2.transform.parent = Helper.transform;
            Target.transform.position = transform.position + new Vector3(0f, 0.75f, 0f);
            Target.transform.parent = Helper.transform;
            Camera.main.transform.position = transform.position + new Vector3(0f, 3f, -5f);
            Helper2.transform.position = transform.position + new Vector3(0f, 3f, -5f);
            Camera.main.transform.parent = Helper.transform;
            Camera.main.transform.LookAt(Target.transform);

            zoom = 0f;
            trans = GetComponent<Transform>();
            rigid = GetComponent<Rigidbody>();
            anim = Model.GetComponent<Animator>();
            express = 0f;
            InputMoveDir = transform.forward;
            Helper.transform.Rotate(Vector3.up * 180);
            if (Model.GetComponent<CapsuleCollider>() != null) Model.GetComponent<CapsuleCollider>().enabled = false;
            if (Model.GetComponent<Playanimation>() != null) Model.GetComponent<Playanimation>().enabled = false;
            if (AuxChar > 1) anim.runtimeAnimatorController = ElderController;
            if (AuxChar == 0) anim.runtimeAnimatorController = MaleController;
            if (AuxChar == 1) anim.runtimeAnimatorController = FemaleController;
        }
        void Update()
        {
            if (!jumping) CheckGround();
            SetMoveDir();
            if (active) MoveChar();
            GetInput();
            MoveCam();
            if (!grounded) grtime += Time.deltaTime;
            else { grtime = 0f; anim.SetBool("grounded", true); }
            if (grtime > 0.25f) anim.SetBool("grounded", false);
            if (!grounded) rigid.AddForce((Vector3.up - dirslide) * grtime * -4f);
        }

        void SetMoveDir()
        {            
            RaycastHit hit; RaycastHit hit1;
            Physics.Raycast(trans.position + new Vector3(0f, 0.2f, 0f), Vector3.down * 0.25f, out hit);
            Physics.Raycast(trans.position + new Vector3(0f, 0.2f, 0f) + dirforw * 0.18f * speed, Vector3.down * 0.25f, out hit1);
            dirforw = Vector3.Slerp(dirforw, -Vector3.Cross(hit.normal + hit1.normal, Model.transform.right), 18f * Time.deltaTime).normalized;

            angleforward = Vector3.SignedAngle(Model.transform.forward, dirforw, Model.transform.right);
            angleright = Mathf.Lerp(angleright, Vector3.SignedAngle(Model.transform.forward, InputMoveDir, Vector3.up) * 0.125f, 12f * Time.deltaTime);
            anim.SetFloat("turn", angleright);
            anim.SetFloat("angle", angleforward);

            if (!grounded) dirforw = Model.transform.forward;
        }
        void CheckGround()
        {
            RaycastHit hit;
            Vector3 targetposition = trans.position;
            if (Physics.SphereCast(trans.position + Vector3.up * 0.5f, 0.175f, -Vector3.up, out hit, 0.6f))
            {
                if (Vector3.Angle(Vector3.up, hit.normal) > 45f)
                {
                    grounded = false;
                    dirslide = Vector3.ProjectOnPlane(hit.normal, Vector3.up);
                }
                else
                {
                    grounded = true;
                    targetposition.y = hit.point.y;
                    trans.position = Vector3.Lerp(trans.position, targetposition, Time.deltaTime * 20f);
                    dirslide = Vector3.zero;
                }
            }
            else
            {
                grounded = false;
                dirslide = Vector3.zero;
            }
        }
        void CheckFront()
        {
            RaycastHit hit;
            if (Physics.SphereCast(trans.position + Vector3.up * 0.65f, 0.25f, Model.transform.forward, out hit, 0.1f))
            {
                if (hit.transform.tag != "push")
                    blocked = 0;
            }
            else blocked = 1;

            Physics.Raycast(trans.position + new Vector3(0f, 0.3f, 0f) + dirforw * 0.25f, Vector3.down * 0.35f, out hit);
            if (Vector3.SignedAngle(hit.normal, trans.up, Model.transform.right) > 45f) blocked = 0f;
        }
        void GetInput()
        {
            //walkin
            if (stand == 0)
            {
                //idles
                if (grounded && !jumping) idletime += Time.deltaTime;
                if (!idles && idletime > 3f && !Input.anyKey) { anim.SetLayerWeight(1, 1f); idles = true; idletime = 0f; anim.SetInteger("idles", 0); }
                if (Input.anyKey) idletime = 0f;
                if (idles)
                {
                    if (idletime > 1f) { anim.SetInteger("idles", Random.Range(1, 7)); idletime = 0f; }
                    if (Input.anyKey) { idles = false; anim.SetLayerWeight(1, 0f); idletime = 0f; anim.SetInteger("idles", 0); }
                }
                //walk run sprint
                if (active)
                {
                    if (Input.GetButtonDown("Jump") && grounded) StartCoroutine("Jump");
                    if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
                    {
                        InputMoveDir = (Helper.transform.forward * Input.GetAxis("Vertical") + Helper.transform.right * Input.GetAxis("Horizontal")).normalized;
                        if (Input.GetKey("left shift"))
                        {
                            tospeed = runspeed + express * (sprintspeed - runspeed);
                            toAspeed = 2f + express;
                        }
                        else
                        {
                            tospeed = walkspeed;
                            toAspeed = 1f;
                        }
                        if (Input.GetKeyDown("left shift") && Aspeed > 1.5f) express = 1f;
                        if (Input.GetKeyUp("left shift")) express = 0f;
                    }
                    else if (Mathf.Abs(Input.GetAxis("Vertical")) < 1f && Mathf.Abs(Input.GetAxis("Horizontal")) < 1f)
                    {
                        tospeed = 0f;
                        toAspeed = 0f;
                    }
                }
                speed = Mathf.Lerp(speed, tospeed, 5f * Time.deltaTime);
                Aspeed = Mathf.Lerp(Aspeed, toAspeed, 5f * Time.deltaTime);
                anim.SetFloat("Aspeed", Aspeed * blocked);
                divergence = Mathf.Abs(Vector3.SignedAngle(Model.transform.forward, InputMoveDir, Vector3.up));
                                
                if (Input.GetKeyDown("x") && grounded) //crouch
                {
                    anim.SetBool("wakeup", false);
                    StopAllCoroutines();
                    anim.Play("crouch");
                    stand = 7;
                    active = false; 
                    StopChar();
                }
                if (Input.GetKeyDown("c") && grounded) //sitdown
                {
                    RaycastHit hit;
                    if (Physics.SphereCast(trans.position + Vector3.up * 0.35f, 0.1f, -Model.transform.forward, out hit, 0.2f)
                     && !Physics.SphereCast(trans.position + Vector3.up * 0.65f, 0.2f, -Model.transform.forward, out hit, 0.5f))
                    {
                        anim.SetBool("wakeup", false);
                        StopAllCoroutines();
                        anim.Play("sitdown");
                        stand = 8;
                        active = false;
                        StopChar();
                    }
                    else
                    {
                        anim.Play("lookback");
                        StartCoroutine("DelayActive", 0.25f);
                    }                    
                }
            }
            //crouch
            else if (stand == 7)
            {
                if (Input.anyKeyDown)
                {
                    stand = 0;
                    anim.SetBool("wakeup", true);
                    StartCoroutine("DelayActive", 0.5f);
                }
            }
            //sitdown
            else if (stand == 8)
            {
                if (Input.anyKeyDown)
                {
                    stand = 0;
                    anim.SetBool("wakeup", true);
                    StartCoroutine("DelayActive", 1f);
                }
            }
            //always
            if (Input.GetKeyDown("r")) //randomize character
            {
                if (Model.GetComponent<TPFemalePrefabMaker>() != null)
                {
                    Model.GetComponent<TPFemalePrefabMaker>().Getready();
                    Model.GetComponent<TPFemalePrefabMaker>().Randomize();
                }
                if (Model.GetComponent<TPMalePrefabMaker>() != null)
                {
                    Model.GetComponent<TPMalePrefabMaker>().Getready();
                    Model.GetComponent<TPMalePrefabMaker>().Randomize();
                }
            }
            if (Input.GetKeyDown("o")) //reset position
            {
                trans.position = new Vector3(0f, 25f, 0f);
                active = true;
                StopChar();
            }
        }
        void MoveChar()
        {
            if (grounded)
            {
                CheckFront();
                if (Aspeed < 0.5f) //standstill
                {
                    if (divergence < 80)
                    {
                        Quaternion qAUX = Quaternion.LookRotation(InputMoveDir);
                        Model.transform.rotation = Quaternion.Lerp(Model.transform.rotation, qAUX, 5f * Time.deltaTime);
                        rigid.velocity = dirforw * speed * blocked;
                    }
                    else if (divergence < 175f) StartCoroutine("Turn90");
                    else StartCoroutine("Turn180");
                }
                else //moving
                {
                    Quaternion qAUX = Quaternion.LookRotation(InputMoveDir);
                    Model.transform.rotation = Quaternion.Lerp(Model.transform.rotation, qAUX, 5f * Time.deltaTime);
                    rigid.velocity = dirforw * speed * blocked;
                }
            }
            else //falling
            {
                Quaternion qAUX = Quaternion.LookRotation(InputMoveDir);
                Model.transform.rotation = Quaternion.Lerp(Model.transform.rotation, qAUX, 2.5f * Time.deltaTime);
                rigid.velocity += dirforw * speed * 0.005f;
            }
        }
        void MoveCam()
        {
            //Camera position
            Helper.transform.Rotate(0f, Input.GetAxis("Mouse X"), 0f);

            Target.transform.position = trans.position + Vector3.up * 1.5f;
            Target.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0f, 0f));
            Vector3 v3AUX = Target.transform.forward.normalized;

            zoom -= Input.GetAxis("Mouse ScrollWheel");
            zoom = Mathf.Clamp(zoom, -4f, 18f);

            Helper2.transform.position = Target.transform.position + v3AUX * -(5f + zoom);
            Helper2.transform.LookAt(Target.transform.position);

            RaycastHit hit;
            if (Physics.Raycast(Helper2.transform.position, v3AUX * 25f, out hit))
            {
                if (hit.transform.tag != "Player")
                {
                    RaycastHit hit2;
                    Physics.Raycast(Target.transform.position, -v3AUX, out hit2);
                    Camera.main.transform.position = hit2.point;
                }
                else
                {
                    Camera.main.transform.position = Helper2.transform.position;
                }
            }

            Camera.main.transform.LookAt(Target.transform.position + (Vector3.up * ((Camera.main.transform.position - Target.transform.position).magnitude) * -0.15f));


        }
        void StopChar()
        {
            rigid.velocity = Vector3.zero;
            Aspeed = 0f;
            speed = 0f;
            anim.SetFloat("Aspeed",0f);
            tospeed = 0f;
            toAspeed = 0f;
        }
        
        IEnumerator DelayActive( float delaytime)
        {
            yield return new WaitForSeconds(delaytime);
            active = true;
        }
        IEnumerator Jump()
        {
            jumping = true;
            grounded = false;
            anim.SetBool("grounded", false);
            if (Aspeed < 0.25)
            {
                anim.Play("jump");
                yield return new WaitForSeconds(0.125f);
            }
            else
            {
                anim.Play("runjump");
                yield return new WaitForSeconds(0.01f);
            }
            toAspeed = 0f;
            speed = speed * 0.5f;
            rigid.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            yield return new WaitForSeconds(0.5f);

            jumping = false;
        }
        IEnumerator Turn180()
        {
            anim.CrossFade("turn180", 0.1f);
            active = false;
            while (divergence > 6f)
            {
                Model.transform.Rotate(Vector3.up * -420f * Time.deltaTime);                
                if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
                { speed = 1f; Aspeed = 1f; anim.SetFloat("Aspeed", 1f); }
                else { speed = 0f; Aspeed = 0f; anim.SetFloat("Aspeed", 0f); }
                yield return null;
            }
            active = true;
        }
        IEnumerator Turn90()
        {
            float spin = 1;
            if (Vector3.SignedAngle(Model.transform.forward, InputMoveDir, Vector3.up) > 0f)
            {
                anim.CrossFade("turnR90", 0.01f);
                spin = 1;
            }
            else
            {
                anim.CrossFade("turnL90", 0.01f);
                spin = -1;
            }
            active = false;
            while (divergence > 6f)
            {
                Model.transform.Rotate(Vector3.up * 237.5f * spin * Time.deltaTime);

                if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
                { speed = 1f; Aspeed = 1f; anim.SetFloat("Aspeed", 1f); }
                else { speed = 0f; Aspeed = 0f; anim.SetFloat("Aspeed", 0f); }
                yield return null;
            }
            active = true;
        }
    }
}
