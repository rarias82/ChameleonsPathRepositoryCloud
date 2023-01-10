using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salpicar : MonoBehaviour
{
    ParticleSystem particulas;
    
    // Start is called before the first frame update
    void Start()
    {
        particulas = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SalpicarOn()
    {
        particulas.Play();
    }
}
