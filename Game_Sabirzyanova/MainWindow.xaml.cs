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
        }
        /// <summary> Метод, который наносит периодический урон игроку </summary>
        private void AttackPlayer(object sender, System.EventArgs e)
        {
            // Наносим урон в процентном соотношении имеющейся брони
            Player.Health -= Convert.ToInt32(Enemy.Damage * 100f / (100f - Player.Armor));
            // Обновляем характеристики персонажа
            UserInfoPlayer();
        }
        /// <summary> Повышение уровня и обновление данных на UI </summary>
        public void UserInfoPlayer()
        {
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
