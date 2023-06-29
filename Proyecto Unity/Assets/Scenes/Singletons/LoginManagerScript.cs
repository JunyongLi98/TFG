using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManagerScript : MonoBehaviour
{
    public static LoginManagerScript Instance { get; private set; }

    public User user;

    public string Respuesta;

    private void Awake()
    {
        Instance = this;
        user = new User();

    }
}
