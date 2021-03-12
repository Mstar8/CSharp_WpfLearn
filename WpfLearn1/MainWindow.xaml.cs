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

namespace WpfLearn1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        const int N = 4;
        // 多维数组声明
        Button[,] buttons = new Button[N, N];
        Random random = new Random();

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            // 打乱顺序
            Shuffle();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            // 生成所有按钮
            GenerateAllButtons();
        }

        private void GenerateAllButtons()
        {
            int x0 = 281, y0 = 30, w = 50, d = 56;
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {

                    int num = r * N + c;
                    Button btn = new Button();
                    // 按钮文本
                    btn.Content = (num + 1).ToString();
                    btn.Height = w;
                    btn.Width = w;
                    btn.HorizontalAlignment = HorizontalAlignment.Left;
                    btn.VerticalAlignment = VerticalAlignment.Top;
                    btn.Margin = new Thickness(x0 + c * d, y0 + r * d, 0, 0);
                    btn.Visibility = Visibility.Visible;
                    // 存储按钮位置
                    btn.Tag = num;

                    btn.Click += Btn_Click;
                    buttons[r, c] = btn;
                    this.mainGrid.Children.Add(btn);
                }
            }
            buttons[N - 1, N - 1].Visibility = Visibility.Hidden;
        }

        private void Shuffle()
        {
            int a, b, c, d;
            // 循环多次随即交换两个按钮内容
            for (int i = 0; i < 100; i++)
            {
                a = random.Next(N);
                b = random.Next(N);
                c = random.Next(N);
                d = random.Next(N);
                Swap(buttons[a, b], buttons[c, d]);
            }
        }

        private void Swap(Button aBtn, Button bBtn)
        {
            object temp = aBtn.Content;
            aBtn.Content = bBtn.Content;
            bBtn.Content = temp;

            // 交换可见性用于与空白按钮交换
            Visibility visibileTemp = aBtn.Visibility;
            aBtn.Visibility = bBtn.Visibility;
            bBtn.Visibility = visibileTemp;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            // sender既是发起事件的对象，将其转换为Button
            Button btn = sender as Button;
            // 空白按钮
            Button blankBtn = FindHiddenBtn();

            // 找到空白按钮，判断是否与当前按钮相邻
            if (IsNeighbor(btn, blankBtn))
            {
                Swap(btn, blankBtn);
                blankBtn.Focus();
            }

            // 判断游戏是否完成
            if (ResultIsOk())
            {
                MessageBox.Show("游戏完成");
            }
        }

        private Button FindHiddenBtn()
        {
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    if (!buttons[r, c].IsVisible)
                        return buttons[r, c];
                }
            }
            return null;
        }

        private bool IsNeighbor(Button aBtn, Button bBtn)
        {
            // 利用Tag得出按钮位置
            int aTag = (int)aBtn.Tag;
            int bTag = (int)bBtn.Tag;
            int r1 = aTag / N, c1 = aTag % N;
            int r2 = bTag / N, c2 = bTag % N;

            // 判断相邻，左右相邻或上下相邻
            if ((r1 == r2) && Math.Abs(c1 - c2) == 1
                || (c1 == c2) && (Math.Abs(r1 - r2) == 1))
                return true;
            return false;
        }

        private bool ResultIsOk()
        {
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    // 悲观准则
                    if ((string)buttons[r, c].Content != (r * N + c + 1).ToString())
                        return false;
                }
            }
            return true;
        }
    }
}
