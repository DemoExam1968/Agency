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

namespace Agency.View
{
    public partial class FlatInfo : Form
    {
        Model.Flat flat;    //Выбранная квартира
        //Путь к папкам с картинками относительно exe-файла проекта
        string pathPhotoDocument = Environment.CurrentDirectory + @"\PhotoDocument\";
        string pathPhotoFlat = Environment.CurrentDirectory + @"\PhotoFlat\";

        public FlatInfo()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="idFlat">id выбранной квартиры</param>
        public FlatInfo(int idFlat)
        {
            InitializeComponent();
            //По idFkat получить полную информацию о квартире из БД 
            flat = Helper.DB.Flat.Where(f=>f.FlatId == idFlat).FirstOrDefault();
        }

        /// <summary>
        /// При закрузке окна отобразить в интерфейсе всю информацию о квартире
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlatInfo_Load(object sender, EventArgs e)
        {
            //Информация заполняется из полей переданной квартиры
            textBoxSquare.Text = flat.FlatCost.ToString();
            textBoxCost.Text = flat.FlatCost.ToString();    
            textBoxAddress.Text = flat.FlatAdrees.ToString();
            textBoxRooms.Text = flat.FlatRooms.ToString();
            //Для получения названия района используется переход через Region
            textBoxRegion.Text = flat.Region.RegionName.ToString();
            if (flat.FlatBalcony)
            {
                labelBalcony.Text = "Балкон есть";
            }
            else
            {
                labelBalcony.Text = "Балкон отсутствует";
            }
            //Для получения данных о владельце используется переход через Client
            textBoxFullName.Text = flat.Client.ClientFullName.ToString();
            maskedTextBoxPhone.Text = flat.Client.ClientTelephon.ToString();
            textBoxEmail.Text = flat.Client.ClientEmail.ToString();

            textBoxCondition.Text = flat.FlatCondition.ToString();
            if (!String.IsNullOrEmpty(flat.FlatDescription))
                textBoxDescription.Text = flat.FlatDescription.ToString();
            //Определяется имя файла для загрузки картинки для документа
            string filePhotoDocument = pathPhotoDocument + flat.FlatId + ".jpg";
            if (!File.Exists(filePhotoDocument))
            {
                filePhotoDocument = pathPhotoDocument + "Заглушка.png";
            }
            pictureBoxDocument.Load(filePhotoDocument);
            //Определяется имя файла для загрузки картинки для вида квартиры
            string filePhotoFlat = pathPhotoFlat + flat.FlatId + ".jpg";
            if (!File.Exists(filePhotoFlat))
            {
                filePhotoFlat = pathPhotoFlat + "Заглушка.png";
            }
            pictureBoxFlat.Load(filePhotoFlat);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
