using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using HarmonyLib;
using System.Reflection;

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
            var newDonL = typeof(Keyboard).GetProperty(Plugin.Instance.ConfigExtraDonL.Value + "Key", BindingFlags.Public | BindingFlags.Instance)?.GetValue(Keyboard.current) as KeyControl;
            var newDonR = typeof(Keyboard).GetProperty(Plugin.Instance.ConfigExtraDonR.Value + "Key", BindingFlags.Public | BindingFlags.Instance)?.GetValue(Keyboard.current) as KeyControl;
            var newKaL = typeof(Keyboard).GetProperty(Plugin.Instance.ConfigExtraKaL.Value + "Key", BindingFlags.Public | BindingFlags.Instance)?.GetValue(Keyboard.current) as KeyControl;
            var newKaR = typeof(Keyboard).GetProperty(Plugin.Instance.ConfigExtraKaR.Value + "Key", BindingFlags.Public | BindingFlags.Instance)?.GetValue(Keyboard.current) as KeyControl;

            TaikoCoreTypes.UserInputType NewMoreKeyboardInputs()
            {
                EnsoInput.EnsoInputFlag flag = __instance.GetLastInput(player);

                if ((newDonL != null && newDonL.wasPressedThisFrame) || (newDonR != null && newDonR.wasPressedThisFrame))
                {
                    flag = EnsoInput.EnsoInputFlag.DaiDon;
                }
                else if ((newKaL != null && newKaL.wasPressedThisFrame) || (newKaR != null && newKaR.wasPressedThisFrame))
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
