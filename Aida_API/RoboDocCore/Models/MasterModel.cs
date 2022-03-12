using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocCore.Models
{
    public class CompanyModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyUEN { get; set; }
        public string IncorpDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string GstRegNo { get; set; }
        public string IndustryType { get; set; }
        public string Status { get; set; }
        public string StatusDate { get; set; }
    }
    public class EmployeeModelWrapper
    {
        public EmployeeModel Master { get; set; }
        public List<DropDownModel> Position { get; set; }
        public List<DropDownModel> Role { get; set; }
    }
    public class EmployeeModel
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string Position { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Userpassword { get; set; }
        public string LoginRole { get; set; }


    }

    public class LoginModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
    public class DropDownModel
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
