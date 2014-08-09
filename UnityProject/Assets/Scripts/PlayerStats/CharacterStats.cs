﻿using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour {

	public string name;
	public int level;
	public int exp;

	public int health, maxhealth;

	public delegate void DeathDelegate();
	public DeathDelegate deathDelegate;
	public bool isDeath;
	
	// Use this for initialization
	void Start () {
		isDeath = false;
		health = maxhealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadFromData(Character characterData) {
		name = characterData.Name;
		level = characterData.Level;
		exp = characterData.Exp;
		health = maxhealth = characterData.MaxHealth;
	}

	public void decreaseHealth(int amount) {
		health -= amount;
		
		if( health <= 0 ) {
			doDead();
			health = 0;
		}
	}
	
	void doDead() {
		isDeath = true;
		deathDelegate();
	}
}