using UnityEngine;
using UnityEngine.InputSystem;
using BepInEx;
using HarmonyLib;

namespace MoreKeyboardInputs.Plugins 
{
    internal class MoreKeyboardInputs 
    {
        [HarmonyPatch(typeof(EnsoInput))]
        [HarmonyPatch(nameof(EnsoInput.GetLastInputForCore))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPrefix]

        public static bool EnsoInput_GetLastInputForCore_Prefix(EnsoInput __instance, ref TaikoCoreTypes.UserInputType __result, int player)
        {
            TaikoCoreTypes.UserInputType NewGetLastInputForCore()
            {
                EnsoInput.EnsoInputFlag flag = __instance.GetLastInput(player);

                if (Keyboard.current.vKey.wasPressedThisFrame)
                {
                    flag = EnsoInput.EnsoInputFlag.DonL;
                    Logger.Log("Test");
                }
                else if (Keyboard.current.nKey.wasPressedThisFrame)
                {
                    flag = EnsoInput.EnsoInputFlag.DonR;
                    Logger.Log("Test");
                }
                if (__instance.ensoParam.EnsoEndType == EnsoPlayingParameter.EnsoEndTypes.OptionPerfect || __instance.ensoParam.EnsoEndType == EnsoPlayingParameter.EnsoEndTypes.OptionTraining)
                {
                    flag = EnsoInput.EnsoInputFlag.None;
                }

                return __instance.ToUserInputType(flag);
            }

            if ((__instance.ensoParam.networkGameMode != Scripts.EnsoGame.Network.NetworkGameMode.RankMatch) &&
                (__instance.ensoParam.networkGameMode != Scripts.EnsoGame.Network.NetworkGameMode.RoomMatchVs))
            {
                __result = NewGetLastInputForCore();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
