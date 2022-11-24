using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Mapa _map;
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

    public bool moverInv =true;

    public MovSlot[] mov;

    public GameObject panelItem;

    public Image[] iconosVacios;

    public bool canPickUp;

    
    private void Awake()
    {
        instance = this;

    }
    // Start is called before the first frame update

    public void UseObject()
    {
        //if (_map.Jugador.Interactuar.WasPressedThisFrame())
        //{
            
            for (int i = 0; i < allSlots; i++)
            {
                if (/*(mov[i].rtSlot.anchoredPosition.x == -15)*/ /*&&*/ !slot[i].GetComponent<Slot>().empty)
                {

                    slot[i].GetComponent<Slot>().Quitar();
                   


                    return;
                }
            }


            //for (int i = 0; i < mov.Length; i++)
            //{
            //    if (/*(mov[i].rtSlot.anchoredPosition.x == -15)*/ /*&&*/ !mov[i].vacio)
            //    {


            //mov[1].gameObject.GetComponent<Slot>().Quitar();

            //return;
            //    }
            //}

        //}



    }
    public void SetInputActions(Mapa map)
    {
        _map = map;

    }
    public void AddItem(GameObject itemObject, int itemID, string itemType, string itemDescription, Sprite itemIcon)
    {


        for (int i = 0; i < allSlots; i++)
        {
            if (slot[i].GetComponent<Slot>().empty)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.selectGood);

                itemObject.GetComponent<Item>().pickeUp = true;
                slot[i].GetComponent<Slot>().item = itemObject;
                slot[i].GetComponent<Slot>().ID = itemID;
                slot[i].GetComponent<Slot>().type = itemType;
                slot[i].GetComponent<Slot>().description = itemDescription;
                

                itemObject.transform.parent = slot[i].transform;


                //slot[i].GetComponent<Slot>().UpdateSlot();


                slot[i].GetComponent<Slot>().empty = false;

                slot[i].transform.Find("Panel").transform.Find("Image").GetComponent<Image>().sprite = itemIcon;
               
                itemObject.SetActive(false);

                slot[i].gameObject.GetComponent<MovSlot>().vacio = false;

                Item[] objetosTodo = FindObjectsOfType<Item>();

                FindObjectOfType<NPC_Rana>().finalA = true;

                slot[i].GetComponent<Slot>().Panel.color = new Color(
                slot[i].GetComponent<Slot>().Panel.color.r,
                slot[i].GetComponent<Slot>().Panel.color.g,
                slot[i].GetComponent<Slot>().Panel.color.b, 
                1.0f);



                foreach (Item objeto in objetosTodo)
                {
                    objeto.gameObject.SetActive(false);
                }

                slot[0].GetComponent<Slot>().empty = true;
                slot[2].GetComponent<Slot>().empty = true;

                

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

        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotFolder.transform.GetChild(i).gameObject;

            if (slot[i].GetComponent<Slot>().item == null)
            {
                slot[i].GetComponent<Slot>().empty = true;
            }

        }

        slot[0].GetComponent<Slot>().empty = false;
        slot[2].GetComponent<Slot>().empty = false;

        iconosVacios[0].color = new Color(iconosVacios[1].color.r, iconosVacios[1].color.g, iconosVacios[1].color.b, 0.0f);
        iconosVacios[1].color = new Color(iconosVacios[1].color.r, iconosVacios[1].color.g, iconosVacios[1].color.b, 0.0f);
        iconosVacios[2].color = new Color(iconosVacios[1].color.r, iconosVacios[1].color.g, iconosVacios[1].color.b, 0.0f);




        id_selectorM = 1;

        //for (int i = 0; i < pos.Length; i++)
        //{
        //    pos[0] = rtSlot[0].position;
        //}

        _map = new Mapa();
        
        _map.Jugador.Enable();
    }


    void Update()
    {
        //if (panelItem.activeInHierarchy)
        //{
        //    //if (Input.GetButtonDown("ArribaInventario") && moverInv && !Input.GetButtonDown("AbajoInventario"))
        //    //{

        //    //    for (int i = 0; i < mov.Length; i++)
        //    //    {
        //    //        mov[i].StartCoroutine(mov[i].PosicionarElementos(false));
        //    //    }
        //    //}

        //    //if (Input.GetButtonDown("AbajoInventario") && moverInv && !Input.GetButtonDown("ArribaInventario"))
        //    //{

        //    //    for (int i = 0; i < mov.Length; i++)
        //    //    {
        //    //        mov[i].StartCoroutine(mov[i].PosicionarElementos(true));
        //    //    }
        //    //}

        //    //if (mov[0].quieto && mov[1].quieto && mov[2].quieto)
        //    //{
        //    //    moverInv = true;
        //    //}
        //    //else
        //    //{
        //    //    moverInv = false;


        //    //}


        //    //if (!canPickUp)
        //    //{
        //    //    UseObject();
        //    //}
        //}
     



    }

    void LateUpdate()
    {
        //invAnim.
        
    }
}
    
  

