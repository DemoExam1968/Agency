using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agency
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Завершение работы приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Загрузка гравного окна - запуск приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Authorization_Load(object sender, EventArgs e)
        {
            //Подключение к БД
            try
            {
                Helper.DB = new Model.DBAgency();
                MessageBox.Show("Успешное подключение к БД", "Валидация подключения", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Ошибка при подключения к БД","Валидация подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

        }
        /// <summary>
        /// Вход в систему по логину и паролю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonInput_Click(object sender, EventArgs e)
        {
            //Получить логин и пароль от пользователя
            string login = this.textBoxLogin.Text;
            string password = this.textBoxPassword.Text;
            //Получить всех пользователей из БД
            //List<Model.User> users = new List<Model.User>();
            //users = Helper.DB.User.ToList();
            //MessageBox.Show("кол-во пользователей: " + users.Count);

            List<Model.User> users = Helper.DB.User.ToList();
            //Получить список отфильтрованных пользователей по логину и паролю
            List<Model.User> usersLoginPassword = users.Where(u => u.UserLogin == login
                                                && u.UserPassword == password).ToList();
            //MessageBox.Show("кол-во пользователей после фильтрации: " + usersLoginPassword.Count);

            //Получить единственного пользователя или его отсутствие
            Helper.user = usersLoginPassword.FirstOrDefault();
            //Проверка наличия единственного пользователя
            if (Helper.user != null)       //Есть
            {
                MessageBox.Show("Пользователь найден. Вы вошли с ролью " + Helper.user.Role.RoleName,
                    "Валидация пользователя", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                //Переход в окно для функцинала роли
                switch(Helper.user.Role.RoleId)     //Id роли вошедшего пользователя
                {
                    case 1:     //Агент
                        View.MenuAgent menuAgent = new View.MenuAgent();
                        menuAgent.ShowDialog();
                        break;
                    case 3:     //Директор
                        View.MenuDirector menuDirector = new View.MenuDirector();
                        menuDirector.ShowDialog();
                        break;
                    case 4:     //Директор
                        View.MenuAdmin menuAdmin = new View.MenuAdmin();
                        menuAdmin.ShowDialog();
                        break;
                }
                this.Show();
            }
            else               //Отсутствует
            {
                MessageBox.Show("Пользователь не найден", "Валидация пользователя", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// Переход в каталог квартир агентства
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCatalog_Click(object sender, EventArgs e)
        {
            //Создали объект окна каталога
            View.CatalogFlat catalogFlat = new View.CatalogFlat();
            this.Hide();    //Скрывает окно авторизации
            catalogFlat.ShowDialog();   //Отобразить окно каталога
            this.Show();    //Опять отобразить окно авторизации
        }
    }
}
