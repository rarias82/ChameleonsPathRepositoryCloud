using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class InicioEfectoBloom : MonoBehaviour
{
    [SerializeField] Volume m_Volume;
    [SerializeField] Bloom m_Bloom;
    [SerializeField] float silderMaxValue;
    [SerializeField] float sildermMinValue;
    [SerializeField] float valueRate;

    private void Awake()
    {
        m_Volume = GetComponent<Volume>();
        m_Volume.profile.TryGet(out m_Bloom);
    }
    void Start()
    {
        // Create an instance of a bloom
       

        StartCoroutine(BloomIntensity());
       
    }
    
    IEnumerator BloomIntensity()
    {

        m_Bloom.intensity.value = silderMaxValue;
        float num = 10.0f;


        while (num >  0.0f && m_Bloom.intensity.value > sildermMinValue)
        {
            m_Bloom.intensity.value -= valueRate * Time.deltaTime;
            num -=  Time.deltaTime;
            yield return null;
        }

        Debug.Log("Salio del bucle");
        m_Bloom.intensity.value = sildermMinValue;

    }


       
    }
