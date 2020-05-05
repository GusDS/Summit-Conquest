using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmDialog : MonoBehaviour
{
    public bool isConfirmDialogOn = false;
    public GameObject dialogPanel;
    public Text dialogText;
    public Button yesButton;
    public Button noButton;
    public bool isYesSelected = false;
    public bool isNoSelected = false;

    void Start()
    {
        yesButton.onClick.AddListener(ConfirmationYesSelected);
        noButton.onClick.AddListener(ConfirmationNoSelected);
    }

    void Update()
    {
        if (dialogPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Y)) ConfirmationYesSelected();
            if (Input.GetKeyDown(KeyCode.N)) ConfirmationNoSelected();
        }
    }

    public void Close()
    {
        isConfirmDialogOn = false;
        isYesSelected = false;
        isNoSelected = false;
    }

    public void ConfirmationDialog(string textHeader)
    {
        StartCoroutine(ConfirmationDialogIE(textHeader));
    }

    private IEnumerator ConfirmationDialogIE(string textHeader)
    {
        isConfirmDialogOn = true;
        isYesSelected = false;
        isNoSelected = false;
        dialogText.text = textHeader;
        dialogPanel.SetActive(true);

        while (!isYesSelected && !isNoSelected && isConfirmDialogOn) /* isConfirmDialogOn: dialog can be cancelled from outside */
        {
            yield return null;
        }
        dialogPanel.SetActive(false);
        isConfirmDialogOn = false;
    }
    void ConfirmationYesSelected() { isYesSelected = true; }
    void ConfirmationNoSelected() { isNoSelected = true; }
}
