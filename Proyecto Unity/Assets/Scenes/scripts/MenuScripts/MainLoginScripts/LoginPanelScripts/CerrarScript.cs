using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CerrarScript : MonoBehaviour
{
    public GameObject panel;
    public void cerrarPanel()
    {
        panel.SetActive(false);
    }
}
