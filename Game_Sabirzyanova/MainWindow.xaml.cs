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
using System.Windows.Threading;

namespace Game_Sabirzyanova
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary> Данные игрока </summary>
        public PersonInfo Player = new PersonInfo("Student", 100, 10, 1, 0, 0, 5);
        public PersonInfo Enemy;
        /// <summary> Коллекция противников </summary>
        public List<PersonInfo> Enemys = new List<PersonInfo>();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            UserInfoPlayer();
            // Добавляем данные о противниках в коллекцию
            Enemys.Add(new PersonInfo("Название врага №1", 100, 20, 1, 15, 5, 20));
            Enemys.Add(new PersonInfo("Название врага №2", 20, 5, 1, 5, 2, 5));
            Enemys.Add(new PersonInfo("Название врага №3", 50, 3, 1, 10, 10, 15));
            // Задаём настройки для таймера
            dispatcherTimer.Tick += AttackPlayer;
            // Задаём интервал с которым выполняется таймер
            dispatcherTimer.Interval = new System.TimeSpan(0, 0, 10);
            // Запускаем таймер
            dispatcherTimer.Start();
            SelectEnemy();
        }
        /// <summary> Выбор случайного противника </summary>
        public void SelectEnemy()
        {
            // Выбираем случайный индекс противника
            int Id = new Random().Next(0, Enemys.Count);
            // Создаём экземпляр с данными противника
            Enemy = new PersonInfo(
                Enemys[Id].Name,
                Enemys[Id].Health,
                Enemys[Id].Armor,
                Enemys[Id].Level,
                Enemys[Id].Glasses,
                Enemys[Id].Money,
                Enemys[Id].Damage);
            monster1.Visibility = System.Windows.Visibility.Hidden;
            monster2.Visibility = System.Windows.Visibility.Hidden;
            monster3.Visibility = System.Windows.Visibility.Hidden;
            if (Id == 0)
            {
                monster1.Visibility = System.Windows.Visibility.Visible;
            }
            else if (Id == 1)
            {
                monster2.Visibility = System.Windows.Visibility.Visible;
            }
            else if (Id == 2)
            {
                monster3.Visibility = System.Windows.Visibility.Visible;
            }

            emptyHealth.Content = "Жизненные показатели: " + Enemy.Health;
            emptyArmor.Content = "Броня: " + Enemy.Armor;
        }
        /// <summary> Метод, который наносит периодический урон игроку </summary>
        private void AttackPlayer(object sender, System.EventArgs e)
        {
            // Наносим урон в процентном соотношении имеющейся брони
            Player.Health -= Convert.ToInt32(Enemy.Damage * 100f / (100f - Player.Armor));
            // Обновляем характеристики персонажа
            UserInfoPlayer();
        }
        /// <summary> Метод, который наносит периодический урон врагу </summary>
        private void AttackEnemy(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Если игрок уже проиграл, кликать больше нельзя
            if (Player.Health <= 0) return;
            // Наносим урон в процентном соотношении имеющейся брони
            Enemy.Health -= Convert.ToInt32(Player.Damage * 100f / (100f - Enemy.Armor));
            // Если жизненные показатели меньше или равны 0
            if (Enemy.Health <= 0)
            {
                // Создаем генератор случайных чисел
                Random rnd = new Random();
                // Получаем процент бонуса до 30%
                int Percent = rnd.Next(0, 31);
                // Считаем, сколько это будет в очках опыта 
                int Mnozitel = Convert.ToInt32(Enemy.Glasses * Percent / 100);
                // Увеличиваем очки персонажа c добавлением множителя
                Player.Glasses += (Enemy.Glasses + Mnozitel);
                // Увеличиваем монеты персонажа
                Player.Money += Enemy.Money;
                // Обновляем информацию на UI
                UserInfoPlayer();
                // Выбираем нового противника
                SelectEnemy();
            }
            else
            {
                // Обновляем UI персонажа
                emptyHealth.Content = "Жизненные показатели: " + Enemy.Health;
                emptyArmor.Content = "Броня: " + Enemy.Armor;
            }
        }
        /// <summary> Повышение уровня и обновление данных на UI </summary>
        public void UserInfoPlayer()
        {
            // Если здоровье упало до 0 или меньше
            if (Player.Health <= 0)
            {
                Player.Health = 0; 
                playerHealth.Content = "Жизненные показатели: 0";
                dispatcherTimer.Stop(); // Останавливаем таймер
                MessageBox.Show("Конец игры! Ваши жизненные показатели опустели.");
                return; // Выходим из метода
            }
            // Если уровень персонажа больше чем 100 * уровень персонажа
            if (Player.Glasses > 100 * Player.Level)
            {
                // Увеличиваем уровень на 1
                Player.Level++;
                // Обновляем очки уровня
                Player.Glasses = 0;
                // Увеличиваем здоровье на 100
                Player.Health += 100;
                // Увеличиваем урон на 1
                Player.Damage++;
                // Увеличиваем броню на 1
                Player.Armor++;
            }
            // выводим данные на экран
            playerHealth.Content = "Жизненные показатели: " + Player.Health;
            playerArmor.Content = "Броня: " + Player.Armor;
            playerLevel.Content = "Уровень: " + Player.Level;
            playerGlasses.Content = "Опыт: " + Player.Glasses;
            playerMoney.Content = "Монеты: " + Player.Money;
        }
    }
}
