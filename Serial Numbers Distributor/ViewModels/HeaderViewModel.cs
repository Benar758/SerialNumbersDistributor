using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Serial_Numbers_Distributor.Models;
using Serial_Numbers_Distributor.ViewModels.Commands;
using System.Windows;

namespace Serial_Numbers_Distributor.ViewModels
{
    internal class HeaderViewModel : BaseViewModel
    {
        public HeaderViewModel()
        {
            //Создание файлового менеджера
            FileDialog = new OpenFileDialog() 
            { 
                Multiselect = false 
            };

            Distributor = new Distributor();

            UploadNewFileCmd = new UploadNewFileCommand(UploadNewFile);

            ComboboxNValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        } 

        /// <summary>
        /// Файловый менеджер
        /// </summary>
        private OpenFileDialog FileDialog { get; set; }

        /// <summary>
        /// Настройки для расспределения серийных номеров
        /// </summary>
        public static Distributor Distributor { get; set; }

        /// <summary>
        /// Имя текущего файла (необходимо для отображения и динамического изменения в UI
        /// </summary>
        private string _currentFileName;

        public string CurrentFileName
        {
            get { return _currentFileName; }
            set 
            {
                _currentFileName = value;
                OnPropertyChanged("CurrentFileName");
            }
        }

        /// <summary>
        /// Значение N, вводимое пользователем в текстове поле
        /// </summary>
        private string _inputNValue;

        public string InputNValue
        {
            get { return _inputNValue; }
            set 
            {
                _inputNValue = value;
                int N;

                if (int.TryParse(value.Trim().Replace(" ", ""), out N))
                {
                    Distributor.N = N;
                }
            }
        }

        /// <summary>
        /// ItemsSource для Combobox для выбора числа N
        /// </summary>
        public List<int> ComboboxNValues { get; set; }

        /// <summary>
        /// Команда, отрабатывающая полсе нажатия на кнопку для загрузки нового файла
        /// </summary>
        public UploadNewFileCommand UploadNewFileCmd { get; private set; }

        /// <summary>
        /// Открывает окно для выбора нового файла и считывает с него информацию
        /// </summary>
        private void UploadNewFile()
        {
            if (FileDialog.ShowDialog() == true)
            {
                SerialsFile CurrentFile = new SerialsFile(FileDialog.FileName, FileDialog.SafeFileName.ToLower());
                DirectoryInfo CurrentFileInfo = new DirectoryInfo(FileDialog.FileName);

                if (CurrentFileInfo.Extension.ToLower() == ".txt")
                {
                    CurrentFile.Content = File.ReadAllLines(CurrentFile.Path).ToList();
                    CurrentFileName = CurrentFile.Name;

                    if (Distributor.N == 0 && Distributor.CurrentFile == null)
                    {
                        Distributor = new Distributor(CurrentFile);
                    }
                    else
                    {
                        Distributor.CurrentFile = CurrentFile;
                    }
                }
                else
                {
                    MessageBox.Show("Допустимые расширения файла: .txt!",
                                    "Выбор файла", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Error);
                }
            }
        }
    }
}
