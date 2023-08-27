using UnityEngine;
using System.Collections;
using AC;

public class JournalPageTexture : MonoBehaviour
{

    [SerializeField] private PageTexture[] pageTextures;
    [SerializeField] private string journalMenuName = "Journal";
    [SerializeField] private string journalElementName = "PageText";
    [SerializeField] private string leftGraphicElementName = "Left Texture";
    [SerializeField] private string rightGraphicElementName = "Right Texture";
    [SerializeField] private Texture2D emptyTexture;

    private void OnEnable()
    {
        EventManager.OnMenuTurnOn += OnMenuTurnOn;
        EventManager.OnMenuElementShift += OnMenuElementShift;
    }

    private void OnDisable()
    {
        EventManager.OnMenuTurnOn -= OnMenuTurnOn;
        EventManager.OnMenuElementShift -= OnMenuElementShift;
    }

    private void OnMenuTurnOn(Menu menu, bool isInstant)
    {
        if (menu.title == journalMenuName)
        {
            UpdateLeftPageTexture();
            UpdateRigthPageTexture();
        }
    }

    private void OnMenuElementShift(MenuElement _element, AC_ShiftInventory shiftType)
    {
        if (PlayerMenus.GetMenuWithName(journalMenuName).elements.Contains(_element))
        {
            UpdateLeftPageTexture();
            UpdateRigthPageTexture();
        }
    }

    private void UpdateRigthPageTexture()
    {
        MenuJournal journal = PlayerMenus.GetElementWithName(journalMenuName, journalElementName) as MenuJournal;
        int pageIndex = journal.showPage - 1;

        pageIndex++;

        MenuGraphic graphic = PlayerMenus.GetElementWithName(journalMenuName, rightGraphicElementName) as MenuGraphic;

        Texture2D texture = emptyTexture;
        if (pageIndex < journal.pages.Count)
        {
            int pageID = journal.pages[pageIndex].lineID;
            texture = GetTextureWithID(pageID);
        }

        if (texture)
        {
            graphic.graphic.texture = texture;
            graphic.graphic.ClearCache();
        }
    }

    private void UpdateLeftPageTexture()
    {
        MenuJournal journal = PlayerMenus.GetElementWithName(journalMenuName, journalElementName) as MenuJournal;
        int pageIndex = journal.showPage - 1;

        MenuGraphic graphic = PlayerMenus.GetElementWithName(journalMenuName, leftGraphicElementName) as MenuGraphic;

        Texture2D texture = emptyTexture;
        if (pageIndex < journal.pages.Count)
        {
            int pageID = journal.pages[pageIndex].lineID;
            texture = GetTextureWithID(pageID);
        }

        if (texture)
        {
            graphic.graphic.texture = texture;
            graphic.graphic.ClearCache();
        }       

    }

    private Texture2D GetTextureWithID(int ID)
    {
        foreach (PageTexture pageTexture in pageTextures)
        {
            if (pageTexture.ID == ID)
            {
                return pageTexture.texture;
            }
        }
        return null;
    }

    [System.Serializable]
    private struct PageTexture
    {

        public int ID;
        public Texture2D texture;

    }

}