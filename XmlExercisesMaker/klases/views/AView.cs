using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExercisesMaker
{
    public class AView
    {
        public enum Styles
        {
            normal, bold, italic
        }
        public enum Aligns
        {
            left, center, right
        }

        public ObservableCollection<AItem> mItems = new ObservableCollection<AItem>();

        private MainWindow.Buttons type;
        public MainWindow.Buttons Type
        {
            get { return type; }
            set { type = value; }
        }

        private string content;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private Styles style;
        public Styles Style
        {
            get { return style; }
            set { style = value; }
        }

        private Aligns align;
        public Aligns Align
        {
            get { return align; }
            set { align = value; }
        }


        private string textColor;
        public string TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        private int textSize;
        public int TextSize
        {
            get { return textSize; }
            set { textSize = value; }
        }


        private string answer;
        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }

        private int maxLenght;
        public int MaxLenght
        {
            get { return maxLenght; }
            set { maxLenght = value; }
        }

        public AView()
        {
        }

        public void AddAItem(AItem item)
        {
            mItems.Add(item);
        }

        public ObservableCollection<AItem> GetAItems()
        {
            return mItems;
        }

    }
}
