using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.IO;

namespace RandomStringGenerator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string SettingFileName =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            +"\\"
            + System.IO.Path.GetFileNameWithoutExtension(System.Windows.Forms.Application.ExecutablePath)
            + ".xml";
        public MainWindow()
        {
            InitializeComponent();
            //Window_Loaded(new object(), new RoutedEventArgs());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectAllBox.IsThreeState = false;
                FixedButton.IsChecked = true;
                SelectAllBox.IsChecked = true;
                LengthBox.Text = "5";
                if (!File.Exists(SettingFileName))
                {
                    CreateXmlFile();
                }
                else
                {
                    XmlReaderSettings readerSettings = new XmlReaderSettings();
                    readerSettings.IgnoreComments = true;
                    XmlReader xmlReader = XmlReader.Create(SettingFileName, readerSettings);
                    XmlDocument document = new XmlDocument();
                    document.Load(xmlReader);
                    xmlReader.Close();
                    XmlElement root = document.DocumentElement;
                    this.Top = Convert.ToInt32(root["Form"].GetAttribute("Top"));
                    this.Left = Convert.ToInt32(root["Form"].GetAttribute("Left"));
                    MainExpander.IsExpanded = Convert.ToBoolean(root["MainExpander"].GetAttribute("Expanded"));
                    NumBox.IsChecked = Convert.ToBoolean(root["NumBox"].GetAttribute("Checked"));
                    UpperBox.IsChecked = Convert.ToBoolean(root["UpperBox"].GetAttribute("Checked"));
                    LowerBox.IsChecked = Convert.ToBoolean(root["LowerBox"].GetAttribute("Checked"));
                    bool isfixed = Convert.ToBoolean(root["IsFixed"].InnerText);
                    FixedButton.IsChecked = isfixed;
                    IndefiniteButton.IsChecked = !isfixed;
                    if (isfixed)
                        FixedButton_Checked(sender, e);
                    else
                        IndefiniteButton_Checked(sender, e);
                    LengthBox.Text = root["Length"].InnerText;
                    MinLengthBox.Text = root["MinLength"].InnerText;
                    MaxLengthBox.Text = root["MaxLength"].InnerText;
                    ExpressionBox.Text = root["Expression"].InnerText;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
            finally
            {
                BeginStoryboard(WindowLoad);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.IgnoreComments = true;
                XmlReader xmlReader = XmlReader.Create(SettingFileName, readerSettings);
                XmlDocument document = new XmlDocument();
                document.Load(xmlReader);
                xmlReader.Close();
                XmlElement root = document.DocumentElement;
                root["Form"].SetAttribute("Top", this.Top.ToString());
                root["Form"].SetAttribute("Left", this.Left.ToString());
                root["MainExpander"].SetAttribute("Expanded", MainExpander.IsExpanded.ToString());
                root["NumBox"].SetAttribute("Checked", NumBox.IsChecked.ToString());
                root["UpperBox"].SetAttribute("Checked", UpperBox.IsChecked.ToString());
                root["LowerBox"].SetAttribute("Checked", LowerBox.IsChecked.ToString());
                root["IsFixed"].InnerText = FixedButton.IsChecked.ToString();
                root["Length"].InnerText = LengthBox.Text;
                root["MinLength"].InnerText = MinLengthBox.Text;
                root["MaxLength"].InnerText = MaxLengthBox.Text;
                root["Expression"].InnerText = ExpressionBox.Text;
                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.Encoding = Encoding.UTF8;
                writerSettings.Indent = true;
                XmlWriter xmlWriter = XmlWriter.Create(SettingFileName, writerSettings);
                document.Save(xmlWriter);
                xmlWriter.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
            finally
            {
                e.Cancel = true;
                this.ShowInTaskbar = false;
                WindowClose.Completed += new EventHandler(WindowClose_Completed);
                BeginStoryboard(WindowClose);
            }
        }

        private void WindowClose_Completed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void CreateXmlFile()
        {
            try
            {
                XmlDocument document = new XmlDocument();
                XmlElement root = document.CreateElement("Setting");
                XmlElement form = document.CreateElement("Form");
                form.SetAttribute("Top", this.Top.ToString());
                form.SetAttribute("Left", this.Left.ToString());
                root.AppendChild(form);
                XmlElement mainexpander = document.CreateElement("MainExpander");
                mainexpander.SetAttribute("Expanded", MainExpander.IsExpanded.ToString());
                root.AppendChild(mainexpander);
                XmlElement numbox = document.CreateElement("NumBox");
                numbox.SetAttribute("Checked", (NumBox.IsChecked == true).ToString());
                root.AppendChild(numbox);
                XmlElement upperbox = document.CreateElement("UpperBox");
                upperbox.SetAttribute("Checked", (UpperBox.IsChecked == true).ToString());
                root.AppendChild(upperbox);
                XmlElement lowerbox = document.CreateElement("LowerBox");
                lowerbox.SetAttribute("Checked", (LowerBox.IsChecked == true).ToString());
                root.AppendChild(lowerbox);
                XmlElement isfixed = document.CreateElement("IsFixed");
                isfixed.InnerText = (FixedButton.IsChecked == true).ToString();
                root.AppendChild(isfixed);
                XmlElement length = document.CreateElement("Length");
                length.InnerText = LengthBox.Text;
                root.AppendChild(length);
                XmlElement minlength = document.CreateElement("MinLength");
                minlength.InnerText = MinLengthBox.Text;
                root.AppendChild(minlength);
                XmlElement maxlength = document.CreateElement("MaxLength");
                maxlength.InnerText = MaxLengthBox.Text;
                root.AppendChild(maxlength);
                XmlElement expression = document.CreateElement("Expression");
                expression.InnerText = ExpressionBox.Text; ;
                root.AppendChild(expression);
                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.Encoding = Encoding.UTF8;
                writerSettings.Indent = true;
                XmlWriter xmlWriter = XmlWriter.Create(SettingFileName, writerSettings);
                document.AppendChild(root);
                document.Save(xmlWriter);
                xmlWriter.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }

        private void MainExpander_Expanded(object sender, RoutedEventArgs e)
        {
            this.Height = 420;
        }

        private void MainExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Collapsed");
            this.Height = 230;
        }

        private void CheckBoxs_Changed(object sender, RoutedEventArgs e)
        {
            SelectAllBox.IsChecked = null;
            if ((NumBox.IsChecked == true) && (UpperBox.IsChecked == true) && (LowerBox.IsChecked == true))
                SelectAllBox.IsChecked = true;
            if ((NumBox.IsChecked == false) && (UpperBox.IsChecked == false) && (LowerBox.IsChecked == false))
                SelectAllBox.IsChecked = false;
        }

        private void SelectAllBox_Changed(object sender, RoutedEventArgs e)
        {
            bool newVal = (SelectAllBox.IsChecked == true);
            NumBox.IsChecked = newVal;
            UpperBox.IsChecked = newVal;
            LowerBox.IsChecked = newVal;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            MainRichTextBox.Document.Blocks.Clear();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ExpressionBox.Text.Trim() == "")
                {
                    bool canParseMin = int.TryParse(MinLengthBox.Text.Trim(), out int minlength);
                    bool canParseMax = int.TryParse(MaxLengthBox.Text.Trim(), out int maxlength);
                    if (canParseMin && canParseMax)
                    {
                        if (minlength > maxlength)
                        {
                            int temp = minlength;
                            minlength = maxlength;
                            maxlength = temp;
                            MinLengthBox.Text = minlength.ToString();
                            MaxLengthBox.Text = maxlength.ToString();
                        }
                    }
                    string Expression = "[";
                    if (NumBox.IsChecked == true)
                        Expression += "0-9,";
                    if (UpperBox.IsChecked == true)
                        Expression += "A-Z,";
                    if (LowerBox.IsChecked == true)
                        Expression += "a-z,";
                    Expression = Expression.TrimEnd(',');
                    Expression += "](";
                    if (FixedButton.IsChecked == true)
                        Expression += LengthBox.Text.Trim();
                    if (IndefiniteButton.IsChecked == true)
                        Expression += MinLengthBox.Text.Trim() + "," + MaxLengthBox.Text.Trim();
                    Expression += ")";
                    StringGenerator stringGenerator = new StringGenerator();
                    string result = stringGenerator.Generator(Expression);
                    MainRichTextBox.Document.Blocks.Clear();
                    Paragraph p = new Paragraph();
                    p.Inlines.Add(new Run(result));
                    MainRichTextBox.Document.Blocks.Add(p);
                }
                else
                {
                    StringGenerator stringGenerator = new StringGenerator();
                    string result = stringGenerator.Generator(ExpressionBox.Text.Trim());
                    MainRichTextBox.Document.Blocks.Clear();
                    Paragraph p = new Paragraph();
                    p.Inlines.Add(new Run(result));
                    MainRichTextBox.Document.Blocks.Add(p);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }

        private void FixedButton_Checked(object sender, RoutedEventArgs e)
        {
            LengthBox.IsEnabled = true;
            MinLengthBox.IsEnabled = false;
            MaxLengthBox.IsEnabled = false;
        }

        private void IndefiniteButton_Checked(object sender, RoutedEventArgs e)
        {
            LengthBox.IsEnabled = false;
            MinLengthBox.IsEnabled = true;
            MaxLengthBox.IsEnabled = true;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Resource.Readme, "帮助");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
