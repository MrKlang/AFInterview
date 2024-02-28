using System;
using System.Collections.Generic;

namespace AFSInterview
{
    [Serializable]
    public class UnitData
    {
        public int id;
        public string unitType;
        public List<UnitAttribute> unitAttributes;
        public int maxHp;
        public int armour;
        public int attackIntervalInTurns;
        public int damageDealtPerAttack;
        public List<UnitDamageAgainstAttribute> unitDamageOverrides;
    }
}
