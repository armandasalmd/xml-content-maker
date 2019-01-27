using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExercisesMaker
{
    public class ALayout
    {
        public ObservableCollection<MyTreeItem> mTreeList = new ObservableCollection<MyTreeItem>();

        public enum Tags
        {
            Layout, View, Item
        };        

        public ALayout()
        {
            mTreeList.Add(new MyTreeItem());
        }

        public void AddNewView(string mTitle, Tags mTag, int index, MainWindow.Buttons mViewType)
        {
            MyTreeItem itemToPut = new MyTreeItem(mTitle, mTag, mViewType);
            if (mViewType == MainWindow.Buttons.spinner || mViewType == MainWindow.Buttons.radio || mViewType == MainWindow.Buttons.check)
                itemToPut.View.AddAItem(new AItem("tekstas"));

            if (mTreeList.Count == index + 1)
                mTreeList.Add(itemToPut);
            else
            {
                mTreeList.Add(new MyTreeItem());
                for (int i = mTreeList.Count - 1; index + 1 < i; i--)
                    mTreeList[i] = mTreeList[i - 1];
                mTreeList[index + 1] = itemToPut;
            }
        }

        public void DeleteTreeItem(MyTreeItem item)
        {
            mTreeList.Remove(item);
        }

    }
}
