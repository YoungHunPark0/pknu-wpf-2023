using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wp08_personalInfoApp_mvvm.Logics;

namespace wp08_personalInfoApp_mvvm.Models
{
    internal class Person
    {
        // private 외부에서 접근불가. 접근할려면 property 만들어야함
        private string email;
        private DateTime date;

        // private에 firstname 등.. Alt+엔터 필드캡슐화
        public string FirstName { get; set; } // firstname과 lastname은 할게없어서 AutoProperty로 처리
        public string LastName { get; set; }
        public string Email 
        { 
            get => email; 
            set
            {
                if (Commons.IsValidEmail(value) != true) // 이 이메일 형식은 일치안함 
                {
                    throw new Exception("유효하지 않은 이메일형식");
                }
                else
                {
                    email = value;
                }
            }
        }
        public DateTime Date
        {
            get => date;
            set 
            {
                var result = Commons.GetAge(value);
                if (result > 120 || result <= 0) // 120살이 넘거나 0살 이하거나
                {
                    throw new Exception("유효하지 않은 생일");
                }
                else
                {
                    date = value;
                }
            }            
        }

        public bool IsAdult
        {
            get => Commons.GetAge(date) > 18; // 만18세] 19살 이상이면 true 
            //{  위는 람다식 밑과 동일
            //    return Commons.GetAge(date) > 18; // 만18세] 19살 이상이면 true 
            //}
        }

        public bool IsBirthDay
        {
            get
            {
                return DateTime.Now.Month == date.Month &&
                    DateTime.Now.Day == date.Day; // 오늘하고 월일 같으면 생일
            }
        }

        public string Zodiac
        {   // 람다식으로 표현해봄
            get => Commons.GetZodiac(date); // 12지로 띠를 받아옴
        }

        public Person(string firstName, string lastName, string email, DateTime date)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Date = date;
        }
    }
}
