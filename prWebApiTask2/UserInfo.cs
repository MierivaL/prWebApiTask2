using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prWebApiTask2
{
    public class UserInfo
    {
        private string _realName, _phoneNumber;
        private DateTime _birthDate;
        public string RealName
        {
            get { return _realName; }
            set
            {
                if ("" == value)
                    throw new Exception("Не введено реальное имя.");
                else
                    _realName = value;
            }
        }
        public DateTime BirthDate {
            get { return _birthDate; } 
            set 
            { 
                if (value > DateTime.Now) 
                    throw new Exception("Указанная дата ещё не наступила"); 
                else _birthDate = value; 
            } 
        }
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                //if ("" == value)
                //    throw new Exception("Не введён номер телефона.");
                //else 
                    try
                    {
                        long.Parse(value);
                        _phoneNumber = value;
                    }
                    catch { throw new FormatException("Неверный формат номера телефона."); }
            }
        } // Номер телефона

        public UserInfo(string RealName, DateTime BirthDate, string PhoneNumber)
        {
            this.RealName = RealName; this.BirthDate = BirthDate; this.PhoneNumber = PhoneNumber;
        }
    }
}
