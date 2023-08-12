using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class MaleTPPrefabMaker : MonoBehaviour
{
    public bool allOptions;
    int hair;
    int chest;
    int legs;
    int feet;
    int tie;
    int jacket;
    public bool tieactive;
    public bool tieactivecolor;
    public bool glassesactive;
    public bool jacketactive;
    public bool hatactive;
    public bool beardactive;
    public bool haircoloractive;
    GameObject GOhead;
    GameObject GOheadsimple;
    GameObject GObeard;
    GameObject GObeardsimple;
    GameObject[] GOfeet;
    GameObject[] GOhair;
    GameObject[] GOchest;
    GameObject[] GOlegs;
    GameObject GOglasses;
    GameObject[] GOjackets;
    GameObject[] GOties;
    public Object[] MATSkins;
    public Object[] MATElderSkins;
    public Object[] MAThairA;
    public Object[] MAThairB;
    public Object[] MAThairC;
    public Object[] MAThairD;
    public Object[] MAThairE;
    public Object[] MAThairF;
    public Object[] MAThairG;
    public Object[] MATGlasses;
    public Object[] MATTshirt;
    public Object[] MATShirtA;
    public Object[] MATShirtB;
    public Object[] MATEyes;
    public Object[] MATJacket;
    public Object[] MATSweater;
    public Object[] MATLegs;
    public Object[] MATFeetA;
    public Object[] MATFeetB;
    public Object[] MATHatA;
    public Object[] MATHatB;
    public Object[] MATHatC;
    public Object[] MATBowtie;
    public Object[] MATTie;
    public Object[] MATBeard;
    Vector4 beard;
    public Material trans;
    public Object[] MATteeth;    
    public bool elder;
    Material headskin;

    void Start()
    {
        allOptions = false;
    }

    public void Getready()

    {
        GOhead = (GetComponent<Transform>().GetChild(2).gameObject);
        GOheadsimple = (GetComponent<Transform>().GetChild(3).gameObject);
        GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
        GetComponent<Transform>().GetChild(3).gameObject.SetActive(false);
        GOfeet = new GameObject[2];
        GOhair = new GameObject[10];
        GOchest = new GameObject[7];
        GOlegs = new GameObject[2];
        GOjackets = new GameObject[2];
        GOties = new GameObject[3];
        beardactive = true;
        beard = new Vector4 (1, 1, 1, 1);        

        for (int forAUX = 0; forAUX < 2; forAUX++) GOfeet[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 6).gameObject);
        for (int forAUX = 0; forAUX < 10; forAUX++) GOhair[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 8).gameObject); 
        for (int forAUX = 0; forAUX < 4; forAUX++) GOchest[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 20).gameObject);
        for (int forAUX = 0; forAUX < 3; forAUX++) GOchest[forAUX + 4] = (GetComponent<Transform>().GetChild(forAUX + 28).gameObject);
        for (int forAUX = 0; forAUX < 2; forAUX++) GOlegs[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 18).gameObject);
        for (int forAUX = 0; forAUX < 2; forAUX++) GOjackets[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 24).gameObject);
        for (int forAUX = 0; forAUX < 2; forAUX++) GOties[forAUX + 1] = (GetComponent<Transform>().GetChild(forAUX + 26).gameObject);
        GOties[0] = (GetComponent<Transform>().GetChild(5).gameObject); 
        GObeard = (GetComponent<Transform>().GetChild(0).gameObject);
        GObeardsimple = (GetComponent<Transform>().GetChild(1).gameObject);
        GOglasses = transform.Find("ROOT/TP/TP Pelvis/TP Spine/TP Spine1/TP Spine2/TP Neck/TP Head/Glasses").gameObject as GameObject;

        if (GOfeet[0].activeSelf && GOfeet[1].activeSelf)
        {
            ResetSkin();
            Randomize();
            elder = false;
            haircoloractive = true;
        }
        else
        {
            while (!GOhair[hair].activeSelf) hair++;            
            while (!GOchest[chest].activeSelf) chest++;
            while (!GOlegs[legs].activeSelf) legs++;
            while (!GOfeet[feet].activeSelf) feet++;
            if (GOjackets[0].activeSelf) jacket = 0; if (GOjackets[1].activeSelf) jacket = 1;
            if (!GOjackets[0].activeSelf && !GOjackets[1].activeSelf) jacket = 2;
            tie = 3;
            for (int forAUX = 0; forAUX > 3; forAUX++)
            {
                if (GOties[forAUX].activeSelf) tie = forAUX;
            }
            if (GOglasses.activeSelf) glassesactive = true;
            Checkties();
            Checkbeard();
            Checkelder();
        }
    }
    void ResetSkin()
    {
        string[] allskins = new string[8] { "TPMaleA0", "TPMaleB0", "TPMaleC0", "TPMaleD0", "TP_E_MaleA0", "TP_E_MaleB0", "TP_E_MaleC0", "TP_E_MaleD0" };
        Material[] AUXmaterials;
        int materialcount;

        //ref head material
        AUXmaterials = GOhead.GetComponent<Renderer>().sharedMaterials;
        materialcount = GOhead.GetComponent<Renderer>().sharedMaterials.Length;
        for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
            for (int forAUX3 = 0; forAUX3 < allskins.Length; forAUX3++)
                for (int forAUX4 = 1; forAUX4 < MATSkins.Length + 1; forAUX4++)
                {
                    if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                    {
                        headskin = AUXmaterials[forAUX2];
                    }
                }        
        //legs
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++)
        {
            AUXmaterials = GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials;
            materialcount = GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials.Length;
            for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
                for (int forAUX3 = 0; forAUX3 < 4; forAUX3++)
                    for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                    {
                        if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                        {
                            AUXmaterials[forAUX2] = headskin;
                            GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                        }
                    }            
        }
        //chest
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++)
        {
            AUXmaterials = GOchest[forAUX].GetComponent<Renderer>().sharedMaterials;
            materialcount = GOchest[forAUX].GetComponent<Renderer>().sharedMaterials.Length;
            for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
                for (int forAUX3 = 0; forAUX3 < 4; forAUX3++)
                    for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                    {
                        if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                        {
                            AUXmaterials[forAUX2] = headskin;
                            GOchest[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                        }
                    }
        }
        haircoloractive = true;
    }
    void Deactivateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOjackets.Length; forAUX++) GOjackets[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOties.Length; forAUX++) GOties[forAUX].SetActive(false);
        GOglasses.SetActive(false);
        GObeard.SetActive(false);
        glassesactive = false;
        jacketactive = false;
        tieactivecolor = false;
        tieactive = false;
        tieactivecolor = false;
        hatactive = false;
    }
    void Activateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOjackets.Length; forAUX++) GOjackets[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOties.Length; forAUX++) GOties[forAUX].SetActive(true);
        GOglasses.SetActive(true);
        GObeard.SetActive(true);
    }
    public void Menu()
    {
        allOptions = !allOptions;
    }
    void Checkelder()
    {
        Material[] AUXmaterials;
        elder = false;
        haircoloractive = true;
        AUXmaterials = GOhead.GetComponent<Renderer>().sharedMaterials;
        int materialcount = GOhead.GetComponent<Renderer>().sharedMaterials.Length;
        for (int forAUX = 0; forAUX < materialcount; forAUX++)
        {
            if (AUXmaterials[forAUX].name == MATteeth[1].name)
            {
                elder = true;
                haircoloractive = false;
            }
        }
    }
    void Checkties()
    {
        if (chest < 2)
        {
            tieactive = true;
            if (tie != 3)
            {
                GOties[tie].SetActive(true);
                tieactivecolor = true;
            }
            else tieactivecolor = false;
        }
        else
        {
            if (tie != 3) GOties[tie].SetActive(false);
            tieactive = false;
            tieactivecolor = false;
        }
    }
    void Checkbeard()
    {
        if (GObeard.activeSelf)
        {
            beardactive = true;
            beard = new Vector4(1, 1, 1, 1);
            Material[] AUXmaterials;
            AUXmaterials = GObeard.GetComponent<Renderer>().sharedMaterials;
            if (AUXmaterials[0] == trans) beard.x = 0;
            if (AUXmaterials[1] == trans) beard.y = 0;
            if (AUXmaterials[2] == trans) beard.z = 0;
            if (AUXmaterials[3] == trans) beard.w = 0;
        }
        else beardactive = false;
    }
    
       
    //models
    public void Nexthat()
    {
        hatactive = true;
        if (hair < 7)
        {
            GOhair[hair].SetActive(false);
            hair = 7;
            GOhair[hair].SetActive(true);
        }
        else
        {
            GOhair[hair].SetActive(false);
            hair++;
            if (hair > GOhair.Length - 1) hair = 7;
            GOhair[hair].SetActive(true);
        }
    }
    public void Prevhat()
    {
        hatactive = true;
        if (hair < 7)
        {
            GOhair[hair].SetActive(false);
            hair = 9;
            GOhair[hair].SetActive(true);
        }
        else
        {
            GOhair[hair].SetActive(false);
            hair--;
            if (hair < 7) hair = 9;
            GOhair[hair].SetActive(true);
        }
    }
    public void Nexthair()
    {
        hatactive = false;
        
        GOhair[hair].SetActive(false);
        if (hair < GOhair.Length - 4) hair++;
        else hair = 0;
        GOhair[hair].SetActive(true);
    }
    public void GlassesOn()
    {
        glassesactive = !glassesactive;
        GOglasses.SetActive(glassesactive);
    }
    public void Nextchest()
    {
        GOchest[chest].SetActive(false);
        if (chest < GOchest.Length - 1) chest++;
        else chest = 0;
        GOchest[chest].SetActive(true);
        Checkties();
    }
    public void Nextlegs()
    {
        GOlegs[legs].SetActive(false);
        if (legs < GOlegs.Length - 1) legs++;
        else legs = 0;
        GOlegs[legs].SetActive(true);
    }
    public void Nextfeet()
    {
        GOfeet[feet].SetActive(false);
        if (feet < GOfeet.Length - 1) feet++;
        else feet = 0;
        GOfeet[feet].SetActive(true);
    }
    public void Nexttie()
    {
        if (tie != 3) GOties[tie].SetActive(false);
        if (tie < GOties.Length) tie++;
        else tie = 0;
        if (tie != 3) GOties[tie].SetActive(true);
        if (tie == 3) tieactivecolor = false;
        else tieactivecolor = true;
    }
    public void Nextjacket()
    {
        if (jacket == 2)
        {
            jacket = 0;
            GOjackets[jacket].SetActive(true);
            jacketactive = true;
        }
        else
        {
            if (jacket == 1)
            {
                GOjackets[jacket].SetActive(false);
                jacket = 2;
                jacketactive = false;
            }
            if (jacket == 0)
            {
                GOjackets[jacket].SetActive(false);
                jacket = 1;
                GOjackets[jacket].SetActive(true);
            }
        }
    }
    public void Prevhair()
    {
        hatactive = false;
        GOhair[hair].SetActive(false);
        if (hair > 0) hair--;
        else hair = 6;
        GOhair[hair].SetActive(true);
    }
    public void Prevchest()
    {
        GOchest[chest].SetActive(false);
        chest--;
        if (chest < 0) chest = GOchest.Length - 1;
        GOchest[chest].SetActive(true);
        Checkties();
    }
    public void Prevlegs()
    {
        GOlegs[legs].SetActive(false);
        if (legs > 0) legs--;
        else legs = GOlegs.Length - 1;
        GOlegs[legs].SetActive(true);
    }
    public void Prevfeet()
    {
        GOfeet[feet].SetActive(false);
        if (feet > 0) feet--;
        else feet = GOfeet.Length - 1;
        GOfeet[feet].SetActive(true);
    }
    public void Prevtie()
    {
        if (tie != 3) GOties[tie].SetActive(false);
        tie--;
        if (tie < 0) tie = 3;
        if (tie != 3) GOties[tie].SetActive(true);
        if (tie == 3) tieactivecolor = false;
        else tieactivecolor = true;
    }
    public void Prevjacket()
    {
        if (jacket == 0)
        {
            GOjackets[jacket].SetActive(false);
            jacket = 2;
            jacketactive = false;
        }
        else
        {
            if (jacket == 1)
            {
                GOjackets[jacket].SetActive(false);
                jacket = 0;
                GOjackets[jacket].SetActive(true);
            }
            if (jacket == 2)
            {
                jacket = 1;
                jacketactive = true;
                GOjackets[jacket].SetActive(true);
            }
        }
    }

    public void BeardOn()
    {
        beardactive = !beardactive;
        GObeard.SetActive(beardactive);
        if (beardactive)
        {
            beard = new Vector4(1, 1, 1, 1);
            Setbeard();
        }
    }
    public void Randombeard()
    {
        beard = new Vector4(1, 1, 1, 1);
        Setbeard();
        beard = new Vector4(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
        Setbeard();
    }
    public void Setbeard()
    {
        int MATindex=0;
        Material Op;
        Op = GOhair[0].GetComponent<Renderer>().sharedMaterial;
        while (Op.name != MAThairA[MATindex].name) MATindex ++;
        Material[] AUXmaterials;
        AUXmaterials = GObeard.GetComponent<Renderer>().sharedMaterials;
        if (beard.x == 0) AUXmaterials[0] = trans;
        else AUXmaterials[0] = MATBeard[MATindex] as Material;
        if (beard.y == 0) AUXmaterials[1] = trans;
        else AUXmaterials[1] = MATBeard[MATindex] as Material;
        if (beard.z == 0) AUXmaterials[2] = trans;
        else AUXmaterials[2] = MATBeard[MATindex] as Material;
        if (beard.w == 0) AUXmaterials[3] = trans;
        else AUXmaterials[3] = MATBeard[MATindex] as Material;
        GObeard.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
        //beardsimple
        AUXmaterials = GObeard.GetComponent<Renderer>().sharedMaterials;
        GObeardsimple.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
    }
    
    //materials    
    public void Nexthatcolor(int todo)
    {
        if (hatactive)
        {
            if (hair == 7) ChangeMaterials(MATHatA, todo);
            if (hair == 8) ChangeMaterials(MATHatB, todo);
            if (hair == 9) ChangeMaterials(MATHatC, todo);
        }        
    }
    public void Nextskincolor(int todo)
    {
        ChangeMaterials(MATSkins, todo);
        ChangeMaterials(MATElderSkins, todo);
    }
    public void Nexthaircolor(int todo)
    {
        if (!elder)
        {
            int intindex = 0;
            Material AUXmaterial;
            AUXmaterial = GOhair[0].GetComponent<Renderer>().sharedMaterial;
            while (AUXmaterial != MAThairA[intindex]) intindex++;
            if (intindex == 2 && todo == 0) todo = 3;
            if (intindex == 0 && todo == 1) todo = 4;

            ChangeMaterials(MAThairA, todo);
            ChangeMaterials(MAThairB, todo);
            ChangeMaterials(MAThairC, todo);
            ChangeMaterials(MAThairD, todo);
            ChangeMaterials(MAThairE, todo);
            ChangeMaterials(MAThairF, todo);
            ChangeMaterials(MAThairG, todo);            
            Setbeard();            
        }
    }
    public void Nextglasses(int todo)
    {
        ChangeMaterials(MATGlasses, todo);
    }
    public void Nexteyescolor(int todo)
    {
        ChangeMaterials(MATEyes, todo);        
    }
    public void Nextchestcolor(int todo)
    {
        if (chest < 2) ChangeMaterials(MATShirtA, todo);
        if (chest > 1 && chest < 4) ChangeMaterials(MATShirtB, todo);
        if (chest > 3) ChangeMaterials(MATTshirt, todo);        
    }
    public void Nextjacketcolor(int todo)
    {
        if (jacket == 0) ChangeMaterials(MATJacket, todo);
        if (jacket == 1) ChangeMaterials(MATSweater, todo);
    }
    public void Nextlegscolor(int todo)
    {
        ChangeMaterials(MATLegs, todo);
    }
    public void Nextfeetcolor(int todo)
    {
        if (feet == 0) ChangeMaterials(MATFeetA, todo); 
        if (feet == 1) ChangeMaterials(MATFeetB, todo);
    }
    public void Nexttiecolor(int todo)
    {
        if (tie == 0) ChangeMaterials(MATBowtie, todo); 
        if (tie > 0) ChangeMaterials(MATTie, todo);
    }
    

    public void ResetModel()
    {
        ElderOff();
        beard = new Vector4(1, 1, 1, 1);
        Activateall();
        ChangeMaterials(MATHatA, 3);
        ChangeMaterials(MATHatB, 3);
        ChangeMaterials(MATHatC, 3);
        ChangeMaterials(MATSkins, 3);
        ChangeMaterials(MAThairA, 3);
        ChangeMaterials(MAThairB, 3);
        ChangeMaterials(MAThairC, 3);
        ChangeMaterials(MAThairD, 3);
        ChangeMaterials(MAThairE, 3);
        ChangeMaterials(MAThairF, 3);
        ChangeMaterials(MAThairG, 3);        
        Setbeard();
        ChangeMaterials(MATGlasses, 3);
        ChangeMaterials(MATEyes, 3);
        ChangeMaterials(MATShirtA, 3);
        ChangeMaterials(MATShirtB, 3);
        ChangeMaterials(MATTshirt, 3);
        ChangeMaterials(MATJacket, 3);
        ChangeMaterials(MATSweater, 3);
        ChangeMaterials(MATLegs, 3);
        ChangeMaterials(MATFeetA, 3);
        ChangeMaterials(MATFeetB, 3);
        ChangeMaterials(MATBowtie, 3);
        ChangeMaterials(MATTie, 3);
        ChangeMaterials(MATteeth, 3);
        Menu();
    }
    public void Randomize()
    {
        Deactivateall();
        ResetSkin();
        hair = Random.Range(0, 15);
        if (hair > 9) hair = Random.Range(0, 5);
        GOhair[hair].SetActive(true);
        if (hair > 5) hatactive = true;
        chest = Random.Range(0, GOchest.Length); GOchest[chest].SetActive(true);
        tie = Random.Range(0, 4);
        Checkties();
        legs = Random.Range(0, 2); GOlegs[legs].SetActive(true);
        feet = Random.Range(0, 2); GOfeet[feet].SetActive(true);        
        jacket = Random.Range(0, 3);
        if (jacket < 2)
        {
            jacketactive = true;
            GOjackets[jacket].SetActive(true);           
        }
        else jacketactive = false;
        if (Random.Range(0, 6) < 4)  BeardOn(); 
        if (Random.Range(0, 5) < 3 & beardactive) Randombeard();
        if (Random.Range(0, 4) > 2)
        {
            glassesactive = true;
            GOglasses.SetActive(true);
            ChangeMaterial(GOglasses, MATGlasses, 2);
        }
        else glassesactive = false;

        //materials
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 4)); forAUX2++) Nextskincolor(0);        
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 8)); forAUX2++) Nexthaircolor(0);
        if (tieactivecolor)
        {
            for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 10)); forAUX2++) Nexttiecolor(0);       
        }
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 26)); forAUX2++) Nextjacketcolor(0);        
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 34)); forAUX2++) Nextchestcolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 32)); forAUX2++) Nextlegscolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 26)); forAUX2++) Nextfeetcolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 24)); forAUX2++) Nexthatcolor(0);
        ChangeMaterial(GOhead, MATEyes, 2);
    }
    public void CreateCopy()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 30; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GObeard.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(0).gameObject);
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TP/TP Pelvis/TP Spine/TP Spine1/TP Spine2/TP Neck/TP Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<MaleTPPrefabMaker>());
    }
    public void FIX()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 30; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GObeard.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(0).gameObject);
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TP/TP Pelvis/TP Spine/TP Spine1/TP Spine2/TP Neck/TP Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<MaleTPPrefabMaker>());
        DestroyImmediate(gameObject);
    }

    public void ElderOn()
    {
        elder = true;
        haircoloractive = false;
        //blendshapes
        SkinnedMeshRenderer rendhead;
        rendhead = GOhead.GetComponent<SkinnedMeshRenderer>();
        rendhead.SetBlendShapeWeight(26, 100);
        SkinnedMeshRenderer rendbeard;
        rendbeard = GObeard.GetComponent<SkinnedMeshRenderer>();
        rendbeard.SetBlendShapeWeight(9, 100);

        //skin        
        SwitchMaterial(GOhead, MATSkins, MATElderSkins);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) SwitchMaterial(GOchest[forAUX], MATSkins, MATElderSkins);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++)  SwitchMaterial(GOlegs[forAUX], MATSkins, MATElderSkins);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) SwitchMaterial(GOfeet[forAUX], MATSkins, MATElderSkins);

        
        //teeth
        ChangeMaterials(MATteeth, 1);

        //hair & beard        
        ChangeMaterials(MAThairA, 5);
        ChangeMaterials(MAThairB, 5);
        ChangeMaterials(MAThairC, 5);
        ChangeMaterials(MAThairD, 5);
        ChangeMaterials(MAThairE, 5);
        ChangeMaterials(MAThairF, 5);
        ChangeMaterials(MAThairG, 5);
        Setbeard();
        
    }
    public void ElderOff()
     
        {
        elder = false;
        haircoloractive = true;
        //blendshapes
        SkinnedMeshRenderer rendhead;
        rendhead = GOhead.GetComponent<SkinnedMeshRenderer>();
        rendhead.SetBlendShapeWeight(26, 0);
        SkinnedMeshRenderer rendbeard;
        rendbeard = GObeard.GetComponent<SkinnedMeshRenderer>();
        rendbeard.SetBlendShapeWeight(9, 0);

        //skin
        SwitchMaterial(GOhead, MATElderSkins, MATSkins);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) SwitchMaterial(GOchest[forAUX], MATElderSkins, MATSkins);
        for (int forAUX = 0; forAUX < GOlegs.Length;  forAUX++) SwitchMaterial(GOlegs [forAUX], MATElderSkins, MATSkins);
        for (int forAUX = 0; forAUX < GOfeet.Length;  forAUX++) SwitchMaterial(GOfeet [forAUX], MATElderSkins, MATSkins);

        //teeth
        ChangeMaterials(MATteeth, 1);

        //hair & beard
        ChangeMaterials(MAThairA, 3);
        ChangeMaterials(MAThairB, 3);
        ChangeMaterials(MAThairC, 3);
        ChangeMaterials(MAThairD, 3);
        ChangeMaterials(MAThairE, 3);
        ChangeMaterials(MAThairF, 3);
        ChangeMaterials(MAThairG, 3);
        Setbeard();
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
        ChangeMaterial(GObeard, MAT, todo);
        ChangeMaterial(GOjackets[0], MAT, todo);
        ChangeMaterial(GOjackets[1], MAT, todo);
        for (int forAUX = 0; forAUX < GOties.Length; forAUX++) ChangeMaterial(GOties[forAUX], MAT, todo);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) ChangeMaterial(GOchest[forAUX], MAT, todo);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) ChangeMaterial(GOlegs[forAUX], MAT, todo);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) ChangeMaterial(GOfeet[forAUX], MAT, todo);

    }
    void SwitchMaterial(GameObject GO, Object[] MAT1, Object[] MAT2)
    {        
        Material[] AUXmaterials;
        AUXmaterials = GO.GetComponent<Renderer>().sharedMaterials;
        int materialcount = GO.GetComponent<Renderer>().sharedMaterials.Length;

        for (int forAUX = 0; forAUX < materialcount; forAUX++)
            for (int forAUX2 = 0; forAUX2 < MAT1.Length; forAUX2++)
            {
                if (AUXmaterials[forAUX].name == MAT1[forAUX2].name)
                {
                    AUXmaterials[forAUX] = MAT2[forAUX2] as Material;
                    GO.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                }
            }        
    }
}