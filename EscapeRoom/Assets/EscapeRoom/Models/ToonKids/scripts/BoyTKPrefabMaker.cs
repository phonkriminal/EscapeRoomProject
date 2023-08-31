using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class BoyTKPrefabMaker : MonoBehaviour
{
    public bool allOptions;
    int hair;
    int chest;
    int legs;
    int feet;
    int skintone;
    public bool glassesactive;
    public bool hatactive;
    GameObject GOhead;
    GameObject GOheadsimple;
    GameObject[] GOfeet;
    GameObject[] GOhair;
    GameObject[] GOchest;
    GameObject[] GOlegs;
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
    public Object[] MATLegs;
    public Object[] MATFeetA;
    public Object[] MATFeetB;
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
        GOheadsimple = (GetComponent<Transform>().GetChild(2).gameObject);
        GOheadsimple.SetActive(false);

        GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);
        GOfeet = new GameObject[3];
        GOhair = new GameObject[7];
        GOchest = new GameObject[6];
        GOlegs = new GameObject[5];

        //load models
        for (int forAUX = 0; forAUX < 3; forAUX++) GOfeet[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 10).gameObject);
        for (int forAUX = 0; forAUX < 6; forAUX++) GOhair[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 4).gameObject);
        GOhair[6] = (GetComponent<Transform>().GetChild(0).gameObject);
        for (int forAUX = 0; forAUX < 6; forAUX++) GOchest[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 18).gameObject);
        for (int forAUX = 0; forAUX < 5; forAUX++) GOlegs[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 13).gameObject);
        GOglasses = transform.Find("ROOT/TK/TK Pelvis/TK Spine/TK Spine1/TK Spine2/TK Neck/TK Head/Glasses").gameObject as GameObject;
                
        if (GOfeet[0].activeSelf && GOfeet[1].activeSelf && GOfeet[2].activeSelf)
        {
            ResetSkin();
            Randomize();
        }
        else
        {
            for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) { if (GOhair[forAUX].activeSelf) hair = forAUX; }
            while (!GOchest[chest].activeSelf) chest++;
            if (chest == 0 || chest > 3) while (!GOlegs[legs].activeSelf) legs++;
            while (!GOfeet[feet].activeSelf) feet++;
            if (hair > 6) hatactive = true;
        }
    }
    void ResetSkin()
    {
        string[] allskins = new string[6] { "TKBoyA0", "TKBoyB0", "TKBoyC0" , "TKGirlA0", "TKGirlB0", "TKGirlC0" };
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
        //legs
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++)
        {
            AUXmaterials = GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials;
            materialcount = GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials.Length;
            for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
                for (int forAUX3 = 0; forAUX3 < allskins.Length; forAUX3++)
                    for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                    {
                        if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                        {
                            AUXmaterials[forAUX2] = headskin;
                            GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                        }
                    }
        }
        //feet
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++)
        {
            AUXmaterials = GOfeet[forAUX].GetComponent<Renderer>().sharedMaterials;
            materialcount = GOfeet[forAUX].GetComponent<Renderer>().sharedMaterials.Length;
            for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
                for (int forAUX3 = 0; forAUX3 < allskins.Length; forAUX3++)
                    for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                    {
                        if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                        {
                            AUXmaterials[forAUX2] = headskin;
                            GOfeet[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                        }
                    }
        }
    }
    public void Deactivateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(false);
        GOglasses.SetActive(false);
        glassesactive = false;
    }
    public void Activateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(true);        
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
        if (hair < GOhair.Length - 2) hair++;
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
    public void Nextlegs()
    {
        GOlegs[legs].SetActive(false);
        if (legs < GOlegs.Length - 1) legs++;
        else legs = 0;
        GOlegs[legs].SetActive(true);
    }
    public void Prevlegs()
    {
        GOlegs[legs].SetActive(false);
        if (legs > 0) legs--;
        else legs = GOlegs.Length - 1;
        GOlegs[legs].SetActive(true);
    }
    public void Nextfeet()
    {
        GOfeet[feet].SetActive(false);
        if (feet < GOfeet.Length - 1) feet++;
        else feet = 0;
        GOfeet[feet].SetActive(true);
    }
    public void Prevfeet()
    {
        GOfeet[feet].SetActive(false);
        if (feet > 0) feet--;
        else feet = GOfeet.Length - 1;
        GOfeet[feet].SetActive(true);
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
        if (chest < 2) ChangeMaterials(MATShirt, todo); 
        if (chest == 2) ChangeMaterials(MATSweater, todo);
        if (chest > 2) ChangeMaterials(MATTshirt, todo); 
    }
    public void Nextlegscolor(int todo)
    {
        ChangeMaterials(MATLegs, todo);
    }
    public void Nextfeetcolor(int todo)
    {
        if (feet < 2) ChangeMaterials(MATFeetA, todo);
        if (feet == 2) ChangeMaterials(MATFeetB, todo);
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
        ChangeMaterials(MATLegs, 3);
        ChangeMaterials(MATFeetA, 3);
        ChangeMaterials(MATFeetB, 3);
        Menu();
    }
    public void Randomize()
    {
        Deactivateall();
        ResetSkin();
        //models
        hair = Random.Range(0, 7);
        GOhair[hair].SetActive(true);
        if (hair > 4) hatactive = true;else hatactive = false;
        chest = Random.Range(1, GOchest.Length); GOchest[chest].SetActive(true);
        legs = Random.Range(1, GOlegs.Length); GOlegs[legs].SetActive(true);
        feet = Random.Range(1, GOfeet.Length); GOfeet[feet].SetActive(true);
                
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
        for (int forAUX = 0; forAUX < (Random.Range(0, 4)); forAUX++)  Nexthaircolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 13)); forAUX++) Nextfeetcolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 25)); forAUX++) Nextlegscolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 12)); forAUX++) Nexthatcolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 17)); forAUX++) Nextchestcolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 5)); forAUX++)  Nextskincolor(0);
    }
    public void CreateCopy()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 23; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TK/TK Pelvis/TK Spine/TK Spine1/TK Spine2/TK Neck/TK Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<BoyTKPrefabMaker>());
    }
    public void FIX()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 23; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TK/TK Pelvis/TK Spine/TK Spine1/TK Spine2/TK Neck/TK Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<BoyTKPrefabMaker>());
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
        ChangeMaterial(GOheadsimple, MAT, todo);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) ChangeMaterial(GOchest[forAUX], MAT, todo);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) ChangeMaterial(GOlegs[forAUX], MAT, todo);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) ChangeMaterial(GOfeet[forAUX], MAT, todo);
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