using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterController : MonoBehaviour
{
    //Datos
    private ShopController shop;
    private GameObject[] scrollsContent;
    private GameObject panel;
    private bool[] estados = {true, true, true, true, true};

    // Start is called before the first frame update
    void Start()
    {
        shop = GameObject.Find("ShopController").GetComponent<ShopController>();
        scrollsContent = shop.getScrollContent();
        panel = gameObject.transform.Find("Panel").gameObject;

        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /***********************************************************************************************************************
     * Filtro
     ***********************************************************************************************************************/

    public void filterComun()
    {
        estados[0] = !estados[0];
        for(int i = 0; i < scrollsContent.Length; i++)
        {
            for(int j = 0; j < scrollsContent[i].transform.childCount; j++)
            {
                if(scrollsContent[i].transform.GetChild(j).GetComponent<itemController>().getRareza() == 0)
                {
                    scrollsContent[i].transform.GetChild(j).gameObject.SetActive(estados[0]);
                }
            }
        }
    }

    public void filterRaro()
    {
        estados[1] = !estados[1];
        for (int i = 0; i < scrollsContent.Length; i++)
        {
            for (int j = 0; j < scrollsContent[i].transform.childCount; j++)
            {
                if (scrollsContent[i].transform.GetChild(j).GetComponent<itemController>().getRareza() == 1)
                {
                    scrollsContent[i].transform.GetChild(j).gameObject.SetActive(estados[1]);
                }
            }
        }
    }

    public void filterExotico()
    {
        estados[2] = !estados[2];
        for (int i = 0; i < scrollsContent.Length; i++)
        {
            for (int j = 0; j < scrollsContent[i].transform.childCount; j++)
            {
                if (scrollsContent[i].transform.GetChild(j).GetComponent<itemController>().getRareza() == 2)
                {
                    scrollsContent[i].transform.GetChild(j).gameObject.SetActive(estados[2]);
                }
            }
        }
    }

    public void filterEpico()
    {
        estados[3] = !estados[3];
        for (int i = 0; i < scrollsContent.Length; i++)
        {
            for (int j = 0; j < scrollsContent[i].transform.childCount; j++)
            {
                if (scrollsContent[i].transform.GetChild(j).GetComponent<itemController>().getRareza() == 3)
                {
                    scrollsContent[i].transform.GetChild(j).gameObject.SetActive(estados[3]);
                }
            }
        }
    }

    public void filterLegendario()
    {
        estados[4] = !estados[4];
        for (int i = 0; i < scrollsContent.Length; i++)
        {
            for (int j = 0; j < scrollsContent[i].transform.childCount; j++)
            {
                if (scrollsContent[i].transform.GetChild(j).GetComponent<itemController>().getRareza() == 4)
                {
                    scrollsContent[i].transform.GetChild(j).gameObject.SetActive(estados[4]);
                }
            }
        }
    }

    /***********************************************************************************************************************
     * Control de panel
     ***********************************************************************************************************************/

    public void openFilter()
    {
        panel.SetActive(true);
    }

    public void closeFilter()
    {
        panel.SetActive(false);
    }
}
