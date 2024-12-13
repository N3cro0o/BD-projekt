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

        public static TYPE StringToType(string s)
        {
            if (s.ToLower() == "student")
            {
                return TYPE.Student;
            }
            else if (s.ToLower() == "teacher")
            {
                return TYPE.Teacher;
            }
            else if (s.ToLower() == "admin")
            {
                return TYPE.Admin;
            }
            else
            {
                return TYPE.Guest;
            }
        }

        string[] names = { "Staszek", "Mathew" , "Franio", "Domino", "Karol"};

        [JsonInclude]
        int? ID;

        [JsonInclude]
        string? login;

        [JsonInclude]
        string? password;

        [JsonInclude]
        public string? FirstName;

        public string? LastName;

        [JsonInclude]
        string? email;

        public TYPE UserType = TYPE.Guest;

        [JsonInclude]
        List<int> Courses = new List<int>();

        string Token = "";
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
            
        }

        public void SetID(int id)
        {
            ID = id;
        }
        public int? GetID()
        {
            return ID;
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
    }
}