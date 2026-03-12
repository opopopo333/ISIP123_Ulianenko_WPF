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

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            // Загружаем сборки из БД (включая связанные детали для подсчета суммы)
            LViewAssemblies.ItemsSource = Core.Context.assembly_.ToList();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить эту сборку?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var assembly = (sender as Button).DataContext as assembly_;

                // 1. Сначала удаляем связи из partassembly_
                var links = Core.Context.partassembly_.Where(pa => pa.assemblyid == assembly.id);
                Core.Context.partassembly_.RemoveRange(links);

                // 2. Затем удаляем саму сборку
                Core.Context.assembly_.Remove(assembly);

                Core.Context.SaveChanges();
                RefreshData();
            }
        }
    }
}
