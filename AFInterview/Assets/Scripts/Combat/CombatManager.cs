using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AFSInterview
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private UnitDataLibrary unitDataLibrary;
        [SerializeField] private List<ArmyUnit> whitePlayerArmy;
        [SerializeField] private List<ArmyUnit> blackPlayerArmy;
        [SerializeField] private List<Transform> whiteArmySlots;
        [SerializeField] private List<Transform> blackArmySlots;
        [SerializeField] private int unitTurnIntervalInSeconds;

        private List<Unit> whiteUnits = new List<Unit>();
        private List<Unit> blackUnits = new List<Unit>();

        private bool clashStarted;
        private WaitForSeconds automaticTurnInterval;

        private void Start()
        {
            automaticTurnInterval = new WaitForSeconds(unitTurnIntervalInSeconds);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                SetupCombat();
        }

        private void SetupCombat()
        {
            if (clashStarted)
            {
                return;
            }

            SpawnArmy(ref whitePlayerArmy, ref whiteArmySlots, ref whiteUnits);
            SpawnArmy(ref blackPlayerArmy, ref blackArmySlots, ref blackUnits);
            SetTurnsOrder();
        }

        private void SpawnArmy(ref List<ArmyUnit> playerArmy, ref List<Transform> slots, ref List<Unit> listToFill)
        {
            for(int i = 0; i < slots.Count; i++)
            {
                Unit unitToSpawn = Instantiate(playerArmy[i].unitPrefab, slots[i]).GetComponent<Unit>();
                unitToSpawn.Setup(unitDataLibrary.GetItemUsageData(unitToSpawn.id), playerArmy[i].amount);
                listToFill.Add(unitToSpawn);
            }
        }

        private void SetTurnsOrder() //Shuffling with Fisher-Yates and a random "coin toss" for first team to go
        {
            for(int i = whiteUnits.Count - 1; i > 0; i--) 
            {
                int j = Random.Range(0, i + 1);
                Unit tmpUnitHolder = whiteUnits[i];
                whiteUnits[i] = whiteUnits[j];
                whiteUnits[j] = tmpUnitHolder;
            }

            for (int i = blackUnits.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                Unit tmpUnitHolder = blackUnits[i];
                blackUnits[i] = blackUnits[j];
                blackUnits[j] = tmpUnitHolder;
            }

            if((Random.Range(0, 100) % 2) > 0)
            {
                StartCoroutine(Fight(whiteUnits, blackUnits));
            }
            else
            {
                StartCoroutine(Fight(blackUnits, whiteUnits));
            }
        }

        private IEnumerator Fight(List<Unit> firstTeam, List<Unit> secondTeam)
        {
            clashStarted = true;

            int firstIndex = 0;
            int secondIndex = 0;
            int firstEnemyIndex = 0;
            int secondEnemyIndex = 0;

            yield return automaticTurnInterval;

            while (firstTeam.Count > 0 && secondTeam.Count> 0)
            {
                firstTeam.RemoveAll(e => e.Equals(null));
                secondTeam.RemoveAll(e => e.Equals(null));
                
                if (firstTeam.Count <= 0)
                {
                    Debug.Log($"First team lost");
                    yield return new WaitForSeconds(1);
                    PerformCleanupAfterFight();
                    yield break;
                }

                if (secondTeam.Count <= 0)
                {
                    Debug.Log($"Second team lost");
                    yield return new WaitForSeconds(1);
                    PerformCleanupAfterFight();
                    yield break;
                }

                firstEnemyIndex = Random.Range(0, secondTeam.Count);
                secondEnemyIndex = Random.Range(0, firstTeam.Count);
                firstIndex = firstIndex % firstTeam.Count > 0 ? firstIndex : 0;
                secondIndex = secondIndex % secondTeam.Count > 0 ? secondIndex : 0;

                if(firstIndex < firstTeam.Count)
                {
                    firstTeam[firstIndex].PlayAttackAnimation();
                    firstTeam[firstIndex].AttackEnemy(secondTeam[firstEnemyIndex]);
                    firstIndex++;
                }

                yield return automaticTurnInterval;

                if (secondIndex < secondTeam.Count)
                {
                    secondTeam[secondIndex].PlayAttackAnimation();
                    secondTeam[secondIndex].AttackEnemy(firstTeam[secondEnemyIndex]);
                    secondIndex++;
                }

                yield return automaticTurnInterval;
            }
        }

        private void PerformCleanupAfterFight()
        {
            whiteUnits.RemoveAll(e => e.Equals(null));
            blackUnits.RemoveAll(e => e.Equals(null));

            if(whiteUnits.Count > 0)
            {
                for (int i = 0; i < whiteUnits.Count; i++)
                {
                    Destroy(whiteUnits[i].gameObject);
                }
            }

            whiteUnits.Clear();

            if (blackUnits.Count > 0)
            {
                for (int i = 0; i < blackUnits.Count; i++)
                {
                    Destroy(blackUnits[i].gameObject);
                }
            }

            blackUnits.Clear();

            clashStarted = false;
        }
    }
}
