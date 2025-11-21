using GameNetcodeStuff;
using HarmonyLib;
using NoNameDisplayRestrictions.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace NoNameDisplayRestrictions.Patches
{
	[HarmonyPatch(typeof(PlayerControllerB))]
	internal static class PlayerControllerBPatcher
	{
		[HarmonyTranspiler]
		[HarmonyPatch(nameof(PlayerControllerB.SendNewPlayerValuesClientRpc))]
		static IEnumerable<CodeInstruction> SendNewPlayerValuesClientRpcTranspiler(IEnumerable<CodeInstruction> instructions)
		{
			MethodInfo NoPunctuation = typeof(PlayerControllerB).GetMethod(nameof(PlayerControllerB.NoPunctuation), BindingFlags.NonPublic | BindingFlags.Instance);
			ConstructorInfo stringConstructor = typeof(string).GetConstructor([typeof(char[])]);
			List<CodeInstruction> codes = new(instructions);
			int index = 0;
			Tools.FindMethod(ref index, ref codes, NoPunctuation, skip: true, errorMessage: "Couldn't find the NoPunctuation method callback which removes the non-letter characters from the player's name");
			codes.RemoveAt(index-1);
			codes.Insert(index - 1, new CodeInstruction(opcode: OpCodes.Newobj, stringConstructor));
			int ldlockIndex = index;
			Tools.FindLocalField(ref index, ref codes, localIndex: 0, store: true, skip: true, errorMessage: "Couldn't find the store instruction which removes characters from the player's name");
			codes.RemoveRange(ldlockIndex, index - ldlockIndex - 1);
			return codes;
		}
	}
}
