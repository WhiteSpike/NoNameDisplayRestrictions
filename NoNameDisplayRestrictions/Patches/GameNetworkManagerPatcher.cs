using HarmonyLib;
using NoNameDisplayRestrictions.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
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
			Plugin.mls.LogDebug(NoPunctuation);
			List<CodeInstruction> codes = new(instructions);
			int index = 0;
			Tools.FindMethod(ref index, ref codes, NoPunctuation, skip: true, errorMessage: "Couldn't find the NoPunctuation method callback which removes the non-letter characters from the player's name");
			codes.RemoveAt(index-1);
			int ldlockIndex = index;
			Tools.FindLocalField(ref index, ref codes, localIndex: 5, store: true, skip: true, errorMessage: "Couldn't find the store instruction which removes characters from the player's name");
			codes.RemoveRange(ldlockIndex, index - ldlockIndex-1);
			return codes;
		}
	}
}
