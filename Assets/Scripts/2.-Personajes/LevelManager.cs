using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool misionCompletadaHermnanos;
    public bool misionCompletadaRana;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	public void TransicionNivel2()
	{
		if (misionCompletadaRana && misionCompletadaHermnanos)
		{
            MainCharacter.sharedInstance._map.Disable();
            FollowCameras.instance.trDuende.gameObject.SetActive(true);
            FollowCameras.instance.mode = Modo.Enano;
		}
	}
}
