using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class PanelController : MonoBehaviour
{
    private GameObject panel;

    //Panel principal
    private GameObject panelPrincipal;
    private Text mensajePrincipal;
    private Text cantidad;

    //Panel insuficiente
    private GameObject panelInsuficiente;
    private Text mensajeInsuficiente;

    //Panel opciones
    private GameObject panelOpciones;
    private GameObject usarBTN;
    private GameObject equiparBTN;
    private GameObject venderBTN;

    //Datos
    private ShopController shop;
    private InventoryController inventory;
    private CofreShopController cofreShop;
    private int cantidadMax;
    private int cantidadElegida;
    private int precio;
    private int monedas;
    private itemController objeto;
    private string caso;


    // Start is called before the first frame update
    void Start()
    {
        panel = gameObject.transform.Find("Panel").gameObject;
        panelPrincipal = panel.transform.GetChild(0).gameObject;
        panelInsuficiente = panel.transform.GetChild(1).gameObject;
        mensajePrincipal = panelPrincipal.transform.GetChild(0).GetComponent<Text>();
        mensajeInsuficiente = panelInsuficiente.transform.GetChild(0).GetComponent<Text>();
        cantidad = panelPrincipal.transform.GetChild(1).GetComponent<Text>();
        shop = GameObject.Find("ShopController").GetComponent<ShopController>();
        cofreShop = GameObject.Find("CofreShopController").GetComponent<CofreShopController>();
        inventory = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        panelOpciones = panel.transform.GetChild(2).gameObject;
        usarBTN = panelOpciones.transform.Find("ScrollView/Viewport/Content/Usar").gameObject;
        equiparBTN = panelOpciones.transform.Find("ScrollView/Viewport/Content/Equipar").gameObject;
        venderBTN = panelOpciones.transform.Find("ScrollView/Viewport/Content/Vender").gameObject;

        cantidadElegida = 1;
        cantidadMax = 1;
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /***********************************************************************************************************************
     * Set y Get
     ***********************************************************************************************************************/

    public void setMensajePrincipal(string mensaje)
    {
        this.mensajePrincipal.text = mensaje;
    }

    public void setMensajeInsuficiente(string mensaje)
    {
        this.mensajeInsuficiente.text = mensaje;
    }

    public void setCantidad()
    {
        this.cantidad.text = cantidadElegida.ToString();
    }

    public void addCantidadElegida()
    {
        if(cantidadElegida < cantidadMax)
        {
            this.cantidadElegida++;
            setCantidad();
            setMonedas();
        }
    }

    public void decreaseCantidadElegida()
    {
        if(cantidadElegida > 1)
        {
            this.cantidadElegida--;
            setCantidad();
            setMonedas();
        }
    }

    public void setCantidadMax()
    {
        this.cantidadMax = objeto.getCantidad();
    }

    public void setObjeto(GameObject objeto)
    {
        this.objeto = objeto.GetComponent<itemController>();
    }

    public void setPrecio()
    {
        this.precio = objeto.GetComponent<itemController>().getPrecio();
    }

    public void setMonedas()
    {
        this.monedas = precio * cantidadElegida;
    }

    public void setPanelActive(bool estado)
    {
        panel.SetActive(estado);
    }

    public void setPanelPrincipalActive(bool estado)
    {
        panelPrincipal.SetActive(estado);
    }

    public void setPanelInsuficienteActive(bool estado)
    {
        this.panelInsuficiente.SetActive(estado);
    }

    public void setCaso(string caso)
    {
        this.caso = caso;
    }

    public void setPanelOpcionesActive(bool estado)
    {
        this.panelOpciones.SetActive(estado);
    }

    public void setBTNStatus(bool usarStatus, bool equiparStatus, bool venderStatus)
    {
        this.usarBTN.SetActive(usarStatus);
        this.equiparBTN.SetActive(equiparStatus);
        this.venderBTN.SetActive(venderStatus);
    }

    /***********************************************************************************************************************
     * BTN funcion
     ***********************************************************************************************************************/

    public void usar()
    {
        this.setMensajePrincipal("Quieres usar " + objeto.getName() + "?");
        this.setMensajeInsuficiente("El personaje elegido tiene vida llena.");
        this.setPanelOpcionesActive(false);
        this.setPanelPrincipalActive(true);
        setCaso("usar");
    }

    public void equipar()
    {
        this.setMensajePrincipal("Quieres equipar " + objeto.getName() + "?");
        this.setMensajeInsuficiente("El personaje elegido ya tiene equipamento equipado.");
        this.setPanelOpcionesActive(false);
        this.setPanelPrincipalActive(true);
        setCaso("equipar");
    }

    public void vender()
    {
        this.precio = objeto.getPrecio();
        if(objeto.getTipo() != 5)
        {
            this.precio = precio / 10;
        }
        this.setMensajePrincipal("Quieres vender " + objeto.getName() + " por " + precio.ToString() + " monedas/unidad?");
        this.setMensajeInsuficiente("");
        this.setPanelOpcionesActive(false);
        this.setPanelPrincipalActive(true);
        this.setMonedas();
        setCaso("vender");
    }

    /***********************************************************************************************************************
     * Casos
     ***********************************************************************************************************************/

    public void switchFuncionalidad()
    {
        switch (caso)
        {
            case "comprar":
                comprarAceptada();
                break;
            case "actualizar":
                actualizarAceptada();
                break;
            case "usar":
                break;
            case "equipar":
                break;
            case "vender":
                venderAceptada();
                break;
            case "abrir cofre":
                abrirAceptada();
                break;
            default:
                break;
        }
    }

    /***********************************************************************************************************************
     * Comprar
     ***********************************************************************************************************************/

    private void comprarAceptada()
    {
        if(shop.getMonedas() > monedas)
        {
            inventory.openInvShop();
            shop.setMonedas(monedas * -1);
            shop.addMonedasUsadas(monedas);
            inventory.setItem(objeto.getId(), objeto.getAtributos(), cantidadElegida, objeto.getDurabilidad());
            objeto.addCantidad(cantidadElegida * -1);
            shop.externoGuardarDato();
            shop.externoGuardarMonedas();
            inventory.externoGuardarDato();
            restaurarPanel();  
            panel.SetActive(false);
        }
        else
        {
            panelPrincipal.SetActive(false);
            panelInsuficiente.SetActive(true);
        }
    }

    /***********************************************************************************************************************
     * Actualizar
     ***********************************************************************************************************************/

    private void actualizarAceptada()
    {
        inventory.openInvShop();
        objeto.addCantidad(-1);
        shop.restaurarTienda();
        inventory.externoGuardarDato();
        restaurarPanel();
        panel.SetActive(false);
    }

    /***********************************************************************************************************************
     * Usar
     ***********************************************************************************************************************/

    private void utilizarAceptada()
    {

    }

    /***********************************************************************************************************************
     * Equipar
     ***********************************************************************************************************************/

    private void equiparAceptada()
    {

    }

    /***********************************************************************************************************************
     * Vender
     ***********************************************************************************************************************/

    private void venderAceptada()
    {
        shop.setShopActive();
        this.objeto.addCantidad(cantidadElegida * -1);
        shop.setMonedas(monedas);
        if(objeto.getTipo() != 5)
        {
            shop.addReclamarObjeto(objeto.getId(), objeto.getAtributos(), this.cantidadElegida);
        }
        shop.externoGuardarReclamado();
        shop.externoGuardarMonedas();
        inventory.externoGuardarDato();
        restaurarPanel();
        panel.SetActive(false);
    }

    /***********************************************************************************************************************
     * Vender
     ***********************************************************************************************************************/

    private void abrirAceptada()
    {
        cofreShop.aceptarAbrir(cantidadElegida);
        inventory.openInvShop();
        objeto.addCantidad(cantidadElegida * -1);
        inventory.externoGuardarDato();
        restaurarPanel();
        panel.SetActive(false);
    }

    /***********************************************************************************************************************
     * Rechazar
     ***********************************************************************************************************************/

    public void Rechazada()
    {
        restaurarPanel();
        panel.SetActive(false);
    }

    /***********************************************************************************************************************
     * Resturar panel
     ***********************************************************************************************************************/

    private void restaurarPanel()
    {
        panelPrincipal.SetActive(true);
        panelInsuficiente.SetActive(true);
        panelOpciones.SetActive(true);
        usarBTN.SetActive(true);
        equiparBTN.SetActive(true);
        venderBTN.SetActive(true);
        this.cantidad.text = "1";
        this.mensajePrincipal.text = "";
        this.mensajeInsuficiente.text = "";
        this.cantidadElegida = 1;
        this.cantidadMax = 1;
        this.precio = 0;
        this.monedas = 0;
        this.objeto = null;
    }

}
