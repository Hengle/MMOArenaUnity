﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSpellcasting : MonoBehaviour {

	//Spell prefabs
	public GameObject[] prefabs;
	
	//List with spell data from server
	public List<SpellPossession> spellPossession;
	
	// setting of current spellCasting
	public int currentSpellNum = 1;
	
	public float spellHeight = 1.4f;
	public float currentCooldown = 0.0f;
	
	CharacterControlInterface controlInterface;

	// Use this for initialization
	void Start () {
		controlInterface = GetComponent<CharacterControlInterface>();
		LoadSpellPrefabs();
	}
	
	// Update is called once per frame
	void Update () {
		if( spellPossession == null ) {
			return;
		}
		
		if( currentCooldown > 0.0f ) {
			currentCooldown -= Time.deltaTime * 1000;
		}
		
		if( controlInterface.isPunch ) {
			castCurrentSpell();
		} else if( controlInterface.previousSpell ) {
			PreviousSpell();
		} else if( controlInterface.nextSpell ) {
			NextSpell();
		}
	}

	void castCurrentSpell() {
		if( currentCooldown <= 0.0f ) {
			MouseController mouse = GetComponent<MouseController>();
			if( mouse ) mouse.AlignRotation();
			
			Spell cntSpell = spellPossession[currentSpellNum].spell;
			GameObject cntPrefab = prefabs[cntSpell.prefabType];
			
			Vector3 instantiatePos = (transform.position + new Vector3(0.0f, spellHeight, 0.0f)) + transform.forward;
			Instantiate(cntPrefab, instantiatePos, transform.rotation);
			
			currentCooldown = cntSpell.cooldownTime;
		}
	}
	
	void PreviousSpell ()
	{
		if( currentSpellNum <= 0 ) {
			currentSpellNum = spellPossession.Count - 1;
		} else {
			currentSpellNum--;
		}
	}
	
	void NextSpell ()
	{
		if( currentSpellNum >= spellPossession.Count - 1) {
			currentSpellNum = 0;
		} else {
			currentSpellNum++;
		}
	}
	
	public void LoadSpells(Character characterData) {
		spellPossession = characterData.Spells;
	}

	private void LoadSpellPrefabs() {
		RetreiveDataScript script = FindObjectOfType(typeof( RetreiveDataScript )) as RetreiveDataScript;
		prefabs = script.prefabs;
	}
}