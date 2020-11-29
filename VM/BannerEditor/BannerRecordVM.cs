using BannerReload.Data;
using BannerReload.Utils;
using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.View;

namespace BannerReload.VM.Banner
{
    class BannerRecordVM: ViewModel
    {
        List<BannerRecordData> _data;
        MBBindingList<BannerRecordItemVM> _bannerRecordItemVMs;

        int MaxSaveCount = 50;

        BannerEditorView _bannerView;

        private BannerRecordItemVM _lastSelectedItem;

        public BannerRecordVM(BannerEditorView bannerEditorView, List<BannerRecordData> data)
        {
            this._bannerView = bannerEditorView;
            this._data = data;
            if (null == this._bannerRecordItemVMs)
            {
                this._bannerRecordItemVMs = new MBBindingList<BannerRecordItemVM>();
            }
            else
            {
                this._bannerRecordItemVMs.Clear();
            }


            if(null != this._data && this._data.Count > 0)
            {
                this._data.ForEach(obj => {
                    this._bannerRecordItemVMs.Add(new BannerRecordItemVM(obj, OnSelectedItem));
                });
            }

        }

        [DataSourceProperty]
        public bool HasSelectedItem
        {
            get
            {
                return null != this._lastSelectedItem;
            }
        }

        [DataSourceProperty]
        public string SavedListText
        {
            get
            {
                return new TextObject("{=bottom_SavedList}Saved Banner", null).ToString();
            }

        }

        [DataSourceProperty]
        public string SaveCurrentText
        {
            get
            {
                return new TextObject("{=bottom_SaveCurrent}Save Current", null).ToString();
            }

        }

        [DataSourceProperty]
        public string LoadCurrentText
        {
            get
            {
                return new TextObject("{=bottom_LoadCurrent}Load Current", null).ToString();
            }

        }

        [DataSourceProperty]
        public string DeleteCurrentText
        {
            get
            {
                return new TextObject("{=bottom_DeleteCurrent}Delete Current", null).ToString();
            }

        }

        [DataSourceProperty]
        public MBBindingList<BannerRecordItemVM> RecordItems
        {
            get
            {
                return this._bannerRecordItemVMs;
            }
            set
            {
                if (value != this._bannerRecordItemVMs)
                {
                    this._bannerRecordItemVMs = value;
                    base.OnPropertyChanged("RecordItems");
                }
            }
        }

        public void OnSelectedItem(BannerRecordItemVM item)
        {
            if(null != _lastSelectedItem)
            {
               this._lastSelectedItem.IsSelected = false;
            }
            this._lastSelectedItem = item;
            base.OnPropertyChanged("HasSelectedItem");
        }


        public void ExecuteSaveSelected()
        {
            InformationManager.ShowTextInquiry(new TextInquiryData(new TextObject("{=tips_cr_InputSaveName}Enter the saved name").ToString(), new TextObject("{=tips_cr_MaxSaved}Can only save up to " + MaxSaveCount).ToString(),
             true, true, GameTexts.FindText("str_done", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action<string>(OnEnterNameAfter), InformationManager.HideInquiry, false));
        }

        public void OnEnterNameAfter(string saveName)
        {
            String bannerRecordString = this._bannerView.DataSource.BannerVM.Banner.Serialize();
            BannerRecordData data = new BannerRecordData(saveName, bannerRecordString);
            data.SaveName = saveName;
            if (this._data.Count >= MaxSaveCount)
            {
                this._data.RemoveAt(this._data.Count -1);
                this._bannerRecordItemVMs.RemoveAt(this._data.Count);
            }
            this._data.Insert(0, data);
            this._bannerRecordItemVMs.Insert(0, new BannerRecordItemVM(data, OnSelectedItem));
            base.OnPropertyChanged("RecordItems");
            if(null != this._lastSelectedItem)
            {
                this._lastSelectedItem.IsSelected = false;
                this._lastSelectedItem = null;
            }
            base.OnPropertyChanged("HasSelectedItem");
        }

        public void ExecuteLoadSelected()
        {
            if (null != this._lastSelectedItem)
            {
                InformationUtils.ShowComfirInformation(new TextObject("{=tips_cr_ConfirmLoad}Confirm to load"), null, () => {
                    String code = this._lastSelectedItem.GetBannerRecordData().BannerCode;
                    TaleWorlds.Core.Banner banner = new TaleWorlds.Core.Banner(code);
                    banner.ConvertToMultiMesh();
                    this._bannerView.DataSource.BannerVM.BannerCode = code;

                });
            }
        }

    

        public void ExecuteDeleteSelected()
        {
            if (null != this._lastSelectedItem)
            {
                InformationUtils.ShowComfirInformation(new TextObject("{=tips_cr_WarningToDelete}Warning: Confirm to Delete"), new TextObject("{=tips_cr_WarningToDelete2}This operation will delete the data and cannot be undone"), () => {
                    this._lastSelectedItem.IsSelected = false;
                    this._data.Remove(this._lastSelectedItem.GetBannerRecordData());
                    this._bannerRecordItemVMs.Remove(this._lastSelectedItem);
                    base.OnPropertyChanged("RecordItems");
                    this._lastSelectedItem = null;
                    base.OnPropertyChanged("HasSelectedItem");
                });

              
              
            }
        }

    }
}
