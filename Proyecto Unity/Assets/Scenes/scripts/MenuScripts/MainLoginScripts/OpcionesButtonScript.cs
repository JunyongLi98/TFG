using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcionesButtonScript : MonoBehaviour
{
    [SerializeField]
    private GameObject opcion;


    public void abrirOpciones()
    {
        opcion.SetActive(true);
    }
}
