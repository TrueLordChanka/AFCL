using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 

    
    public class ShatterableArmour : MonoBehaviour
    {

        public MatDef DefaultMatDef;
        public MatDef BrokenMatDef;
        public float Durability;
        public AudioEvent ShatterAudio;
        public bool DestroyOnShatter;
        public System.Collections.Generic.List<ShatterableArmourPiece> ArmourParts;
        public GameObject ShatterEffect;

        [Header("Evo Armour")]
        public bool IsEvoArmour;
        public int[] EvoKillReqs;
        public float[] EvoArmourLvlDurability;
        public GameObject[] EvoArmorMeshArray;
        public GameObject[] EvoArmorUpgradeEffects;
        private int _currentEvoLvl = 0;
        private int _maxEvoLvl;
        public float EvoReqMultiplier = 0.5f;

        [HideInInspector]
        public float _damageTaken;
        private bool PlayedAudio = false;
        private int _numKills;

#if !(UNITY_EDITOR || UNITY_5)

        public void Start()
        {
            _maxEvoLvl = EvoKillReqs.Length;
            GM.CurrentSceneSettings.SosigKillEvent += SosigKillEvent;
            foreach (var ShatterableArmourPiece in ArmourParts)
            {
                ShatterableArmourPiece.Initialize(DefaultMatDef);
            }
        }

        private void SosigKillEvent(Sosig s) //Is called when a sosig is killed, wil be used for evo armour.
        {
            _numKills++;
        }

        public void Update()
        {

            if(_damageTaken >= Durability && !PlayedAudio) //for damaging armour
            {
                if(ShatterAudio != null)
                {
                    SM.PlayGenericSound(ShatterAudio, this.transform.position);
                }
                PlayedAudio = true;
                if (!DestroyOnShatter)
                {
                    foreach (var ShatterableArmourPiece in ArmourParts)
                    {
                        ShatterableArmourPiece.Shatter(BrokenMatDef);
                    }
                    if (ShatterEffect != null)
                    {
                        GameObject.Instantiate(ShatterEffect, transform.position, transform.rotation);
                    }
                }
                else 
                {
                    if (ShatterEffect != null)
                    {
                        GameObject.Instantiate(ShatterEffect, transform.position, transform.rotation);
                    }
                    DestroyObject(gameObject);
                }
            }
            if (IsEvoArmour) //This dont work quite right atm....
            {
                if (_numKills >= EvoKillReqs[_currentEvoLvl])
                {
                    int _nextEvoLvl;
                    if (_currentEvoLvl + 1 <= _maxEvoLvl)
                    {
                        _nextEvoLvl = _currentEvoLvl + 1;
                    }
                    else
                    {
                        _nextEvoLvl = _maxEvoLvl;
                    }

                    if (_damageTaken >= EvoArmourLvlDurability[_nextEvoLvl] * EvoReqMultiplier)
                    //This heals the armour if its damage taken is more than the next damage times the multiplier
                    {
                        _damageTaken -= EvoArmourLvlDurability[_nextEvoLvl] * EvoReqMultiplier;
                        _numKills = 0;
                    }
                    else
                    //This actually Evos the armour since the damage taken is not more than the next 
                    //durability * the set multiplier.
                    {
                        _numKills = 0;
                        EvoArmorMeshArray[_currentEvoLvl].SetActive(false); //disable all the old meshes + effects
                        EvoArmorUpgradeEffects[_currentEvoLvl].SetActive(false);
                        if (_currentEvoLvl != _maxEvoLvl) _currentEvoLvl++;
                        EvoArmorMeshArray[_currentEvoLvl].SetActive(true); //Enable all the new meshes + effects
                        EvoArmorUpgradeEffects[_currentEvoLvl].SetActive(true);
                    }
                }
            }

        }

        public void FixedUpdate()
        {
            Debug.Log("Current damage taken is at:" + _damageTaken);
        }

        public void Evo()
        {

        }

#endif
    }
}



