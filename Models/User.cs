using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Windows.Controls;

namespace BD.Models
{

    public class User
    {
        public enum TYPE
        {
            Guest = 0,
            Student = 1,
            Teacher = 2,
            Admin = 10,
        }

        string[] names = { "Staszek", "Mathew" , "Franio", "Domino", "Karol"};

        [JsonInclude]
        int ID;

        [JsonInclude]
        string login;

        [JsonInclude]
        string password;

        [JsonInclude]
        public string FirstName;

        public string LastName;

        [JsonInclude]
        string email;

        public TYPE UserType = TYPE.Guest;

        [JsonInclude]
        List<int> Courses = new List<int>();

        string Token = "";

        public User(int id, string login, string pass, string email, string fName, string lName, TYPE type = TYPE.Guest)
        {
            ID = id;
            Login = login;
            Password = pass;
            Email = email;
            FirstName = fName;
            LastName = lName;
            UserType = type;
        }

        public User()
        {
            Random rng = new Random();
            int lSize = 8;
            int pSize = 10;
            string login = "";
            string password = "";

            for (int i = 0; i < lSize; i++)
            {
                login += Convert.ToChar(rng.Next(0, 26) + 65);
            }
            for (int i = 0; i < pSize; i++)
            {
                password += Convert.ToChar(rng.Next(0, 26) + 65);
            }

            ID = rng.Next();
            Login = login;
            this.password = password;
            email = login + "@" + password + ".com";
            FirstName = names[rng.Next(0, 5)];
            LastName = names[rng.Next(0, 5)];
            UserType = TYPE.Guest;
        }

        public void SetID(int id)
        {
            ID = id;
        }

        public bool CorrectLoginData(string login, string pass)
        {
            if (login != Login || pass != Password)
                return false;
            return true;
        }

        public void SetToken(string token)
        {
            if (token != null) 
                Token = token;
        }

        public string Login 
        {
            get => login;
            set => login = value;
        }

        public string Password
        {
            get => password;
            set => password = value;
        }

        public string Email
        {
            get => email;
            set => email = value;
        }
    }
}