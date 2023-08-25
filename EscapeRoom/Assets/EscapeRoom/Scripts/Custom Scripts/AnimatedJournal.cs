using UnityEngine;
using AC;

public class AnimatedJournal : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private string pageParameter = "Page";
    [SerializeField] private string journalMenuName = "Journal";
    [SerializeField] private string journalElementName = "PageText";

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
            UpdatePageTexture();
        }
    }

    private void OnMenuElementShift(MenuElement _element, AC_ShiftInventory shiftType)
    {
        if (_element.title == journalElementName)
        {
            UpdatePageTexture();
        }
    }

    private void UpdatePageTexture()
    {
        MenuJournal journal = PlayerMenus.GetElementWithName(journalMenuName, journalElementName) as MenuJournal;
        int pageIndex = journal.showPage - 1;
        _animator.SetInteger(pageParameter, pageIndex);
    }

}