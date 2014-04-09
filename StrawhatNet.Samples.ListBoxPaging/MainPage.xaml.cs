using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;

namespace StrawhatNet.Samples.ListBoxPaging
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const int FetchSize = 50;
        private ObservableCollection<string> source;

        public MainPage()
        {
            this.source = new ObservableCollection<string>();
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadMore(0, FetchSize);

            this.listbox.ItemsSource = this.source;
            this.ListBoxCompressionHandling(this.listbox);

            return;
        }

        // WP7.5 でリストボックスのスクロールエンドで圧縮されるときのイベントを取る
        // http://blogs.msdn.com/b/shintak/archive/2011/08/06/10193347.aspx
        private void ListBoxCompressionHandling(ListBox targetlistbox)
        {
            VisualStateGroup vgroup = new VisualStateGroup();

            // ListBox の初めに定義されている ScrollViewerを取り出す 
            ScrollViewer ListboxScrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(targetlistbox, 0);

            // Visual State はコントロールテンプレートの常に最上位に定義されている 
            FrameworkElement element = (FrameworkElement)VisualTreeHelper.GetChild(ListboxScrollViewer, 0);
            
            // Visual State を取り出しその中から 縦横Compression のVisualStateを取り出す 
            foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(element))
            {
                if (group.Name == "VerticalCompression")
                {
                    vgroup = group;
                }
            }

            //縦横Compressionの状態が変わった時のイベントハンドラ 
            vgroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(ScrollViewer_CurrentStateChanging);
        }

        private void ScrollViewer_CurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            switch (e.NewState.Name)
            {
                case "CompressionTop":
                    break;
                case "CompressionBottom":
                    this.LoadMore(this.source.Count(), FetchSize);
                    break;
                case "NoVerticalCompression":
                    break;
                default:
                    break;
            }
            return;
        }

        private void LoadMore(int startNumber, int number)
        {
            for (int i = 0; i < number; i++)
            {
                this.source.Add(String.Format("{0}{1}_{2}",
                    (i == 0) ? "★" : "",
                    "item", i + startNumber));
            }
            return;
        }
    }
}