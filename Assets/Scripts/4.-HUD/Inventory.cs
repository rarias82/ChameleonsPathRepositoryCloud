using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] slot;
    public GameObject inventory,selector;
    public bool inventoryEnabled;
    public int allSlots, enableSlots;
    public GameObject slotFolder;
    public sbyte id_selectorM;

    public static Inventory instance;
    public float scl;
    public Animator invAnim;

    

    public Vector2[] pos;

    public Vector2 diferenciaVector;

    public float speedInventory;

    public bool moverInv;

    public MovSlot[] mov;

    public GameObject panelItem;

    public bool canPickUp;
    private void Awake()
    {
        instance = this;

    }
    // Start is called before the first frame update

    public void UseObject()
    {
        //if (Input.GetButtonDown("Recoger"))
        //{

        //    for (int i = 0; i < mov.Length; i++)
        //    {
        //        if ((mov[i].rtSlot.anchoredPosition.x == -15) && !mov[i].vacio)
        //        {

        //            if (true)
        //            {

        //            }
        //            mov[i].gameObject.GetComponent<Slot>().Quitar();

        //            return;
        //        }
        //    }

        //}



    }
    public void AddItem(GameObject itemObject, int itemID, string itemType, string itemDescription, Sprite itemIcon)
    {


        for (int i = 0; i < allSlots; i++)
        {
            if (slot[i].GetComponent<Slot>().empty)
            {
                itemObject.GetComponent<Item>().pickeUp = true;
                slot[i].GetComponent<Slot>().item = itemObject;
                slot[i].GetComponent<Slot>().ID = itemID;
                slot[i].GetComponent<Slot>().type = itemType;
                slot[i].GetComponent<Slot>().description = itemDescription;
                slot[i].GetComponent<Slot>().icon = itemIcon;

                itemObject.transform.parent = slot[i].transform;



                slot[i].GetComponent<Slot>().UpdateSlot();


                slot[i].GetComponent<Slot>().empty = false;
     
                itemObject.SetActive(false);

                slot[i].gameObject.GetComponent<MovSlot>().vacio = false;

               
               break;
            }

        }


        StartCoroutine(LiberarUso());

       



    }

    IEnumerator LiberarUso()
    {
        yield return null;
        Inventory.instance.canPickUp = false;
    }
    void Start()
    {
        slotFolder = GameObject.Find("SlotHandler");
        allSlots = slotFolder.transform.childCount;

        //slot = new GameObject[allSlots];

        //for (int i = 0; i < allSlots; i++)
        //{
        //    slot[i] = slotFolder.transform.GetChild(i).gameObject;

        //    if (slot[i].GetComponent<Slot>().item == null)
        //    {
        //        slot[i].GetComponent<Slot>().empty = true;
        //    }

        //}

      

        id_selectorM = 1;

        //for (int i = 0; i < pos.Length; i++)
        //{
        //    pos[0] = rtSlot[0].position;
        //}
    }


    void Update()
    {
        if (panelItem.activeInHierarchy)
        {
            //if (Input.GetButtonDown("ArribaInventario") && moverInv && !Input.GetButtonDown("AbajoInventario"))
            //{

            //    for (int i = 0; i < mov.Length; i++)
            //    {
            //        mov[i].StartCoroutine(mov[i].PosicionarElementos(false));
            //    }
            //}

            //if (Input.GetButtonDown("AbajoInventario") && moverInv && !Input.GetButtonDown("ArribaInventario"))
            //{

            //    for (int i = 0; i < mov.Length; i++)
            //    {
            //        mov[i].StartCoroutine(mov[i].PosicionarElementos(true));
            //    }
            //}

            if (mov[0].quieto && mov[1].quieto && mov[2].quieto)
            {
                moverInv = true;
            }
            else
            {
                moverInv = false;


            }


            if (!canPickUp)
            {
                UseObject();
            }
        }
     



    }

    void LateUpdate()
    {
        //invAnim.
        
    }
}
    
  

