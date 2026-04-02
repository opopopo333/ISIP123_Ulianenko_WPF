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
using WpfApp1.Logic;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameEngine _engine;

        public MainWindow()
        {
            InitializeComponent();
            _engine = new GameEngine();
            LogBox.ItemsSource = _engine.EventLog;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            MainMenuPanel.Visibility = Visibility.Collapsed;
            GameOverPanel.Visibility = Visibility.Collapsed;
            GamePanel.Visibility = Visibility.Visible;

            _engine.StartNewGame();
            UpdateUI();
        }

        private void BtnAttack_Click(object sender, RoutedEventArgs e)
        {
            if (_engine.State != GameEngine.GameState.InCombat) return;

            var target = ListEnemies.SelectedItem as Enemy;
            if (target == null && _engine.CurrentEnemies.Count > 0)
                target = _engine.CurrentEnemies[0]; 

            if (target != null)
            {
                _engine.PlayerTurn(defend: false, target);
                UpdateUI();
            }
            else
            {
                MessageBox.Show("Выберите врага для атаки!");
            }
        }

        private void BtnDefend_Click(object sender, RoutedEventArgs e)
        {
            if (_engine.State != GameEngine.GameState.InCombat) return;
            _engine.PlayerTurn(defend: true);
            UpdateUI();
        }

        private void BtnTakeLoot_Click(object sender, RoutedEventArgs e) { _engine.HandleLoot(true); UpdateUI(); }
        private void BtnSkipLoot_Click(object sender, RoutedEventArgs e) { _engine.HandleLoot(false); UpdateUI(); }

        private void UpdateUI()
        {
            if (_engine.State == GameEngine.GameState.GameOver)
            {
                GameOverPanel.Visibility = Visibility.Visible;
                return;
            }

            TxtFloor.Text = $"Этаж: {_engine.Floor}";
            TxtHp.Text = $"Здоровье: {_engine.CurrentPlayer.CurrentHp}/{_engine.CurrentPlayer.MaxHp}";

            TxtWeapon.Text = $"Оружие: {_engine.CurrentPlayer.EquippedWeapon?.Name} (+{_engine.CurrentPlayer.EquippedWeapon?.AttackBonus} к атаке)";
            TxtArmor.Text = $"Доспех: {_engine.CurrentPlayer.EquippedArmor?.Name} (+{_engine.CurrentPlayer.EquippedArmor?.DefenseBonus} к защите)";

            if (_engine.State == GameEngine.GameState.InCombat)
            {
                LootPanel.Visibility = Visibility.Collapsed;
                ListEnemies.Visibility = Visibility.Visible;
                ListEnemies.ItemsSource = null;
                ListEnemies.ItemsSource = _engine.CurrentEnemies;
            }
            else if (_engine.State == GameEngine.GameState.Looting)
            {
                ListEnemies.Visibility = Visibility.Collapsed;
                LootPanel.Visibility = Visibility.Visible;
                TxtLootDesc.Text = $"Найдено: {_engine.DroppedItem.Name}";
            }
        }
    }
}
