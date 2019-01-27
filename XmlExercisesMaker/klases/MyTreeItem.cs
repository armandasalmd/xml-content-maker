using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;

namespace XmlExercisesMaker
{

    public class MyTreeItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private AView view = new AView();
        public AView View
        {
            get
            {
                return view;
            }
            set
            {
                view = value;
            }
        }

        private string title; // to be shown in List...
        public string Title
        {
            get { return title; }
            set { title = value;
                NotifyChange(value); }
        }

        private ALayout.Tags tag;
        public ALayout.Tags Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        private MainWindow.Buttons viewType;
        public MainWindow.Buttons ViewType
        {
            get { return viewType; }
            set { viewType = value; }
        }


        public MyTreeItem()
        {
            Tag = ALayout.Tags.Layout;
            Title = "Layout";
            ViewType = MainWindow.Buttons.layout;
        }

        public MyTreeItem(string mTitle, ALayout.Tags mTag, MainWindow.Buttons type)
        {
            Title = mTitle;
            Tag = mTag;
            ViewType = type;
        }

        private void NotifyChange(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
