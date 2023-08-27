#region Source Code


using UnityEngine;
using AC;

public class JournalSnapPage : MonoBehaviour
{

    public string journalMenuName = "Journal";
    public string journalElementName = "PageLeft";
    public string shiftRightButtonName = "ShiftRight";
    public string shiftLeftButtonName = "ShiftLeft";
   
    private void OnEnable()
    {
        EventManager.OnMenuElementShift += OnMenuElementShift;
        EventManager.OnMenuTurnOn += OnMenuTurnOn;
    }

    private void OnDisable()
    {
        EventManager.OnMenuElementShift -= OnMenuElementShift;
        EventManager.OnMenuTurnOn -= OnMenuTurnOn;
    }

    private void OnMenuElementShift(MenuElement _element, AC_ShiftInventory shiftType)
    {
        if (PlayerMenus.GetMenuWithName(journalMenuName).elements.Contains(_element))
        {
            MenuJournal journal = PlayerMenus.GetElementWithName(journalMenuName, journalElementName) as MenuJournal;

            bool isEven = (journal.showPage % 2 == 0);

            if (isEven)
            {
                int newShowPage = journal.showPage + ((shiftType == AC_ShiftInventory.ShiftNext) ? 1 : -1);

                if (newShowPage < 1)
                {
                    newShowPage = 1;
                }

                if (newShowPage >= journal.pages.Count - 1)
                {
                    newShowPage = journal.pages.Count - 1;
                }

                journal.showPage = newShowPage;
            }

            UpdateShiftRightButton();
            UpdateShiftLeftButton();
        }
    }


    private void OnMenuTurnOn(Menu _menu, bool isInstant)
    {
        if (_menu.title == journalMenuName)
        {
            UpdateShiftRightButton();
            UpdateShiftLeftButton();

        }
    }


    private void UpdateShiftRightButton()
    {
        MenuJournal journal = PlayerMenus.GetElementWithName(journalMenuName, journalElementName) as MenuJournal;
        AC.MenuButton shiftRightButton = PlayerMenus.GetElementWithName(journalMenuName, shiftRightButtonName) as AC.MenuButton;
        int pageDiff = journal.pages.Count - journal.showPage;
        shiftRightButton.IsVisible = (pageDiff > 1);
    }

    private void UpdateShiftLeftButton()
    {
        MenuJournal journal = PlayerMenus.GetElementWithName(journalMenuName, journalElementName) as MenuJournal;
        AC.MenuButton shiftLeftButton = PlayerMenus.GetElementWithName(journalMenuName, shiftLeftButtonName) as AC.MenuButton;
        int pageDiff = journal.showPage;
        shiftLeftButton.IsVisible = (pageDiff != 1);
    }

}


#endregion
/*
#region New Code
using UnityEngine;
using System.Collections;
using AC;

public class JournalSnapPage : MonoBehaviour
{

    public string journalMenuName = "Journal";


    private void OnEnable()
    {
        EventManager.OnMenuElementShift += OnMenuElementShift;
    }


    private void OnDisable()
    {
        EventManager.OnMenuElementShift -= OnMenuElementShift;
    }


    private void OnMenuElementShift(MenuElement _element, AC_ShiftInventory shiftType)
    {
        if (PlayerMenus.GetMenuWithName(journalMenuName).elements.Contains(_element))
        {
            MenuJournal journal = _element as MenuJournal;
            bool isEven = (journal.showPage % 2 == 0);

            if (isEven)
            {
                int newShowPage = journal.showPage + ((shiftType == AC_ShiftInventory.ShiftNext) ? 1 : -1);

                if (newShowPage < 1)
                {
                    newShowPage = 1;
                }
                if (newShowPage >= journal.pages.Count - 1)
                {
                    newShowPage = journal.pages.Count - 1;
                }

                journal.showPage = newShowPage;
            }
        }
    }

}
#endregion
*/