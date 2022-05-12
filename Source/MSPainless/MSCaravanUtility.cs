using RimWorld.Planet;
using Verse;

namespace MSPainless;

public class MSCaravanUtility
{
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

    public static void PawnOnCaravanTakeDrug(Caravan caravan, Pawn pawn, Thing drug, Pawn owner)
    {
        caravan.needs.IngestDrug(pawn, drug, owner);
    }
}