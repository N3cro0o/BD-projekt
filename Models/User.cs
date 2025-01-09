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
        int _id;

        public int ID
        {
            get { return GetID(); }
            set { SetID(value); }
        }

        [JsonInclude]
        string? login;

        [JsonInclude]
        string? password;

        [JsonInclude]
        public string? firstName;

        public string? lastName;

        public string? role;

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
        public string FirstName
        {
            get => firstName;
            set => firstName = value;
        }

        public string LastName
        {
            get => lastName;
            set => lastName = value;
        }
        public string Role
        {
            get => role;
            set => role = value;
        }

        public User(int id, string login, string pass, string email, string fName, string lName, string role, TYPE type = TYPE.Guest)
        {
            ID = id;
            Login = login;
            Password = pass;
            Email = email;
            FirstName = fName;
            LastName = lName;
            Role = role;
            UserType = type;
        }

        public User()
        {
            
        }

        public void SetID(int id)
        {
            _id = id;
        }
        public int GetID()
        {
            return _id;
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