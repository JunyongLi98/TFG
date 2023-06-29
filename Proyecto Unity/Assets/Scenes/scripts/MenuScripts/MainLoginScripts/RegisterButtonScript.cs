using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterButtonScript : MonoBehaviour
{
    [SerializeField]
    private GameObject register;

    public void abrirregistrarse()
    {
        register.SetActive(true); 
    }
}
