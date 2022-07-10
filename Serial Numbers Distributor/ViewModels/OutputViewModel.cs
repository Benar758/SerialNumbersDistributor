using Serial_Numbers_Distributor.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Serial_Numbers_Distributor.ViewModels
{
    internal class OutputViewModel : BaseViewModel
    {
        public OutputViewModel()
        {
            CleanOutputFieldCmd = new CleanOutputFieldCommand(CleanOutputField);
            StartDistributionCmd = new StartDistributionCommand(DistributeSerials);
        }

        /// <summary>
        /// Информация для вывода в текстовое поле
        /// </summary>
        private string _serialsOutput;

        public string SerialsOutput
        {
            get { return _serialsOutput; }
            set
            { 
                _serialsOutput = value;
                OnPropertyChanged("SerialsOutput");
            }
        }

        public CleanOutputFieldCommand CleanOutputFieldCmd { get; private set; }

        public StartDistributionCommand StartDistributionCmd { get; private set; }

        /// <summary>
        /// Распределяет серийные номера в правильном порядке
        /// </summary>
        private void DistributeSerials()
        {
            List<string> serialsList = HeaderViewModel.Distributor.CurrentFile != null ?
                HeaderViewModel.Distributor.CurrentFile.Content : null;
            double nValue = HeaderViewModel.Distributor.N;

            if (serialsList == null)
                MessageBox.Show("Загрузите файл");
            else if (serialsList.Count == 0)
                MessageBox.Show("Файл пуст");
            else if (nValue == 0 || nValue > 1_000_000)
                MessageBox.Show("Размер адресного прострнства не может быть меньше 1 или больше 1 млн");
            else
            {
                double multiplyer = serialsList.Count / nValue;
                double spaceNeeded = nValue < serialsList.Count ? nValue * Math.Ceiling(multiplyer) : nValue;

                string[] DistributedSerials = new string[Convert.ToInt32(spaceNeeded)];

                int step = Convert.ToInt32(Math.Ceiling((double)DistributedSerials.Length / serialsList.Count));

                int counterForSerialsList = 0;
                for (int i = 0; i < DistributedSerials.Length; i++)
                {
                    if (counterForSerialsList == serialsList.Count) break;
                    if (i == 0 || i % step == 0 || step % i == 0)
                    {
                        if (counterForSerialsList <= serialsList.Count - 1)
                        {
                            DistributedSerials[i] = serialsList[counterForSerialsList++];
                        }
                    }
                    else if (i == DistributedSerials.Length)
                    {
                        DistributedSerials[i] = serialsList[counterForSerialsList];
                    }
                    else DistributedSerials[i] = string.Empty;
                }

                for (int i = 0; i < serialsList.Count; i++)
                {
                    if (DistributedSerials[DistributedSerials.Length - (i + 1)] == string.Empty)
                    DistributedSerials[DistributedSerials.Length - (i + 1)] = serialsList[serialsList.Count - (i + 1)];
                }

                SerialsOutput = FormatContent(DistributedSerials.ToList());
                SendToComPort(SerialsOutput);
            }
        }

        /// <summary>
        /// Форматирует выводимыый контент требуемым образом
        /// </summary>
        /// <param name="content">Контент для форматирования</param>
        /// <returns></returns>
        private string FormatContent(List<string> content)
        {
            StringBuilder formattedContent = new StringBuilder();

            int addressCounter = 1;

            for (int i = 0; i < content.Count; i++)
            {
                if (string.IsNullOrEmpty(content[i]))
                {
                    if (addressCounter >= HeaderViewModel.Distributor.N)
                        addressCounter = 1;
                    else
                        addressCounter++;

                    continue;
                }
                formattedContent.Append($" Address {addressCounter++} - serial number {content[i]}\n");
                    if (addressCounter >= HeaderViewModel.Distributor.N)
                    addressCounter = 1;
            }

            return formattedContent.ToString();
        }

        private void SendToComPort(string dataToSend)
        {
            SerialPort sPort = new SerialPort("COM19", 9600, Parity.None, 8, StopBits.One);

            try
            {
                sPort.Open();
                sPort.NewLine = "\n";
                sPort.Write(dataToSend);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            sPort.Close();
        }

        /// <summary>
        /// Очищает поле вывода
        /// </summary>
        private void CleanOutputField() => SerialsOutput = string.Empty;
    }
}
