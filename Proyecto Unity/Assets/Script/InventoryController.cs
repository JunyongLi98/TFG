using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class InventoryController : MonoBehaviour
{
    private GameObject inventario;
    private GameObject inventoryContent;
    private GameObject prefOrigen;



    private bool open = false;

    //Control de panel
    private PanelController panel;

    //Json data
    public class Objeto
    {
        public int id { get; set; }
        public int cantidad { get; set; }
        public int fuerza { get; set; }
        public int agilidad { get; set; }
        public int aguante { get; set; }
        public int movimiento { get; set; }
        public int suerte { get; set; }
        public int durabilidadRestante { get; set; }
        public string email { get; set; }
    }

    // Start is called before the first frame update
    void Start()
    {
        inventario = buscarObjeto("Inventario");
        inventoryContent = buscarObjeto("Inventario/Bag/Viewport/InventoryContent");
        panel = GameObject.Find("PanelController").GetComponent<PanelController>();


        prefOrigen = Resources.Load<GameObject>("Prefab/PrefOrigen/Item");

        inventario.SetActive(open);

        StartCoroutine(cogerDato());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Buscar objetos hijos
    GameObject buscarObjeto(string objectName)
    {
        return gameObject.transform.Find(objectName).gameObject;
    }

    /***********************************************************************************************************************
     * Insertar objetos en el inventario
     ***********************************************************************************************************************/

    public void setItem(int id, int[] atributos,int num, int durabilidadRestante)
    {
        ItemScriptable itemDato = Resources.Load<ItemScriptable>("ItemDato/" + id);
        GameObject objAux = null;
        if (inventoryContent.transform.Find(itemDato.id.ToString()) != null && itemDato.tipo != 2 && itemDato.tipo != 3)
        {
            objAux = inventoryContent.transform.Find(itemDato.id.ToString()).gameObject;
        }
        if (objAux == null)
        {
            Instantiate(prefOrigen, inventoryContent.transform);
            itemController iController = inventoryContent.transform.GetChild(inventoryContent.transform.childCount-1).GetComponent<itemController>();
            iController.setEstado("enInventario");
            iController.initObjeto(itemDato, num, atributos, durabilidadRestante);
        }
        else
        {
            objAux.GetComponent<itemController>().addCantidad(num);
        }
    }

    public void openInventario()
    {
        open = !open;
        inventario.SetActive(open);
    }

    public void openInvShop()
    {
        open = true;
        inventario.SetActive(open);
    }

    /***********************************************************************************************************************
     * Funcionalidad usar
     ***********************************************************************************************************************/

    public void usar(GameObject gameObject)
    {
        utilizar(gameObject);
    }

    private void utilizar(GameObject gameObject)
    {   
        itemController itemScript = gameObject.GetComponent<itemController>();

        panel.setCaso(itemScript.getObjetoFuncion());
        panel.setPanelActive(true);
        panel.setPanelPrincipalActive(false);
        panel.setPanelInsuficienteActive(false);
        panel.setPanelOpcionesActive(true);
        panel.setObjeto(gameObject);

        switch (itemScript.getObjetoFuncion())
        {
            case "usar":
                panel.setBTNStatus(true, false, true);
                panel.setCantidadMax();
                break;
            case "equipar":
                panel.setBTNStatus(false, true, true);
                break;
            case "vender":
                panel.setBTNStatus(false, false, true);
                panel.setCantidadMax();
                break;
        }

        if(itemScript.getTipo() == 4)
        {
            panel.setBTNStatus(false, false, true);
        }
    }

    /***********************************************************************************************************************
     * Ordenar producto
     ***********************************************************************************************************************/

    public void ordenar()
    {
        List<Objeto> objetos = generateListOrdenado();
        instanciaListOrdenado(objetos);
        StartCoroutine(guardarDato());

    }

    private List<Objeto> generateListOrdenado()
    {
        List<Objeto> objetos = new List<Objeto>();

        while(inventoryContent.transform.childCount > 0)
        {
            int cantidado = inventoryContent.transform.GetChild(0).GetComponent<itemController>().getId();
            int elegido = 0;
            for (int j = 1; j < inventoryContent.transform.childCount; j++)
            {
                if (inventoryContent.transform.GetChild(j).GetComponent<itemController>().getId() < cantidado)
                {
                    cantidado = inventoryContent.transform.GetChild(j).GetComponent<itemController>().getId();
                    elegido = j;
                }
            }

            itemController itemController = inventoryContent.transform.GetChild(elegido).transform.GetComponent<itemController>();
            Objeto objeto = new Objeto();
            objeto.id = itemController.getId();
            objeto.cantidad = itemController.getCantidad();
            objeto.fuerza = itemController.getFuerza();
            objeto.agilidad = itemController.getAgilidad();
            objeto.aguante = itemController.getAguante();
            objeto.movimiento = itemController.getMovimiento();
            objeto.suerte = itemController.getSuerte();
            objeto.durabilidadRestante = itemController.getDurabilidadRestante();
            objetos.Add(objeto);

            DestroyImmediate(inventoryContent.transform.GetChild(elegido).gameObject, true);
        }

        return objetos;
    }

    private void instanciaListOrdenado(List<Objeto> objetos)
    {
        if (objetos != null && objetos.Count != 0)
        {
            int i = 0;
            foreach (Objeto objetoActual in objetos)
            {
                Instantiate(prefOrigen, inventoryContent.transform);
                GameObject auxGameObject = inventoryContent.transform.GetChild(i).gameObject;
                string fileName = "ItemDato/" + objetoActual.id;
                ItemScriptable itemDato = Resources.Load<ItemScriptable>(fileName);
                itemController itemController = auxGameObject.GetComponent<itemController>();
                int[] atributos = { objetoActual.fuerza, objetoActual.agilidad, objetoActual.aguante, objetoActual.movimiento, objetoActual.suerte };
                itemController.setEstado("enInventario");
                itemController.initObjeto(itemDato, objetoActual.cantidad, atributos, objetoActual.durabilidadRestante);
                i++;
            }
        }
    }

    /***********************************************************************************************************************
     * Conexion con el bbdd
     ***********************************************************************************************************************/

    public void externoGuardarDato()
    {
        StartCoroutine(guardarDato());
    }

    IEnumerator guardarDato()
    {
        string json = generarJson();
        var www = new UnityWebRequest("https://localhost:7095/api/inventoryObjetos", "Post");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
    }

    private string generarJson()
    {
        string json = "";
        List<Objeto> objetos = new List<Objeto>();
        for (int i = 0; i < inventoryContent.transform.childCount; i++)
        {
            Objeto objeto = new Objeto();
            itemController itemController = inventoryContent.transform.GetChild(i).GetComponent<itemController>();
            objeto.id = itemController.getId();
            objeto.cantidad = itemController.getCantidad();
            objeto.fuerza = itemController.getFuerza();
            objeto.agilidad = itemController.getAgilidad();
            objeto.aguante = itemController.getAguante();
            objeto.movimiento = itemController.getMovimiento();
            objeto.suerte = itemController.getSuerte();
            objeto.durabilidadRestante = itemController.getDurabilidadRestante();
            objeto.email = LoginManagerScript.Instance.user.correo;
            objetos.Add(objeto);
        }
        json = JsonConvert.SerializeObject(objetos);
        return json;
    }

    IEnumerator cogerDato()
    {
        string json = generarJsonUser();
        var www = new UnityWebRequest("https://localhost:7095/api/inventoryObjetos", "Get");
        DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = dh;
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string jsonObjeto = www.downloadHandler.text;
            List<Objeto> objetos = JsonConvert.DeserializeObject<List<Objeto>>(jsonObjeto);
            if (objetos != null && objetos.Count != 0)
            {
                foreach (Objeto objetoActual in objetos)
                {
                    Instantiate(prefOrigen, inventoryContent.transform);
                    GameObject auxGameObject = inventoryContent.transform.GetChild(inventoryContent.transform.childCount-1).gameObject;
                    string fileName = "ItemDato/" + objetoActual.id;
                    ItemScriptable itemDato = Resources.Load<ItemScriptable>(fileName);
                    itemController itemController = auxGameObject.GetComponent<itemController>();
                    int[] atributos = { objetoActual.fuerza, objetoActual.agilidad, objetoActual.aguante, objetoActual.movimiento, objetoActual.suerte };
                    itemController.setEstado("enInventario");
                    itemController.initObjeto(itemDato, objetoActual.cantidad, atributos, objetoActual.durabilidadRestante);
                }
            }
        }
    }
    private string generarJsonUser()
    {
        string json = "";
        User user = new User();
        user.correo = LoginManagerScript.Instance.user.correo;
        user.password = "";
        user.nombre = "";
        user.avatar = "";
        json = JsonConvert.SerializeObject(user);

        return json;
    }
}
