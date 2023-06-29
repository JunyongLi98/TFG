using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "Item")]
public class ItemScriptable : ScriptableObject
{
    public int id;
    public new string name;
    public int tipo;
    public int rareza;
    public string descripcion;
    public int precio;
    public Sprite sprite;
    public int variable;
    public int cantidadRecuperada;
    public int durabilidad;
    public bool especial = false;
}
