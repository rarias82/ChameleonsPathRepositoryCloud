using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject item;
    public int ID;
    public string type;
    public string description;
    public bool empty;
    public Sprite icon;

    public Transform slotIcon;

    public void UpdateSlot()
    {
        slotIcon.GetComponent<Image>().sprite = icon;
        slotIcon.GetComponent<Image>().preserveAspect = true;
    }

    public void UseItem(string nombre)
    {

        switch (nombre)
        {
            case "Mapa":
                    break;

            case "Llave":
                    break;

            case "Otro":
                    break;
            default:
                break;
        }

    }
    
    void Start()
    {
        slotIcon = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {

    }


  
    public void Quitar()
    {
        item = null;
        ID = 0;
        type = null;
        description = null;
        empty = true;
        icon = null;
        slotIcon.GetComponent<Image>().sprite = null;
        //this.GetComponent<Image>().sprite = null;
        gameObject.GetComponent<MovSlot>().vacio = true;
    }

}
