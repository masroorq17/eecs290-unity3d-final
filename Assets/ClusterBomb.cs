﻿using UnityEngine;
using System.Collections;

public class ClusterBomb : Photon.MonoBehaviour {
	
	private bool active; //Can hurt tanks when active
	public GameObject bullet;
	public int numberOfBullets;
	public int teamNumber;
	
	// Use this for initialization
	void Start () {
		active = true;
		gameObject.GetComponent<TrailRenderer>().material.SetColor("_Color", Color.yellow);
		teamNumber = gameObject.GetComponent<PlayerControler>().teamNumber;
	}
	
	/*
	 * Creates the clustering effect of the cluster bomb on ground contact
	 */
	void OnCollisionEnter(Collision collision) {
		Vector3 dir;
		if (collision.collider.tag == "Ground") {
			dir = new Vector3 (0, 1, 0);
		} else {
			ContactPoint contact = collision.contacts[0];
			dir = contact.normal.normalized;
		}
		for (int shot = 0; shot < numberOfBullets; shot++) {
			Vector3 randdir = new Vector3(dir.x + Random.Range(-100, 100)/1000f, dir.y + Random.Range(-100, 100)/1000f, dir.z + Random.Range(-100, 100)/1000f).normalized;
			GameObject b = PhotonNetwork.Instantiate ("RockProjectile", gameObject.transform.position + randdir*0.5f, Quaternion.identity, teamNumber) as GameObject;
			b.rigidbody.AddForce (randdir * 200);
		}
		PhotonNetwork.Destroy(this.photonView);
	}
}
