using BannerReload.VM;
using BannerReload.Data;
using BannerReload.VM.Banner;
using HarmonyLib;
using SandBox.GauntletUI;
using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.TwoDimension;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.GauntletUI;
using System.Collections.Generic;
using System.Linq;

namespace BannerReload.Pathes
{

    [HarmonyPatch(typeof(BannerEditorView), MethodType.Constructor, new Type[] {
     typeof(BasicCharacterObject),
     typeof(Banner),
     typeof(ControlCharacterCreationStage),
     typeof(TextObject),
     typeof(ControlCharacterCreationStage),
     typeof(TextObject),
     typeof(ControlCharacterCreationStage),
     typeof(ControlCharacterCreationStageReturnInt),
     typeof(ControlCharacterCreationStageReturnInt),
     typeof(ControlCharacterCreationStageReturnInt),
     typeof(ControlCharacterCreationStageWithInt)
    })]
    
    class BannerEditorViewPath
    {
        public static SpriteCategory clanCategory;

        public static void Postfix(ref BannerEditorView __instance)
        {

            SpriteData spriteData = UIResourceManager.SpriteData;
            TwoDimensionEngineResourceContext resourceContext = UIResourceManager.ResourceContext;
            ResourceDepot uIResourceDepot = UIResourceManager.UIResourceDepot;
            clanCategory = spriteData.SpriteCategories["ui_clan"];
            clanCategory.Load(resourceContext, uIResourceDepot);

            BannerRecordVM bannerRecord = new BannerRecordVM(__instance, GlobalDataProvider.Instance.BannerRecordData());
            GauntletMovie movie = __instance.GauntletLayer.LoadMovie("BannerRecord", bannerRecord);
        }
    }

    [HarmonyPatch(typeof(BannerEditorView), "OnFinalize")]
    class BannerEditorViewOnFinalizePath
    {

        public static void Postfix()
        {
            if (null != BannerEditorViewPath.clanCategory)
            {
                GlobalDataProvider.Instance.SaveBannerData();
                BannerEditorViewPath.clanCategory = null;
            }
        }
    }

}
