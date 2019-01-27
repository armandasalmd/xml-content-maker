using System;
using System.ComponentModel;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Generic;

//To kick things off
namespace XmlExercisesMaker
{

    public partial class MainWindow : MetroWindow
    {
        private ALayout mLayout = new ALayout();
        private MyProperties mProperties = new MyProperties();
        private MyAnalyser mAnalyser = new MyAnalyser();

        public class ManoTekstas: INotifyPropertyChanged
        {
            private string _mText;
            public string mText
            {
                get { return _mText; }
                set
                {
                    _mText = value;
                    NotifyPropertyChanged("mText");
                }
            }
            
            public ManoTekstas()
            {
                mText = "<Layout>\n\n</Layout>";
            }

            private void NotifyPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        private ManoTekstas tekstas = new ManoTekstas();

        public enum Buttons
        {
            text, field, nextLine, radio, spinner, check, layout
        }

        public MainWindow()
        {
            InitializeComponent();
            
            mainText.DataContext = tekstas;
            DataContext = new
            {
                ViewTree = mLayout.mTreeList,
                Settings = mProperties.mSettingsView
            };

            mListBox.SelectionChanged += OnSelectionChanged;
            mListBox.SelectedIndex = 0;
        }
        
        private int last_selection = 0;
        private bool JustAfterAction = false;
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (JustAfterAction == false)
            {
                MyTreeItem mItem = (MyTreeItem)mListBox.Items.GetItemAt(mListBox.SelectedIndex);
                PropertiesTitle.Text = mItem.Title;

                mProperties.ShowViewOptions(ref mItem, mListBox.SelectedIndex);
                last_selection = mListBox.SelectedIndex;
            }
            else
                JustAfterAction = false;
            
        }

        private void SaveText(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML file (*.xml)|*.xml|Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, tekstas.mText);
        }
        
        private void Button_CreateNew(object sender, RoutedEventArgs e)
        {
            Buttons btnType = Buttons.text;
            int count = 0;
            bool goMore = true;
            foreach(object child in addPanel1.Children)
            {
                if (child is FrameworkElement && child == sender)
                {
                    switch (count) {
                        case 0: btnType = Buttons.text;
                            break;
                        case 1: btnType = Buttons.field;
                            break;
                        case 2: btnType = Buttons.nextLine;
                            break;
                    }
                    goMore = false;
                    break;
                }
                count++;
            }
            
            if (goMore)
            {
                count = 0;
                foreach (object child in addPanel2.Children)
                {
                    if (child is FrameworkElement && child == sender)
                    {
                        switch (count)
                        {
                            case 0:
                                btnType = Buttons.radio;
                                break;
                            case 1:
                                btnType = Buttons.spinner;
                                break;
                            case 2:
                                btnType = Buttons.check;
                                break;
                        }
                        break;
                    }
                    count++;
                }
            }
            AddNewView(btnType);
        }

        private void AddNewView(Buttons btnType)
        {
            string name = String.Empty;
            switch (btnType) {
                case Buttons.text: name = "Text";
                    break;
                case Buttons.field: name = "Field";
                    break;
                case Buttons.nextLine: name = "NextLine";
                    break;
                case Buttons.radio: name = "RadioGroup";
                    break;
                case Buttons.spinner: name = "Spinner";
                    break;
                case Buttons.check: name = "CheckGroup";
                    break;
            }
            mLayout.AddNewView(name, ALayout.Tags.View, mListBox.SelectedIndex, btnType);
            mListBox.SelectedIndex++;
        }

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            info_window.IsOpen = true;
        }

        private void AboutClose(object sender, RoutedEventArgs e)
        {
            info_window.IsOpen = false;
        }

        private void btnDeleteView(object sender, RoutedEventArgs e)
        {
            if (!mListBox.SelectedItem.GetType().Equals(typeof(MyTreeItem)))
                Console.WriteLine("blogas tipas(btnDeleteView)");
            else
            {
                MyTreeItem mItem = (MyTreeItem)mListBox.SelectedItem;
                if (mItem.Tag != ALayout.Tags.Layout)
                {
                    mListBox.SelectedIndex--;
                    mLayout.DeleteTreeItem(mItem);
                }
                else
                    MessageBox.Show("Layout ištrinti negalima!", "Klaida", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnSaveDetails(object sender, RoutedEventArgs e)
        {
            mProperties.SaveCurrentInstance(); // save info
            tekstas.mText = mAnalyser.GenString(mLayout); // load info to screen
        }


    }
}
