using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuevoPez : MonoBehaviour
{
    // Start is called before the first frame update
    public float numeros;
    Animator obAmin;
	private void Awake()
	{
        obAmin = GetComponent<Animator>();
	}
	void Start()
    {
        obAmin.SetFloat("Velocimetro", numeros);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
