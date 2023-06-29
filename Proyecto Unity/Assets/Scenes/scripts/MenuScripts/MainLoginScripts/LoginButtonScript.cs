using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginButtonScript : MonoBehaviour
{
    public GameObject panel1;

    public void abrirPanelLogin()
    {
        panel1.SetActive(true);
    }
}
