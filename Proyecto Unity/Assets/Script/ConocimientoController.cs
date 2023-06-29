using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class ConocimientoController : MonoBehaviour
{
    //Datos
    private GameObject panel;
    private Text titulo;
    private Image img;
    private Text text;
    private Text info;
    private int numero;
    private GameObject buttomSiguiente;
    private GameObject buttomAnterior;
    private int maxNumero = 4;
    private bool estado;

    // Start is called before the first frame update
    void Start()
    {
        panel = gameObject.transform.Find("Panel").gameObject;
        titulo = gameObject.transform.Find("Panel/Titulo").GetComponent<Text>();
        img = gameObject.transform.Find("Panel/Img").GetComponent<Image>();
        text = gameObject.transform.Find("Panel/Img/Text").GetComponent<Text>();
        info = gameObject.transform.Find("Panel/Info").GetComponent<Text>();
        buttomSiguiente = gameObject.transform.Find("Panel/Siguiente").gameObject;
        buttomAnterior = gameObject.transform.Find("Panel/Anterior").gameObject;
        numero = 0;

        initPanel();
        estado = false;
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(numero == 0)
        {
            buttomAnterior.SetActive(false);
        }
        else
        {
            buttomAnterior.SetActive(true);
        }

        if(numero == (maxNumero - 1))
        {
            buttomSiguiente.SetActive(false);
        }
        else
        {
            buttomSiguiente.SetActive(true);
        }
    }

    /***********************************************************************************************************************
     * Inicial panel
     ***********************************************************************************************************************/

    private void initPanel()
    {
        string fileName = "ConocimientoDato/" + numero.ToString();
        ConocimientoScriptable conocimiento = Resources.Load<ConocimientoScriptable>(fileName);
        titulo.text = conocimiento.titulo;
        img.sprite = conocimiento.img;
        text.text = conocimiento.texto;
        info.text = conocimiento.info;
    }

    /***********************************************************************************************************************
     * Bot¨®n evento
     ***********************************************************************************************************************/

    public void cerrar()
    {
        estado = false;
        panel.SetActive(false);
    }

    public void abrir()
    {
        if (!estado)
        {
            estado = true;
            panel.SetActive(true);
            numero = 0;
            initPanel();
        }
    }

    public void siguiente()
    {
        if(numero < (maxNumero - 1))
        {
            numero++;
            string fileName = "ConocimientoDato/" + numero.ToString();
            ConocimientoScriptable conocimiento = Resources.Load<ConocimientoScriptable>(fileName);
            titulo.text = conocimiento.titulo;
            img.sprite = conocimiento.img;
            text.text = conocimiento.texto;
            info.text = conocimiento.info;
        }
    }

    public void anterior()
    {
        if (numero > 0)
        {
            numero--;
            string fileName = "ConocimientoDato/" + numero.ToString();
            ConocimientoScriptable conocimiento = Resources.Load<ConocimientoScriptable>(fileName);
            titulo.text = conocimiento.titulo;
            img.sprite = conocimiento.img;
            text.text = conocimiento.texto;
            info.text = conocimiento.info;
        }
    }
}
