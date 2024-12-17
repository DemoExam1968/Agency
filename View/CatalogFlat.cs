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
    public partial class CatalogFlat : Form
    {
        public CatalogFlat()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Подпрограмма для отображения данных
        /// </summary>
        private void ShowFlat()
        {
            //Получить список всех квартир из БД
            List<Model.Flat> flats = Helper.DB.Flat.ToList();
            //Получить список квартир в наличии - статус 1
            List<Model.Flat> flatsStatus1 = flats.Where(x => x.FlatStatusId == 1).ToList();
            
            //Поиск по адресу квартиры
            if (textBoxSearchAdress.Text != "")     //Если что-то введено
            {
                string search = textBoxSearchAdress.Text;   //Строка, введенная пользователем
                //Фильтрация по частичному совпадению с адресом квартиры
                flatsStatus1 = flatsStatus1.Where(x=>x.FlatAdrees.Contains(search)).ToList();
            }

            //Фильтрациии квартир по количеству комнат
            if (comboBoxRooms.SelectedIndex >= 1 && comboBoxRooms.SelectedIndex <= 5)
            {
                flatsStatus1 = flatsStatus1.Where(x => x.FlatRooms == comboBoxRooms.SelectedIndex).ToList();
            }
            else
            {
                if (comboBoxRooms.SelectedIndex==6)
                    flatsStatus1 = flatsStatus1.Where(x => x.FlatRooms >=6).ToList();
            }

            //Фильтрация квартир по району города
            if (comboBoxRegions.SelectedIndex>0)    //Фильтровать, если выбран какой-то район
            {
                flatsStatus1 = flatsStatus1.Where(x => x.FlatRegionId == (int)comboBoxRegions.SelectedValue).ToList();
            }

            //Количество отобранных записей
            labelCountFlat.Text = "Количество квартир в агентстве = " + flatsStatus1.Count.ToString();
            //Отобразить данные о квартирах в таблице
            dataGridViewFlat.Rows.Clear();  //Очистить таблицу от строк
            int numberRow = 0;              //Текущий номер строки таблицы
            //Перебрать все квартиры в списке полученных квартир из БД
            foreach (Model.Flat flat in flatsStatus1)
            {
                dataGridViewFlat.Rows.Add();    //Добавить пустую строку в таблицу
                //Заполнить все ячейки новой строки данными о квартире
                dataGridViewFlat.Rows[numberRow].Cells[0].Value = flat.FlatId.ToString();
                dataGridViewFlat.Rows[numberRow].Cells[1].Value = flat.FlatSquare.ToString();
                dataGridViewFlat.Rows[numberRow].Cells[2].Value = flat.FlatCost.ToString("C2");
                dataGridViewFlat.Rows[numberRow].Cells[3].Value = flat.FlatAdrees.ToString();
                dataGridViewFlat.Rows[numberRow].Cells[4].Value = flat.FlatRooms.ToString();
                dataGridViewFlat.Rows[numberRow].Cells[5].Value = flat.Region.RegionName.ToString();
                numberRow++;
            }
        }

        /// <summary>
        /// Переход к пердыдущему окну
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Отобразить информацию о квартирах
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CatalogFlat_Load(object sender, EventArgs e)
        {
            //Выбрать первый элемент из списка
            this.comboBoxRooms.SelectedIndex = 0;
            //Настройка списка районов из БД в ComboBox
            //Получить данные о районах из БД
            List<Model.Region> regions = Helper.DB.Region.ToList();
            //Добавить в спосок районов "Все раоны"
            Model.Region region = new Model.Region();
            region.RegionName = "Все районы";
            region.RegionId = 0;
            //Добавить новый район в список районов из БД
            regions.Insert(0, region);
            //Поместить в ComboBox все данные о районах
            comboBoxRegions.DataSource = regions;
            //Настройка ComboBox на отображение названий районов
            comboBoxRegions.DisplayMember = "RegionName";
            //Настройка ComboBox для получения id выбранного района
            comboBoxRegions.ValueMember = "RegionId";
            
            comboBoxRegions.SelectedIndex = 0;
            ShowFlat();
        }

        /// <summary>
        /// Ввод адреса для поиска квартиры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSearchAdress_TextChanged(object sender, EventArgs e)
        {
            ShowFlat();
        }

        /// <summary>
        /// При выборе элемента из списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowFlat();
        }

        /// <summary>
        /// Фильтрация квартир по району города
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxRegions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowFlat();
        }

        /// <summary>
        /// Перейти на форму с подробным описание выбранной квартиры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonInfo_Click(object sender, EventArgs e)
        {
            //Если есть выбранная строка в таблице - квартира
            if(dataGridViewFlat.SelectedRows.Count > 0)
            {
                //Получить Id этой квартиры
                //Обратится к левому стобцу (индекс 0) выделенной строки
                string temp = dataGridViewFlat.CurrentRow.Cells[0].Value.ToString();
                int idFlat = Convert.ToInt32(temp);
                //Открыть окно просмотра одной квартиры с передачей id выбранной квартиры
                //В окне View.FlatInfo надо будет создать конструктор с параметром
                View.FlatInfo flatInfo = new View.FlatInfo(idFlat);
                this.Hide();
                flatInfo.ShowDialog();
                this.Show();
            }
        }

        /// <summary>
        /// Кнопка доступна только Юристу для добавления новой квартиры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            View.FlatAddition flatAddition = new View.FlatAddition();
            this.Hide();
            flatAddition.ShowDialog();
            this.Show();
        }
    }
}
