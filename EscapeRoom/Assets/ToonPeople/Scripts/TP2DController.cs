using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonPeople
{
    public class TP2DController : MonoBehaviour

    {
        public GameObject[] Males;
        public GameObject[] Females;
        public bool Male;
        public bool Female;
        public float walkspeed;
        public float runspeed;
        public float sprintspeed;
        public float jumpforce;
        public RuntimeAnimatorController MaleController;
        public RuntimeAnimatorController FemaleController;
        GameObject Character;
        GameObject Model;
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
        Vector3 dirside;
        float angleforward;
        Vector3 targetpoint;
        bool active = true;
        float zoom;
        float moveAUX;
        float blocked;
        int stand;

        void Start()
        {
            int AuxChar = 0;
            if (!Male && !Female) Male = true;
            if (Male && !Female) Character = Males[Random.Range(0, Males.Length)];
            if (Female && !Male) { Character = Females[Random.Range(0, Females.Length)]; AuxChar = 1; }
            if (Female && Male)
            {
                int AUX = Random.Range(0, Males.Length + Females.Length);
                if (AUX < Males.Length) { Character = Males[Random.Range(0, AUX)]; AuxChar = 0; }
                else { Character = Females[Random.Range(0, AUX - Males.Length)]; AuxChar = 1; }
            }
            Model = Instantiate(Character, transform.position, transform.rotation, transform);
            if (Model.GetComponent<TPMalePrefabMaker>() != null)
            {
                Model.GetComponent<TPMalePrefabMaker>().Getready();
                Model.GetComponent<TPMalePrefabMaker>().Randomize();
                Model.GetComponent<TPMalePrefabMaker>().ElderOff();
            }
            if (Model.GetComponent<TPFemalePrefabMaker>() != null)
            {
                Model.GetComponent<TPFemalePrefabMaker>().Getready();
                Model.GetComponent<TPFemalePrefabMaker>().Randomize();
                Model.GetComponent<TPFemalePrefabMaker>().ElderOff();
            }

            Model.transform.Rotate(Vector3.up * 90f);
            Camera.main.transform.position = transform.position + new Vector3(0f, 1.75f, 6f);
            Camera.main.transform.parent = transform;
            Camera.main.transform.LookAt(transform.position + new Vector3(0f, 1.5f, 0f));
            trans = GetComponent<Transform>();
            rigid = GetComponent<Rigidbody>();
            anim = Model.GetComponent<Animator>();
            express = 0f;
            InputMoveDir = trans.right;
            jumping = false;
            grounded = false;
            if (Model.GetComponent<CapsuleCollider>() != null) Model.GetComponent<CapsuleCollider>().enabled = false;
            if (Model.GetComponent<Playanimation>() != null) Model.GetComponent<Playanimation>().enabled = false;
            if (AuxChar == 0) anim.runtimeAnimatorController = MaleController;
            if (AuxChar == 1) anim.runtimeAnimatorController = FemaleController;
        }
        
        void Update()
        {
            if (!jumping) CheckGround();
            Setdir();
            if (active) MoveChar();
            GetInput();

            if (!grounded) grtime += Time.deltaTime;
            else { grtime = 0f; anim.SetBool("grounded", true); }
            if (grtime > 0.25f) anim.SetBool("grounded", false);

            if (!grounded) rigid.AddForce(Vector3.up * grtime * -4f);            

            zoom = Mathf.Lerp(zoom, Aspeed * 6f, 0.75f * Time.deltaTime);
            Camera.main.transform.position = trans.position + new Vector3(0f, 1.75f, 6f) + new Vector3(0f, 0f, zoom);
            Camera.main.transform.LookAt(trans.position + new Vector3(0f, 1.5f, 0f));
        }
        void CheckGround()
        {
            RaycastHit hit;
            Vector3 targetposition = trans.position;
            if (Physics.SphereCast(trans.position + Vector3.up * 0.5f, 0.175f, -Vector3.up, out hit, 0.6f))
            {
                grounded = true;
                //Debug.DrawRay(hit.point, new Vector3(1f, 1f, 1f), Color.black);
                targetposition.y = hit.point.y;
                trans.position = Vector3.Lerp(trans.position, targetposition, Time.deltaTime * 20f);
                //trans.position = targetposition;
            }
            else
            {
                grounded = false;
                //Debug.DrawRay(hit.point, new Vector3(1f, 1f, 1f), Color.red);
            }
        }
        void CheckFront()
        {
            RaycastHit hit;
            if (Physics.SphereCast(trans.position + Vector3.up * 0.65f, 0.25f, InputMoveDir, out hit, 0.1f)) blocked = 0;            
            else blocked = 1;                
            Physics.Raycast(trans.position + new Vector3(0f, 0.3f, 0f) + InputMoveDir * 0.25f, Vector3.down * 0.35f, out hit);
            if (Vector3.SignedAngle(hit.normal, trans.up, Model.transform.right) > 45f) blocked = 0f;
        }
        void Setdir()
        {
            RaycastHit hit; RaycastHit hit1;

            Physics.Raycast(trans.position + new Vector3(0f, 0.2f, 0f), Vector3.down, out hit);
            Physics.Raycast(trans.position + new Vector3(0f, 0.2f, 0f) + InputMoveDir * 0.175f, Vector3.down, out hit1);

            dirforw = Vector3.Slerp(dirforw, -Vector3.Cross(hit.normal + hit1.normal, Model.transform.right), 18f * Time.deltaTime).normalized;

            angleforward = Vector3.SignedAngle(Model.transform.forward, dirforw, Model.transform.right);
            anim.SetFloat("Angle", angleforward);

            if (!grounded) dirforw = Model.transform.forward;
            //if(!jumping) Debug.DrawRay(trans.position + Vector3.up*0.25f, dirforw, Color.cyan);
        }
        void GetInput()
        {
            if (active)
            {
                //actions
                if (Input.GetButtonDown("Jump") && grounded) StartCoroutine("Jump");
                //walk run sprint
                if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
                {
                    anim.SetBool("Walking", true);
                    InputMoveDir = (-Vector3.right * Input.GetAxis("Horizontal")).normalized;
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
                else if (Mathf.Abs(Input.GetAxis("Horizontal")) < 1f)
                {
                    tospeed = 0f;
                    toAspeed = 0f;
                    anim.SetBool("Walking", false);
                }
            }
            if (Input.GetKey("left shift")) anim.SetBool("Running", true);
            else anim.SetBool("Running", false);
            //if (Input.GetKey("a") || Input.GetKey("d")) anim.SetBool("Walking", true);
            //else anim.SetBool("Walking", false);
            if (blocked == 0) { anim.SetBool("Walking", false); anim.SetBool("Running", false); }
            speed = Mathf.Lerp(speed, tospeed, 5f * Time.deltaTime);
            Aspeed = Mathf.Lerp(Aspeed, toAspeed, 5f * Time.deltaTime);
            anim.SetFloat("Aspeed", Aspeed * blocked);
            divergence = Mathf.Abs(Vector3.SignedAngle(Model.transform.forward, InputMoveDir, Vector3.up));

            if (Input.GetKeyDown("r"))
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
        }
        void MoveChar()
        {
            if (grounded)
            {
                CheckFront();
                if (divergence > 175f)
                {
                    if (Aspeed < 1.5f) StartCoroutine("Turn180");
                    else StartCoroutine("RunTurn180");
                }
                else
                {
                    Quaternion qAUX = Quaternion.LookRotation(InputMoveDir);
                    Model.transform.rotation = Quaternion.Lerp(Model.transform.rotation, qAUX, 5f * Time.deltaTime);
                    rigid.velocity = dirforw * speed * blocked;
                }
            }
            else rigid.velocity += dirforw * speed * 0.005f;
        }


        IEnumerator Jump()
        {
            jumping = true;
            grounded = false;
            grtime = 0.26f;
            if (Aspeed < 0.25)
            {
                anim.Play("Jump");
                yield return new WaitForSeconds(0.125f);
            }
            else
            {
                anim.Play("Runjump");
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
            anim.CrossFade("Turn180", 0.05f);
            active = false;
            while (divergence > 6f)
            {
                Model.transform.Rotate(Vector3.up * -440f * Time.deltaTime);
                if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f) { speed = 1f; Aspeed = 1f; anim.SetFloat("Aspeed", 1f); }
                else { speed = 0f; Aspeed = 0f; anim.SetFloat("Aspeed", 0f); }
                rigid.velocity = Vector3.ProjectOnPlane (dirforw * speed * blocked , trans.forward);
                yield return null;
            }
            active = true;
            Quaternion qAUX = Quaternion.LookRotation(InputMoveDir);
            Model.transform.rotation = qAUX;

        }
        IEnumerator RunTurn180()
        {
            anim.Play("Runturn180");
            active = false;
            while (divergence > 6f)
            {
                Model.transform.Rotate(Vector3.up * -380f * Time.deltaTime);
                rigid.velocity = Vector3.ProjectOnPlane(dirforw * speed * blocked, trans.forward);
                yield return null;
            }
            active = true;
            Quaternion qAUX = Quaternion.LookRotation(InputMoveDir);
            Model.transform.rotation = qAUX;
        }
    }
}
