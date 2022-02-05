using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModalWindowView : MonoBehaviour
{
    public GameObject root;

    public GameObject exitWarning;
    public TextMeshProUGUI exitWarningText;
    public Button exitCancel;
    public Button exitAccept;

    public GameObject simpleWarning;
    public TextMeshProUGUI text;
    public Button accept;

    public void ShowExitWarning(bool val)
    {
        root.SetActive(val);
        exitWarning.SetActive(val);
    }
}
