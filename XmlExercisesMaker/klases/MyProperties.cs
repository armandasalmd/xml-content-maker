using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace XmlExercisesMaker
{
    public class MyProperties : INotifyPropertyChanged
    {
        private int[] PADDING = new int[] { 7, 9 };
        private int[] MARGIN = new int[] { 6, 0, 6, 5 };
        private int[] TEXTSIZE = new int[] { 10, 40 };
        private string[] STYLES = new string[] { "normal", "bold", "italic" };
        private string[] ALIGNS = new string[] { "left", "center", "right" };
        private int ITEMSLIMIT = 10;

        private int ItemsCount = 2;

        private const int TRANSP = 0x7F;
        private const string BACKCOLOR = "#ccff66";
        private const string ANSGOOD = "#008000";
        private const string ANSBAD = "#e60000";

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<StackPanel> mSettingsView;
        private ListBox mListBox = new ListBox();
        
        private MyTreeItem currentItemInEdit = new MyTreeItem();
        private int layoutListId = -1; 

        // MainWindow.Buttons - text, field, nextLine, radio, spinner, check

        public MyProperties()
        {
            mSettingsView = new ObservableCollection<StackPanel>();
            
        }

        public void ShowViewOptions(ref MyTreeItem editableItem, int layoutId)
        {
            currentItemInEdit = editableItem;
            layoutListId = layoutId;
            mSettingsView.Clear();
            JustLoaded = true;

            switch (editableItem.ViewType)
            {
                case MainWindow.Buttons.text:
                    {
                        mSettingsView.Add(GenTextBox("Tekstas:", currentItemInEdit.View.Content)); // content
                        mSettingsView.Add(GenComboBox("Stilius:", STYLES)); // style
                        mSettingsView.Add(GenColorPicker("Teksto spalva:")); // textColor
                        mSettingsView.Add(GenSlider("Teksto dydis:", TEXTSIZE[0], TEXTSIZE[1], 20)); // textSize
                        mSettingsView.Add(GenComboBox("Lyguotė:", ALIGNS)); // align
                        break;
                    }
                case MainWindow.Buttons.field:
                    {
                        mSettingsView.Add(GenTextBox("Atsakymas:", currentItemInEdit.View.Answer)); // answer
                        mSettingsView.Add(GenColorPicker("Teksto spalva:")); // textColor
                        mSettingsView.Add(GenSlider("Max teksto ilgis:", 1, 20, 6)); // mexLenght
                        break;
                    }
                case MainWindow.Buttons.radio:
                    {
                        mSettingsView.Add(GenItemsList(false));
                        mSettingsView.Add(GenTextBox("Reikšmė:", currentItemInEdit.View.mItems[mListBox.SelectedIndex].Title));
                        mSettingsView.Add(GenColorPicker("Teksto spalva:"));
                        mSettingsView.Add(GenToggleButton("Atsakymas teisingas", "Atsakymas neteisingas"));
                        break;
                    }
                case MainWindow.Buttons.spinner:
                    {
                        mSettingsView.Add(GenItemsList(true));
                        mSettingsView.Add(GenTextBox("Reikšmė:", currentItemInEdit.View.mItems[mListBox.SelectedIndex].Title));
                        mSettingsView.Add(GenColorPicker("Teksto spalva:"));
                        mSettingsView.Add(GenToggleButton("Atsakymas teisingas", "Atsakymas neteisingas"));
                        break;
                    }
                case MainWindow.Buttons.check:
                    {
                        mSettingsView.Add(GenItemsList(true));
                        mSettingsView.Add(GenTextBox("Reikšmė:", currentItemInEdit.View.mItems[mListBox.SelectedIndex].Title));
                        mSettingsView.Add(GenColorPicker("Teksto spalva:"));
                        mSettingsView.Add(GenToggleButton("Atsakymas teisingas", "Atsakymas neteisingas"));
                        break;
                    }
                case MainWindow.Buttons.layout:
                    {
                        mSettingsView.Add(GenSlider("Teksto dydis:", TEXTSIZE[0], TEXTSIZE[1], 20)); // textSize
                        mSettingsView.Add(GenColorPicker("Teksto spalva:")); // textColor
                        break;
                    }
            }
        }

        private StackPanel GenComboBox(string labelText, string[] contents)
        {
            StackPanel mPanel = new StackPanel();
            ComboBox mCombo = new ComboBox();
            Label mlabel = new Label();
            List<string> data = new List<string>(contents);

            mlabel.Content = labelText;
            mCombo.ItemsSource = data;
            mCombo.SelectedIndex = 0;
            mCombo.Margin = new Thickness(MARGIN[0], MARGIN[1], MARGIN[2], MARGIN[3]);
            mPanel.Margin = new Thickness(PADDING[0], PADDING[1], PADDING[0], PADDING[1]);

            // loading current selection
            if (contents == STYLES && currentItemInEdit.View.Style != AView.Styles.normal)
                mCombo.SelectedIndex = (int)currentItemInEdit.View.Style;
            else if (contents == ALIGNS && currentItemInEdit.View.Align != AView.Aligns.left)
                mCombo.SelectedIndex = (int)currentItemInEdit.View.Align;

            mPanel.Background = GetColorFromHexa(BACKCOLOR);
            mPanel.Children.Add(mlabel);
            mPanel.Children.Add(mCombo);
            return mPanel;
        }

        private bool JustLoaded = false;
        private StackPanel GenSlider(string labelText, int from, int to, int _default)
        {
            StackPanel mPanel = new StackPanel();
            StackPanel mLabelPanel = new StackPanel();
            Slider mSlider = new Slider();
            CheckBox mCheck = new CheckBox();
            Label mLabel = new Label();

            mSlider.Tag = _default; // tagas
            mLabel.Content = labelText;
            mLabelPanel.Orientation = Orientation.Horizontal;
            mLabelPanel.Children.Add(mLabel);
            mLabelPanel.Children.Add(mCheck);
            mPanel.Margin = new Thickness(PADDING[0], PADDING[1], PADDING[0], PADDING[1]);
            mSlider.Margin = new Thickness(MARGIN[0], MARGIN[1], MARGIN[2], MARGIN[3]);
            mCheck.Content = "Nenaudoti";
            mSlider.IsEnabled = false;
            mCheck.Checked += (object sender, RoutedEventArgs e) =>
            {
                mSlider.IsEnabled = false;
                mSlider.Opacity = 0.4;
                if (JustLoaded == false)
                {
                    currentItemInEdit.View.TextSize = 0;
                    currentItemInEdit.View.MaxLenght = 0;
                    mSlider.Value = (int)mSlider.Tag;
                }
                else
                    JustLoaded = false;
                
            };
            mCheck.Unchecked += (object sender, RoutedEventArgs e) =>
            {
                mSlider.IsEnabled = true;
                mSlider.Opacity = 1;
            };
            mCheck.IsChecked = true;

            mSlider.Minimum = from;
            mSlider.Maximum = to;
            mSlider.Value = _default;
            
            mSlider.TickFrequency = 1;
            mSlider.IsSnapToTickEnabled = true;
            mSlider.AutoToolTipPlacement = System.Windows.Controls.Primitives.AutoToolTipPlacement.BottomRight;

            // ikelia current item spalva
            if (currentItemInEdit.View.MaxLenght != 0)
            {
                mSlider.Value = Convert.ToDouble(currentItemInEdit.View.MaxLenght);
                mCheck.IsChecked = false;
            }
            else if (currentItemInEdit.View.TextSize != 0)
            {
                mSlider.Value = Convert.ToDouble(currentItemInEdit.View.TextSize);
                mCheck.IsChecked = false;
            }

            mPanel.Background = GetColorFromHexa(BACKCOLOR);
            mPanel.Children.Add(mLabelPanel);
            mPanel.Children.Add(mSlider);
            return mPanel;
        }

        private StackPanel GenColorPicker(string labelText)
        {
            StackPanel mPanel = new StackPanel();
            StackPanel mLabelPanel = new StackPanel();
            ColorPicker mPicker = new ColorPicker();
            CheckBox mCheck = new CheckBox();
            Label mLabel = new Label();
            
            mCheck.Content = "Nenaudoti";
            mPanel.Margin = new System.Windows.Thickness(PADDING[0], PADDING[1], PADDING[0], PADDING[1]);
            mLabel.Content = labelText;
            mLabelPanel.Orientation = Orientation.Horizontal;
            mLabelPanel.Children.Add(mLabel);
            mLabelPanel.Children.Add(mCheck);
            
            mPicker.IsEnabled = false;
            mCheck.Checked += (object sender, RoutedEventArgs e) =>
            {
                mPicker.IsEnabled = false;
                mPicker.Opacity = 0.4;
                mPicker.SelectedColor = Colors.Black;
            };
            mCheck.Unchecked += (object sender, RoutedEventArgs e) =>
            {
                mPicker.IsEnabled = true;
                mPicker.Opacity = 1;
            };
            mCheck.IsChecked = true;

            mPicker.Margin = new System.Windows.Thickness(MARGIN[0], MARGIN[1], MARGIN[2], MARGIN[3]);
            mPicker.UsingAlphaChannel = false;
            mPicker.SelectedColorChanged += (object sender, System.Windows.RoutedPropertyChangedEventArgs<Color?> e) =>
            {
                string color = e.NewValue.ToString();
                currentItemInEdit.View.TextColor = "#" + color.Substring(3);
            };

            // ikelia current item spalva
            if (currentItemInEdit.View.TextColor != null && currentItemInEdit.View.TextColor != "")
            {
                mPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(currentItemInEdit.View.TextColor);
                mCheck.IsChecked = false;
            }

            mPanel.Background = GetColorFromHexa(BACKCOLOR);
            mPanel.Children.Add(mLabelPanel);
            mPanel.Children.Add(mPicker);
            return mPanel;
        }

        private StackPanel GenTextBox(string labelText)
        {
            return GenTextBox(labelText, "");
        }

        private StackPanel GenTextBox(string labelText, string value)
        {
            StackPanel mPanel = new StackPanel();
            Label mLabel = new Label();
            TextBox mTextBox = new TextBox();

            mPanel.Margin = new Thickness(PADDING[0], PADDING[1], PADDING[0], PADDING[1]);
            mLabel.Content = labelText;
            mTextBox.Text = value;
            mTextBox.AcceptsReturn = true;

            BrushConverter MyBrush = new BrushConverter();
            mTextBox.Background = (Brush)MyBrush.ConvertFrom("#66BB6A");
            mTextBox.TextWrapping = TextWrapping.Wrap;
            mTextBox.Margin = new Thickness(MARGIN[0], MARGIN[1], MARGIN[2], MARGIN[3]);

            mPanel.Background = GetColorFromHexa(BACKCOLOR);
            mPanel.Children.Add(mLabel);
            mPanel.Children.Add(mTextBox);

            return mPanel;
        }
        
        private StackPanel GenToggleButton(string labelChecked, string labelUnchecked)
        {
            StackPanel mPanel = new StackPanel();
            ToggleButton mToggle = new ToggleButton();

            mToggle.Content = labelUnchecked;
            mToggle.Background = GetColorFromHexa(ANSBAD);

            mToggle.Click += delegate
            {
                currentItemInEdit.View.mItems[mListBox.SelectedIndex].IsRight = (bool)mToggle.IsChecked; // issaugomas atasakymas
            };

            mToggle.Checked += delegate
            {
                mToggle.Content = labelChecked;
                mToggle.Background = GetColorFromHexa(ANSGOOD);
            };
            mToggle.Unchecked += delegate
            {
                mToggle.Content = labelUnchecked;
                mToggle.Background = GetColorFromHexa(ANSBAD);
            };
            
            mPanel.Margin = new Thickness(PADDING[0], PADDING[1], PADDING[0], PADDING[1]);
            mPanel.Children.Add(mToggle);
            return mPanel;
        }

        private SolidColorBrush GetColorFromHexa(string hexaColor)
        {
            byte R = Convert.ToByte(hexaColor.Substring(1, 2), 16);
            byte G = Convert.ToByte(hexaColor.Substring(3, 2), 16);
            byte B = Convert.ToByte(hexaColor.Substring(5, 2), 16);
            SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(TRANSP, R, G, B));
            return scb;
        }

        public void SaveCurrentInstance()
        {
            switch (currentItemInEdit.ViewType)
            {
                case MainWindow.Buttons.check:
                case MainWindow.Buttons.spinner:
                case MainWindow.Buttons.radio:
                    SaveCurrentListItem();
                    break;
                case MainWindow.Buttons.text:
                    currentItemInEdit.View.Content = ValueTextBox(0);
                    currentItemInEdit.View.Style = Styles(ValueComboBox(1));
                    currentItemInEdit.View.TextColor = ValueColor(2);
                    currentItemInEdit.View.TextSize = ValueSlider(3);
                    currentItemInEdit.View.Align = Aligns(ValueComboBox(4));
                    break;
                case MainWindow.Buttons.field:
                    currentItemInEdit.View.Answer = ValueTextBox(0);
                    currentItemInEdit.View.TextColor = ValueColor(1);
                    currentItemInEdit.View.MaxLenght = ValueSlider(2);
                    break;
                case MainWindow.Buttons.layout:
                    currentItemInEdit.View.TextSize = ValueSlider(0);
                    currentItemInEdit.View.TextColor = ValueColor(1);
                    break;
            }
            
        }

        private AView.Styles Styles(string value)
        {
            for (int i = 0; i < 3; i++)
                if (STYLES[i].Equals(value))
                    switch (i)
                    {
                        case 0: return AView.Styles.normal;
                        case 1: return AView.Styles.bold;
                        case 2: return AView.Styles.italic;
                    }
            return AView.Styles.normal;
        }

        private AView.Aligns Aligns(string value)
        {
            for (int i = 0; i < 3; i++)
                if (ALIGNS[i].Equals(value))
                    switch (i)
                    {
                        case 0: return AView.Aligns.left;
                        case 1: return AView.Aligns.center;
                        case 2: return AView.Aligns.right;
                    }
            return AView.Aligns.left;
        }

        private string ValueTextBox(int row)
        {
            TextBox tb = (TextBox)mSettingsView[row].Children[1];
            return tb.Text;
        }

        private void MakeFocus(int row)
        {
            TextBox tb = (TextBox)mSettingsView[row].Children[1];
            tb.Focus();
            tb.CaretIndex = tb.Text.Length;
            mSettingsView[row].Children[1] = tb;
        }

        private string ValueComboBox(int row)
        {
            ComboBox cb = (ComboBox)mSettingsView[row].Children[1];
            return cb.SelectedItem.ToString();
        }

        private int ValueSlider(int row)
        {
            StackPanel panel = (StackPanel)mSettingsView[row].Children[0];
            CheckBox cb = (CheckBox)panel.Children[1];
            if (cb.IsChecked == false)
            {
                Slider mSlider = (Slider)mSettingsView[row].Children[1];
                return Convert.ToInt32(mSlider.Value);
            }
            else
                return -1;
        }

        private string ValueColor(int row)
        {
            StackPanel panel = (StackPanel)mSettingsView[row].Children[0];
            CheckBox cb = (CheckBox)panel.Children[1];

            if (cb.IsChecked == false)
            {
                ColorPicker cp = (ColorPicker)mSettingsView[row].Children[1];
                string color = "#" + cp.SelectedColor.ToString().Substring(3);
                return color;
            }
            else
                return "";
        }

        // List bullshit
        private bool deleting = false, justSaved = false;
        private int lastSelection = 0;
        private StackPanel GenItemsList(bool multipleAnswer)
        {
            StackPanel mainPanel = new StackPanel(), btnPanel = new StackPanel();
            Button addBtn = new Button(), deleteBtn = new Button();
            mListBox = new ListBox();

            mainPanel.Background = GetColorFromHexa(BACKCOLOR);
            mainPanel.Margin = new Thickness(PADDING[0], PADDING[1], PADDING[0], PADDING[1]);
            int[] mMargin = new int[] { 6, 6 };

            addBtn.Content = "Pridėti";
            addBtn.Margin = new Thickness(mMargin[0], mMargin[1], mMargin[0], mMargin[1]);
            addBtn.Padding = new Thickness(15, 0, 15, 0);

            deleteBtn.Content = "Ištrinti";
            deleteBtn.Margin = new Thickness(mMargin[0], mMargin[1], mMargin[0], mMargin[1]);
            deleteBtn.Padding = new Thickness(15, 0, 15, 0);
            
            mListBox.DisplayMemberPath = "Title";
            mListBox.ItemsSource = currentItemInEdit.View.mItems;
            mListBox.Margin = new Thickness(MARGIN[0], MARGIN[1], MARGIN[2], MARGIN[3]);
            mListBox.SelectionChanged += delegate
            {
                if (lastSelection != -1 && mListBox.SelectedIndex != -1)
                    SaveCurrentListItem(lastSelection, mListBox.SelectedIndex);
                lastSelection = mListBox.SelectedIndex;

                if (mSettingsView.Count >= 2 && deleting == false && justSaved == false)
                    UpdateListSettings();
                else
                {
                    deleting = false;
                    justSaved = false;
                }
            };

            addBtn.Click += delegate
            {
                if (mListBox.Items.Count < ITEMSLIMIT)
                {
                    //SaveCurrentListItem();
                    AddNewListItem("naujas " + ItemsCount.ToString(), mListBox.SelectedIndex);
                    mListBox.SelectedIndex ++;
                    ItemsCount++;
                }
                else
                    System.Windows.MessageBox.Show("Pasiektas limitas (" + ITEMSLIMIT.ToString() + ")" , "Klaida", MessageBoxButton.OK, MessageBoxImage.Information);
            };

            deleteBtn.Click += delegate
            {
                if (mListBox.Items.Count <= 1)
                    System.Windows.MessageBox.Show("Privalo būti bent vienas 'item'", "Klaida", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    if (mListBox.SelectedIndex - 1 >= 0 || mListBox.SelectedIndex - 1 < mListBox.Items.Count)
                    {
                        deleting = true;
                        int selId = mListBox.SelectedIndex;
                        DeleteListItem((AItem)mListBox.SelectedItem);

                        if (selId > 0)
                            mListBox.SelectedIndex = selId - 1;
                        else
                            mListBox.SelectedIndex = 0;
                        ItemsCount--;
                    }
                }
            };

            mListBox.SelectedIndex = 0;
            btnPanel.Orientation = Orientation.Horizontal;
            btnPanel.Children.Add(addBtn);
            btnPanel.Children.Add(deleteBtn);
            btnPanel.HorizontalAlignment = HorizontalAlignment.Center;

            mainPanel.Children.Add(btnPanel);
            mainPanel.Children.Add(mListBox);
            return mainPanel;
        }

        private void UpdateListSettings()
        {
            TextBox tb = (TextBox)mSettingsView[1].Children[1];
            if (mListBox.SelectedIndex > -1 && mListBox.SelectedIndex < mListBox.Items.Count)
                tb.Text = currentItemInEdit.View.mItems[mListBox.SelectedIndex].Title;
            tb.Focus();
            tb.CaretIndex = tb.Text.Length;
            mSettingsView[1].Children[1] = tb;

            StackPanel panel = mSettingsView[2];
            StackPanel helperPanel = (StackPanel)panel.Children[0];
            CheckBox check = (CheckBox)helperPanel.Children[1];
            if (currentItemInEdit.View.mItems[mListBox.SelectedIndex].TextColor != "")
            {
                ColorPicker picker = (ColorPicker)panel.Children[1];
                string color = currentItemInEdit.View.mItems[mListBox.SelectedIndex].TextColor;
                if (color[0] == '#')
                    picker.SelectedColor = (Color)ColorConverter.ConvertFromString(color);
                check.IsChecked = false; // naudosim spalvas
            }
            else
                check.IsChecked = true; // nenaudosim

            ToggleButton mToggle = (ToggleButton)mSettingsView[3].Children[0];
            mToggle.IsChecked = currentItemInEdit.View.mItems[mListBox.SelectedIndex].IsRight;
            mSettingsView[3].Children[0] = mToggle;

            helperPanel.Children[1] = check;
            panel.Children[0] = helperPanel;
        }

        public void AddNewListItem(string mTitle, int index)
        {
            AItem itemToPut = new AItem(mTitle);

            if (currentItemInEdit.View.mItems.Count == index + 1)
                currentItemInEdit.View.mItems.Add(itemToPut);
            else
            {
                currentItemInEdit.View.mItems.Add(new AItem(""));
                for (int i = currentItemInEdit.View.mItems.Count - 1; index + 1 < i; i--)
                    currentItemInEdit.View.mItems[i] = currentItemInEdit.View.mItems[i - 1];
                currentItemInEdit.View.mItems[index + 1] = itemToPut;
            }
        }

        public void DeleteListItem(AItem item)
        {
            currentItemInEdit.View.mItems.Remove(item);
        }

        private void SaveCurrentListItem()
        {
            SaveCurrentListItem(mListBox.SelectedIndex, mListBox.SelectedIndex);
        }

        private void SaveCurrentListItem(int saveItemId, int pickedItemId)
        {
            // button-prideti, button-issaugoti, mListbox-indexChanged
            if (mSettingsView.Count >= 3)
            {
                StackPanel panel = mSettingsView[2];
                StackPanel helperPanel = (StackPanel)panel.Children[0];
                TextBox tb = (TextBox)mSettingsView[1].Children[1];
                ColorPicker picker = (ColorPicker)panel.Children[1];
                CheckBox check = (CheckBox)helperPanel.Children[1];
                AItem itm = new AItem("");
                
                itm.Title = tb.Text;
                if (check.IsChecked == false)
                {
                    string color = "#" + picker.SelectedColor.ToString().Substring(3);
                    itm.TextColor = color;
                }
                else
                    itm.TextColor = "";
                justSaved = true;

                ToggleButton toggle = (ToggleButton)mSettingsView[3].Children[0];
                itm.IsRight = (bool)toggle.IsChecked;

                currentItemInEdit.View.mItems[saveItemId] = itm;
                mListBox.SelectedIndex = pickedItemId;
                UpdateListSettings();
            }
        }
    }
}
