using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDialog : MonoBehaviour {

    private string text;
    private bool displayed = true;

    [SerializeField]
    private Text Dialog;

    [SerializeField]
    public string Text {
        get { return text; }
        set {
            text = value;
            UpdateDialogBox();
        }
    }

    [SerializeField]
    public bool Displayed
    {
        get { return displayed; }
        set {
            displayed = value;
            ShowDialog(displayed);
        }
    }

    public void UpdateDialogBox()
    {
        Dialog.text = text;
    }

    private void ShowDialog(bool show)
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(show);
        }
    }

}
