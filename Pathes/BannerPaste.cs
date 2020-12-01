using System;
using HarmonyLib;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;

namespace BannerReload.Pathes
{
	[HarmonyPatch(typeof(BannerEditorView), "OnTick")]
	internal class BannerEditorView_OnTick
	{
		private static void Postfix(BannerEditorView __instance)
		{
			if (__instance.SceneLayer.Input.IsHotKeyPressed("Copy") || __instance.GauntletLayer.Input.IsHotKeyPressed("Copy"))
			{
				try
				{
					Input.SetClipboardText(__instance.DataSource.BannerVM.Banner.Serialize());
				}
				catch (Exception ex2)
				{
					//Log.write("Error copying banner code");
					InformationManager.DisplayMessage(new InformationMessage("Error copying banner code", new Color(1f, 0f, 0f)));
					//Log.write(ex2);
				}
			}
			else if (__instance.SceneLayer.Input.IsHotKeyPressed("Paste") || __instance.GauntletLayer.Input.IsHotKeyPressed("Paste"))
			{
				try
				{
					string bannerCode = Input.GetClipboardText();
					Banner banner = new Banner(bannerCode);
					banner.ConvertToMultiMesh();
					__instance.DataSource.BannerVM.BannerCode = bannerCode;
					Traverse.Create(__instance).Method("RefreshShieldAndCharacter").GetValue();
				}
				catch (Exception ex)
				{
					//Log.write("Error deserializing banner code");
					InformationManager.DisplayMessage(new InformationMessage("Error pasting banner code", new Color(1f, 0f, 0f)));
					//Log.write(ex);
				}
			}
		}

		[HarmonyPatch(typeof(BannerEditorVM), "SetClanRelatedRules")]
		public class SetClanRelatedRulesPatch
		{
			private static bool Prefix(ref bool canChangeBackgroundColor)
			{
				try
				{
					canChangeBackgroundColor = true;
					return false;
				}
				catch
				{
					InformationManager.DisplayMessage(new InformationMessage("Error patching method 1.", new Color(1f, 0f, 0f)));
					return true;
				}
			}
		}

		[HarmonyPatch(typeof(MBBannerEditorGauntletScreen), "OnDone")]
		public class MBBannerEditorGauntletScreen_OnDone
		{
			public static object KingdomColorModule
			{
				get;
				private set;
			}

			private static void Prefix(MBBannerEditorGauntletScreen __instance)
			{
				try
				{
					BannerEditorView value = Traverse.Create(__instance).Field<BannerEditorView>("_bannerEditorLayer").Value;
					Clan value2 = Traverse.Create(__instance).Field<Clan>("_clan").Value;
					if (ShouldReplaceKingdomColor(value2))
					{
						Kingdom kingdom = value2.Kingdom;
						uint primaryColor = value.DataSource.BannerVM.GetPrimaryColor();
						uint sigilColor = value.DataSource.BannerVM.GetSigilColor();
						SetKingdomColors(kingdom, primaryColor, sigilColor);
					}
				}
				catch
				{
					InformationManager.DisplayMessage(new InformationMessage("Error patching method 2.", new Color(1f, 0f, 0f)));
				}
			}
		}

		public static bool ShouldReplaceKingdomColor(Clan playerClan)
		{
			if (playerClan != null)
			{
				return playerClan.Kingdom != null;
			}
			return false;
		}

		public static void SetKingdomColors(Kingdom kingdom, uint color1, uint color2)
		{
			try
			{
				Clan playerClan = Clan.PlayerClan;
				Traverse traverse = Traverse.Create(kingdom);
				traverse.Property<uint>("Color").Value = color1;
				traverse.Property<uint>("Color2").Value = color2;
				traverse.Property<uint>("PrimaryBannerColor").Value = color1;
				traverse.Property<uint>("SecondaryBannerColor").Value = color2;
				foreach (Clan clan in kingdom.Clans)
				{
					if (clan != playerClan)
					{
						clan.Banner?.ChangePrimaryColor(kingdom.PrimaryBannerColor);
						clan.Banner?.ChangeIconColors(kingdom.SecondaryBannerColor);
					}
				}
				foreach (MobileParty item in MobileParty.All)
				{
					if (item.Party.Owner?.Clan?.Kingdom == kingdom)
					{
						item.Party.Visuals?.SetMapIconAsDirty();
					}
				}
				foreach (Settlement settlement in kingdom.Settlements)
				{
					settlement.Party.Visuals?.SetMapIconAsDirty();
				}
			}
			catch
			{
				InformationManager.DisplayMessage(new InformationMessage("Error with method 3.", new Color(1f, 0f, 0f)));
			}
		}


	}
}
