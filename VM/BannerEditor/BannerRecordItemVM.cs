using BannerReload.Data;
using BannerReload.Utils;
using HarmonyLib;
using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ObjectSystem;

namespace BannerReload.VM.Banner
{
    class BannerRecordItemVM: ViewModel
    {
        private BannerRecordData _item;

        private bool _isSelected;

        private ImageIdentifierVM _visual;

        private readonly Action<BannerRecordItemVM> _onRecordSelected;


        public BannerRecordItemVM(BannerRecordData item, Action<BannerRecordItemVM> onRecordSelected)
        {
            this._item = item;
            BannerCode bannerCode =  CreateFrom(item);
            this.Visual = new ImageIdentifierVM(bannerCode);
            this._onRecordSelected = onRecordSelected;
        }

        public  BannerCode CreateFrom(BannerRecordData item)
        {
            string code = item.BannerCode;
            //BannerCode bannerCode = new BannerCode();
            var bannerCode = BannerCode.CreateFrom(code);

            return bannerCode;
        }



        public BannerRecordData GetBannerRecordData() {

            return _item;
        }

        [DataSourceProperty]
        public ImageIdentifierVM Visual
        {
            get
            {
                return this._visual;
            }
            set
            {
                if (value != this._visual)
                {
                    this._visual = value;
                    base.OnPropertyChanged("Visual");
                }
            }
        }


        [DataSourceProperty]
		public string DisplayName
		{
			get
			{
				return this._item.SaveName;
			}
			
		}

		[DataSourceProperty]
		public string DateString
		{
			get
			{
				return this._item.DateString;
			}

		}


        [DataSourceProperty]
        public bool IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                if (value != this._isSelected)
                {
                    this._isSelected = value;
                    base.OnPropertyChanged("IsSelected");
                }
            }
        }

        public void OnHistoryRecordSelected()
        {
            if (!this.IsSelected)
            {
                this.IsSelected = true;
                this._onRecordSelected(this);
            }

        }


    }
}
