using UnityEngine;
using UnityEngine.InputSystem;
using BepInEx;
using HarmonyLib;

namespace MoreKeyboardInputs.Plugins 
{
    [HarmonyPatch]
    internal class MoreKeyboardInputsPatch
    {
        [HarmonyPatch(typeof(EnsoInput))]
        [HarmonyPatch(nameof(EnsoInput.GetLastInputForCore))]
        [HarmonyPrefix]

        public static bool MoreDonKa(EnsoInput __instance, ref TaikoCoreTypes.UserInputType __result, int player)
        {
            TaikoCoreTypes.UserInputType NewMoreKeyboardInputs()
            {
                EnsoInput.EnsoInputFlag flag = __instance.GetLastInput(player);

                if (Keyboard.current.vKey.wasPressedThisFrame || Keyboard.current.nKey.wasPressedThisFrame)
                {
                    flag = EnsoInput.EnsoInputFlag.DaiDon;
                }
                else if (Keyboard.current.cKey.wasPressedThisFrame || Keyboard.current.mKey.wasPressedThisFrame)
                {
                    flag = EnsoInput.EnsoInputFlag.DaiKatsu;
                }

                if (flag == EnsoInput.EnsoInputFlag.DonR ||
                    flag == EnsoInput.EnsoInputFlag.DonL ||
                    flag == EnsoInput.EnsoInputFlag.DaiDon)
                {
                    flag = EnsoInput.EnsoInputFlag.DaiDon;
                }
                else if (flag == EnsoInput.EnsoInputFlag.KatsuR ||
                         flag == EnsoInput.EnsoInputFlag.KatsuL ||
                         flag == EnsoInput.EnsoInputFlag.DaiKatsu)
                {
                    flag = EnsoInput.EnsoInputFlag.DaiKatsu;
                }

                return __instance.ToUserInputType(flag);
            }

            __result = NewMoreKeyboardInputs();
            return false;
        }
    }
}
