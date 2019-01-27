using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExercisesMaker
{
    public class AItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isRight;
        public bool IsRight
        {
            get { return isRight; }
            set { isRight = value; }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value;
                NotifyChange(value);
            }
        }

        private string textColor;
        public string TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }
        
        public AItem(string mTitle)
        {
            Title = mTitle;
            TextColor = "";
            isRight = false;
        }

        public AItem(string mTitle, string mTextColor)
        {
            Title = mTitle;
            TextColor = mTextColor;
        }

        private void NotifyChange(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}
