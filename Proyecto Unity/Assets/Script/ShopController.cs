using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class ShopController : MonoBehaviour
{
    private GameObject shop;

    //Categorias
    private GameObject[] scrolls = new GameObject[5];
    private GameObject[] scrollsContent = new GameObject[5];
    private GameObject scrollReclamar;
    private GameObject reclamarContent;

    //Tipos de rareza
    private string[] tiposRareza = { "Comun", "Raro", "Exotico", "Epico", "Legendario" };

    //Control de rareza
    private int[] rarezaControl = { 60, 20, 12, 5, 3 };

    //Cantidad de variables de cada tipo de producto
    private int[] numComidas = {3, 2, 1, 1, 1 };
    private int[] numPociones = { 0, 0, 0, 0, 0 };
    private int[] numArmaduras = { 2, 2, 4, 2, 2 };
    private int[] numArmas = { 2, 3, 4, 3, 1 };
    private int[] numEspeciales = { 0, 0, 2, 1, 1 };

    //Productos que va a vender
    private int[] comidas;
    private int[] pociones;
    private int[] armaduras;
    private int[] armas;
    private int[] especiales;

    //Prefab original
    private GameObject prefOrigen;

    //Maximo productos que puede tener cada categoria
    private int maxComida = 30;
    private int maxPocion = 0;
    private int maxArmadura = 20;
    private int maxArma = 10;
    private int maxEspecial = 5;

    //Control de visibilidad del shop
    private bool show = false;
    private int preScroll = 0;

    //Acceder al inventario
    private GameObject inventario;
    private GameObject inventarioContent;
    private InventoryController inventoryController;

    //Control de panel de uso
    private PanelController panel;

    //Tiempo de actualizar la tienda
    private GameObject reload;
    private float tiempoActualizar = 2;
    private float cicloTiempo = 60;
    private int hora = 0;
    private int minuto = 0;

    //Control de monedas
    private int monedas = 0;
    private int monedasUsadas = 0;

    //Cofres
    private CofreShopController cofreShop;

    //Componentes
    private ConocimientoController conocimientoController;
    private FilterController filterController;

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
        public int descuento { get; set; }
        public string email { get; set; }
    }

    public class Monedas
    {
        public int cantidad { get; set; }
        public int cantidadUsada { get; set; }
        public string email { get; set; }
    }

    public class Tiempo
    {
        public int hora { get; set; }
        public int minuto { get; set; }
        public string email { get; set; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Buscar los gameObject
        shop = buscarObjeto("Shop");
        scrolls[0] = buscarObjeto("Shop/ScrollFood");
        scrolls[1] = buscarObjeto("Shop/ScrollPotion");
        scrolls[2] = buscarObjeto("Shop/ScrollArmor");
        scrolls[3] = buscarObjeto("Shop/ScrollWeapon");
        scrolls[4] = buscarObjeto("Shop/ScrollSpecial");
        scrollReclamar = buscarObjeto("Shop/ScrollReclamar");
        scrollsContent[0] = buscarObjeto("Shop/ScrollFood/Viewport/FoodContent");
        scrollsContent[1] = buscarObjeto("Shop/ScrollPotion/Viewport/PotionContent");
        scrollsContent[2] = buscarObjeto("Shop/ScrollArmor/Viewport/ArmorContent");
        scrollsContent[3] = buscarObjeto("Shop/ScrollWeapon/Viewport/WeaponContent");
        scrollsContent[4] = buscarObjeto("Shop/ScrollSpecial/Viewport/SpecialContent");
        reclamarContent = buscarObjeto("Shop/ScrollReclamar/Viewport/ReclamarContent");
        panel = GameObject.Find("PanelController").GetComponent<PanelController>();
        reload = buscarObjeto("Shop/Actualizar");
        inventario = GameObject.Find("InventoryController");
        inventarioContent = inventario.transform.Find("Inventario/Bag/Viewport/InventoryContent").gameObject;
        inventoryController = inventario.GetComponent<InventoryController>();
        cofreShop = GameObject.Find("CofreShopController").GetComponent<CofreShopController>();
        conocimientoController = buscarObjeto("Shop/Conocimiento").GetComponent<ConocimientoController>();
        filterController = buscarObjeto("Shop/Filtro").GetComponent<FilterController>();

        //Cargar pref original
        prefOrigen = Resources.Load<GameObject>("Prefab/PrefOrigen/Item");

        //Shop desactivar
        shop.SetActive(false);

        //inicializar array de producto
        comidas = new int[maxComida];
        pociones = new int[maxPocion];
        armaduras = new int[maxArmadura];
        armas = new int[maxArma];
        especiales = new int[maxEspecial];

        StartCoroutine(cogerDato());
        StartCoroutine(cogerDatoReclamar());
        StartCoroutine(cogerDatoMoneda());
        StartCoroutine(cogerDatoMonedaUsadas());
        StartCoroutine(cogerDatoTiempo());
    }

    // Update is called once per frame
    void Update()
    {
        cicloTiempo -= Time.deltaTime;
        if (cicloTiempo < 0)
        {
            cicloTiempo = 60;
            calcularTiempo();
            reload.transform.GetChild(0).gameObject.GetComponent<Text>().text = tiempoToString();
            StartCoroutine(guardarDatoTiempo());
        }
    }

    //Buscar objetos hijos
    GameObject buscarObjeto(string objectName)
    {
        return gameObject.transform.Find(objectName).gameObject;
    }

    /***********************************************************************************************************************
     * Set y Get
     ***********************************************************************************************************************/

    public void setMonedas(int cantidad)
    {
        this.monedas += cantidad;
        this.gameObject.transform.Find("Shop/Money/Cantidad").GetComponent<Text>().text = monedas.ToString();
    }

    public int getMonedas()
    {
        return this.monedas;
    }

    public void addMonedasUsadas(int cantidad)
    {
        this.monedasUsadas += cantidad;
    }

    public void resetMonedasUsadas()
    {
        monedasUsadas = 0;
    }

    public void setDefaultTime()
    {
        hora = (int)tiempoActualizar;
        minuto = (int)((tiempoActualizar - hora) * 60);
        reload.transform.GetChild(0).gameObject.GetComponent<Text>().text = tiempoToString();
        StartCoroutine(guardarDatoTiempo());
    }

    public GameObject[] getScrollContent()
    {
        return this.scrollsContent;
    }

    public void setShopActive()
    {
        if(this.show == false)
        {
            ShopOpen();
        }
    }

    /***********************************************************************************************************************
     * Inicial la tienda
     ***********************************************************************************************************************/

    private void initShop()
    {
        initItem(comidas, numComidas, 0);
        initItem(pociones, numPociones, 1);
        initItem(armaduras, numArmaduras, 2);
        initItem(armas, numArmas, 3);
        initItem(especiales, numEspeciales, 4);

        instanciaItem(comidas, scrollsContent[0], itemCount(numComidas));
        instanciaItem(pociones, scrollsContent[1], itemCount(numPociones));
        instanciaItem(armaduras, scrollsContent[2], itemCount(numArmaduras));
        instanciaItem(armas, scrollsContent[3], itemCount(numArmas));
        instanciaItem(especiales, scrollsContent[4], itemCount(numEspeciales));

        StartCoroutine(guardarDato());
        StartCoroutine(guardarDatoReclamar());
    }

    private void initItem(int[] items, int[] itemNum, int tipo)
    {
        if (itemCount(itemNum) != 0)
        {
            tipo *= 1000;
            int i = 0;
            while (i < items.Length)
            {
                int x = choiceRareza();
                if(itemNum[x] != 0)
                {
                    int rareza = x * 100;
                    int num = Random.Range(0, itemNum[x]);
                    items[i] = 10000 + tipo + rareza + num;
                    i++;
                }
            }
            System.Array.Sort(items);
        }
    }

    private int itemCount(int[] itemNum)
    {
        int num = 0;
        for(int i = 0; i < itemNum.Length; i++)
        {
            num += itemNum[i];
        }
        return num;
    }

    private int choiceRareza()
    {
        int n = Random.Range(0, sumaProbabilidad(5));
        int rareza;

        if (n < sumaProbabilidad(1))
        {
            rareza = 0;
        }
        else if (n < sumaProbabilidad(2))
        {
            rareza = 1;
        }
        else if (n < sumaProbabilidad(3))
        {
            rareza = 2;
        }
        else if (n < sumaProbabilidad(4))
        {
            rareza = 3;
        }
        else
        {
            rareza = 4;
        }

        return rareza;
    }

    private int sumaProbabilidad(int x)
    {
        int sumaTotal = 0;
        for (int i = 0; i < x; i++)
        {
            sumaTotal += rarezaControl[i];
        }
        return sumaTotal;
    }

    private void instanciaItem(int[] items, GameObject gameObject, int numItem)
    {
        if (numItem != 0)
        {
            int itemId = items[0];
            int hijo = -1;
            int itemTipo = Resources.Load<ItemScriptable>("ItemDato/" + itemId.ToString()).tipo;
            for (int i = 0; i < items.Length; i++)
            {
                if (i == 0 || itemId != items[i] || itemTipo == 2 || itemTipo == 3)
                {
                    itemId = items[i];
                    hijo++;
                    Instantiate(prefOrigen, gameObject.transform);
                    GameObject auxGameObject = gameObject.transform.GetChild(hijo).gameObject;
                    string fileName = "ItemDato/" + itemId.ToString();
                    ItemScriptable itemDato = Resources.Load<ItemScriptable>(fileName);
                    auxGameObject.GetComponent<itemController>().setEstado("enTienda");
                    auxGameObject.GetComponent<itemController>().initObjeto(itemDato, 1, null, itemDato.durabilidad);
                }
                else
                {
                    gameObject.transform.GetChild(hijo).GetComponent<itemController>().addCantidad(1);
                }
            }
        }
    }

    /***********************************************************************************************************************
     * Control de estado
     ***********************************************************************************************************************/

    public void ShopOpen()
    {
        show = !show;
        if (!show)
        {
            conocimientoController.cerrar();
            filterController.closeFilter();
        }
        shop.SetActive(show);
        if (show)
        {
            scrolls[0].SetActive(true);
            scrolls[1].SetActive(false);
            scrolls[2].SetActive(false);
            scrolls[3].SetActive(false);
            scrolls[4].SetActive(false);
            scrollReclamar.SetActive(false);
        }
        cofreShop.cerrarTienda();
    }

    public void mostrarCategoria(int i)
    {
        if(preScroll != 5)
        {
            scrolls[preScroll].SetActive(false);
        }
        else
        {
            scrollReclamar.SetActive(false);
        }

        if(i != 5)
        {
            scrolls[i].SetActive(true);

        }
        else
        {
            scrollReclamar.SetActive(true);
        }

        preScroll = i;
    }

    public void cerrarTienda()
    {
        this.preScroll = 0;
        this.show = false;
        conocimientoController.cerrar();
        filterController.closeFilter();
        shop.SetActive(false);
    }

    /***********************************************************************************************************************
     * Funcionalidad actualizar
     ***********************************************************************************************************************/

    public void restaurarTienda()
    {
        destruirObjetos(scrollsContent[0]);
        destruirObjetos(scrollsContent[1]);
        destruirObjetos(scrollsContent[2]);
        destruirObjetos(scrollsContent[3]);
        destruirObjetos(scrollsContent[4]);
        destruirObjetos(reclamarContent);
        initShop();
        setDefaultTime();

        if(this.monedasUsadas >= 5000)
        {
            newDescuento();
            StartCoroutine(guardarDato());
        }
        resetMonedasUsadas();
        StartCoroutine(guardarDatoMoneda());
    }

    public void actualizarTieda()
    {
        string texto1 = "Quieres usar Piedra de tiempo para actualizar la tienda?";
        string texto2 = "No tienes Piedra de tiempo suficiente";

        panel.setPanelActive(true);
        panel.setMensajePrincipal(texto1);
        panel.setMensajeInsuficiente(texto2);
        panel.setPanelOpcionesActive(false);
        panel.setCaso("actualizar");
        if (inventarioContent.transform.Find("14200") != null)
        {
            panel.setPanelInsuficienteActive(false);
            panel.setObjeto(inventarioContent.transform.Find("14200").gameObject);
        }
        else
        {
            panel.setPanelPrincipalActive(false);
        }
    }

    public void destruirObjetos(GameObject gameObject)
    {
        for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(gameObject.transform.GetChild(i).gameObject, true);
        }
    }

    /***********************************************************************************************************************
     * Temporizador
     ***********************************************************************************************************************/

    void calcularTiempo()
    {
        if (minuto == 0)
        {
            if (hora != 0)
            {
                hora--;
                minuto = 59;
            }
        }
        else
        {
            minuto--;
        }

        if (minuto == 0 && hora == 0)
        {
            hora = (int)tiempoActualizar;
            minuto = (int)((tiempoActualizar - hora) * 60);
            restaurarTienda();
        }
    }

    private string tiempoToString()
    {
        string tiempo;
        string horaText, minutoText;
        if (hora < 10)
        {
            horaText = "0" + hora.ToString();
        }
        else
        {
            horaText = hora.ToString();
        }
        if (minuto < 10)
        {
            minutoText = "0" + minuto.ToString();
        }
        else
        {
            minutoText = minuto.ToString();
        }

        tiempo = horaText + ":" + minutoText;
        return tiempo;
    }

    /***********************************************************************************************************************
     * Funcionalidad comprar
     ***********************************************************************************************************************/

    public void comprarObjeto(GameObject gameObject)
    {
        itemController itemScript = gameObject.GetComponent<itemController>();
        string texto1 = "Comprar " + itemScript.getName() + "?";
        string texto2 = "No tienes monedas suficientes";

        panel.setCaso("comprar");
        panel.setPanelActive(true);
        panel.setMensajePrincipal(texto1);
        panel.setMensajeInsuficiente(texto2);
        panel.setPanelInsuficienteActive(false);
        panel.setPanelOpcionesActive(false);
        panel.setObjeto(gameObject);
        panel.setCantidadMax();
        panel.setPrecio();
        panel.setMonedas();
        
    }

    /***********************************************************************************************************************
     * Reclamar
     ***********************************************************************************************************************/

    public void addReclamarObjeto(int id, int[] atributos, int num)
    {
        ItemScriptable itemDato = Resources.Load<ItemScriptable>("ItemDato/" + id);
        GameObject objAux = null;
        if (reclamarContent.transform.Find(itemDato.id.ToString()) != null && itemDato.tipo != 2 && itemDato.tipo != 3)
        {
            objAux = reclamarContent.transform.Find(itemDato.id.ToString()).gameObject;
        }
        if (objAux == null)
        {
            int numItem = reclamarContent.transform.childCount;
            Instantiate(prefOrigen, reclamarContent.transform);
            itemController iController = reclamarContent.transform.GetChild(numItem).GetComponent<itemController>();
            iController.setEstado("enTienda");
            iController.estadoReclamar();
            iController.initObjeto(itemDato, num, atributos, itemDato.durabilidad);
        }
        else
        {
            objAux.GetComponent<itemController>().addCantidad(num);
        }
    }

    /***********************************************************************************************************************
     * Descuento
     ***********************************************************************************************************************/

    private void newDescuento()
    {
        bool ok = false;

        while (!ok)
        {
            int i = Random.Range(0, 5);
            if(i != 1 && scrollsContent[i].transform.childCount > 0)
            {
                ok = true;
                int child = Random.Range(0, scrollsContent[i].transform.childCount);
                scrollsContent[i].transform.GetChild(child).GetComponent<itemController>().addDescuento(10);
            }
        }
    }

    /***********************************************************************************************************************
     * Conexion con bbdd
     ***********************************************************************************************************************/

    public void externoGuardarDato()
    {
        StartCoroutine(guardarDato());
        StartCoroutine(guardarDatoReclamar());

        //Comprobar el numero total del producto
        int productNum = -1;
        for(int i = 0; i < 5; i++)
        {
            productNum += scrollsContent[i].transform.childCount;
        }
        //Si no hay productos, actualizar la tienda
        if (productNum == 0)
        {
            restaurarTienda();
        }
    }

    public void externoGuardarMonedas()
    {
        StartCoroutine(guardarDatoMoneda());
    }

    public void externoGuardarReclamado()
    {
        StartCoroutine(guardarDatoReclamar());
    }

    IEnumerator cogerDato()
    {
        string json = generarJsonUser();
        var www = new UnityWebRequest("https://localhost:7095/api/shopObjetos", "Get");
        DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = dh;
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            initShop();
        }
        else
        {
            string jsonObjeto = www.downloadHandler.text;
            List<Objeto> objetos = JsonConvert.DeserializeObject<List<Objeto>>(jsonObjeto);
            if (objetos != null && objetos.Count != 0)
            {
                int i = 0;
                int j = 0;
                int tipo = 11000;
                foreach (Objeto objetoActual in objetos)
                {
                    if (objetoActual.id >= tipo)
                    {
                        j++;
                        tipo += 1000;
                        i = 0;
                    }

                    Instantiate(prefOrigen, scrollsContent[j].transform);
                    GameObject auxGameObject = scrollsContent[j].transform.GetChild(i).gameObject;
                    string fileName = "ItemDato/" + objetoActual.id;
                    ItemScriptable itemDato = Resources.Load<ItemScriptable>(fileName);
                    itemController itemController = auxGameObject.GetComponent<itemController>();
                    int[] atributos = { objetoActual.fuerza, objetoActual.agilidad, objetoActual.aguante, objetoActual.movimiento, objetoActual.suerte };
                    itemController.setEstado("enTienda");
                    itemController.initObjeto(itemDato, objetoActual.cantidad, atributos, objetoActual.durabilidadRestante);
                    if(objetoActual.descuento != 0)
                    {
                        itemController.addDescuento(objetoActual.descuento);
                    }
                    i++;
                }
            }
            else
            {
                initShop();
            }
        }
        scrolls[1].SetActive(false);
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

    IEnumerator guardarDato()
    {
        string json = generarJson();
        var www = new UnityWebRequest("https://localhost:7095/api/shopObjetos", "Post");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
    }

    private string generarJson()
    {
        string json = "";
        List<Objeto> objetos = new List<Objeto>();
        for (int i = 0; i < 5; i++)
        {
            for(int j = 0; j < scrollsContent[i].transform.childCount && i != 1; j++)
            {
                Objeto objeto = new Objeto();
                itemController itemController = scrollsContent[i].transform.GetChild(j).GetComponent<itemController>();
                objeto.id = itemController.getId();
                objeto.cantidad = itemController.getCantidad();
                objeto.fuerza  = itemController.getFuerza();
                objeto.agilidad = itemController.getAgilidad();
                objeto.aguante = itemController.getAguante();
                objeto.movimiento = itemController.getMovimiento();
                objeto.suerte = itemController.getSuerte();
                objeto.durabilidadRestante = itemController.getDurabilidadRestante();
                objeto.descuento = itemController.getDescuento();
                objeto.email = LoginManagerScript.Instance.user.correo;
                objetos.Add(objeto);
            }
        }

        json = JsonConvert.SerializeObject(objetos);
        return json;
    }

    IEnumerator cogerDatoReclamar()
    {
        string json = generarJsonUser();
        var www = new UnityWebRequest("https://localhost:7095/api/shopObjetos/Reclamar", "Get");
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
                int i = 0;
                foreach (Objeto objetoActual in objetos)
                {
                    Instantiate(prefOrigen, reclamarContent.transform);
                    GameObject auxGameObject = reclamarContent.transform.GetChild(i).gameObject;
                    string fileName = "ItemDato/" + objetoActual.id;
                    ItemScriptable itemDato = Resources.Load<ItemScriptable>(fileName);
                    itemController itemController = auxGameObject.GetComponent<itemController>();
                    int[] atributos = { objetoActual.fuerza, objetoActual.agilidad, objetoActual.aguante, objetoActual.movimiento, objetoActual.suerte };
                    itemController.setEstado("enTienda");
                    itemController.estadoReclamar();
                    itemController.initObjeto(itemDato, objetoActual.cantidad, atributos, objetoActual.durabilidadRestante);
                    i++;
                }
            }
        }
    }

    IEnumerator guardarDatoReclamar()
    {
        string json = generarJsonReclamar();
        var www = new UnityWebRequest("https://localhost:7095/api/shopObjetos/Reclamar", "Post");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
    }

    private string generarJsonReclamar()
    {
        string json = "";
        List<Objeto> objetos = new List<Objeto>();
        
        if(reclamarContent.transform.childCount != 0)
        {
            for (int i = 0; i < reclamarContent.transform.childCount; i++)
            {
                Objeto objeto = new Objeto();
                itemController itemController = reclamarContent.transform.GetChild(i).GetComponent<itemController>();
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
        }
        else
        {
            Objeto objeto = new Objeto();
            objeto.id = -1;
            objeto.email = LoginManagerScript.Instance.user.correo;
            objetos.Add(objeto);
        }
        
        json = JsonConvert.SerializeObject(objetos);
        return json;
    }

    IEnumerator cogerDatoMoneda()
    {
        string json = generarJsonUser();
        var www = new UnityWebRequest("https://localhost:7095/api/Monedas", "Get");
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
            string jsonMonedas = www.downloadHandler.text;
            int monedas = JsonConvert.DeserializeObject<int>(jsonMonedas);
            if (monedas != null)
            {
                this.setMonedas(monedas);
            }
        }
    }

    IEnumerator guardarDatoMoneda()
    {
        Monedas moneda = new Monedas();
        moneda.cantidad = this.monedas;
        moneda.cantidadUsada = this.monedasUsadas;
        moneda.email = LoginManagerScript.Instance.user.correo;
        string json = JsonConvert.SerializeObject(moneda);
        var www = new UnityWebRequest("https://localhost:7095/api/Monedas", "Post");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
    }

    IEnumerator cogerDatoMonedaUsadas()
    {
        string json = generarJsonUser();
        var www = new UnityWebRequest("https://localhost:7095/api/MonedasUsadas", "Get");
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
            string jsonMonedas = www.downloadHandler.text;
            int monedas = JsonConvert.DeserializeObject<int>(jsonMonedas);
            if (monedas != null)
            {
                this.monedasUsadas = monedas;
            }
        }
    }

    IEnumerator cogerDatoTiempo()
    {
        string json = generarJsonUser();
        var www = new UnityWebRequest("https://localhost:7095/api/ShopTiempo", "Get");
        DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = dh;
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            hora = (int)tiempoActualizar;
            minuto = (int)((tiempoActualizar - hora) * 60);
            reload.transform.GetChild(0).gameObject.GetComponent<Text>().text = tiempoToString();
        }
        else
        {
            string jsonTiempo = www.downloadHandler.text;
            Tiempo tiempo = JsonConvert.DeserializeObject<Tiempo>(jsonTiempo);
            if (tiempo == null || (tiempo.hora == 0 && tiempo.minuto == 0))
            {
                setDefaultTime();
            }
            else
            {
                this.hora = tiempo.hora;
                this.minuto = tiempo.minuto;
                reload.transform.GetChild(0).gameObject.GetComponent<Text>().text = tiempoToString();
            }
        }
    }

    IEnumerator guardarDatoTiempo()
    {
        Tiempo tiempo = new Tiempo();
        tiempo.hora = this.hora;
        tiempo.minuto = this.minuto;
        tiempo.email = LoginManagerScript.Instance.user.correo;
        string json = JsonConvert.SerializeObject(tiempo);
        var www = new UnityWebRequest("https://localhost:7095/api/ShopTiempo", "Post");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
    }

}
