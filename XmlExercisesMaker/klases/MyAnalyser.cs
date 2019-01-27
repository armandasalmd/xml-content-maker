using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExercisesMaker
{
    public class MyAnalyser
    {
        private string[] ATRIBUTE = new string[] { "type", "content", "style", "align"
            , "textColor", "textSize", "answer", "maxLenght", "correct", "title" };

        private string[] VIEWS = new string[] {
            "<Layout >", "</Layout>",
            "<View >", "</View>", "<View />",
            "<Item />" };

        private const string NEXTLINE = "<View type=\"nextLine\" />";
        private enum PARAM
        {
            type, content, style, align, textColor, textSize, answer, maxLenght, correct, title
        }

        public ObservableCollection<MyTreeItem> mViews;

        public MyAnalyser()
        {
            mViews = new ObservableCollection<MyTreeItem>();
        }

        public string GenString(ALayout layout)
        {
            mViews = layout.mTreeList;
            string answer = string.Empty,
                mLine = string.Empty;
            int writeIndex;

            // <Layout>
            mLine = VIEWS[0]; // <Layout>
            if (mViews[0].View.TextSize > 0)
                mLine = FillTagWith(mLine, PARAM.textSize, mViews[0].View.TextSize.ToString());
            if (mViews[0].View.TextColor != null && mViews[0].View.TextColor != "")
                mLine = FillTagWith(mLine, PARAM.textColor, mViews[0].View.TextColor.ToString());
            answer += mLine;
            // All other tags
            for (int i = 1; i < mViews.Count; i++)
            {
                mLine = GenerateLine(mViews[i]);
                answer += ( "\n\t" + mLine);
            }

            // </Layout>
            answer += ('\n' + VIEWS[1]);

            Console.WriteLine(answer);
            return answer;
        }

        private string GenerateLine(MyTreeItem view) // atskiras view generavimas i tekstine eilute
        {
            if (view.ViewType == MainWindow.Buttons.nextLine)
                return NEXTLINE;

            string answer = "";
            switch(view.ViewType)
            {
                case MainWindow.Buttons.text:
                    {
                        answer = VIEWS[4]; // <View />
                        answer = FillTagWith(answer, PARAM.type, "text");

                        answer = FillTagWith(answer, PARAM.content, view.View.Content); // content
                        if (view.View.Style != AView.Styles.normal) // style
                            answer = FillTagWith(answer, PARAM.style, view.View.Style.ToString());
                        if (view.View.TextColor != null && view.View.TextColor != "") // textColor
                            answer = FillTagWith(answer, PARAM.textColor, view.View.TextColor);
                        if (view.View.TextSize > 0) // textSize
                            answer = FillTagWith(answer, PARAM.textSize, view.View.TextSize.ToString());
                        if (view.View.Align != AView.Aligns.left) // align
                            answer = FillTagWith(answer, PARAM.align, view.View.Align.ToString());

                            break;
                    }
                case MainWindow.Buttons.field:
                    {
                        answer = VIEWS[4]; // <View />
                        answer = FillTagWith(answer, PARAM.type, "field");

                        answer = FillTagWith(answer, PARAM.answer, view.View?.Answer); // answer
                        if (view.View.TextColor != null && view.View.TextColor != "") // textColor
                            answer = FillTagWith(answer, PARAM.textColor, view.View.TextColor);
                        if (view.View.MaxLenght > 0)
                            answer = FillTagWith(answer, PARAM.maxLenght, view.View.MaxLenght.ToString());
                        else
                            answer = FillTagWith(answer, PARAM.maxLenght, "6");

                        break;
                    }
                case MainWindow.Buttons.check:
                    {
                        answer = VIEWS[2]; // <View >
                        answer = FillTagWith(answer, PARAM.type, "group_check");
                        answer = FillTagWith(answer, PARAM.correct, GroupAnswers(view.View));
                        answer = FillGroupTag(view.View, answer);
                        answer += ("\n\t" + VIEWS[4]);

                        break;
                    }
                case MainWindow.Buttons.spinner:
                    {
                        answer = VIEWS[2]; // <View >
                        answer = FillTagWith(answer, PARAM.type, "group_spinner");
                        answer = FillTagWith(answer, PARAM.correct, GroupAnswers(view.View));
                        answer = FillGroupTag(view.View, answer);
                        answer += ("\n\t" + VIEWS[4]);

                        break;
                    }
                case MainWindow.Buttons.radio:
                    {
                        answer = VIEWS[2]; // <View >
                        answer = FillTagWith(answer, PARAM.type, "group_radio");
                        answer = FillTagWith(answer, PARAM.correct, GroupAnswers(view.View));
                        answer = FillGroupTag(view.View, answer);
                        answer += ("\n\t" + VIEWS[4]);

                        break;
                    }
            }
            return answer;
        }

        private string GroupAnswers(AView view)
        {
            string ans = "";

            for (int i = 0; i < view.mItems.Count; i++)
                if (view.mItems[i].IsRight)
                    ans += ((i + 1).ToString() + ',');
            if (ans.Length > 1)
                ans = ans.Substring(0, ans.Length - 1);

            return ans;
        }

        private string FillGroupTag(AView view, string preparedString)
        {
            string temp = "";
            for (int i = 0; i < view.mItems.Count; i++)
            {
                temp = VIEWS[5];
                temp = FillTagWith(temp, PARAM.title, view.mItems[i].Title);
                if (view.mItems[i].TextColor != null && view.mItems[i].TextColor != "")
                    temp = FillTagWith(temp, PARAM.textColor, view.mItems[i].TextColor);
                preparedString += ( "\n\t\t" + temp);
                temp = "";
            }
            return preparedString;
        }

        private string GetStringParam(PARAM par)
        {
            return ATRIBUTE[(int)par];
        }

        private string FillTagWith(string formatedString, PARAM parameter, string value)
        {
            // pasiruosimas
            int writeIndex = PrepareForFill(ref formatedString);
            string sParameter = GetStringParam(parameter);
            // teksto iterpimas
            string extra;
            if (formatedString[formatedString.Length - 2] == '/')
                extra = " />";
            else
                extra = " >";
            string answer = formatedString.Substring(0, writeIndex);
            answer += (sParameter + '=' + '"' + value + '"' + extra);
            return answer;
        }

        private string MakeSpaces(int writeIndex, string str, int spaceCount)
        {
            string temp = "";
            temp = str.Substring(0, writeIndex);
            for (int i = 0; i < spaceCount; i++)
                temp += '*';
            temp += " />";

            return temp;
        }

        private int PrepareForFill(ref string readyString)
        {
            int ans = -1;
            string temp = "";

            if (readyString[readyString.Length - 1] == '>')
            {
                if (readyString[readyString.Length - 2] == '/')
                { // closed tag
                    temp += readyString.Substring(0, readyString.Length - 2);
                    temp += "  />";
                    ans = temp.Length - 4;
                }
                else
                { // open tag
                    temp += readyString.Substring(0, readyString.Length - 1);
                    temp += "  >";
                    ans = temp.Length - 3;
                }
            }else
                return ans;
            readyString = temp;
            return ans;
        }

    }
}
