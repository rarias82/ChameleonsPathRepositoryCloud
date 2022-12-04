using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string type;
    public string description;
    public Sprite icon;

    public bool pickeUp;
    public bool equiped;

    
    public GameObject marker;
    public bool isRange;
    

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


    
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("P1"))
        {
            marker.SetActive(true);
            isRange = !isRange;

            AudioManager.Instance.PlaySoundBien();


            Inventory.instance.canPickUp = true;

            Inventory.instance.AddItem(gameObject, ID, type, description, icon);
            


        }
    
    
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("P1"))
    //    {
    //        marker.SetActive(false);
    //        isRange = !isRange;

    //        Inventory.instance.canPickUp = false;
    //    }


    //}

    
                 
}
