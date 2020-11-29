using HarmonyLib;
using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using System.IO;

namespace BannerReload
{
    public class SubModule : MBSubModuleBase
    {
	
		protected override void OnSubModuleLoad()
		{
			try
			{
				base.OnSubModuleLoad();
				new Harmony("mod.BannerReload.cnedwin").PatchAll();
			}
			catch (Exception)
			{
				InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=misc_cr_loaderro}Error Initialising BannerReload", null).ToString(), Color.FromUint(ModuleColors.green)));
			}
		}

		protected override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=misc_cr_onmapload}Loaded BannerReload succeeded", null).ToString(), Color.FromUint(ModuleColors.green)));
		}

		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			base.OnGameStart(game, gameStarterObject);


			if (!(game.GameType is Campaign))
			{
				Helper.Log("GameType is not Campaign. CharacterTrainer disabled.");
				return;
			}
			if (!(gameStarterObject is CampaignGameStarter))
			{ 
				return;
		    }

			if (!Directory.Exists(Helper.SavePath))
			{
				Directory.CreateDirectory(Helper.SavePath);
			}
			Helper.ClearLog();

			//LoadXMLFiles(gameStarterObject as CampaignGameStarter);


		}


		//private void LoadXMLFiles(CampaignGameStarter gameInitializer)
		//{
		//	gameInitializer.LoadGameTexts(BasePath.Name + "Modules/BannerReload/ModuleData/strings.xml");

		//}
	}
	

	public static class ModuleColors
	{
		public static readonly uint modMainColor = 14906114U;

		public static readonly uint green = 4282569842U;

		public static readonly uint red = 12517376U;

		public static readonly uint grey = 14606046U;

		public static readonly uint yellow = 15120402U;
	}
}