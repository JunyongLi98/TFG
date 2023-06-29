using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main instance;
    public LlamadasServidor Llamadas;
    /*public JugadorHelper jhelper;
    public EquipoHelper eqhelper;*/
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Llamadas = GetComponent<LlamadasServidor>();
        /*jhelper = GetComponent<JugadorHelper>();
        eqhelper = GetComponent<EquipoHelper>();*/

        DontDestroyOnLoad(gameObject);
        
    }

}
