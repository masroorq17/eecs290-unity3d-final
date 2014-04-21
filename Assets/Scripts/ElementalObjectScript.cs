﻿using UnityEngine;
using System.Collections;

public enum Element {Rock, Paper, Scissors};

public class ElementalObjectScript : MonoBehaviour {
	
	public int Health = 100;
	public float attackSpeed = 1.0f;
	public float defense = 1.0f;
	public float moveSpeed = 1.0f;
	public Element thisType;
	
	//Decreases the health of the object
	public void Hurt(int amount){
		int damageDealt = Mathf.FloorToInt(amount / defense);
		Debug.Log("Damage dealt: "+damageDealt);
		if (Health - damageDealt > 0) {
			Health -= damageDealt;
		} else {
			if(transform.tag == "Player"){
				Destroy(this.gameObject);
			} else {
				Debug.Log("Death flag.");
				transform.parent.GetComponent<TowerScript>().Death();
			}
		}
	}
	
	public void decreaseAttackSpeed(float amount) {
		this.attackSpeed -= amount;
	}
	
	public void resetAttackSpeed() {
		this.attackSpeed = 1.0f;
	}
	
	public void decreaseDefense(float amount) {
		this.defense -= amount;
	}
	
	public void resetDefense() {
		this.defense = 1.0f;
	}
	
	public void changeMoveSpeed(float amount) {
		this.moveSpeed += amount;
		CharacterMotor script = this.gameObject.GetComponent<CharacterMotor>();
		script.movement.maxForwardSpeed = script.movement.maxForwardSpeed*moveSpeed;
		script.movement.maxSidewaysSpeed = script.movement.maxSidewaysSpeed*moveSpeed;
		script.movement.maxBackwardsSpeed = script.movement.maxBackwardsSpeed*moveSpeed;
	}
	
	public void resetMoveSpeed() {
		CharacterMotor script = this.gameObject.GetComponent<CharacterMotor>();
		script.movement.maxForwardSpeed = script.movement.maxForwardSpeed/moveSpeed;
		script.movement.maxSidewaysSpeed = script.movement.maxSidewaysSpeed/moveSpeed;
		script.movement.maxBackwardsSpeed = script.movement.maxBackwardsSpeed/moveSpeed;
		this.moveSpeed = 1.0f;
	}
	
	public Element getElementalType(){
		return thisType;
	}
}
