using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Obeliskial_Content;
using Obeliskial_Essentials;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections;

namespace TraitMod
{
    [HarmonyPatch]
    internal class Traits
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), "BeginTurnPerks")]
        public static void BeginTurnPerksPostfix(ref Character __instance)
        {
            int _perk_burnCharges = __instance.GetAuraCharges("burn");
            int _perk_chillCharges = __instance.GetAuraCharges("chill");
            int _perk_sparkCharges = __instance.GetAuraCharges("spark");
            int _position = Traverse.Create(__instance).Field("position").GetValue<int>();
            if (!__instance.IsHero && _perk_sparkCharges > 0 && _perk_burnCharges > 0 && _perk_chillCharges > 0 && AtOManager.Instance.TeamHaveTrait("evelynelementalweaver"))
            {
                List<NPC> npcsides = MatchManager.Instance.GetNPCSides(_position);
                for (int i = 0; i < npcsides.Count; i++)
                {
                    npcsides[i].SetAura(null, Globals.Instance.GetAuraCurseData("burn"), Functions.FuncRoundToInt((float)_perk_sparkCharges * 0.3f), false, Enums.CardClass.None, true, true);
                    npcsides[i].SetAura(null, Globals.Instance.GetAuraCurseData("chill"), Functions.FuncRoundToInt((float)_perk_sparkCharges * 0.3f), false, Enums.CardClass.None, true, true);
                }
                __instance.SetAura(null, Globals.Instance.GetAuraCurseData("burn"), Functions.FuncRoundToInt((float)_perk_sparkCharges * 0.3f), false, Enums.CardClass.None, true, true);
                __instance.SetAura(null, Globals.Instance.GetAuraCurseData("chill"), Functions.FuncRoundToInt((float)_perk_sparkCharges * 0.3f), false, Enums.CardClass.None, true, true);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), "SetAura")]
        public static void SetAuraPostfix(ref Character __instance)
        {
            int _trait_burnCharges = __instance.GetAuraCharges("burn");
            int _trait_chillCharges = __instance.GetAuraCharges("chill");
            int _trait_sparkCharges = __instance.GetAuraCharges("spark");
            if (!__instance.IsHero && _trait_burnCharges == 200 && _trait_chillCharges == 200 && _trait_sparkCharges == 200 && AtOManager.Instance.TeamHaveTrait("evelynelementalamplifier"))
            {
                __instance.IndirectDamage(Enums.DamageType.Fire, 600);
                __instance.IndirectDamage(Enums.DamageType.Cold, 600);
                __instance.IndirectDamage(Enums.DamageType.Lightning, 600);
                for (int i = __instance.AuraList.Count - 1; i >= 0; i--)
                {
                    if (__instance.AuraList[i].ACData.Id == "burn" || __instance.AuraList[i].ACData.Id == "chill" || __instance.AuraList[i].ACData.Id == "spark")
                    {
                        __instance.AuraList.RemoveAt(i);
                    }
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Character), "SetEvent")]
        public static void SetEventPrefix(ref Character __instance, ref Enums.EventActivation theEvent, Character target = null)
        {
            if (theEvent == Enums.EventActivation.BeginTurn && !__instance.IsHero && AtOManager.Instance.TeamHaveTrait("evelynelementalweaver") && __instance.HasEffect("burn"))
            { // if team have trait "evelynelementalweaver", burn on enemy deals 15% of burn charges as fire and lightning damage
                __instance.IndirectDamage(Enums.DamageType.Fire, Functions.FuncRoundToInt((float)__instance.GetAuraCharges("burn") * 0.15f));
                __instance.IndirectDamage(Enums.DamageType.Lightning, Functions.FuncRoundToInt((float)__instance.GetAuraCharges("burn") * 0.15f));
            }
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "GlobalAuraCurseModificationByTraitsAndItems")]
        public static void GlobalAuraCurseModificationByTraitsAndItemsPostfix(ref AtOManager __instance, ref AuraCurseData __result, string _type, string _acId, Character _characterCaster, Character _characterTarget)
        {
            bool flag = false;
            bool flag2 = false;
            if (_characterCaster != null && _characterCaster.IsHero)
            {
                flag = _characterCaster.IsHero;
            }
            if (_characterTarget != null && _characterTarget.IsHero)
            {
                flag2 = true;
            }
            if (_acId == "burn" && _type == "consume" && !flag && __instance.TeamHavePerk("mainperkburn2d"))
            { // burn on enemy deals double damage if the target have 5 or less curses (Burn included)
                __result.DoubleDamageIfCursesLessThan = 6;
            }
            if (_acId == "chill" && _type == "set" && !flag2 && __instance.TeamHaveTrait("evelynelementalweaver"))
            {
                __result.ResistModified3 = Enums.DamageType.Lightning;
                __result.ResistModifiedPercentagePerStack3 = -0.3f;
            }
        }
    }
}
