using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TipoRespuestaLogin
{
    public const string resp1 = "Login Success";
    public const string resp2 = "No existe el usuario";
    public const string resp3 = "Las credenciales son incorrectas";
    public const string resp4 = "*El correo electrónico es obligatorio rellenarlo";
    public const string resp5 = "*La contraseña no puede ser vacía";
}

public class LoginHelper : MonoBehaviour
{
    [SerializeField]
    private InputField Correo;
    [SerializeField]
    private InputField Contrasena;
    [SerializeField]
    private GameObject respuesta;

    

    private void Start()
    {
        respuesta.SetActive(false);
    }
    public void loguearse()
    {
        string email = Correo.text;
        string password = Contrasena.text;
        
        if(email!="" && password!="")
        {
            Main.instance.Llamadas.Loguear(email, password,gameObject);
        }
        else if (email == "")
        {
            respuesta.SetActive(true);
            respuesta.GetComponent<UnityEngine.UI.Text>().text = TipoRespuestaLogin.resp4;
        }
        else
        {
            respuesta.SetActive(true);
            respuesta.GetComponent<UnityEngine.UI.Text>().text = TipoRespuestaLogin.resp5;
        }

    }

    public void darrespuesta()
    {
        if (LoginManagerScript.Instance.Respuesta == TipoRespuestaLogin.resp1)
        {
            Recuperardatos();
        }
        else if (LoginManagerScript.Instance.Respuesta == TipoRespuestaLogin.resp2)
        {
            respuesta.SetActive(true);
            respuesta.GetComponent<UnityEngine.UI.Text>().text = "*"+ TipoRespuestaLogin.resp2;
        }
        else
        {
            respuesta.SetActive(true);
            respuesta.GetComponent<UnityEngine.UI.Text>().text = "*"+ TipoRespuestaLogin.resp3;
        }
    }

    public void Entrar()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Recuperardatos()
    {
        string email = Correo.text;
        string password = Contrasena.text;
        Main.instance.Llamadas.getUser(email, password,gameObject);
    }

}
