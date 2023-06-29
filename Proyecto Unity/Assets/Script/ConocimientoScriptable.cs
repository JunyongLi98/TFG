using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Conocimiento", menuName = "Conocimiento")]
public class ConocimientoScriptable : ScriptableObject
{
    public string titulo;
    public Sprite img;
    public string texto;
    public string info;
}
