using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAWNER : MonoBehaviour {

    public GameObject[] characters;
    float randomTime;
    float timeCounter;
    public float deviation;
    int coin;
    

	void Update ()
    {
        if (timeCounter > randomTime)
        {
            coin = Random.Range(0, 8);
            GameObject newcharacter = Instantiate(characters[coin], transform.position + (transform.right * Random.Range(-1f, 1f)),transform.rotation * Quaternion.Euler(Vector3.up * Random.Range(-deviation, deviation)));
            if (coin < 4)
            {
                if (newcharacter.GetComponent<playanimation>() != null)
                {
                    newcharacter.GetComponent<FemaleTPPrefabMaker>().Getready();
                    newcharacter.GetComponent<FemaleTPPrefabMaker>().Randomize();
                    newcharacter.GetComponent<playanimation>().playtheanimation("TPF_walk");
                    newcharacter.GetComponent<FemaleTPPrefabMaker>().FIX();
                }
            }
            else
            {
                if (newcharacter.GetComponent<playanimation>() != null)
                {
                    newcharacter.GetComponent<MaleTPPrefabMaker>().Getready();
                    newcharacter.GetComponent<MaleTPPrefabMaker>().Randomize();
                    newcharacter.GetComponent<playanimation>().playtheanimation("TPM_walk");
                    newcharacter.GetComponent<MaleTPPrefabMaker>().FIX();
                }
            }
            randomTime = Random.Range(1, 4);
            timeCounter = 0f;
        }
        timeCounter += Time.deltaTime;
    }
}
