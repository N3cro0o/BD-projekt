using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Windows.Controls;
using System.Diagnostics;

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
            if (s.ToLower() == "student" || s.ToLower() == "uczen")
            {
                return TYPE.Student;
            }
            else if (s.ToLower() == "teacher" || s.ToLower() == "nauczyciel")
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

        string[] names = { "Staszek", "Mathew", "Franio", "Domino", "Karol" };

        [JsonInclude]
        int _id;

        public int ID
        {
            get { return GetID(); }
            set { SetID(value); }
        }

        string? login;

        string? password;

        string _firstName = "";

        string _lastName = "";

        [JsonInclude]
        string? email;

        TYPE _userType = TYPE.Guest;

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
            get => _firstName;
            set => _firstName = value;
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = value;
        }

        public TYPE UserType 
        { 
            get => _userType; 
            set => _userType = value;
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

        public void DebugPrintUser()
        {
            Debug.Print("ID {0}, Login {1}, First Name {4}, Last Name {5}\nPassword {2}, Email {3}, Type {6}", ID, Login, Password, Email, FirstName, LastName, UserType.ToString());
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

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        public bool IsEmpty()
        {
            return ID < 0;
        }
    }
}