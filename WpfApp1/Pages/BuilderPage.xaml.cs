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
        // Список для хранения текущей сборки
        private List<basepart_> _currentAssembly = new List<basepart_>();

        public BuilderPage()
        {
            InitializeComponent();
            LoadFilters();
            UpdateCatalog();
        }

        // Загрузка данных в ComboBox
        private void LoadFilters()
        {
            // Загружаем производителей
            var manufacturers = Core.Context.manufacturer_.ToList();
            manufacturers.Insert(0, new manufacturer_ { id = 0, name = "Все производители" });
            ManufacturerFilter.ItemsSource = manufacturers;
            ManufacturerFilter.DisplayMemberPath = "name";
            ManufacturerFilter.SelectedIndex = 0;

            // Загружаем типы запчастей
            var types = Core.Context.parttype_.ToList();
            types.Insert(0, new parttype_ { id = 0, name = "Все типы" });
            TypeFilter.ItemsSource = types;
            TypeFilter.DisplayMemberPath = "name";
            TypeFilter.SelectedIndex = 0;
        }

        // Метод фильтрации
        private void UpdateCatalog()
        {
            var data = Core.Context.basepart_.ToList();

            // Поиск по тексту
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                data = data.Where(p => p.name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();
            }

            // Фильтр по производителю
            if (ManufacturerFilter.SelectedIndex > 0)
            {
                var selectedManuf = (manufacturer_)ManufacturerFilter.SelectedItem;
                data = data.Where(p => p.manufacturerid == selectedManuf.id).ToList();
            }

            // Фильтр по типу
            if (TypeFilter.SelectedIndex > 0)
            {
                var selectedType = (parttype_)TypeFilter.SelectedItem;
                data = data.Where(p => p.parttypeid == selectedType.id).ToList();
            }

            CatalogList.ItemsSource = data;
        }

        // Обработчики фильтров
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => UpdateCatalog();
        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateCatalog();

        // Кнопка "Добавить"
        private void BtnAddPart_Click(object sender, RoutedEventArgs e)
        {
            // 1. Получаем выбранную деталь из кнопки
            var selectedPart = (sender as Button).DataContext as basepart_;

            if (selectedPart != null)
            {
                // 2. Ищем, есть ли в текущей сборке деталь ТАКОГО ЖЕ ТИПА
                // (сравниваем по parttypeid)
                var existingPart = _currentAssembly.FirstOrDefault(p => p.parttypeid == selectedPart.parttypeid);

                if (existingPart != null)
                {
                    // 3. Если нашли — удаляем старую
                    _currentAssembly.Remove(existingPart);
                }

                // 4. Добавляем новую деталь
                _currentAssembly.Add(selectedPart);

                // 5. Обновляем интерфейс и проверку совместимости
                RefreshAssemblyList();
                CheckCompatibility();
            }
        }

        // Кнопка "Удалить" (X)
        private void BtnRemovePart_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is basepart_ partToRemove)
            {
                _currentAssembly.Remove(partToRemove);
                RefreshAssemblyList();
            }
        }

        // Обновление правой колонки
        private void RefreshAssemblyList()
        {
            AssemblyList.ItemsSource = null;
            AssemblyList.ItemsSource = _currentAssembly;

            // Считаем цену
            decimal total = _currentAssembly.Sum(p => p.price);
            TotalPriceText.Text = $"Итого: {total:F0} ₽";

            // Запускаем проверку совместимости
            CheckCompatibility();
        }

        private void CheckCompatibility()
        {
            // 1. Получаем список ошибок из нашего нового класса
            var errors = CompatibilityChecker.Check(_currentAssembly);

            // 2. Дополнительная проверка: считаем сборку "незавершенной", если в ней меньше 3 деталей
            // (Например: Процессор, Мать и БП — это минимум)
            bool isIncomplete = _currentAssembly.Count < 3;

            // 3. Выводим ошибки в текстовый блок
            if (errors.Count > 0)
            {
                CompatibilityWarningText.Text = string.Join("\n", errors);
                CompatibilityWarningText.Visibility = Visibility.Visible;
            }
            else
            {
                CompatibilityWarningText.Visibility = Visibility.Collapsed;
            }

            // 4. Управляем кнопкой сохранения:
            // Кнопка активна ТОЛЬКО если ошибок 0 И сборка не пустая
            // Твоя кнопка в XAML называется BtnSaveAssembly_Click (событие), 
            // но в XAML у нее нет x:Name. Давай добавим его.
            BtnSaveAssembly.IsEnabled = (errors.Count == 0 && !isIncomplete);

            // Подсказка для пользователя на кнопке
            if (errors.Count > 0)
                BtnSaveAssembly.Content = "⚠️ Ошибка совместимости";
            else if (isIncomplete)
                BtnSaveAssembly.Content = "Сборка не завершена";
            else
                BtnSaveAssembly.Content = "Сохранить сборку";
        }

        // Кнопка "Сохранить" (заглушка для компиляции)
        private void BtnSaveAssembly_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверки перед сохранением
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
                // 2. Создаем запись в таблице assembly_
                var newAssembly = new assembly_
                {
                    name = AssemblyNameBox.Text,
                    author = string.IsNullOrWhiteSpace(AuthorNameBox.Text) ? "Аноним" : AuthorNameBox.Text
                    // Поле partassembly_ EF заполнит сам позже, либо оно останется пустым при создании
                };

                Core.Context.assembly_.Add(newAssembly);

                // Сохраняем, чтобы получить сгенерированный ID сборки
                Core.Context.SaveChanges();

                // 3. Добавляем каждую деталь в таблицу partassembly_
                foreach (var part in _currentAssembly)
                {
                    var link = new partassembly_
                    {
                        assemblyid = newAssembly.id, // ID только что созданной сборки
                        partid = part.id            // ID детали
                    };
                    Core.Context.partassembly_.Add(link);
                }

                // Финальное сохранение всех связей
                Core.Context.SaveChanges();

                MessageBox.Show($"Сборка '{newAssembly.name}' успешно сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // 4. Очищаем форму для новой сборки
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
            // Мы уже на этой странице, можно просто обновить данные
            UpdateCatalog();
        }

        private void BtnGoToHistory_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HistoryPage());
        }
    }
}
