using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency
{
    //Данные класса будут доступны в любом месте кода
    public class Helper
    {
        //Объявление объекта связи с БД
        public static Model.DBAgency DB;
        //Объявление объекта пользователя, вошедшего в систему
        public static Model.User user;
    }
}
