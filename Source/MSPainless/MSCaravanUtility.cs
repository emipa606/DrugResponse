using RimWorld.Planet;
using Verse;

namespace MSPainless
{
    // Token: 0x02000005 RID: 5
    public class MSCaravanUtility
    {
        // Token: 0x06000047 RID: 71 RVA: 0x00005074 File Offset: 0x00003274
        public static bool CaravanHasDrug(Caravan car, ThingDef drugDef, out Thing drug, out Pawn owner)
        {
            owner = null;
            drug = null;
            if (!CaravanInventoryUtility.TryGetThingOfDef(car, drugDef, out var thing, out var carOwner))
            {
                return false;
            }

            drug = thing;
            owner = carOwner;
            return true;
        }

        // Token: 0x06000048 RID: 72 RVA: 0x0000509D File Offset: 0x0000329D
        public static void PawnOnCaravanTakeDrug(Caravan caravan, Pawn pawn, Thing drug, Pawn owner)
        {
            caravan.needs.IngestDrug(pawn, drug, owner);
        }
    }
}