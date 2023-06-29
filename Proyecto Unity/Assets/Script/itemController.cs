using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemController : MonoBehaviour
{
    private int id;
    private new string name;
    private int tipo;
    private int rareza;
    private string descripcion = "";
    private int precio;
    private int cantidad = 1;
    private int variable;
    private int cantidadRecuperada;
    private int durabilidad = 1;
    private int durabilidadRestante = 1;
    private GameObject panelDescripcion;
    private int descuento = 0;
    private bool especial;

    private int[] itemAtributos = {0, 0, 0, 0, 0};
    private string[] atributoNames = {"Fuerza", "Agilidad", "Aguante", "Movimiento", "Suerte" };

    //Administrar compra
    private GameObject shop;

    //Administrar objeto
    private string objetoFuncion;
    private string estado;

    //Administrar precio
    private GameObject panelPrecio;

    //Administrar inventario
    private GameObject inventario;

    //Atributos
    private int[] totalAtributos = { 5, 7, 10, 14, 20 };

    //Objeto de reclamar
    private bool reclamar = false;

    // Start is called before the first frame update
    void Start()
    {
        shop = GameObject.Find("ShopController");
        inventario = GameObject.Find("InventoryController");

        panelDescripcion = this.transform.GetChild(1).gameObject;
        panelDescripcion.SetActive(false);

        panelPrecio = this.transform.GetChild(0).gameObject;

        this.gameObject.name = id.ToString();

        initEstado();
    }

    // Update is called once per frame
    void Update()
    {
        if (cantidad == 0)
        {
            DestroyImmediate(this.gameObject, true);
        }

        if(tipo == 2 || tipo == 3)
        {
            if(durabilidadRestante == 0)
            {
                DestroyImmediate(this.gameObject, true);
            }
        }
        
    }

    /***********************************************************************************************************************
     * Set y Get
     ***********************************************************************************************************************/

    public void setId(int id)
    {
        this.id = id;
        this.gameObject.name = id.ToString();
    }

    public int getId()
    {
        return id;
    }

    public void setObjetoName()
    {
        this.gameObject.name = id.ToString();
    }

    public void setTipo(int tipo)
    {
        this.tipo = tipo;
    }

    public int getTipo()
    {
        return tipo;
    }

    public void setCantidad(int cantidad)
    {
        this.cantidad = cantidad;
        this.transform.GetChild(4).GetComponent<Text>().text = this.cantidad.ToString();
    }

    public void addCantidad(int cantidad)
    {
        this.cantidad += cantidad;
        this.transform.GetChild(4).GetComponent<Text>().text = this.cantidad.ToString();
    }

    public int getCantidad()
    {
        return cantidad;
    }

    public void setRareza(int rareza)
    {
        this.rareza = rareza;
    }

    public int getRareza()
    {
        return rareza;
    }

    public void setName(string name)
    {
        this.name = name;
        this.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = name;
    }

    public string getName()
    {
        return this.name;
    }

    public void setDescripcion(string descripcion)
    {
        if(tipo == 2 || tipo == 3)
        {
            if (this.estado == "enTienda" && !reclamar && !especial)
            {
                this.descripcion = "? ? ?";
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (itemAtributos[i] != 0)
                    {
                        this.descripcion += atributoNames[i] + ": " + itemAtributos[i].ToString() + "\n";
                    }
                }
                if (this.durabilidad != -1)
                {
                    this.descripcion += "Durabilidad: " + durabilidadRestante.ToString() + "/" + durabilidad.ToString();
                }
                else
                {
                    this.descripcion += "Durabilidad: " + "¡Þ";
                }
            }
            
            
        }
        else
        {
            this.descripcion = descripcion;
        }
        this.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = this.descripcion;
    }
        

    public string getDescripcion()
    {
        return descripcion;
    }

    public void setPrecio(int precio)
    {
        this.precio = precio;
        this.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = precio.ToString();
    }

    public int getPrecio()
    {
        return precio;
    }

    public void setSprite(Sprite sprite)
    {
        this.transform.GetChild(3).GetComponent<Image>().sprite = sprite;
    }

    public int getFuerza()
    {
        return itemAtributos[0];
    }

    public int getAgilidad()
    {
        return itemAtributos[1];
    }

    public int getAguante()
    {
        return itemAtributos[2];
    }

    public int getMovimiento()
    {
        return itemAtributos[3];
    }

    public int getSuerte()
    {
        return itemAtributos[4];
    }

    public void setAtributos(int[] atributos)
    {
        if(atributos != null)
        {
            for (int i = 0; i < atributos.Length; i++)
            {
                this.itemAtributos[i] = atributos[i];
            }
        }
        else
        {
            if(this.tipo == 2 || this.tipo == 3)
            {
                setAtributos();
            }
        }
    }

    public int[] getAtributos()
    {
        return itemAtributos;
    }

    public void setVariable(int variable)
    {
        this.variable = variable;
    }

    public int getVariable()
    {
        return this.variable;
    }

    public void setCantidadRecuperada(int cantidadRecuperada)
    {
        this.cantidadRecuperada = cantidadRecuperada;
    }

    public int getCantidadRecuperada()
    {
        return cantidadRecuperada;
    }

    public void setDurabilidad(int durabilidad)
    {
        if (durabilidad != 0)
        {
            this.durabilidad = durabilidad;
        }
    }

    public int getDurabilidad()
    {
        return durabilidad;
    }

    public void setDurabilidadRestante(int durabilidadRestante)
    {
        if(durabilidadRestante != 0)
        {
            this.durabilidadRestante = durabilidadRestante;
        }
    }

    public int getDurabilidadRestante()
    {
        return durabilidadRestante;
    }

    public void addDurabilidadRestante(int cantidad)
    {
        if(this.durabilidad != -1)
        {
            this.durabilidadRestante += cantidad;
        }
        if(this.durabilidadRestante == 0)
        {
            DestroyImmediate(this.gameObject, true);
        }
    }

    public string getObjetoFuncion()
    {
        return this.objetoFuncion;
    }

    public void setObjetoFuncion(string funcion)
    {
        this.objetoFuncion = funcion;
    }

    public void setEstado(string estado)
    {
        this.estado = estado;
    }

    public string getEstado()
    {
        return estado;
    }

    public int getDescuento()
    {
        return this.descuento;
    }

    public void addDescuento(int descuento)
    {
        this.descuento = descuento;
        this.precio = (precio * (100 - descuento))/100;
        this.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = precio.ToString();
        this.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = Color.green;
    }

    public void setEspecial(bool especial)
    {
        this.especial = especial;
    }

    public void estadoReclamar()
    {
        reclamar = true;
    }

    /***********************************************************************************************************************
     * Inicial el estado del objeto
     ***********************************************************************************************************************/

    public void initObjeto(ItemScriptable itemDato, int cantidad, int[] atributos, int durabilidadRestante)
    {
        setId(itemDato.id);
        setName(itemDato.name);
        setTipo(itemDato.tipo);
        setRareza(itemDato.rareza);
        setPrecio(itemDato.precio);
        setSprite(itemDato.sprite);
        setCantidad(cantidad);
        setDurabilidad(itemDato.durabilidad);
        setDurabilidadRestante(durabilidadRestante);
        setAtributos(atributos);
        setCantidadRecuperada(itemDato.cantidadRecuperada);
        setVariable(itemDato.variable);
        setEspecial(itemDato.especial);
        initObjetoFuncion();
        setDescripcion(itemDato.descripcion);
    }

    public void initObjetoFuncion()
    {
        switch (tipo)
        {
            case 0:
            case 4:
                setObjetoFuncion("usar");
                break;
            case 2:
            case 3:
                setObjetoFuncion("equipar");
                break;
            case 5:
                setObjetoFuncion("vender");
                break;
        }
    }

    public void initEstado()
    {
        switch (this.estado){
            case "enTienda":
                setTienda();
                break;
            case "enInventario":
                setInventario();
                break;
            case "enJuego":
                setJuego();
                break;
            case "enEquipado":
                setEquipado();
                break;

        }
    }

    private void setTienda()
    {
        panelPrecio.SetActive(true);
    }

    private void setInventario()
    {
        panelPrecio.SetActive(false);
    }

    private void setJuego()
    {
        panelPrecio.SetActive(false);
    }

    private void setEquipado()
    {
        panelPrecio.SetActive(false);
    }



    /***********************************************************************************************************************
     * Inicial atributos
     ***********************************************************************************************************************/

    private void setAtributos()
    {
        switch (this.id)
        {
            case 13401:
            case 12408:
                atributoEquilibrio();
                break;
            case 13407:
            case 12404:
            case 12410:
                atributoUnico(0);
                break;
            case 12406:
            case 12411:
                atributoUnico(1);
                break;
            case 13402:
            case 12403:
                atributoUnico(2);
                break;
            case 12405:
                atributoUnico(3);
                break;
            case 13405:
            case 12407:
                atributoUnico(4);
                break;
            case 13403:
                atributoDoble(0, 2);
                break;
            case 13406:
                atributoDoble(3, 4);
                break;
            case 12412:
                atributoDoble(2, 4);
                break;
            case 13408:
                atributoAlto();
                break;
            case 13404:
            case 12402:
            case 12409:
                atributoBajo();
                break;
            default:
                randomAtributo();
                break;
        }
    }

    private void randomAtributo()
    {
        int atributos = totalAtributos[this.rareza];
        int atributo, valor;
        while (atributos != 0)
        {
            atributo = Random.Range(0, 5);
            valor = Random.Range(0, atributos + 1);
            itemAtributos[atributo] += valor;
            atributos -= valor;
        }
    }

    private void atributoEquilibrio()
    {
        for (int i = 0; i < itemAtributos.Length; i++)
        {
            itemAtributos[i] = 5;
        }
    }

    private void atributoUnico(int atributo)
    {
        itemAtributos[atributo] = 18;
    }

    private void atributoDoble(int atr1, int atr2)
    {
        itemAtributos[atr1] = 12;
        itemAtributos[atr2] = 12;
    }

    private void atributoAlto()
    {
        int atributos = 30;
        int atributo, valor;
        while (atributos != 0)
        {
            atributo = Random.Range(0, 5);
            valor = Random.Range(0, atributos + 1);
            itemAtributos[atributo] += valor;
            atributos -= valor;
        }
    }

    private void atributoBajo()
    {
        int atributos = 12;
        int atributo, valor;
        while (atributos != 0)
        {
            atributo = Random.Range(0, 5);
            valor = Random.Range(0, atributos + 1);
            itemAtributos[atributo] += valor;
            atributos -= valor;
        }
    }

    /***********************************************************************************************************************
     * Funcionalidades de apoyo
     ***********************************************************************************************************************/

    public void comprarProducto()
    {
        if(estado == "enTienda")
        {
            shop.GetComponent<ShopController>().comprarObjeto(this.gameObject);
        }
    }

    public void onClickUsar()
    {
        if (estado == "enInventario")
        {
            inventario.GetComponent<InventoryController>().usar(this.gameObject);
        }
    }

    /***********************************************************************************************************************
     * Control del panel de detalle
     ***********************************************************************************************************************/

    public void OnMouseOver()
    {
        panelDescripcion.SetActive(true);
    }

    public void OnMouseExit()
    {
        panelDescripcion.SetActive(false);
    }

}
