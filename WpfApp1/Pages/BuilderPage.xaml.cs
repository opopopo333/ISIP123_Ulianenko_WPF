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
    /// Логика взаимодействия для BuilderPage.xaml
    /// </summary>
    public partial class BuilderPage : Page
    {
        
        private List<basepart_> _currentAssembly = new List<basepart_>();

        public BuilderPage()
        {
            InitializeComponent();
            LoadFilters();
            UpdateCatalog();
        }

       
        private void LoadFilters()
        {
            
            var manufacturers = Core.Context.manufacturer_.ToList();
            manufacturers.Insert(0, new manufacturer_ { id = 0, name = "Все производители" });
            ManufacturerFilter.ItemsSource = manufacturers;
            ManufacturerFilter.DisplayMemberPath = "name";
            ManufacturerFilter.SelectedIndex = 0;

            
            var types = Core.Context.parttype_.ToList();
            types.Insert(0, new parttype_ { id = 0, name = "Все типы" });
            TypeFilter.ItemsSource = types;
            TypeFilter.DisplayMemberPath = "name";
            TypeFilter.SelectedIndex = 0;
        }

        
        private void UpdateCatalog()
        {
            var data = Core.Context.basepart_.ToList();

            
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                data = data.Where(p => p.name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();
            }

           
            if (ManufacturerFilter.SelectedIndex > 0)
            {
                var selectedManuf = (manufacturer_)ManufacturerFilter.SelectedItem;
                data = data.Where(p => p.manufacturerid == selectedManuf.id).ToList();
            }

            
            if (TypeFilter.SelectedIndex > 0)
            {
                var selectedType = (parttype_)TypeFilter.SelectedItem;
                data = data.Where(p => p.parttypeid == selectedType.id).ToList();
            }

            CatalogList.ItemsSource = data;
        }

        
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => UpdateCatalog();
        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateCatalog();

        
        private void BtnAddPart_Click(object sender, RoutedEventArgs e)
        {
            
            var selectedPart = (sender as Button).DataContext as basepart_;

            if (selectedPart != null)
            {
                
                var existingPart = _currentAssembly.FirstOrDefault(p => p.parttypeid == selectedPart.parttypeid);

                if (existingPart != null)
                {
                    
                    _currentAssembly.Remove(existingPart);
                }

                
                _currentAssembly.Add(selectedPart);

                
                RefreshAssemblyList();
                CheckCompatibility();
            }
        }

        
        private void BtnRemovePart_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is basepart_ partToRemove)
            {
                _currentAssembly.Remove(partToRemove);
                RefreshAssemblyList();
            }
        }

       
        private void RefreshAssemblyList()
        {
            AssemblyList.ItemsSource = null;
            AssemblyList.ItemsSource = _currentAssembly;

            
            decimal total = _currentAssembly.Sum(p => p.price);
            TotalPriceText.Text = $"Итого: {total:F0} ₽";

            
            CheckCompatibility();
        }

        private void CheckCompatibility()
        {
            
            var errors = CompatibilityChecker.Check(_currentAssembly);

            
            bool isIncomplete = _currentAssembly.Count < 3;

           
            if (errors.Count > 0)
            {
                CompatibilityWarningText.Text = string.Join("\n", errors);
                CompatibilityWarningText.Visibility = Visibility.Visible;
            }
            else
            {
                CompatibilityWarningText.Visibility = Visibility.Collapsed;
            }

            
            BtnSaveAssembly.IsEnabled = (errors.Count == 0 && !isIncomplete);

            
            if (errors.Count > 0)
                BtnSaveAssembly.Content = "⚠️ Ошибка совместимости";
            else if (isIncomplete)
                BtnSaveAssembly.Content = "Сборка не завершена";
            else
                BtnSaveAssembly.Content = "Сохранить сборку";
        }

        
        private void BtnSaveAssembly_Click(object sender, RoutedEventArgs e)
        {
           
            if (_currentAssembly.Count == 0)
            {
                MessageBox.Show("Сборка пуста! Добавьте хотя бы одну деталь.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(AssemblyNameBox.Text) || AssemblyNameBox.Text == "Моя супер сборка")
            {
                MessageBox.Show("Введите уникальное название для вашей сборки.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                
                var newAssembly = new assembly_
                {
                    name = AssemblyNameBox.Text,
                    author = string.IsNullOrWhiteSpace(AuthorNameBox.Text) ? "Аноним" : AuthorNameBox.Text
                    
                };

                Core.Context.assembly_.Add(newAssembly);

               
                Core.Context.SaveChanges();

                
                foreach (var part in _currentAssembly)
                {
                    var link = new partassembly_
                    {
                        assemblyid = newAssembly.id, 
                        partid = part.id            
                    };
                    Core.Context.partassembly_.Add(link);
                }

               
                Core.Context.SaveChanges();

                MessageBox.Show($"Сборка '{newAssembly.name}' успешно сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                
                _currentAssembly.Clear();
                AssemblyNameBox.Text = "";
                AuthorNameBox.Text = "";
                RefreshAssemblyList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnGoToBuilder_Click(object sender, RoutedEventArgs e)
        {
            UpdateCatalog();
        }

        private void BtnGoToHistory_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HistoryPage());
        }
    }
}
