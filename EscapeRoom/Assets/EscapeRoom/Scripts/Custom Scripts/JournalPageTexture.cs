using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AC;

public class JournalPageTexture : MonoBehaviour
{

    [Header("---- AC Menu Settings ----")]
    [SerializeField] private string journalMenuName = "Journal";
    [SerializeField] private string journalElementName = "PageText";
    [Header("---- Pages Texture Setting ----")]
    [SerializeField] private PageTexture[] pageTextures;
    [SerializeField] private string leftGraphicElementName = "Left Texture";
    [SerializeField] private string rightGraphicElementName = "Right Texture";
    [SerializeField] private Texture2D emptyTexture;
    [Header("---- Footer Settings ----")]
    [SerializeField] private string leftPageNumberName = "LeftPageNumber";
    [SerializeField] private string rightPageNumberName = "RightPageNumber";
    [SerializeField] private bool showPageNumber;
    [SerializeField] private bool showPageCount;
    [SerializeField] private bool addSummaryPage;
    [SerializeField] private string summaryText = "Summary";
    [Header("---- Font Override ----")]
    [SerializeField] private bool overrideFont;
    [SerializeField] private bool onlySummary;
    [Header("---- Footer Section ----")]
    [SerializeField] private Font footerFont;
  //  [SerializeField] private int footerFontSize = 14;
  //  [SerializeField] private FontStyle footerFontStyle;
  //  [SerializeField] private TextAlignment footerAlignment;
    [SerializeField] private Color footerColor = Color.black;
    [Header("---- Summary Section ----")]
    [SerializeField] private Font summaryFont;
  //  [SerializeField] private int summaryFontSize = 14;
  //  [SerializeField] private FontStyle summaryFontStyle;
  //  [SerializeField] private TextAlignment summaryAlignment;
    [SerializeField] private Color summaryColor = Color.black;



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
        MenuLabel label = PlayerMenus.GetElementWithName(journalMenuName, rightPageNumberName) as MenuLabel;
        label.font = (overrideFont && !onlySummary) ? footerFont : label.font;
        label.fontColor = (overrideFont && !onlySummary) ? footerColor : label.fontColor;
    

        Texture2D texture = emptyTexture;
        if (pageIndex < journal.pages.Count)
        {
            int pageID = journal.pages[pageIndex].lineID;
            int pageNumber = addSummaryPage ? journal.showPage: journal.showPage + 1;
            int pageCount = addSummaryPage ? journal.pages.Count - 1 : journal.pages.Count;
            texture = GetTextureWithID(pageID);
            label.label = showPageNumber ? $"{pageNumber}" : $"";
            label.label += showPageCount ? $" - {pageCount}" : $"";
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
        MenuLabel label = PlayerMenus.GetElementWithName(journalMenuName, leftPageNumberName) as MenuLabel;
        label.font = (overrideFont && !onlySummary) ? footerFont : label.font;
        label.fontColor = (overrideFont && !onlySummary) ? footerColor : label.fontColor;

        Texture2D texture = emptyTexture;
        if (pageIndex < journal.pages.Count)
        {
            int pageID = journal.pages[pageIndex].lineID;
            int pageNumber = addSummaryPage ? journal.showPage - 1 : journal.showPage;
            int pageCount = addSummaryPage ? journal.pages.Count - 1 : journal.pages.Count;

            texture = GetTextureWithID(pageID);
            
            label.label = showPageNumber ? $"{pageNumber}" : $"";
            label.label += showPageCount ? $" - {pageCount}" : $"";
            label.label = (addSummaryPage && pageNumber == 0) ? summaryText: label.label;
            label.font = (addSummaryPage && pageNumber == 0 && overrideFont) ? summaryFont : label.font;
            label.fontColor = (addSummaryPage && pageNumber == 0 && overrideFont) ? summaryColor : label.fontColor;

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