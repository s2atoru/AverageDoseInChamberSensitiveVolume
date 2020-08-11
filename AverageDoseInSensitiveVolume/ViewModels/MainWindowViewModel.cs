using AverageDoseInSensitiveVolume.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.IO;
using VolumeAverage;

namespace AverageDoseInSensitiveVolume.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string folderPath;
        public string FolderPath
        {
            get { return folderPath; }
            set { SetProperty(ref folderPath, value); }
        }
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { SetProperty(ref fileName, value); }
        }

        private bool doesOverWrite;
        public bool DoseOverWrite
        {
            get { return doesOverWrite; }
            set { SetProperty(ref doesOverWrite, value); }
        }

        private double xCenter;
        public double XCenter
        {
            get { return xCenter; }
            set { SetProperty(ref xCenter, value); }
        }
        private double yCenter;
        public double YCenter
        {
            get { return yCenter; }
            set { SetProperty(ref yCenter, value); }
        }
        private double zCenter;
        public double ZCenter
        {
            get { return zCenter; }
            set { SetProperty(ref zCenter, value); }
        }

        private FieldReferencePoint selectedFieldReferencePoint = null;
        public FieldReferencePoint SelectedFieldReferencePoint
        {
            get { return selectedFieldReferencePoint; }
            set
            {
                SetProperty(ref selectedFieldReferencePoint, value);
                if (SelectedFieldReferencePoint != null)
                {
                    XCenter = SelectedFieldReferencePoint.X;
                    YCenter = SelectedFieldReferencePoint.Y;
                    ZCenter = SelectedFieldReferencePoint.Z;
                }
            }
        }

        public ObservableCollection<FieldReferencePoint> FieldReferencePoints { get; private set; } = new ObservableCollection<FieldReferencePoint>();

        public ObservableCollection<Cylinder> Cylinders { get; set; } = new ObservableCollection<Cylinder>();


        public DelegateCommand CalculateCommand { get; private set; }

        public MainWindowViewModel()
        {
            CalculateCommand = new DelegateCommand(CalculateAverageDose);
        }

        private void CalculateAverageDose()
        {
            foreach (var c in Cylinders)
            {
                c.XCenter = selectedFieldReferencePoint.XDcs;
                c.YCenter = selectedFieldReferencePoint.YDcs;
                c.ZCenter = selectedFieldReferencePoint.ZDcs;
                c.VolumeAverageDose();
            }
        }

        private void ChooseFolder()
        {
            // ダイアログのインスタンスを生成
            var dialog = new CommonOpenFileDialog
            {
                Title = "フォルダ選択",
                // フォルダ選択ダイアログの場合は true
                IsFolderPicker = true,
                //// ダイアログが表示されたときの初期ディレクトリを指定
                InitialDirectory = Path.GetDirectoryName(FolderPath),
                DefaultFileName = Path.GetFileName(FolderPath),
                //// ユーザーが最近したアイテムの一覧を表示するかどうか
                //AddToMostRecentlyUsedList = false,
                //// ユーザーがフォルダやライブラリなどのファイルシステム以外の項目を選択できるようにするかどうか
                //AllowNonFileSystemItems = false,
                //// 最近使用されたフォルダが利用不可能な場合にデフォルトとして使用されるフォルダとパスを設定する
                //DefaultDirectory = @"C:\Users\s2ato",
                //// 存在するファイルのみ許可するかどうか
                //EnsureFileExists = true,
                //// 存在するパスのみ許可するかどうか
                //EnsurePathExists = true,
                //// 読み取り専用ファイルを許可するかどうか
                //EnsureReadOnly = false,
                //// 有効なファイル名のみ許可するかどうか（ファイル名を検証するかどうか）
                //EnsureValidNames = true,
                // 複数選択を許可するかどうか
                Multiselect = false,
                // PC やネットワークなどの場所を表示するかどうか
                ShowPlacesList = true
            };

            // ダイアログを表示
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FolderPath = dialog.FileName;
            }

        }
    }
}
