using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimuladorFisica
{
    	
	public Transform transform;
	
	public Vector3 aceleracao = new Vector3( 0, 0, 0 );
	public Vector3 velocidade = new Vector3( 0, 0, 0 );
	
	public float massa = 1.0f;
	
	public List<Vector3> forcas = new List<Vector3>();
	
	
	public SimuladorFisica( Transform gameObjectTransform ) {
		
		this.transform = gameObjectTransform;
		
	}
	
	/* */ 
	
	public void addForca( Vector3 v ) {
		
		forcas.Add( v );
		
	}
	
	public void addForcaElastica( float k ) {
		
		addForca( -k * transform.position );
		
	}
	
	public void addForcaElastica( float k, Vector3 ancoragem ) {
		
		addForca( -k * (transform.position - ancoragem) );
		
	}
	
	public void addForcaElastica( float k, float b ) {
		
		addForca( -k * transform.position - velocidade * b );
		
	}
	
	public void addForcaElastica( float k, float b, Vector3 ancoragem ) {
		
		addForca( -k * (transform.position - ancoragem) - velocidade * b );
		
	}
	
	/* */ 
	
	public void addAceleracao( Vector3 v ) {
		
		aceleracao += v;
		
	}
	
	/* */ 
	
	public void atualizar() {
		
		atualizarAceleracao();
		atualizarVelocidade();
		atualizarPosicao();
		
		/// limpa as forças que atuaram nesse frame
		forcas.Clear();
		
	}
	
	public void atualizarAceleracao() {
		
		Vector3 resultante = new Vector3(0,0,0);
		
		foreach( Vector3 f in forcas ) 
			resultante += f;
		
		aceleracao = resultante / massa;
		
	}
	
	public void atualizarVelocidade() {
		
		velocidade += aceleracao * Time.deltaTime;
		
	}
	
	public void atualizarPosicao() {
		
		transform.position += velocidade * Time.deltaTime;
		
	}
	
}
