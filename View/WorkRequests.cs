using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agency.View
{
    public partial class WorkRequests : Form
    {
        public WorkRequests()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Подпрограмма для отображения данных о заявках
        /// </summary>
        private void ShowRequests()
        {
            //Получить список всех заявок из БД
            List<Model.Request> requests = Helper.DB.Request.ToList();
            //Получить список моих заявок со статусом "В работе=1"
            requests = requests.Where(x => x.RequestAgentId == Helper.user.UserId &&
                                      x.RequestStatusId==1).ToList();
            labelCountRequests.Text = "Количество моих заявок: " + requests.Count.ToString();

            //Поиск по заявки по ФИО клиента
            if (textBoxSearchClient.Text != "")     //Если что-то введено
            {
                string search = textBoxSearchClient.Text;   //Строка, введенная пользователем
                //Фильтрация по частичному совпадению с ФИО клиента
                requests = requests.Where(x => x.Client.ClientFullName.Contains(search)).ToList();
            }

            //Количество отобранных записей
            labelCountRequests.Text = "Количество моих заявок: " + requests.Count.ToString();

            //Отобразить данные о заявках в таблице
            dataGridViewRequests.Rows.Clear();  //Очистить таблицу от строк
            int numberRow = 0;              //Текущий номер строки таблицы
            //Перебрать все заявки в списке полученных заявок из БД
            foreach (Model.Request request in requests)
            {
                dataGridViewRequests.Rows.Add();    //Добавить пустую строку в таблицу
                //Заполнить все ячейки новой строки данными о заявке
                dataGridViewRequests.Rows[numberRow].Cells[0].Value = request.RequestId.ToString();
                dataGridViewRequests.Rows[numberRow].Cells[1].Value = request.Client.ClientFullName.ToString();
                dataGridViewRequests.Rows[numberRow].Cells[2].Value = request.RequestDate.ToShortDateString();
                dataGridViewRequests.Rows[numberRow].Cells[3].Value = request.Region.RegionName.ToString();
                dataGridViewRequests.Rows[numberRow].Cells[4].Value = request.RequestRooms.ToString();
                numberRow++;
            }
        }

        /// <summary>
        /// Переход к предыдущему окну
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Поиск по частичному совпадению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSearchClient_TextChanged(object sender, EventArgs e)
        {
            ShowRequests();
        }

        private void WorkRequests_Load(object sender, EventArgs e)
        {
            //Заполняем списки клиентов из БД
            List<Model.Client> clients = Helper.DB.Client.ToList();
            comboBoxClient.DataSource = clients;
            comboBoxClient.DisplayMember = "ClientFullName";
            comboBoxClient.ValueMember = "ClientId";
            //Заполняем списки районов из БД
            List<Model.Region> regions = Helper.DB.Region.ToList();
            comboBoxRegion.DataSource = regions;
            comboBoxRegion.DisplayMember = "RegionName";
            comboBoxRegion.ValueMember = "RegionId";
            //Пока не доступно для изменения
            buttonFixed.Visible = false;
            //groupBoxDataRequest.Enabled = false;
            ShowRequests();
        }
    }
}
