using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class BoyVRTKPrefabMaker : MonoBehaviour
{
    public bool allOptions;
    int hair;
    int chest;
    int skintone;
    public bool glassesactive;
    public bool hatactive;
    GameObject GOhead;
    GameObject GOhands;
    GameObject[] GOhair;
    GameObject[] GOchest;
    GameObject GOglasses;
    public Object[] MATSkins;
    public Object[] MATHairA;
    public Object[] MATHairB;
    public Object[] MATHairC;
    public Object[] MATHairD;
    public Object[] MATHairE;
    public Object[] MATHairF;
    public Object[] MATEyes;
    public Object[] MATGlasses;
    public Object[] MATTshirt;
    public Object[] MATShirt;
    public Object[] MATSweater;
    public Object[] MATHat;
    public Object[] MATTeeth;
    Material headskin;

    void Start()
    {
        allOptions = false;
    }

    public void Getready()
    {
        GOhead = (GetComponent<Transform>().GetChild(1).gameObject);
        GOhands = (GetComponent<Transform>().GetChild(9).gameObject);
        GOhair = new GameObject[7];
        GOchest = new GameObject[4];

        //load models
        for (int forAUX = 0; forAUX < 6; forAUX++) GOhair[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 3).gameObject);
        GOhair[6] = (GetComponent<Transform>().GetChild(0).gameObject);
        for (int forAUX = 0; forAUX < 4; forAUX++) GOchest[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 10).gameObject);
        GOglasses = transform.Find("ROOT/TK/TK Pelvis/TK Spine/TK Spine1/TK Spine2/TK Neck/TK Head/Glasses").gameObject as GameObject;  
        if (GOhair[0].activeSelf && GOhair[1].activeSelf && GOhair[2].activeSelf)
        {
            ResetSkin();
            Randomize();
        }
        else
        {
            for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) { if (GOhair[forAUX].activeSelf) hair = forAUX; }
            while (!GOchest[chest].activeSelf) chest++;
            if (hair > 6) hatactive = true;            
        }
    }
    void ResetSkin()
    {
        string[] allskins = new string[6] { "TKBoyA0", "TKBoyB0", "TKBoyC0", "TKGirlA0", "TKGirlB0", "TKGirlC0" };
        Material[] AUXmaterials;
        int materialcount = GOhead.GetComponent<Renderer>().sharedMaterials.Length;
        //ref head material
        AUXmaterials = GOhead.GetComponent<Renderer>().sharedMaterials;
        materialcount = GOhead.GetComponent<Renderer>().sharedMaterials.Length;
        for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
            for (int forAUX3 = 0; forAUX3 < allskins.Length; forAUX3++)
                for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                {
                    if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                    {
                        headskin = AUXmaterials[forAUX2];
                    }
                }
        //hands        
        GOhands.GetComponent<Renderer>().sharedMaterial = headskin;
        //chest
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++)
        {
            AUXmaterials = GOchest[forAUX].GetComponent<Renderer>().sharedMaterials;
            materialcount = GOchest[forAUX].GetComponent<Renderer>().sharedMaterials.Length;
            for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
                for (int forAUX3 = 0; forAUX3 < allskins.Length; forAUX3++)
                    for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                    {
                        if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                        {
                            AUXmaterials[forAUX2] = headskin;
                            GOchest[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                        }
                    }
        }        
    }
    public void Deactivateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(false);
        GOglasses.SetActive(false);
        glassesactive = false;
    }
    public void Activateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(true);
        GOglasses.SetActive(true);
        glassesactive = true;
    }
    public void Menu()
    {
        allOptions = !allOptions;
    }
    public void Glasseson()
    {
        glassesactive = !glassesactive;
        GOglasses.SetActive(glassesactive);
    }
    public void HatOn()
    {
        if (!hatactive)
        {
            GOhair[hair].SetActive(false);
            hair = 6;
            GOhair[hair].SetActive(true);
            hatactive = true;
        }
        else
        {
            GOhair[hair].SetActive(false);
            hair = 0;
            GOhair[hair].SetActive(true);
            hatactive = false;
        }
    }

    //models
    public void Nexthair()
    {
        GOhair[hair].SetActive(false);
        if (hatactive) hair = 0;
        hatactive = false;
        if (hair < GOhair.Length - 3) hair++;
        else hair = 0;
        GOhair[hair].SetActive(true);
    }
    public void Prevhair()
    {
        GOhair[hair].SetActive(false);
        if (hatactive) hair = GOhair.Length - 2;
        hatactive = false;
        if (hair > 0) hair--;
        else hair = GOhair.Length - 2;
        GOhair[hair].SetActive(true);
    }
    public void Nextchest()
    {
        GOchest[chest].SetActive(false);
        if (chest < GOchest.Length - 1) chest++;
        else chest = 0;
        GOchest[chest].SetActive(true);
    }
    public void Prevchest()
    {
        GOchest[chest].SetActive(false);
        chest--;
        if (chest < 0) chest = GOchest.Length - 1;
        GOchest[chest].SetActive(true);
    }


    //materials
    public void Nextskincolor(int todo)
    {
        ChangeMaterials(MATSkins, todo);
    }
    public void Nextglasses(int todo)
    {
        ChangeMaterials(MATGlasses, todo);
    }
    public void Nexteyescolor(int todo)
    {
        ChangeMaterials(MATEyes, todo);
    }
    public void Nextteethcolor(int todo)
    {
        ChangeMaterials(MATTeeth, todo);
    }
    public void Nexthaircolor(int todo)
    {
        ChangeMaterials(MATHairA, todo);
        ChangeMaterials(MATHairB, todo);
        ChangeMaterials(MATHairC, todo);
        ChangeMaterials(MATHairD, todo);
        ChangeMaterials(MATHairE, todo);
        ChangeMaterials(MATHairF, todo);
    }
    public void Nexthatcolor(int todo)
    {
        if (hatactive) ChangeMaterials(MATHat, todo);
    }
    public void Nextchestcolor(int todo)
    {
        if (chest == 0) ChangeMaterials(MATShirt, todo);
        if (chest == 1) ChangeMaterials(MATSweater, todo);
        if (chest > 1) ChangeMaterials(MATTshirt, todo);
    }



    public void Resetmodel()
    {
        Activateall();
        ChangeMaterials(MATHat, 3);
        ChangeMaterials(MATSkins, 3);
        ChangeMaterials(MATHairA, 3);
        ChangeMaterials(MATHairB, 3);
        ChangeMaterials(MATHairC, 3);
        ChangeMaterials(MATHairD, 3);
        ChangeMaterials(MATHairE, 3);
        ChangeMaterials(MATHairF, 3);
        ChangeMaterials(MATGlasses, 3);
        ChangeMaterials(MATEyes, 3);
        ChangeMaterials(MATTshirt, 3);
        ChangeMaterials(MATShirt, 3);
        ChangeMaterials(MATSweater, 3);
        Menu();
    }
    public void Randomize()
    {
        Deactivateall();
        ResetSkin();
        //models
        hair = Random.Range(0, GOhair.Length);
        GOhair[hair].SetActive(true);
        if (hair > 5) hatactive = true; else hatactive = false;
        chest = Random.Range(0, GOchest.Length); GOchest[chest].SetActive(true);
        if (Random.Range(0, 4) > 2)
        {
            glassesactive = true;
            GOglasses.SetActive(true);
            ChangeMaterials(MATGlasses, 2);
        }
        else glassesactive = false;

        //materials
        ChangeMaterials(MATEyes, 2);
        ChangeMaterials(MATTeeth, 2);
        for (int forAUX = 0; forAUX < (Random.Range(0, 4)); forAUX++) Nexthaircolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 12)); forAUX++) Nexthatcolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 17)); forAUX++) Nextchestcolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 4)); forAUX++) Nextskincolor(0);

    }
    public void CreateCopy()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 13; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TK/TK Pelvis/TK Spine/TK Spine1/TK Spine2/TK Neck/TK Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<BoyVRTKPrefabMaker>());
    }
    public void FIX()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 13; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TK/TK Pelvis/TK Spine/TK Spine1/TK Spine2/TK Neck/TK Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<BoyVRTKPrefabMaker>());
        DestroyImmediate(gameObject);
    }


    void ChangeMaterial(GameObject GO, Object[] MAT, int todo)
    {
        bool found = false;
        int MATindex = 0;
        int subMAT = 0;
        Material[] AUXmaterials;
        AUXmaterials = GO.GetComponent<Renderer>().sharedMaterials;
        int materialcount = GO.GetComponent<Renderer>().sharedMaterials.Length;

        for (int forAUX = 0; forAUX < materialcount; forAUX++)
            for (int forAUX2 = 0; forAUX2 < MAT.Length; forAUX2++)
            {
                if (AUXmaterials[forAUX].name == MAT[forAUX2].name)
                {
                    subMAT = forAUX;
                    MATindex = forAUX2;
                    found = true;
                }
            }
        if (found)
        {
            if (todo == 0) //increase
            {
                MATindex++;
                if (MATindex > MAT.Length - 1) MATindex = 0;
            }
            if (todo == 1) //decrease
            {
                MATindex--;
                if (MATindex < 0) MATindex = MAT.Length - 1;
            }
            if (todo == 2) //random value
            {
                MATindex = Random.Range(0, MAT.Length);
            }
            if (todo == 3) //reset value
            {
                MATindex = 0;
            }
            if (todo == 4) //penultimate
            {
                MATindex = MAT.Length - 2;
            }
            if (todo == 5) //last one
            {
                MATindex = MAT.Length - 1;
            }
            AUXmaterials[subMAT] = MAT[MATindex] as Material;
            GO.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
        }
    }
    void ChangeMaterials(Object[] MAT, int todo)
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) ChangeMaterial(GOhair[forAUX], MAT, todo);
        ChangeMaterial(GOhead, MAT, todo);
        ChangeMaterial(GOglasses, MAT, todo);
        ChangeMaterial(GOhands, MAT, todo);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) ChangeMaterial(GOchest[forAUX], MAT, todo);
    }
    void SwitchMaterial(GameObject GO, Object[] MAT1, Object[] MAT2)
    {
        Material[] AUXmaterials;
        AUXmaterials = GO.GetComponent<Renderer>().sharedMaterials;
        int materialcount = GO.GetComponent<Renderer>().sharedMaterials.Length;
        int index = 0;
        for (int forAUX = 0; forAUX < materialcount; forAUX++)
            for (int forAUX2 = 0; forAUX2 < MAT1.Length; forAUX2++)
            {
                if (AUXmaterials[forAUX].name == MAT1[forAUX2].name)
                {
                    index = forAUX2;
                    if (forAUX2 > MAT2.Length - 1) index -= (int)Mathf.Floor(index / 4) * 4;
                    AUXmaterials[forAUX] = MAT2[index] as Material;
                    GO.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                }
            }
    }
}




 

