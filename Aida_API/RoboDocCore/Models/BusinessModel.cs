using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocCore.Models
{
    public class BusinessProfileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FormerName { get; set; }
        public string UEN { get; set; }
        public string IncorpDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string IndustryType { get; set; }
        public string Status { get; set; }
        public string StatusDate { get; set; }
               
        public decimal IssuedCapital { get; set; }
        public int IssuedShares { get; set; }
        public string IssuedCurrency { get; set; }
        public string IssuedShareType { get; set; }

        public decimal PaidupCapital { get; set; }
        public int PaidupShares { get; set; }
        public string PaidupCurrency { get; set; }
        public string PaidupShareType { get; set; }

        public string TradingName { get; set; }
        public string Phone { get; set; }
        public string Nature { get; set; }
        public string ClientType { get; set; }
    }

    public class BusinessOfficerModel
    {
        public int OfficerId { get; set; }
        public int MyInfoRequestId { get; set; }
        public int BusinessProfileId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Nationality { get; set; }
        public string BirthDate { get; set; }
        public string BirthCountry { get; set; }
        public string Position { get; set; }
        public string UserRole { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }


        public string NricNo { get; set; }
        public string NricIssueDate { get; set; }
        public string NricIssuePlace { get; set; }
        public string NricExpiryDate { get; set; }
        public string NricIssueCountry { get; set; }
        public string FinNo { get; set; }
        public string FinIssueDate { get; set; }
        public string FinExpiryDate { get; set; }
        public string PassportNo { get; set; }
        public string PassportIssueDate { get; set; }
        public string PassportIssuePlace { get; set; }
        public string PassportExpiryDate { get; set; }
        public string PassportIssueCountry { get; set; }

        public string Sex { get; set; }
        public string Phone { get; set; }
        public string BirthPlace { get; set; }
        public string JoinDate { get; set; }
        public int NumberOfShares { get; set; }

        public string AliasName { get; set; }
        public string ResidentialStatus { get; set; }
        public string Race { get; set; }
        public string PassType { get; set; }
        public string EntityType { get; set; }

    }

    public class BusinessActivityModel
    {
        public int BusinessProfileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class EntityShareholderModel
    {
        public int BusinessProfileId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string FormerName { get; set; }
        public string TradingName { get; set; }
        public string UEN { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string IncorpDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Nature { get; set; }
        public string Status { get; set; }
        public int RepresentativeId { get; set; }
        public string RepresentativeName { get; set; }
    }
}
