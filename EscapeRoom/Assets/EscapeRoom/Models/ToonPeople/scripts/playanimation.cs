using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playanimation : MonoBehaviour
{
    public string anim;
    public bool delayed;
    public bool happy;
    public bool sad;
    public bool angry;
    public bool amazed;
    public bool disgust;

    void Start()
    {
        GetComponent<Animator>().Play(anim);
        if (happy)        GetComponent<Animator>().SetLayerWeight(1, 1f);
        else if (sad)     GetComponent<Animator>().SetLayerWeight(2, 1f);
            else if (angry)   GetComponent<Animator>().SetLayerWeight(3, 1f);
                else if (amazed)  GetComponent<Animator>().SetLayerWeight(4, 1f);
                    else if (disgust) GetComponent<Animator>().SetLayerWeight(5, 1f);        
        if (delayed)
        {            
            StartCoroutine("playanim",anim);
        }        
    }

    IEnumerator playanim(string anim)
    {
        GetComponent<Animator>().speed = 0.65f;
        yield return new WaitForSeconds(Random.Range(0f,2f));
        GetComponent<Animator>().speed = 1f;
        GetComponent<Animator>().Play(anim);        
    }

    public void playtheanimation(string newanim)
    {
        anim = newanim;
        StartCoroutine("playanim", anim);
    }
}
