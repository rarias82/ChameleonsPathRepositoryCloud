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

    public Image Panel;

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
        
        gameObject.GetComponent<MovSlot>().vacio = true;
        Panel.sprite = null;
        Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, 0.0f);

        Debug.Log("Quitar");
    }

}
