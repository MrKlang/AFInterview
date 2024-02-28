using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AFSInterview
{
    public class Unit : MonoBehaviour
    {
        public int id;
        
        [SerializeField] private int currentHp;
        [SerializeField] private Animator unitAnimator;
        [SerializeField] private string animatorTrigger;

        private UnitData unitData;
        private int unitAmount;

        public void Setup(UnitData data, int amount)
        {
            unitData = data;
            unitAmount = amount;
        }

        public void PlayAttackAnimation()
        {
            if (unitAnimator != null)
            {
                unitAnimator.SetTrigger(animatorTrigger);
            }
        }

        public void AttackEnemy(Unit unit)
        {
            var currentDamage = unitData.damageDealtPerAttack;

            for(int i = 0; i < unit.unitData.unitAttributes.Count; i++)
            {
                if (unitData.unitDamageOverrides.Exists(e => e.unitAttribute.Equals(unit.unitData.unitAttributes[i])))
                {
                    currentDamage += unitData.unitDamageOverrides.First(e => e.unitAttribute.Equals(unit.unitData.unitAttributes[i])).additionalDamageToAttribute;
                }
            }

            unit.TakeDamage(currentDamage);
        }

        public void TakeDamage(int incomingDamage)
        {
            if(currentHp > 0)
            {
                currentHp -= Mathf.Max(1, incomingDamage - unitData.armour);
            }

            if(currentHp <= 0)
            {
                unitAmount--;
#if UNITY_EDITOR
                Debug.Log($"1 Unit of {unitData.unitType} got killed, {unitAmount} remaining");

#endif
                if (unitAmount <= 0)
                {
#if UNITY_EDITOR
                    Debug.Log($"Unit {unitData.unitType} wiped.");

#endif
                    if (gameObject != null)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    currentHp = unitData.maxHp;
                }
            }
        }
    }
}
