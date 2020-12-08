using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.Threading;
using System.Windows.Documents;
using IronBarCode;
using System.Drawing.Configuration;

namespace PrinterWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DrawingVisual visual = new DrawingVisual();
        string path = Directory.GetCurrentDirectory();
        SertificateList sertificate = new SertificateList();
        FontFamily myFont = new FontFamily("Calibri");
        PrintDialog dialogUser = new PrintDialog();
        PrintDialog dialogThermo = new PrintDialog();

        double maxProgressSteps;
        bool imageIsOpen = false;
        bool tempLabelTypeFlag = true;
        bool tempLabelPrintMode = false;
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Label Printer";
            statusBar.Items.Add("");
            statusBar.Items.Add("");
            statusBar.Items.Add("");
            for (int j = 0; j < sertificate.sertName.Length; j++)
            {
                listBox_sert.Items.Add(sertificate.sertName[j]);
            }
        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            //PrintDialog dialogUser = new PrintDialog();
            if(dialogUser.ShowDialog() == true)
            {
                int counter;
                int start;
                int end;

                if (Int32.TryParse(startID.Text, out start))
                {
                    //Проверка количества копий для печати
                    if (!Int32.TryParse(qty.Text, out counter))
                    {
                        counter = 1;
                    }
                    //Проверка диапазона ID номеров
                    if (!Int32.TryParse(endID.Text, out end))
                    {
                        end = start;
                    } else if (end < start)
                    {
                        statusBar.Items[0] = "Неправильный диапазон";
                    }
                    //Боже, прости грешного за такие IF-ELSE костыли...
                    //Из FOR в FOR тоже прости...
                    for (int i = start; i <= end; i++)
                    { 
                        for (int j = 1; j <= counter; j++)
                        {
                            using (DrawingContext dctx = visual.RenderOpen())
                            {
                                //Форматирование текста для печати
                                FormattedText text = new FormattedText("sign: " + i.ToString("0000"),
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface("Times New Roman"), 16, Brushes.Black);

                                //Получение размера текста
                                Size textSize = new Size(text.Width, text.Height);

                                //Установка координатов текста на листе
                                double margin = 96 * 0.05;
                                Point point = new Point(
                                    (dialogUser.PrintableAreaWidth - textSize.Width) / 2 - (margin / 4),
                                    (dialogUser.PrintableAreaHeight - textSize.Height) / 2 + (1.6 * margin));

                                //Отрисовка текста
                                dctx.DrawText(text, point);
                            }

                            //Отправка визуала с отрисованным текстом на печать
                            dialogUser.PrintVisual(visual, "Тестовая печать");
                        }
                    }
                }
                else
                {
                    statusBar.Items[0] = "Введите число!";
                }
            }
        }

        private void printButton2_Click(object sender, RoutedEventArgs e)
        {
            //PrintDialog dialogUser = new PrintDialog();
            if (dialogUser.ShowDialog() == true)
            {
                int counter;
                int start;
                int end;

                if (Int32.TryParse(startID2.Text, out start))
                {
                    //Проверка количества копий для печати
                    if (!Int32.TryParse(qty2.Text, out counter))
                    {
                        counter = 1;
                    }
                    //Проверка диапазона ID номеров
                    if (!Int32.TryParse(endID2.Text, out end))
                    {
                        end = start;
                    }
                    
                    /*else if (end < start)
                    {
                        serviceInfo.Content = "Неправильный диапазон";
                    }*/
                    //Боже, прости грешного за такие IF-ELSE костыли...
                    //Из FOR в FOR тоже прости...
                    for (int i = start; i <= end; i++)
                    {
                        for (int j = 1; j <= counter; j++)
                        {
                            using (DrawingContext dctx = visual.RenderOpen())
                            {
                                //Форматирование текста для печати
                                FormattedText date = new FormattedText(dateField.Text,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface("Times New Roman"), Convert.ToDouble(fontSize.Value), Brushes.Black);
                                FormattedText id = new FormattedText("sign: " + i.ToString("0000"),
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface("Times New Roman"), 14, Brushes.Black);

                                //Получение размера текста
                                Size textDateSize = new Size(date.Width, date.Height);
                                Size textIdSize = new Size(id.Width, id.Height);

                                //Установка координатов текста на листе
                                double margin = 96 * 0.03;
                                Point pointDate = new Point(
                                    (dialogUser.PrintableAreaWidth - textDateSize.Width) / 2 -(margin / 4),
                                    (dialogUser.PrintableAreaHeight - 33) / 2 + (1.6 * margin));
                                Point pointId = new Point(
                                    (dialogUser.PrintableAreaWidth - textIdSize.Width) / 2 - (margin / 4),
                                    (dialogUser.PrintableAreaHeight + 3) / 2 + (1.6 * margin));

                                //Отрисовка текста
                                dctx.DrawText(date, pointDate);
                                dctx.DrawText(id, pointId);
                            }

                            //Отправка визуала с отрисованным текстом на печать
                            dialogUser.PrintVisual(visual, "Тестовая печать");
                        }
                    }
                }
                /*else
                {
                    serviceInfo.Content = "Введите число!";
                }*/
            }
        }

        private void printButton3_Click(object sender, RoutedEventArgs e)
        {
            //PrintDialog dialogUser = new PrintDialog();
            if (dialogUser.ShowDialog() == true)
            {
                int counter;
                int startNum;
                int endNum;
                int startSign;
                int signCounter = 0;

                if (Int32.TryParse(startNumber.Text, out startNum))
                {
                    //Проверка количества копий для печати
                    if (!Int32.TryParse(qty3.Text, out counter))
                    {
                        counter = 1;
                    }
                    //Проверка диапазона ID номеров
                    if (!Int32.TryParse(endNumber.Text, out endNum))
                    {
                        endNum = startNum;
                    }
                    else if (endNum < startNum)
                    {
                        statusBar.Items[0] = "Неправильный диапазон";
                    }

                    if (!Int32.TryParse(startSignature.Text, out startSign))
                    {
                        statusBar.Items[0] = "Неправильное значение сигнатуры";
                    }
                    else
                    {
                        Int32.TryParse(startSignature.Text, out startSign);
                        signCounter = startSign;
                        
                    }
                    //Боже, прости грешного за такие IF-ELSE костыли...
                    //Из FOR в FOR тоже прости...
                    for (int i = startNum; i <= endNum; i++) //перебор диапазона
                    {
                        for (int j = 1; j <= counter; j++) //количество
                        {
                            using (DrawingContext dctx = visual.RenderOpen())
                            {
                                FormattedText numb = new FormattedText("№: 000" + i.ToString(),
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface("Times New Roman"), 16, Brushes.Black);
                                FormattedText sign = new FormattedText("sign: " + signCounter.ToString(),
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface("Times New Roman"), 16, Brushes.Black);

                                //Получение размера текста
                                Size textDateSize = new Size(numb.Width, numb.Height);
                                Size textIdSize = new Size(sign.Width, sign.Height);

                                //Установка координатов текста на листе
                                double margin = 96 * 0.03;
                                Point pointNumb = new Point(
                                    (dialogUser.PrintableAreaWidth - textDateSize.Width) / 2 - (margin / 4),
                                    (dialogUser.PrintableAreaHeight - 33) / 2 + (1.6 * margin));
                                Point pointSign = new Point(
                                    (dialogUser.PrintableAreaWidth - textIdSize.Width) / 2 - (margin / 4),
                                    (dialogUser.PrintableAreaHeight - 3) / 2 + (1.6 * margin));

                                //Отрисовка текста
                                dctx.DrawText(numb, pointNumb);
                                dctx.DrawText(sign, pointSign);
                            }

                            //Отправка визуала с отрисованным текстом на печать
                            dialogUser.PrintVisual(visual, "Тестовая печать");
                        }
                        signCounter++;
                    }
                }
                /*else
                {
                    serviceInfo.Content = "Введите число!";
                }*/
            }
        }

        private void printUATR_Click(object sender, RoutedEventArgs e)
        {
            int counter;
            int startNum;
            int endNum;

            //PrintDialog dialogUser = new PrintDialog();

            if(dialogUser.ShowDialog() == true)
            {
                if (Int32.TryParse(startIDUATR.Text, out startNum))
                {
                    //Проверка количества копий для печати
                    if (!Int32.TryParse(qtyUATR.Text, out counter))
                    {
                        counter = 1;
                    }
                    //Проверка диапазона ID номеров
                    if (!Int32.TryParse(endIDUATR.Text, out endNum))
                    {
                        endNum = startNum;
                    }
                    else if (endNum < startNum)
                    {
                        statusBar.Items[0] = "Неправильный диапазон";
                    }
                    // Подготовка текста
                    for (int i = startNum; i <= endNum; i++)
                    {
                        for (int j = 1; j <= counter; j++)
                        {
                            using (DrawingContext dctx = visual.RenderOpen())
                            {
                                //Форматирование текста для печати
                                FormattedText device = new FormattedText("Устройство: " + deviceBox.Text,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface("Times New Roman"),
                                    Convert.ToDouble(textBox_IP101_textSize.Text),
                                    Brushes.Black);
                                device.TextAlignment = TextAlignment.Left;

                                FormattedText id = new FormattedText("Номер: " + i.ToString("000000"),
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface("Times New Roman"),
                                    Convert.ToDouble(textBox_IP101_textSize.Text),
                                    Brushes.Black);
                                id.TextAlignment = TextAlignment.Left;

                                FormattedText sertificate = new FormattedText("Сертификат: " + sertUATR.Text,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface("Times New Roman"),
                                    Convert.ToDouble(textBox_IP101_textSize.Text),
                                    Brushes.Black);
                                sertificate.TextAlignment = TextAlignment.Left;

                                FormattedText date = new FormattedText("Дата: " + dateFieldUATR.Text,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface("Times New Roman"),
                                    Convert.ToDouble(textBox_IP101_textSize.Text),
                                    Brushes.Black);
                                date.TextAlignment = TextAlignment.Left;

                                //Получение размера текста
                                Size textDeviceSize = new Size(device.Width, device.Height);
                                Size textIdSize = new Size(id.Width, id.Height);
                                Size textSertSize = new Size(sertificate.Width, sertificate.Height);
                                Size textDateSize = new Size(date.Width, date.Height);

                                //Установка координатов текста на листе
                                double margin = 96 * 0.03;
                                Point pointDevice = new Point(
                                    (dialogUser.PrintableAreaWidth - textSertSize.Width) / 2 - (margin / 4),
                                    (dialogUser.PrintableAreaHeight - 50) / 2 + (1.6 * margin));
                                Point pointId = new Point(
                                    (dialogUser.PrintableAreaWidth - textSertSize.Width) / 2 - (margin / 4),
                                    (dialogUser.PrintableAreaHeight - 30) / 2 + (1.6 * margin));
                                Point pointSert = new Point(
                                    (dialogUser.PrintableAreaWidth - textSertSize.Width) / 2 - (margin / 4),
                                    (dialogUser.PrintableAreaHeight - 10) / 2 + (1.6 * margin));
                                Point pointDate = new Point(
                                    (dialogUser.PrintableAreaWidth - textSertSize.Width) / 2 - (margin / 4),
                                    (dialogUser.PrintableAreaHeight + 10) / 2 + (1.6 * margin));

                                //Отрисовка текста
                                dctx.DrawText(device, pointDevice);
                                dctx.DrawText(id, pointId);
                                dctx.DrawText(sertificate, pointSert);
                                dctx.DrawText(date, pointDate);
                            }

                            //Отправка визуала с отрисованным текстом на печать
                            dialogUser.PrintVisual(visual, "Тестовая печать");
                        }
                    }
                }
            }
        }

        private void printNAP_Click(object sender, RoutedEventArgs e)
        {
            //PrintDialog dialogUser = new PrintDialog();
            progressBar.Value = 0;
            if (dialogUser.ShowDialog() == true)
            {
                string pattern = @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";
                Regex macReg = new Regex(pattern);
                int counter;
                int startNum;
                int endNum;
                int[] macStart = new int[6];
                //int elem;

                if (Int32.TryParse(textBox_nap_startNumber.Text, out startNum))
                {
                    //Проверка количества копий для печати

                    if (!Int32.TryParse(textBox_nap_qty.Text, out counter))
                    {
                        counter = 1;
                    }
                    //Проверка диапазона серийных номеров

                    if (!Int32.TryParse(textBox_nap_endNumber.Text, out endNum))
                    {
                        endNum = startNum;
                    }
                    else if (endNum < startNum)
                    {
                        statusBar.Items[0] = "Неправильный диапазон";
                    }

                    maxProgressSteps = (double)(counter * (endNum - startNum)); // общее количество листов

                    // Проверка валидности MAC

                    if (!macReg.IsMatch(textBox_nap_mac.Text))
                    {
                        statusBar.Items[0] = "Неверный mac-адрес!";
                    }
                    else
                    {
                        int index = 0;
                        string[] buf = textBox_nap_mac.Text.Split(':');
                        foreach (var str in buf)
                        {
                            Console.Write(str);
                        }
                        Console.WriteLine();
                        foreach (var element in buf)
                        {
                            macStart[index] = Convert.ToInt32(element, 16);
                            index++;
                        }
                    }

                    // Подготовка к печати

                    for (int i = startNum; i <= endNum; i++)
                    {
                        string result = "";

                        for (int el = 0; el < macStart.Length; el++)
                        {
                            if (el != macStart.Length - 1)
                            {
                                result = result + macStart[el].ToString("X2") + ":";
                            }
                            else
                            {
                                result += macStart[el].ToString("X2");
                            }
                        }

                        for (int j = 1; j <= counter; j++)
                        {
                            using (DrawingContext dctx = visual.RenderOpen())
                            {
                                //Форматирование текста для печати

                                // MAC адрес

                                FormattedText napMac = new FormattedText(result,
                                   System.Globalization.CultureInfo.CurrentCulture,
                                   FlowDirection.LeftToRight,
                                   new Typeface(myFont, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                                   Convert.ToDouble(textBox_napTextSize.Text),
                                   Brushes.Black);
                                napMac.TextAlignment = TextAlignment.Left;

                                // Исполнение

                                FormattedText napType = new FormattedText(textBox_nap_type.Text,
                                   System.Globalization.CultureInfo.CurrentCulture,
                                   FlowDirection.LeftToRight,
                                   new Typeface(myFont, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                                   Convert.ToDouble(textBox_napTextSize.Text),
                                   Brushes.Black);
                                napType.TextAlignment = TextAlignment.Left;

                                // Сертификат

                                FormattedText napSert = new FormattedText(textBox_nap_sert.Text,
                                   System.Globalization.CultureInfo.CurrentCulture,
                                   FlowDirection.LeftToRight,
                                   new Typeface(myFont, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                                   Convert.ToDouble(textBox_napTextSize.Text),
                                   Brushes.Black);
                                napSert.TextAlignment = TextAlignment.Left;

                                // Выдан

                                FormattedText napOrg = new FormattedText(textBox_nap_sertOrganization.Text,
                                   System.Globalization.CultureInfo.CurrentCulture,
                                   FlowDirection.LeftToRight,
                                   new Typeface(myFont, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                                   Convert.ToDouble(textBox_napTextSize.Text),
                                   Brushes.Black);
                                napMac.TextAlignment = TextAlignment.Left;

                                // Серийный номер

                                FormattedText napNum = new FormattedText(textBox_NAP_prefix.Text + i.ToString("000000"),
                                System.Globalization.CultureInfo.CurrentCulture,
                                FlowDirection.LeftToRight,
                                new Typeface(myFont, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                                Convert.ToDouble(textBox_napTextSize.Text),
                                Brushes.Black);
                                napNum.TextAlignment = TextAlignment.Left;

                                // Изготовлено

                                FormattedText napDate = new FormattedText(textBox_nap_manufactureDate.Text,
                                System.Globalization.CultureInfo.CurrentCulture,
                                FlowDirection.LeftToRight,
                                new Typeface(myFont, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                                Convert.ToDouble(textBox_napTextSize.Text),
                                Brushes.Black);
                                napDate.TextAlignment = TextAlignment.Left;

                                //Получение размеров текста

                                Size textNapMac         = new Size(napMac.Width, napMac.Height);
                                Size textNapType        = new Size(napType.Width, napType.Height);
                                Size textNapSert        = new Size(napSert.Width, napSert.Height);
                                Size textNapOrg         = new Size(napOrg.Width, napOrg.Height);
                                Size textNapNum         = new Size(napNum.Width, napNum.Height);
                                Size textNapDate        = new Size(napDate.Width, napDate.Height);

                                //Установка координатов текста на листе

                                double margin = 10;

                                Point pointMac = new Point(
                                   (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                   (dialogUser.PrintableAreaHeight - 86) / 2);

                                Point pointType = new Point(
                                   (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                   (dialogUser.PrintableAreaHeight - 43) / 2);

                                Point pointSert = new Point(
                                   (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                   (dialogUser.PrintableAreaHeight + 0) / 2);

                                Point pointOrg = new Point(
                                   (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                   (dialogUser.PrintableAreaHeight + 43) / 2);

                                Point pointNum = new Point(
                                   (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                   (dialogUser.PrintableAreaHeight + 86) / 2);

                                Point pointDate = new Point(
                                   (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                   (dialogUser.PrintableAreaHeight + 129) / 2);

                                // Отрисовка только выбранного текста (серийный номер принудительно)

                                if(checkBox_nap_mac.IsChecked.Value)                dctx.DrawText(napMac,   pointMac);
                                if(checkBox_nap_type.IsChecked.Value)               dctx.DrawText(napType,  pointType);
                                if(checkBox_nap_sert.IsChecked.Value)               dctx.DrawText(napSert,  pointSert);
                                if(checkBox_nap_sertOrganization.IsChecked.Value)   dctx.DrawText(napOrg,   pointOrg);

                                dctx.DrawText(napNum,   pointNum);

                                if(checkBox_nap_date.IsChecked.Value)               dctx.DrawText(napDate,  pointDate);
                            }

                            //Отправка визуала с отрисованным текстом на печать
                            dialogUser.PrintVisual(visual, "NAP " + i.ToString());
                            statusBar.Items[0] = "Готово";
                        }
                        if (macStart[5] == 0xff)
                        {
                            macStart[5] = 0x00;
                            macStart[4] += 1;
                        }
                        else
                        {
                            macStart[5] += 1;
                        }
                    }
                }
                /*else
                {
                    serviceInfo.Content = "Введите число!";
                }*/
            }
        }

        private void calcSignEnd(object sender, TextChangedEventArgs e)
        {
            calcDiff();
        }

        private void calcSignEnd2(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(startSignature.Text))
            {
                calcDiff();
            }
        }

        private void calcDiff()
        {
            int startNum;
            int endNum;
            int startSign;
            int endSign;

            if (!Int32.TryParse(startNumber.Text, out startNum))
            {
                statusBar.Items[0] = "Неправильное значение заводского номера";
            }
            if (!Int32.TryParse(endNumber.Text, out endNum))
            {
                endNum = startNum;
            }

            if (!Int32.TryParse(startSignature.Text, out startSign))
            {
                statusBar.Items[0] = "Неправильное значение сигнатуры";
            }
            else
            {
                Int32.TryParse(startSignature.Text, out startSign);
                endSign = startSign + (endNum - startNum);
                endSignature.Text = endSign.ToString();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            startIDUATR.IsEnabled = false;
            endIDUATR.IsEnabled = false;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            startIDUATR.IsEnabled = true;
            endIDUATR.IsEnabled = true;
        }

        private void image_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(!imageIsOpen)
            {
                tabControl.Opacity = tabControl.Opacity / 10;
                tabControl.IsEnabled = false;
                image_ex.Visibility = Visibility.Visible;
                imageIsOpen = true;
            }
        }

        private void image1_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(imageIsOpen)
            {
                tabControl.Opacity = tabControl.Opacity * 10;
                tabControl.IsEnabled = true;
                image_ex.Visibility = Visibility.Hidden;
                imageIsOpen = false;
            }
        }

        private void checkBox_type_Checked(object sender, RoutedEventArgs e)
        {
            textBox_uatr_type.IsEnabled = true;
        }

        private void checkBox_sert_Checked(object sender, RoutedEventArgs e)
        {
            textBox_uatr_sert.IsEnabled = true;
        }

        private void checkBox_sertOrganization_Checked(object sender, RoutedEventArgs e)
        {
            textBox_uatr_sertOrganization.IsEnabled = true;
        }

        private void checkBox_manufactureDate_Checked(object sender, RoutedEventArgs e)
        {
            textBox_uatr_manufactureDate.IsEnabled = true;
        }

        private void checkBox_type_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox_uatr_type.IsEnabled = false;
        }

        private void checkBox_sert_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox_uatr_sert.IsEnabled = false;
        }

        private void checkBox_sertOrganization_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox_uatr_sertOrganization.IsEnabled = false; ;
        }

        private void checkBox_manufactureDate_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox_uatr_manufactureDate.IsEnabled = false; ;
        }

        private void button_uatr_print_Click(object sender, RoutedEventArgs e)
        {
            int counter;
            int startNum;
            int endNum;

            //PrintDialog dialogUser = new PrintDialog();
            progressBar.Value = 0;
            clearStatusBar();

            if(dialogUser.ShowDialog() == true)
            {
                if (Int32.TryParse(textBox_uatr_startNumber.Text, out startNum))
                {
                    //Проверка количества копий для печати
                    if (!Int32.TryParse(textBox_uatr_qty.Text, out counter))
                    {
                        counter = 1;
                    }
                    //Проверка диапазона ID номеров
                    if (!Int32.TryParse(textBox_uatr_endNumber.Text, out endNum))
                    {
                        endNum = startNum;
                    }
                    else if (endNum < startNum)
                    {
                        statusBar.Items[0] = "Неправильный диапазон";
                    }

                    maxProgressSteps = (double)(counter * (endNum - startNum)); // общее количество листов

                    // Подготовка текста
                    for (int i = startNum; i <= endNum; i++)
                    {
                        for (int j = 1; j <= counter; j++)
                        {
                            using (DrawingContext dctx = visual.RenderOpen())
                            {
                                //Форматирование текста для печати

                                // Исполнение

                                FormattedText uatrType = new FormattedText(textBox_uatr_type.Text,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(myFont, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                                    Convert.ToDouble(textBox_uatrTextSize.Text),
                                    Brushes.Black);
                                uatrType.TextAlignment = TextAlignment.Left;

                                // Сертификат

                                FormattedText uatrSert = new FormattedText(textBox_uatr_sert.Text,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(myFont, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                                    Convert.ToDouble(textBox_uatrTextSize.Text),
                                    Brushes.Black);
                                uatrSert.TextAlignment = TextAlignment.Left;

                                // Кем выдан сертификат

                                FormattedText uatrSertOrganization = new FormattedText(textBox_uatr_sertOrganization.Text,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(myFont, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                                    Convert.ToDouble(textBox_uatrTextSize.Text),
                                    Brushes.Black);
                                uatrSertOrganization.TextAlignment = TextAlignment.Left;

                                //  Серийный номер устройства

                                FormattedText uatrSerialNumber = new FormattedText(i.ToString("000000"),
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(myFont, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                                    Convert.ToDouble(textBox_uatrTextSize.Text),
                                    Brushes.Black);
                                uatrSerialNumber.TextAlignment = TextAlignment.Left;

                                // Дата производства

                                FormattedText uatrManufactureDate = new FormattedText(textBox_uatr_manufactureDate.Text,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(myFont, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                                    Convert.ToDouble(textBox_uatrTextSize.Text),
                                    Brushes.Black);
                                uatrManufactureDate.TextAlignment = TextAlignment.Left;

                                //Получение размера текста

                                Size textUatrTypeSize = new Size(uatrType.Width, uatrType.Height);
                                Size textUatrSertSize = new Size(uatrSert.Width, uatrSert.Height);
                                Size textUatrOrgSize = new Size(uatrSertOrganization.Width, uatrSertOrganization.Height);
                                Size textUatrNumbSize = new Size(uatrSerialNumber.Width, uatrSerialNumber.Height);
                                Size textUatrDateSize = new Size(uatrManufactureDate.Width, uatrManufactureDate.Height);

                                //Установка координатов текста на листе

                                double margin = 5;

                                Point pointUatrType = new Point(
                                    (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                    (dialogUser.PrintableAreaHeight - 50) / 2);
                                Point pointUatrSert = new Point(
                                    (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                    (dialogUser.PrintableAreaHeight + 81) / 2);
                                Point pointUatrOrg = new Point(
                                    (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                    (dialogUser.PrintableAreaHeight + 114) / 2);
                                Point pointUatrNumb = new Point(
                                    (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                    (dialogUser.PrintableAreaHeight + 147) / 2);
                                Point pointUatrDate = new Point(
                                    (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                    (dialogUser.PrintableAreaHeight + 177) / 2);

                                //Отрисовка только выбранных полей (номер - принудительно)

                                if (checkBox_type.IsChecked.Value)
                                {
                                    dctx.DrawText(uatrType, pointUatrType);
                                }

                                if (checkBox_sert.IsChecked.Value)
                                {
                                    dctx.DrawText(uatrSert, pointUatrSert);
                                }

                                if (checkBox_sertOrganization.IsChecked.Value)
                                {
                                    dctx.DrawText(uatrSertOrganization, pointUatrOrg);
                                }

                                dctx.DrawText(uatrSerialNumber, pointUatrNumb);

                                if (checkBox_manufactureDate.IsChecked.Value)
                                {
                                    dctx.DrawText(uatrManufactureDate, pointUatrDate);
                                }
                            }
                            statusBar.Items[0] = "";
                            try
                            {
                                //Отправка визуала с отрисованным текстом на печать
                                dialogUser.PrintVisual(visual, "RM485 " + i.ToString());
                                statusBar.Items[0] = "Готово";
                            }
                            catch (Exception error)
                            {
                                statusBar.Items[0] = error.ToString();
                                throw;
                            }
                        }
                    }
                }
            }
        }

        private void listBox_sert_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // данные о сертификате в статус бар
            statusBar.Items[0] = listBox_sert.SelectedItem.ToString();
            statusBar.Items[1] = sertificate.sertOrganization[listBox_sert.SelectedIndex];
            statusBar.Items[2] = sertificate.sertSerialNumber[listBox_sert.SelectedIndex];
            // подстановка в область ввода
            textBox_uatr_sert.Text = sertificate.sertSerialNumber[listBox_sert.SelectedIndex];
            textBox_uatr_sertOrganization.Text = sertificate.sertOrganization[listBox_sert.SelectedIndex];
        }

        private void checkBox_nap_mac_Checked(object sender, RoutedEventArgs e)
        {
            textBox_nap_mac.IsEnabled = true;
        }

        private void checkBox_nap_type_Checked(object sender, RoutedEventArgs e)
        {
            textBox_nap_type.IsEnabled = true;
        }

        private void checkBox_nap_sert_Checked(object sender, RoutedEventArgs e)
        {
            textBox_nap_sert.IsEnabled = true;
        }

        private void checkBox_nap_sertOrganization_Checked(object sender, RoutedEventArgs e)
        {
            textBox_nap_sertOrganization.IsEnabled = true;
        }

        private void checkBox_nap_date_Checked(object sender, RoutedEventArgs e)
        {
            textBox_nap_manufactureDate.IsEnabled = true;
        }

        private void checkBox_nap_mac_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox_nap_mac.IsEnabled = false;
        }

        private void checkBox_nap_type_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox_nap_type.IsEnabled = false;
        }

        private void checkBox_nap_sert_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox_nap_sert.IsEnabled = false;
        }
        
        private void checkBox_nap_sertOrganization_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox_nap_sertOrganization.IsEnabled = false;
        }

        private void checkBox_nap_date_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox_nap_manufactureDate.IsEnabled = false;
        }

        private void button_rmt_print_Click(object sender, RoutedEventArgs e)
        {
            int counter;
            int startNum;
            int endNum;

            //PrintDialog dialogUser = new PrintDialog();
            progressBar.Value = 0;
            clearStatusBar();

            if(dialogUser.ShowDialog() == true)
            {
                if (Int32.TryParse(textBox_rmt_startNumber.Text, out startNum))
                {
                    //Проверка количества копий для печати
                    if (!Int32.TryParse(textBox_rmt_qty.Text, out counter))
                    {
                        counter = 1;
                    }
                    //Проверка диапазона ID номеров
                    if (!Int32.TryParse(textBox_rmt_endNumber.Text, out endNum))
                    {
                        endNum = startNum;
                    }
                    else if (endNum < startNum)
                    {
                        statusBar.Items[0] = "Неправильный диапазон";
                    }

                    maxProgressSteps = (double)(counter * (endNum - startNum)); // общее количество листов

                    // Подготовка текста
                    for (int i = startNum; i <= endNum; i++)
                    {
                        for (int j = 1; j <= counter; j++)
                        {
                            using (DrawingContext dctx = visual.RenderOpen())
                            {
                                //Форматирование текста для печати

                                // Исполнение

                                FormattedText rmtType = new FormattedText(textBox_rmt_type.Text,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(myFont, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                                    Convert.ToDouble(textBox_rmtTextSize.Text),
                                    Brushes.Black);
                                rmtType.TextAlignment = TextAlignment.Left;

                                // Сертификат

                                FormattedText rmtSert = new FormattedText(textBox_rmt_sert.Text,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(myFont, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                                    Convert.ToDouble(textBox_rmtTextSize.Text),
                                    Brushes.Black);
                                rmtSert.TextAlignment = TextAlignment.Left;

                                // Кем выдан сертификат

                                FormattedText rmtSertOrganization = new FormattedText(textBox_rmt_organization.Text,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(myFont, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                                    Convert.ToDouble(textBox_rmtTextSize.Text),
                                    Brushes.Black);
                                rmtSertOrganization.TextAlignment = TextAlignment.Left;

                                //  Серийный номер устройства

                                FormattedText rmtSerialNumber = new FormattedText(i.ToString("000000"),
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(myFont, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                                    Convert.ToDouble(textBox_rmtTextSize.Text),
                                    Brushes.Black);
                                rmtSerialNumber.TextAlignment = TextAlignment.Left;

                                // Дата производства

                                FormattedText rmtManufactureDate = new FormattedText(textBox_rmt_date.Text,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(myFont, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                                    Convert.ToDouble(textBox_rmtTextSize.Text),
                                    Brushes.Black);
                                rmtManufactureDate.TextAlignment = TextAlignment.Left;

                                //Получение размера текста

                                Size textrmtTypeSize = new Size(rmtType.Width, rmtType.Height);
                                Size textrmtSertSize = new Size(rmtSert.Width, rmtSert.Height);
                                Size textrmtOrgSize = new Size(rmtSertOrganization.Width, rmtSertOrganization.Height);
                                Size textrmtNumbSize = new Size(rmtSerialNumber.Width, rmtSerialNumber.Height);
                                Size textrmtDateSize = new Size(rmtManufactureDate.Width, rmtManufactureDate.Height);

                                //Установка координатов текста на листе

                                double margin = 5;

                                Point pointrmtType = new Point(
                                    (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                    (dialogUser.PrintableAreaHeight - 100) / 2);
                                Point pointrmtSert = new Point(
                                    (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                    (dialogUser.PrintableAreaHeight + 56) / 2);
                                Point pointrmtOrg = new Point(
                                    (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                    (dialogUser.PrintableAreaHeight + 96) / 2);
                                Point pointrmtNumb = new Point(
                                    (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                    (dialogUser.PrintableAreaHeight + 136) / 2);
                                Point pointrmtDate = new Point(
                                    (dialogUser.PrintableAreaWidth - dialogUser.PrintableAreaWidth) + margin,
                                    (dialogUser.PrintableAreaHeight + 176) / 2);

                                //Отрисовка полей

                                dctx.DrawText(rmtType, pointrmtType);
                                dctx.DrawText(rmtSert, pointrmtSert);
                                dctx.DrawText(rmtSertOrganization, pointrmtOrg);
                                dctx.DrawText(rmtSerialNumber, pointrmtNumb);
                                dctx.DrawText(rmtManufactureDate, pointrmtDate);
                            }
                            statusBar.Items[0] = "";

                            //Отправка визуала с отрисованным текстом на печать
                            dialogUser.PrintVisual(visual, "RMT " + i.ToString());
                            statusBar.Items[0] = "Готово";
                        }
                    }
                }
            }
        }

        private void slider_tempTextSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBox_tempTextSize.Text = "Размер текста: " + slider_tempTextSize.Value.ToString();
        }

        private void textBox_tempStartAddr_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            textBox_tempStartAddr.Text = "";
            textBox_tempStartAddr.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0, 0, 0));
            textBox_tempStartAddr.Focusable = true;
        }

        private void textBox_tempStopAddr_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            textBox_tempStopAddr.Text = "";
            textBox_tempStopAddr.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0, 0, 0));
            textBox_tempStopAddr.Focusable = true;
        }

        private void textBox_tempQty_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            textBox_tempQty.Text = "";
            textBox_tempQty.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0, 0, 0));
            textBox_tempQty.Focusable = true;
        }

        private void radioButtonThermoUser_Checked(object sender, RoutedEventArgs e)
        {
            textBox_tempStartAddr.IsEnabled = false;
            textBox_tempStopAddr.IsEnabled  = false;

            tempLabelPrintMode = true;

            textBox_tempUser1.IsEnabled = true;
            textBox_tempUser2.IsEnabled = true;
            textBox_tempUser3.IsEnabled = true;
            textBox_tempUser4.IsEnabled = true;
        }

        private void radioButtonThermoAuto_Checked(object sender, RoutedEventArgs e)
        {
            textBox_tempStartAddr.IsEnabled = true;
            textBox_tempStopAddr.IsEnabled  = true;

            tempLabelPrintMode = false;

            textBox_tempUser1.IsEnabled = false;
            textBox_tempUser2.IsEnabled = false;
            textBox_tempUser3.IsEnabled = false;
            textBox_tempUser4.IsEnabled = false;
        }

        private void clearStatusBar()
        {
            statusBar.Items[0] = "";
            statusBar.Items[1] = "";
            statusBar.Items[2] = "";
        }

        private void button_tempPrint_Click(object sender, RoutedEventArgs e)
        {
            clearStatusBar();
            PrintContextTemp();
        }

        private void PrintContextTemp()
        {
            int counter;
            int startNum;
            int endNum;
            double margin;
            double marginHeight = 18;

            string date = thermoDatePicker.SelectedDate.ToString();
            date = date.Substring(3, 7) + "г.";

            if (dialogThermo.ShowDialog() == true)
            {
                if (Int32.TryParse(textBox_tempStartAddr.Text, out startNum))
                {
                    //Проверка количества копий для печати
                    if (!Int32.TryParse(textBox_tempQty.Text, out counter))
                    {
                        counter = 1;
                    }
                    //Проверка диапазона ID номеров
                    if (!Int32.TryParse(textBox_tempStopAddr.Text, out endNum))
                    {
                        endNum = startNum;
                    }
                    else if (endNum < startNum)
                    {
                        statusBar.Items[0] = "Неправильный диапазон";
                    }
                }
                else
                {
                    startNum = 1;
                    endNum = 4;
                    counter = 1;
                }

                maxProgressSteps = (double)(counter * (endNum - startNum)) / 4; // общее количество листов

                if (tempLabelPrintMode == false)
                {
                    //if (tempLabelTypeFlag == true)       // проверяем тип наклейки; true = 4 в ряд, false = 1 в ряд
                    //{
                        for (int i = startNum; i <= endNum; i += 4)
                        {
                            for (int j = 1; j <= counter; j++)
                            {
                                using (DrawingContext thermoDrawingContext = visual.RenderOpen())
                                {
                                    //Форматирование текста для печати

                                    margin = dialogThermo.PrintableAreaWidth / 4;

                                    FormattedText label1 = new FormattedText(
                                        textBox_tempLabelPrefix.Text + (i).ToString("0000"),
                                        System.Globalization.CultureInfo.CurrentCulture,
                                        FlowDirection.LeftToRight,
                                        new Typeface("Calibri"),
                                        slider_tempTextSize.Value,
                                        Brushes.Black);

                                    FormattedText label2 = new FormattedText(
                                        textBox_tempLabelPrefix.Text + (i + 1).ToString("0000"),
                                        System.Globalization.CultureInfo.CurrentCulture,
                                        FlowDirection.LeftToRight,
                                        new Typeface("Calibri"),
                                        slider_tempTextSize.Value,
                                        Brushes.Black);

                                    FormattedText label3 = new FormattedText(
                                        textBox_tempLabelPrefix.Text + (i + 2).ToString("0000"),
                                        System.Globalization.CultureInfo.CurrentCulture,
                                        FlowDirection.LeftToRight,
                                        new Typeface("Calibri"),
                                        slider_tempTextSize.Value,
                                        Brushes.Black);

                                    FormattedText label4 = new FormattedText(
                                        textBox_tempLabelPrefix.Text + (i + 3).ToString("0000"),
                                        System.Globalization.CultureInfo.CurrentCulture,
                                        FlowDirection.LeftToRight,
                                        new Typeface("Calibri"),
                                        slider_tempTextSize.Value,
                                        Brushes.Black);

                                    FormattedText formattedDate = new FormattedText(
                                        "изготовлено " + date,
                                        System.Globalization.CultureInfo.CurrentCulture,
                                        FlowDirection.LeftToRight,
                                        new Typeface("Calibri"),
                                        slider_tempTextSize.Value - 0.5,
                                        Brushes.Black);

                                    //Получение размера текста

                                    Size label1Size = new Size(label1.Width, label1.Height);
                                    Size label2Size = new Size(label2.Width, label2.Height);
                                    Size label3Size = new Size(label3.Width, label3.Height);
                                    Size label4Size = new Size(label4.Width, label4.Height);
                                    Size formattedDateSize = new Size(formattedDate.Width, formattedDate.Height);

                                    //Установка координатов текста на листе

                                    Point label1Point = new Point(
                                        (margin - label1.Width / 2) - 45,
                                        ((dialogThermo.PrintableAreaHeight / 2) - marginHeight));
                                    Point date1 = new Point(
                                        (margin - formattedDate.Width / 2) - 41,
                                        ((dialogThermo.PrintableAreaHeight / 2) - (marginHeight - formattedDate.Height)));

                                    Point label2Point = new Point(
                                        (margin * 2 - label2.Width / 2) - 45,
                                        ((dialogThermo.PrintableAreaHeight / 2) - marginHeight));
                                    Point date2 = new Point(
                                        (margin * 2 - formattedDate.Width / 2) - 40,
                                        ((dialogThermo.PrintableAreaHeight / 2) - (marginHeight - formattedDate.Height)));

                                    Point label3Point = new Point(
                                        (margin * 3 - label3.Width / 2) - 40,
                                        ((dialogThermo.PrintableAreaHeight / 2) - marginHeight));
                                    Point date3 = new Point(
                                        (margin * 3 - formattedDate.Width / 2) - 38,
                                        ((dialogThermo.PrintableAreaHeight / 2) - (marginHeight - formattedDate.Height)));

                                    Point label4Point = new Point(
                                        (margin * 4 - label4.Width / 2) - 40,
                                        ((dialogThermo.PrintableAreaHeight / 2) - marginHeight));
                                    Point date4 = new Point(
                                        (margin * 4 - formattedDate.Width / 2) - 37,
                                        ((dialogThermo.PrintableAreaHeight / 2) - (marginHeight - formattedDate.Height)));

                                //Отрисовка текста

                                    thermoDrawingContext.DrawText(label1, label1Point);
                                    thermoDrawingContext.DrawText(formattedDate, date1);
                                    thermoDrawingContext.DrawText(label2, label2Point);
                                    thermoDrawingContext.DrawText(formattedDate, date2);
                                    thermoDrawingContext.DrawText(label3, label3Point);
                                    thermoDrawingContext.DrawText(formattedDate, date3);
                                    thermoDrawingContext.DrawText(label4, label4Point);
                                    thermoDrawingContext.DrawText(formattedDate, date4);
                                }
                                dialogThermo.PrintVisual(visual, textBox_tempLabelPrefix.Text + i.ToString());
                            }
                        }
                    statusBar.Items[0] = "Готово";
                    //}
                    //else
                    //{

                    //}
                }
                else if (tempLabelPrintMode == true)
                {
                    for(uint k = 0; k < counter; k++)
                    {
                        using (DrawingContext thermoDrawingContext = visual.RenderOpen())
                        {
                            //Форматирование текста для печати

                            margin = dialogThermo.PrintableAreaWidth / 4;

                            FormattedText label1 = new FormattedText(
                                textBox_tempUser1.Text,
                                System.Globalization.CultureInfo.CurrentCulture,
                                FlowDirection.LeftToRight,
                                new Typeface("Calibri"),
                                slider_tempTextSize.Value,
                                Brushes.Black);

                            FormattedText label2 = new FormattedText(
                                textBox_tempUser2.Text,
                                System.Globalization.CultureInfo.CurrentCulture,
                                FlowDirection.LeftToRight,
                                new Typeface("Calibri"),
                                slider_tempTextSize.Value,
                                Brushes.Black);

                            FormattedText label3 = new FormattedText(
                                textBox_tempUser3.Text,
                                System.Globalization.CultureInfo.CurrentCulture,
                                FlowDirection.LeftToRight,
                                new Typeface("Calibri"),
                                slider_tempTextSize.Value,
                                Brushes.Black);

                            FormattedText label4 = new FormattedText(
                                textBox_tempUser4.Text,
                                System.Globalization.CultureInfo.CurrentCulture,
                                FlowDirection.LeftToRight,
                                new Typeface("Calibri"),
                                slider_tempTextSize.Value,
                                Brushes.Black);

                            //Получение размера текста

                            Size label1Size = new Size(label1.Width, label1.Height);
                            Size label2Size = new Size(label2.Width, label2.Height);
                            Size label3Size = new Size(label3.Width, label3.Height);
                            Size label4Size = new Size(label4.Width, label4.Height);

                            //Установка координатов текста на листе

                            Point label1Point = new Point(
                                (margin - label1.Width / 2) - 45,
                                ((dialogThermo.PrintableAreaHeight / 2) - marginHeight));

                            Point label2Point = new Point(
                                (margin * 2 - label2.Width / 2) - 45,
                                ((dialogThermo.PrintableAreaHeight / 2) - marginHeight));

                            Point label3Point = new Point(
                                (margin * 3 - label3.Width / 2) - 40,
                                ((dialogThermo.PrintableAreaHeight / 2) - marginHeight));

                            Point label4Point = new Point(
                                (margin * 4 - label4.Width / 2) - 40,
                                ((dialogThermo.PrintableAreaHeight / 2) - marginHeight));

                            //Отрисовка текста

                            thermoDrawingContext.DrawText(label1, label1Point);
                            thermoDrawingContext.DrawText(label2, label2Point);
                            thermoDrawingContext.DrawText(label3, label3Point);
                            thermoDrawingContext.DrawText(label4, label4Point);
                        }
                        dialogThermo.PrintVisual(visual, "Manual: " + textBox_tempUser1.Text);
                    }
                    statusBar.Items[0] = "Готово";
                }
            }
        }

        private void radioButton_tempLabelTypeUni_Checked(object sender, RoutedEventArgs e)
        {
            tempLabelTypeFlag = false;
        }

        private void radioButton_tempLabelTypePoly_Checked(object sender, RoutedEventArgs e)
        {
            tempLabelTypeFlag = true;
        }
    }
}
