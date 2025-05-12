using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    
	public SimuladorFisica fisica;
	
	public float timeToDestroy = 5f;
	
	
	public SimuladorFisica getFisica() {
		
		return fisica;
	
	}
	
    void Start()
    {
		fisica = new SimuladorFisica( transform );
		
        //Invoke("Kill", timeToDestroy);
        StartCoroutine(KillAfterTime());
        //Destroy(gameObject, timeToDestroy);
    }
	
	void Update() {
		transform.Rotate(new Vector3(0,0,1f) * Time.deltaTime * 90f);
	}
	
    IEnumerator KillAfterTime()
    {
        yield return new WaitForSeconds(timeToDestroy);
		Kill();
    }

    void Kill()
    {
        Destroy(gameObject);
    }
	
	
	
    private void OnTriggerEnter2D(Collider2D collision) { 

        if( collision.CompareTag("Player") ) {
            Destroy(gameObject);
        }
		
    }
}
