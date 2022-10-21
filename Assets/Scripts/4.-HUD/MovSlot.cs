using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovSlot : MonoBehaviour
{
    public RectTransform rtSlot;
    public int index;
    public int lugar;
    public bool quieto;
    public bool vacio;
    public IEnumerator PosicionarElementos(bool suma)
    {
        quieto = false;

        if (suma)
        {
            if (rtSlot.anchoredPosition.x == -140)
            {
                lugar = 0;
                index = 1;
            }

            if (rtSlot.anchoredPosition.x == -15)
            {
                lugar = 1;
                index = 2;
            }

            if (rtSlot.anchoredPosition.x == 110)
            {
                lugar = 2;
                index = 0;
            }

        }
        else
        {
            if (rtSlot.anchoredPosition.x == -140)
            {
                lugar = 0;
                index = 2;
            }

            if (rtSlot.anchoredPosition.x == -15)
            {
                lugar = 1;
                index = 0;
            }

            if (rtSlot.anchoredPosition.x == 110)
            {
                lugar = 2;
                index = 1;
            }
        }

        while ((rtSlot.anchoredPosition.x != Inventory.instance.pos[index].x))
            {
                if (suma)
                {
                    if (index == 0)
                    {
                        rtSlot.anchoredPosition = Vector2.MoveTowards(rtSlot.anchoredPosition, Inventory.instance.pos[index], (Inventory.instance.speedInventory * 2f) * Time.deltaTime);
                    }
                    else
                    {
                        rtSlot.anchoredPosition = Vector2.MoveTowards(rtSlot.anchoredPosition, Inventory.instance.pos[index], (Inventory.instance.speedInventory) * Time.deltaTime);
                    }
                }
                else
                {
                    if (index == 2)
                    {
                        rtSlot.anchoredPosition = Vector2.MoveTowards(rtSlot.anchoredPosition, Inventory.instance.pos[index], (Inventory.instance.speedInventory * 2f) * Time.deltaTime);
                    }
                    else
                    {
                        rtSlot.anchoredPosition = Vector2.MoveTowards(rtSlot.anchoredPosition, Inventory.instance.pos[index], (Inventory.instance.speedInventory) * Time.deltaTime);
                    }
                }

                if (index == 1)
                {
                    rtSlot.sizeDelta = Vector2.MoveTowards(rtSlot.sizeDelta, new Vector2(135f, 135f), Inventory.instance.speedInventory * Time.deltaTime);
                }
                else
                {
                    rtSlot.sizeDelta = Vector2.MoveTowards(rtSlot.sizeDelta, new Vector2(73.3334f, 63.9167f), (Inventory.instance.speedInventory) * Time.deltaTime);
                }

                yield return null;

            }
        
       
     

       

        quieto = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        quieto = true;
        vacio = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
