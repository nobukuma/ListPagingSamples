using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;

namespace StrawhatNet.Samples.LongListSelectorPaging
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

            this.lls.ItemsSource = this.source;
            this.lls.ItemRealized += new EventHandler<ItemRealizationEventArgs>(realized_handler);

            return;
        }

        private void realized_handler(object sender, ItemRealizationEventArgs e)
        {
            int listItemsCount = (this.lls.ItemsSource as ObservableCollection<string>).Count();
            int sourceCount = this.source.Count;

            // ItemsSourceの最後の要素がリストに表示されたら、
            // データソースから次のデータを取得する。
            if ((e.Container.Content as string) ==
                    (this.lls.ItemsSource as ObservableCollection<string>)[listItemsCount - 1])
            {
                this.LoadMore(sourceCount, FetchSize);
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