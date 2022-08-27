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

    public Vector3 offset,rot_;
    public GameObject marker;
    public bool isRange;

    public float speedRot;
    // Start is called before the first frame update

    void RotateSon()
    {
        Quaternion rotation = Quaternion.Euler(offset);
        marker.transform.rotation = rotation;
    }
    void Start()
    {
        marker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Recoger") && isRange && Inventory.instance.canPickUp)
        {
          
            Inventory.instance.AddItem(gameObject, ID, type, description, icon);
            Debug.Log("Recoger"+gameObject.name);

        }

        //transform.Rotate(rot_ * speedRot * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("P1"))
        {
            marker.SetActive(true);
            isRange = !isRange;


            Inventory.instance.canPickUp = true;

            RotateSon();
          

        }
    
    
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("P1"))
        {
            marker.SetActive(false);
            isRange = !isRange;

            Inventory.instance.canPickUp = false;
        }


    }

    
                 
}
