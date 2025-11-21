using HarmonyLib;
using NoNameDisplayRestrictions.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace NoNameDisplayRestrictions.Patches
{
	[HarmonyPatch(typeof(GameNetworkManager))]
	internal static class GameNetworkManagerPatcher
	{
		[HarmonyTranspiler]
		[HarmonyPatch(nameof(GameNetworkManager.SteamMatchmaking_OnLobbyMemberJoined))]
		static IEnumerable<CodeInstruction> SteamMatchmaking_OnLobbyMemberJoinedTranspiler(IEnumerable<CodeInstruction> instructions)
		{
			MethodInfo NoPunctuation = typeof(GameNetworkManager).GetMethod(nameof(GameNetworkManager.NoPunctuation), BindingFlags.NonPublic | BindingFlags.Instance);
			ConstructorInfo stringConstructor = typeof(string).GetConstructor([typeof(char[])]);
			List<CodeInstruction> codes = new(instructions);
			int index = 0;
			Tools.FindMethod(ref index, ref codes, NoPunctuation, skip: true, errorMessage: "Couldn't find the NoPunctuation method callback which removes the non-letter characters from the player's name");
			codes.RemoveAt(index-1);
			codes.Insert(index - 1, new CodeInstruction(opcode: OpCodes.Newobj, stringConstructor));
			int ldlockIndex = index;
			Tools.FindLocalField(ref index, ref codes, localIndex: 5, store: true, skip: true, errorMessage: "Couldn't find the store instruction which removes characters from the player's name");
			codes.RemoveRange(ldlockIndex, index - ldlockIndex - 1);
			return codes;
		}
	}
}
