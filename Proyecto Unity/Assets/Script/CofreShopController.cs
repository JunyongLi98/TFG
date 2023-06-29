using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class CofreShopController : MonoBehaviour
{
    //Cofre cobre
    private int[] productoCobre = { 40, 70, 94, 99, 100 };
    private int[] menaCobre = { 60, 90, 100, -1, -1 };
    private int[] consumibleCobre = { 44, 77, 92, 97, 100 };
    private int[] equipamientoCobre = { 55, 85, 94, 98, 100 };

    //Cofre plata
    private int[] productoPlata = { 35, 60, 90, 97, 100 };
    private int[] menaPlata = { 45, 85, 100, -1, -1 };
    private int[] consumiblePlata = { 40, 70, 85, 95, 100 };
    private int[] equipamientoPlata = { 40, 80, 90, 97, 100 };

    //Cofre oro
    private int[] productoOro = { 25, 45, 81, 93, 100 };
    private int[] menaOro = { 40, 75, 100, -1, -1 };
    private int[] consumibleOro = { 25, 55, 75, 90, 100 };
    private int[] equipamientoOro = { 33, 58, 78, 93, 100 };

    //Objetos
    private int[] especialObjeto = { 14200, 14201, 14300, 14400 };
    private int[] equipamientoObjeto = { 12402, 12403, 12404, 12405, 12406, 12407, 12408, 12409, 12410, 12411, 12412, 13401, 13402, 13403, 13404, 13405, 13406, 13407, 13408 };
    private int[] numComidas = { 3, 2, 1, 1, 1 };
    private int[] numArmaduras = { 2, 2, 4, 2, 2 };
    private int[] numArmas = { 2, 3, 4, 3, 1 };

    //Datos
    private GameObject cofreShop;
    private bool open = false;
    private int caso = 0;
    private string[] cofresName = { "el cofre de cobre", "el cofre de plata", "el cofre de oro" };
    public List<int> recompensasId;
    private PanelController panel;
    private InventoryController inventario;
    private GameObject inventarioContent;
    private GameObject recompensa;
    private GameObject recompensaContent;
    private GameObject prefOrigen;
    private ShopController tienda;

    // Start is called before the first frame update
    void Start()
    {
        cofreShop = gameObject.transform.Find("Cofre").gameObject;
        recompensasId = new List<int>();
        inventario = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        inventarioContent = inventario.transform.Find("Inventario/Bag/Viewport/InventoryContent").gameObject;
        panel = GameObject.Find("PanelController").GetComponent<PanelController>();
        recompensa = gameObject.transform.Find("Recompensa").gameObject;
        recompensaContent = gameObject.transform.Find("Recompensa/Recompensas/Viewport/RecompensasContent").gameObject;
        prefOrigen = Resources.Load<GameObject>("Prefab/PrefOrigen/Item");
        tienda = GameObject.Find("ShopController").GetComponent<ShopController>();

        recompensa.SetActive(false);
        cofreShop.SetActive(open);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /***********************************************************************************************************************
     * Get y set
     ***********************************************************************************************************************/

    public int getCaso()
    {
        return this.caso;
    }

    /***********************************************************************************************************************
     * Shop control
     ***********************************************************************************************************************/

    public void openShop()
    {
        open = !open;
        cofreShop.SetActive(open);
        tienda.cerrarTienda();
    }

    public void cerrarRecompensa()
    {
        restaurar();
        recompensa.SetActive(false);
    }

    public void cerrarTienda()
    {
        this.open = false;
        cofreShop.SetActive(false);
    }

    /***********************************************************************************************************************
     * Abrir cofre
     ***********************************************************************************************************************/

    public void abrirCofre(int caso)
    {
        this.caso = caso;
        int id = 0;
        if (this.caso == 0)
        {
            id = 14201;
        }
        else if (this.caso == 1)
        {
            id = 14300;
        }
        else
        {
            id = 14400;
        }

        string texto1 = "Quieres abrir " + cofresName[caso] + "?";
        string texto2 = "No tienes llaves suficientes para abrir el cofre";
        
        panel.setCaso("abrir cofre");
        panel.setPanelActive(true);
        panel.setMensajePrincipal(texto1);
        panel.setMensajeInsuficiente(texto2);
        panel.setPanelOpcionesActive(false);
        if (inventarioContent.transform.Find(id.ToString()) != null)
        {
            panel.setPanelInsuficienteActive(false);
            panel.setObjeto(inventarioContent.transform.Find(id.ToString()).gameObject);
            panel.setCantidadMax();
        }
        else
        {
            panel.setPanelPrincipalActive(false);
        }
    }

    public void aceptarAbrir(int num)
    {
        switch (this.caso)
        {
            case 0:
                for(int i = 0; i < num; i++)
                {
                    abrirCofre(productoCobre, menaCobre, consumibleCobre, equipamientoCobre); 
                }
                break;
            case 1:
                for( int i = 0; i < num; i++)
                {
                    abrirCofre(productoPlata, menaPlata, consumiblePlata, equipamientoPlata);
                }
                break;
            case 2:
                for (int i = 0; i < num; i++)
                {
                    abrirCofre(productoOro, menaOro, consumibleOro, equipamientoOro);
                }
                break;
        }
        recompensa.SetActive(true);
        generarProducto();
        transferirProducto();
    }

    private void abrirCofre(int[] productoP, int[] menaP, int[] consumibleP, int[] equipamientoP)
    {
        int x = Random.Range(0, 101);
        int tipo = 0;
        int rareza = 0;
        int num = 0;
        int id = -1;
        switch (elegirProducto(x, productoP))
        {
            case 0:
                tipo = 15000;
                rareza = elegirProducto(Random.Range(0, 101), menaP) * 100;
                id = tipo + rareza + num;
                break;
            case 1:
                tipo = 10000;
                rareza = elegirProducto(Random.Range(0, 101), consumibleP) * 100;
                num = Random.Range(0,numComidas.Length);
                id = tipo + rareza + num;
                break;
            case 2:
                tipo = 10000 + Random.Range(2, 4) * 1000;
                rareza = elegirProducto(Random.Range(0, 101), equipamientoP) * 100;
                if(tipo == 12000)
                {
                    num = Random.Range(0, numArmaduras[rareza/100]);
                }
                else
                {
                    num = Random.Range(0, numArmas[rareza/100]);
                }
                id = tipo + rareza + num;
                break;
            case 3:
                id = especialObjeto[Random.Range(0, especialObjeto.Length)];
                break;
            case 4:
                id = equipamientoObjeto[Random.Range(0, equipamientoObjeto.Length)];
                break;
        }
        if(id != -1)
        {
            recompensasId.Add(id);
        }
    }

    private int elegirProducto(int num, int[] productoP)
    {
        int x = -1;
        for(int i=0; i<productoP.Length && x == -1; i++)
        {
            if(productoP[i] >= num)
            {
                x = i;
            }
        }
        return x;
    }

    private void generarProducto()
    {
        int hijo = -1;
        int count = 0;
        foreach ( int id in recompensasId)
        {
            int itemTipo = Resources.Load<ItemScriptable>("ItemDato/" + id.ToString()).tipo;
            int num = numProductos(itemTipo);
            if (recompensaContent.transform.childCount == 0 || itemTipo == 2 || itemTipo == 3 || recompensaContent.transform.Find(id.ToString()) == null)
            {
                hijo++;
                Instantiate(prefOrigen, recompensaContent.transform);
                GameObject auxGameObject = recompensaContent.transform.GetChild(hijo).gameObject;
                string fileName = "ItemDato/" + id.ToString();
                ItemScriptable itemDato = Resources.Load<ItemScriptable>(fileName);
                auxGameObject.GetComponent<itemController>().setEstado("enInventario");
                auxGameObject.GetComponent<itemController>().initObjeto(itemDato, num, null, itemDato.durabilidad);
            }
            else
            {
                recompensaContent.transform.Find(id.ToString()).GetComponent<itemController>().addCantidad(num);
            }
            count++;
        }
    }

    private int numProductos(int tipo)
    {
        int num = 1;
        int x = 0, y = 0;
        switch (tipo)
        {
            case 0:
                if(this.caso == 0)
                {
                    x = 1;
                    y = 6;
                } else if(this.caso == 1)
                {
                    x = 5;
                    y = 21;
                } else if(this.caso == 2)
                {
                    x = 20;
                    y = 41;
                }
                num = Random.Range(x, y);
                break;
            case 5:
                if (this.caso == 0)
                {
                    x = 10;
                    y = 51;
                }
                else if (this.caso == 1)
                {
                    x = 50;
                    y = 101;
                }
                else if (this.caso == 2)
                {
                    x = 100;
                    y = 201;
                }
                num = Random.Range(x, y);
                break;
        }
        return num;
    }

    private void transferirProducto()
    {
        for(int i = 0; i < recompensaContent.transform.childCount; i++)
        {
            itemController iController = recompensaContent.transform.GetChild(i).GetComponent<itemController>();
            inventario.setItem(iController.getId(), iController.getAtributos(), iController.getCantidad(), iController.getDurabilidadRestante());
        }
    }

    private void restaurar()
    {
        recompensasId.Clear();
        this.caso = 0;
        for (int i = recompensaContent.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(recompensaContent.transform.GetChild(i).gameObject, true);
        }
    }
}
