using UnityEngine;
using AC;

public class JournalSnapPage : MonoBehaviour
{

    public string journalMenuName = "Journal";
    public string journalElementName = "PageLeft";
    public string shiftRightButtonName = "ShiftRight";

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
        }
    }


    private void OnMenuTurnOn(Menu _menu, bool isInstant)
    {
        if (_menu.title == journalMenuName)
        {
            UpdateShiftRightButton();
        }
    }


    private void UpdateShiftRightButton()
    {
        MenuJournal journal = PlayerMenus.GetElementWithName(journalMenuName, journalElementName) as MenuJournal;
        MenuButton shiftRightButton = PlayerMenus.GetElementWithName(journalMenuName, shiftRightButtonName) as MenuButton;
        int pageDiff = journal.pages.Count - journal.showPage;
        shiftRightButton.IsVisible = (pageDiff > 1);
    }

}