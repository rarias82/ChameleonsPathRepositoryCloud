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
    void Start()
    {
        // Create an instance of a bloom
        m_Volume = GetComponent<Volume>();
        m_Volume.profile.TryGet(out m_Bloom);

        StartCoroutine(BloomIntensity());
       
    }
    
    IEnumerator BloomIntensity()
    {

        m_Bloom.intensity.value = silderMaxValue;

        while (m_Bloom.intensity.value != sildermMinValue)
        {
            m_Bloom.intensity.value -= valueRate * Time.deltaTime;
            yield return null;
        }

        m_Bloom.intensity.value = sildermMinValue;

    }


       
    }
