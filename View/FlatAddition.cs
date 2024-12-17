using Agency.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;    //Библиотека работы с файлами

namespace Agency.View
{
    public partial class FlatAddition : Form
    {
        //Путь к папкам с картинками относительно exe-файла проекта
        string pathPhotoDocument = Environment.CurrentDirectory + @"\PhotoDocument\";
        string pathPhotoFlat = Environment.CurrentDirectory + @"\PhotoFlat\";
        //Строки с названиями файлов из диалогов
        string nameFilePictureDocument = null;
        string nameFilePictureFlat = null;

        public FlatAddition()
        {
            InitializeComponent();
        }
        
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Загрузка окна и настройка элементов интерфейса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlatAddition_Load(object sender, EventArgs e)
        {
            //Получить список районов из БД
            List<Model.Region> regions = Helper.DB.Region.ToList();
            //Загрузить список в элемент ComboBox
            comboBoxRegion.DataSource = regions;
            //Настроить список районов
            comboBoxRegion.DisplayMember = "RegionName";
            comboBoxRegion.ValueMember = "RegionId";
            comboBoxRegion.SelectedIndex = 0;
            //Получить список владельцев из БД
            List<Model.Client> clients = Helper.DB.Client.ToList();
            //Загрузить список в элемент ComboBox
            comboBoxOwner.DataSource = clients;
            //Настроить список владельцев
            comboBoxOwner.DisplayMember = "ClientFullName";
            comboBoxOwner.ValueMember = "ClientId";
            comboBoxOwner.SelectedIndex = 0;
            //Загрузка Заглушек для фото - пока пустые
            pictureBoxDocument.Load(pathPhotoDocument + "Заглушка.png");
            pictureBoxFlat.Load(pathPhotoFlat + "Заглушка.png");
        }

        /// <summary>
        /// Сохранить в БД созданную квартиру
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFixed_Click(object sender, EventArgs e)
        {
            String error = "";      //Строка с возможными ошибками
            float cost, sq;
            //1. Создали пустой объект- квартиры
            Model.Flat flat = new Model.Flat();
            //2. Заполняем все поля созданного объекта из элементов интерфейса
            //ID клиента из списка клиентов
            flat.FlatClientId = (int)comboBoxOwner.SelectedValue;
            //Проверка адреса на пустое значение
            if (this.textBoxAddress.Text == "")
            {
                //Накапливаем строку совершенных ошибок
                error += "Вы не ввели адрес" + Environment.NewLine;
            }
            else
            { 
                //Заполняем поле адреса объекта
                flat.FlatAdrees = this.textBoxAddress.Text;
            }

            //Проверка площади
            //Проверка, что ввели число, а не строку
            if (float.TryParse(this.textBoxSquare.Text, out sq))
                {
                if (sq<0)   //Если ввели число, но отрицательное
                {
                    error += "Вы ввели площадь как отрицательное число." + Environment.NewLine;
                }
                else
                {   //Все хорошо-заполняем поле объекта
                    flat.FlatSquare = sq;
                }
            }
            else
            {
                //Накапливаем строку совершенных ошибок
                error += "Вы ввели площадь как не число." + Environment.NewLine;
            }

            //Проверка цены - аналогично площади
            if (float.TryParse(this.textBoxCost.Text, out cost))
            {
                if (cost < 0)
                {
                    error += "Вы ввели цену как отрицательное число." + Environment.NewLine;
                }
                else
                {
                    flat.FlatCost = cost;
                }
            }
            else
            {
                error += "Вы ввели цену как не число." + Environment.NewLine;
            }
            //Остальные поля заполняем из элементов интерфейса
            flat.FlatFloor = (int)this.numericUpDownFloor.Value;
            flat.FlatRooms = (int) this.numericUpDownRooms.Value;
            flat.FlatBalcony = this.checkBoxBalcony.Checked;
            flat.FlatRegionId = (int)comboBoxRegion.SelectedValue;
            flat.FlatStatusId = 1;      //Статус - "В наличии"
            flat.FlatCondition = this.textBoxCondition.Text;
            flat.FlatDescription = this.textBoxDescription.Text;

            //Проверка, что совершились ошибки при заполнении
            if (error != "")
            {
                //Выводим весь список накопленных ошибок
                MessageBox.Show(error);
                return;
            }
            else        //Ошибок нет
            {
                //3. Помещаем заполненный объект в виртуальную таблицу
                Helper.DB.Flat.Add(flat);
                //4. Сохраняем изменения в физической БД
                try    //Контроль совершаемых действий
                {
                    Helper.DB.SaveChanges();
                    MessageBox.Show("Квартира добавлена в каталог агентства");
                    //Найти номер добавленной квартиры для фото
                    int idMax = Helper.DB.Flat.Max(f => f.FlatId);
                    //Имя файла фото документа
                    string nameFilePhotoDocumentDist = pathPhotoDocument + idMax + ".jpg";
                    //Перенос файла с картинкой документа в папку с картинками документов
                    File.Copy(nameFilePictureDocument, nameFilePhotoDocumentDist);
                    // Имя файла фото вида квартиры
                    string nameFilePhotoFlatDist = pathPhotoFlat + idMax + ".jpg";
                    //Перенос файла с картинкой квартиры в папку с картинками квартир
                    File.Copy(nameFilePictureFlat, nameFilePhotoFlatDist);
                }
                catch
                {
                    MessageBox.Show("При добавлении квартиры возникли проблемы");
                }
            }
        }
        /// <summary>
        /// Подгружает картинку для документа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDocument_Click(object sender, EventArgs e)
        {
            if(this.openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                nameFilePictureDocument = this.openFileDialog1.FileName;
                pictureBoxDocument.Load(nameFilePictureDocument);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFlat_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                nameFilePictureFlat = this.openFileDialog1.FileName;
                pictureBoxFlat.Load(nameFilePictureFlat);
            }
        }
    }
}
