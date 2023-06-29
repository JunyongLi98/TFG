using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TipoRespuestaRegister
{
    public const string resp1 = "Registro Success";
    public const string resp2 = "Este usuario ya existe";
    public const string resp3 = "El correo electrónico no es válido";
    public const string resp4 = "*Rellene los campos obligatorios";

}

public class RegistrarHelper : MonoBehaviour
{
    [SerializeField]
    private InputField Correo;
    [SerializeField]
    private InputField Contrasena;
    [SerializeField]
    private InputField Nombre;
    [SerializeField]
    private InputField Nickname;
    [SerializeField]
    private GameObject respuesta;

    private void Start()
    {
        respuesta.SetActive(false);
    }
    public void registrarse()
    {
        string email = Correo.text;
        string password = Contrasena.text;
        string nombre = Nombre.text;
        string nick = Nickname.text;

        if (email != "" && password != "" && nombre != "" && nick!="")
        {
            if (ComprobarEmail(email))
            {
                Main.instance.Llamadas.Registrar(email, password,nombre,nick, gameObject);
            }
            else
            {
                respuesta.SetActive(true);
                respuesta.GetComponent<UnityEngine.UI.Text>().text = TipoRespuestaRegister.resp3;
            }

        }
        else if (email == "" || password=="" || nombre=="" || nick=="")
        {
            respuesta.SetActive(true);
            respuesta.GetComponent<UnityEngine.UI.Text>().text = TipoRespuestaRegister.resp4;
        }

    }

    public void darrespuesta()
    {
        if (LoginManagerScript.Instance.Respuesta == TipoRespuestaRegister.resp1)
        {
            respuesta.SetActive(true);
            respuesta.GetComponent<UnityEngine.UI.Text>().text = "Usuario creado con éxito";
        }
        else if (LoginManagerScript.Instance.Respuesta == TipoRespuestaRegister.resp2)
        {
            respuesta.SetActive(true);
            respuesta.GetComponent<UnityEngine.UI.Text>().text = "*" + TipoRespuestaRegister.resp2;
        }
    }

    public bool ComprobarEmail(string email)
    {
        bool result = false;

        if(email.Contains("@") && (email.Contains(".com") || email.Contains(".es"))){
            result = true;
        }


        return result;
    }
}
