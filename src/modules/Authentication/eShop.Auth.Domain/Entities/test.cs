using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using eShop.Auth.Domain.Entities;

namespace eShop.Auth.Domain.Entities
{

    public class BaseClass
    {
        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public long? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }


    }

    public class WorkStatus : BaseClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
    public class Country : BaseClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
    public class QueryCategory : BaseClass
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation to join table
        public List<LeadVsQueryCategory> LeadVsQueryCategories { get; set; } = new();
    }
    public class LeadMaster : BaseClass
    {
        public long Id { get; set; }
        public string InquiryNo { get; set; } = string.Empty;
        public string LeadName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int ResponsiblePersonId { get; set; }
        public string ResponsiblePersonName { get; set; } = string.Empty;
        public int WorkStatusId { get; set; }
        public List<LeadDetail> Details { get; set; } = new();
        public bool IsDeleted { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsOutBound { get; set; }


        public List<LeadVsQueryCategory> LeadVsQueryCategories { get; set; } = new();
    }

    public class LeadDetail : BaseClass
    {
        public long Id { get; set; }
        public long LeadMasterId { get; set; }
        public string InquiryNo { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int FromDestinationId { get; set; }
        public int ToDestinationId { get; set; }
        public int PaxSize { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }


        public LeadMaster? LeadMaster { get; set; }
    }

    public class LeadVsQueryCategory : BaseClass
    {
        public long LeadMasterId { get; set; }
        public long QueryCategoryId { get; set; }

        public QueryCategory? QueryCategory { get; set; }
        public LeadMaster? LeadInfo { get; set; }
    }

    public class LeadInformatinoStatusHisotry : LeadDetail
    {
        public long HistoryId { get; set; }
        public int WorkStatusId { get; set; }
        public string WorkStatusName { get; set; } = string.Empty;
    }
}



//modelBuilder.Entity<Country>().HasData(
//    new Country { Id = 1, Name = "Afghanistan", CreatedBy = 1 },
//    new Country { Id = 2, Name = "Albania", CreatedBy = 1 },
//    new Country { Id = 3, Name = "Algeria", CreatedBy = 1 },
//    new Country { Id = 4, Name = "Andorra", CreatedBy = 1 },
//    new Country { Id = 5, Name = "Angola", CreatedBy = 1 },
//    new Country { Id = 6, Name = "Antigua and Barbuda", CreatedBy = 1 },
//    new Country { Id = 7, Name = "Argentina", CreatedBy = 1 },
//    new Country { Id = 8, Name = "Armenia", CreatedBy = 1 },
//    new Country { Id = 9, Name = "Australia", CreatedBy = 1 },
//    new Country { Id = 10, Name = "Austria", CreatedBy = 1 },
//    new Country { Id = 11, Name = "Azerbaijan", CreatedBy = 1 },
//    new Country { Id = 12, Name = "Bahamas", CreatedBy = 1 },
//    new Country { Id = 13, Name = "Bahrain", CreatedBy = 1 },
//    new Country { Id = 14, Name = "Bangladesh", CreatedBy = 1 },
//    new Country { Id = 15, Name = "Barbados", CreatedBy = 1 },
//    new Country { Id = 16, Name = "Belarus", CreatedBy = 1 },
//    new Country { Id = 17, Name = "Belgium", CreatedBy = 1 },
//    new Country { Id = 18, Name = "Belize", CreatedBy = 1 },
//    new Country { Id = 19, Name = "Benin", CreatedBy = 1 },
//    new Country { Id = 20, Name = "Bhutan", CreatedBy = 1 },
//    new Country { Id = 21, Name = "Bolivia", CreatedBy = 1 },
//    new Country { Id = 22, Name = "Bosnia and Herzegovina", CreatedBy = 1 },
//    new Country { Id = 23, Name = "Botswana", CreatedBy = 1 },
//    new Country { Id = 24, Name = "Brazil", CreatedBy = 1 },
//    new Country { Id = 25, Name = "Brunei", CreatedBy = 1 },
//    new Country { Id = 26, Name = "Bulgaria", CreatedBy = 1 },
//    new Country { Id = 27, Name = "Burkina Faso", CreatedBy = 1 },
//    new Country { Id = 28, Name = "Burundi", CreatedBy = 1 },
//    new Country { Id = 29, Name = "Cabo Verde", CreatedBy = 1 },
//    new Country { Id = 30, Name = "Cambodia", CreatedBy = 1 },
//    new Country { Id = 31, Name = "Cameroon", CreatedBy = 1 },
//    new Country { Id = 32, Name = "Canada", CreatedBy = 1 },
//    new Country { Id = 33, Name = "Central African Republic", CreatedBy = 1 },
//    new Country { Id = 34, Name = "Chad", CreatedBy = 1 },
//    new Country { Id = 35, Name = "Chile", CreatedBy = 1 },
//    new Country { Id = 36, Name = "China", CreatedBy = 1 },
//    new Country { Id = 37, Name = "Colombia", CreatedBy = 1 },
//    new Country { Id = 38, Name = "Comoros", CreatedBy = 1 },
//    new Country { Id = 39, Name = "Congo (Congo-Brazzaville)", CreatedBy = 1 },
//    new Country { Id = 40, Name = "Costa Rica", CreatedBy = 1 },
//    new Country { Id = 41, Name = "Croatia", CreatedBy = 1 },
//    new Country { Id = 42, Name = "Cuba", CreatedBy = 1 },
//    new Country { Id = 43, Name = "Cyprus", CreatedBy = 1 },
//    new Country { Id = 44, Name = "Czechia", CreatedBy = 1 },
//    new Country { Id = 45, Name = "Democratic Republic of the Congo", CreatedBy = 1 },
//    new Country { Id = 46, Name = "Denmark", CreatedBy = 1 },
//    new Country { Id = 47, Name = "Djibouti", CreatedBy = 1 },
//    new Country { Id = 48, Name = "Dominica", CreatedBy = 1 },
//    new Country { Id = 49, Name = "Dominican Republic", CreatedBy = 1 },
//    new Country { Id = 50, Name = "Ecuador", CreatedBy = 1 },
//    new Country { Id = 51, Name = "Egypt", CreatedBy = 1 },
//    new Country { Id = 52, Name = "El Salvador", CreatedBy = 1 },
//    new Country { Id = 53, Name = "Equatorial Guinea", CreatedBy = 1 },
//    new Country { Id = 54, Name = "Eritrea", CreatedBy = 1 },
//    new Country { Id = 55, Name = "Estonia", CreatedBy = 1 },
//    new Country { Id = 56, Name = "Eswatini", CreatedBy = 1 },
//    new Country { Id = 57, Name = "Ethiopia", CreatedBy = 1 },
//    new Country { Id = 58, Name = "Fiji", CreatedBy = 1 },
//    new Country { Id = 59, Name = "Finland", CreatedBy = 1 },
//    new Country { Id = 60, Name = "France", CreatedBy = 1 },
//    new Country { Id = 61, Name = "Gabon", CreatedBy = 1 },
//    new Country { Id = 62, Name = "Gambia", CreatedBy = 1 },
//    new Country { Id = 63, Name = "Georgia", CreatedBy = 1 },
//    new Country { Id = 64, Name = "Germany", CreatedBy = 1 },
//    new Country { Id = 65, Name = "Ghana", CreatedBy = 1 },
//    new Country { Id = 66, Name = "Greece", CreatedBy = 1 },
//    new Country { Id = 67, Name = "Grenada", CreatedBy = 1 },
//    new Country { Id = 68, Name = "Guatemala", CreatedBy = 1 },
//    new Country { Id = 69, Name = "Guinea", CreatedBy = 1 },
//    new Country { Id = 70, Name = "Guinea-Bissau", CreatedBy = 1 },
//    new Country { Id = 71, Name = "Guyana", CreatedBy = 1 },
//    new Country { Id = 72, Name = "Haiti", CreatedBy = 1 },
//    new Country { Id = 73, Name = "Honduras", CreatedBy = 1 },
//    new Country { Id = 74, Name = "Hungary", CreatedBy = 1 },
//    new Country { Id = 75, Name = "Iceland", CreatedBy = 1 },
//    new Country { Id = 76, Name = "India", CreatedBy = 1 },
//    new Country { Id = 77, Name = "Indonesia", CreatedBy = 1 },
//    new Country { Id = 78, Name = "Iran", CreatedBy = 1 },
//    new Country { Id = 79, Name = "Iraq", CreatedBy = 1 },
//    new Country { Id = 80, Name = "Ireland", CreatedBy = 1 },
//    new Country { Id = 81, Name = "Israel", CreatedBy = 1 },
//    new Country { Id = 82, Name = "Italy", CreatedBy = 1 },
//    new Country { Id = 83, Name = "Jamaica", CreatedBy = 1 },
//    new Country { Id = 84, Name = "Japan", CreatedBy = 1 },
//    new Country { Id = 85, Name = "Jordan", CreatedBy = 1 },
//    new Country { Id = 86, Name = "Kazakhstan", CreatedBy = 1 },
//    new Country { Id = 87, Name = "Kenya", CreatedBy = 1 },
//    new Country { Id = 88, Name = "Kiribati", CreatedBy = 1 },
//    new Country { Id = 89, Name = "Korea, North", CreatedBy = 1 },
//    new Country { Id = 90, Name = "Korea, South", CreatedBy = 1 },
//    new Country { Id = 91, Name = "Kosovo", CreatedBy = 1 },
//    new Country { Id = 92, Name = "Kuwait", CreatedBy = 1 },
//    new Country { Id = 93, Name = "Kyrgyzstan", CreatedBy = 1 },
//    new Country { Id = 94, Name = "Laos", CreatedBy = 1 },
//    new Country { Id = 95, Name = "Latvia", CreatedBy = 1 },
//    new Country { Id = 96, Name = "Lebanon", CreatedBy = 1 },
//    new Country { Id = 97, Name = "Lesotho", CreatedBy = 1 },
//    new Country { Id = 98, Name = "Liberia", CreatedBy = 1 },
//    new Country { Id = 99, Name = "Libya", CreatedBy = 1 },
//    new Country { Id = 100, Name = "Liechtenstein", CreatedBy = 1 },
//    new Country { Id = 101, Name = "Lithuania", CreatedBy = 1 },
//    new Country { Id = 102, Name = "Luxembourg", CreatedBy = 1 },
//    new Country { Id = 103, Name = "Madagascar", CreatedBy = 1 },
//    new Country { Id = 104, Name = "Malawi", CreatedBy = 1 },
//    new Country { Id = 105, Name = "Malaysia", CreatedBy = 1 },
//    new Country { Id = 106, Name = "Maldives", CreatedBy = 1 },
//    new Country { Id = 107, Name = "Mali", CreatedBy = 1 },
//    new Country { Id = 108, Name = "Malta", CreatedBy = 1 },
//    new Country { Id = 109, Name = "Marshall Islands", CreatedBy = 1 },
//    new Country { Id = 110, Name = "Mauritania", CreatedBy = 1 },
//    new Country { Id = 111, Name = "Mauritius", CreatedBy = 1 },
//    new Country { Id = 112, Name = "Mexico", CreatedBy = 1 },
//    new Country { Id = 113, Name = "Micronesia", CreatedBy = 1 },
//    new Country { Id = 114, Name = "Moldova", CreatedBy = 1 },
//    new Country { Id = 115, Name = "Monaco", CreatedBy = 1 },
//    new Country { Id = 116, Name = "Mongolia", CreatedBy = 1 },
//    new Country { Id = 117, Name = "Montenegro", CreatedBy = 1 },
//    new Country { Id = 118, Name = "Morocco", CreatedBy = 1 },
//    new Country { Id = 119, Name = "Mozambique", CreatedBy = 1 },
//    new Country { Id = 120, Name = "Myanmar (Burma)", CreatedBy = 1 },
//    new Country { Id = 121, Name = "Namibia", CreatedBy = 1 },
//    new Country { Id = 122, Name = "Nauru", CreatedBy = 1 },
//    new Country { Id = 123, Name = "Nepal", CreatedBy = 1 },
//    new Country { Id = 124, Name = "Netherlands", CreatedBy = 1 },
//    new Country { Id = 125, Name = "New Zealand", CreatedBy = 1 },
//    new Country { Id = 126, Name = "Nicaragua", CreatedBy = 1 },
//    new Country { Id = 127, Name = "Niger", CreatedBy = 1 },
//    new Country { Id = 128, Name = "Nigeria", CreatedBy = 1 },
//    new Country { Id = 129, Name = "North Macedonia", CreatedBy = 1 },
//    new Country { Id = 130, Name = "Norway", CreatedBy = 1 },
//    new Country { Id = 131, Name = "Oman", CreatedBy = 1 },
//    new Country { Id = 132, Name = "Pakistan", CreatedBy = 1 },
//    new Country { Id = 133, Name = "Palau", CreatedBy = 1 },
//    new Country { Id = 134, Name = "Panama", CreatedBy = 1 },
//    new Country { Id = 135, Name = "Papua New Guinea", CreatedBy = 1 },
//    new Country { Id = 136, Name = "Paraguay", CreatedBy = 1 },
//    new Country { Id = 137, Name = "Peru", CreatedBy = 1 },
//    new Country { Id = 138, Name = "Philippines", CreatedBy = 1 },
//    new Country { Id = 139, Name = "Poland", CreatedBy = 1 },
//    new Country { Id = 140, Name = "Portugal", CreatedBy = 1 },
//    new Country { Id = 141, Name = "Qatar", CreatedBy = 1 },
//    new Country { Id = 142, Name = "Romania", CreatedBy = 1 },
//    new Country { Id = 143, Name = "Russia", CreatedBy = 1 },
//    new Country { Id = 144, Name = "Rwanda", CreatedBy = 1 },
//    new Country { Id = 145, Name = "Saint Kitts and Nevis", CreatedBy = 1 },
//    new Country { Id = 146, Name = "Saint Lucia", CreatedBy = 1 },
//    new Country { Id = 147, Name = "Saint Vincent and the Grenadines", CreatedBy = 1 },
//    new Country { Id = 148, Name = "Samoa", CreatedBy = 1 },
//    new Country { Id = 149, Name = "San Marino", CreatedBy = 1 },
//    new Country { Id = 150, Name = "Sao Tome and Principe", CreatedBy = 1 },
//    new Country { Id = 151, Name = "Saudi Arabia", CreatedBy = 1 },
//    new Country { Id = 152, Name = "Senegal", CreatedBy = 1 },
//    new Country { Id = 153, Name = "Serbia", CreatedBy = 1 },
//    new Country { Id = 154, Name = "Seychelles", CreatedBy = 1 },
//    new Country { Id = 155, Name = "Sierra Leone", CreatedBy = 1 },
//    new Country { Id = 156, Name = "Singapore", CreatedBy = 1 },
//    new Country { Id = 157, Name = "Slovakia", CreatedBy = 1 },
//    new Country { Id = 158, Name = "Slovenia", CreatedBy = 1 },
//    new Country { Id = 159, Name = "Solomon Islands", CreatedBy = 1 },
//    new Country { Id = 160, Name = "Somalia", CreatedBy = 1 },
//    new Country { Id = 161, Name = "South Africa", CreatedBy = 1 },
//    new Country { Id = 162, Name = "Spain", CreatedBy = 1 },
//    new Country { Id = 163, Name = "Sri Lanka", CreatedBy = 1 },
//    new Country { Id = 164, Name = "Sudan", CreatedBy = 1 },
//    new Country { Id = 165, Name = "Sudan, South", CreatedBy = 1 },
//    new Country { Id = 166, Name = "Suriname", CreatedBy = 1 },
//    new Country { Id = 167, Name = "Sweden", CreatedBy = 1 },
//    new Country { Id = 168, Name = "Switzerland", CreatedBy = 1 },
//    new Country { Id = 169, Name = "Syria", CreatedBy = 1 },
//    new Country { Id = 170, Name = "Tajikistan", CreatedBy = 1 },
//    new Country { Id = 171, Name = "Tanzania", CreatedBy = 1 },
//    new Country { Id = 172, Name = "Thailand", CreatedBy = 1 },
//    new Country { Id = 173, Name = "Timor-Leste", CreatedBy = 1 },
//    new Country { Id = 174, Name = "Togo", CreatedBy = 1 },
//    new Country { Id = 175, Name = "Tonga", CreatedBy = 1 },
//    new Country { Id = 176, Name = "Trinidad and Tobago", CreatedBy = 1 },
//    new Country { Id = 177, Name = "Tunisia", CreatedBy = 1 },
//    new Country { Id = 178, Name = "Turkey", CreatedBy = 1 },
//    new Country { Id = 179, Name = "Turkmenistan", CreatedBy = 1 },
//    new Country { Id = 180, Name = "Tuvalu", CreatedBy = 1 },
//    new Country { Id = 181, Name = "Uganda", CreatedBy = 1 },
//    new Country { Id = 182, Name = "Ukraine", CreatedBy = 1 },
//    new Country { Id = 183, Name = "United Arab Emirates", CreatedBy = 1 },
//    new Country { Id = 184, Name = "United Kingdom", CreatedBy = 1 },
//    new Country { Id = 185, Name = "United States", CreatedBy = 1 },
//    new Country { Id = 186, Name = "Uruguay", CreatedBy = 1 },
//    new Country { Id = 187, Name = "Uzbekistan", CreatedBy = 1 },
//    new Country { Id = 188, Name = "Vanuatu", CreatedBy = 1 },
//    new Country { Id = 189, Name = "Vatican City", CreatedBy = 1 },
//    new Country { Id = 190, Name = "Venezuela", CreatedBy = 1 },
//    new Country { Id = 191, Name = "Vietnam", CreatedBy = 1 },
//    new Country { Id = 192, Name = "Yemen", CreatedBy = 1 },
//    new Country { Id = 193, Name = "Zambia", CreatedBy = 1 },
//    new Country { Id = 194, Name = "Zimbabwe", CreatedBy = 1 }
//);

//modelBuilder.Entity<WorkStatus>().HasData(
//    new WorkStatus { Id = 1, Name = "To Supplier", CreatedBy = 1 },
//    new WorkStatus { Id = 2, Name = "To Client", CreatedBy = 1 },
//    new WorkStatus { Id = 3, Name = "Processing", CreatedBy = 1 },
//    new WorkStatus { Id = 4, Name = "Confirmed", CreatedBy = 1 },
//    new WorkStatus { Id = 5, Name = "Cancelled", CreatedBy = 1 }
//);


//modelBuilder.Entity<QueryCategory>().HasData(
//    new QueryCategory { Id = 1, Name = "Hotel", CreatedBy = 1, CreatedAt = DateTime.Now },
//    new QueryCategory { Id = 2, Name = "Package", CreatedBy = 1, CreatedAt = DateTime.Now },
//    new QueryCategory { Id = 3, Name = "Transfer", CreatedBy = 1, CreatedAt = DateTime.Now },
//    new QueryCategory { Id = 4, Name = "Air-ticket", CreatedBy = 1, CreatedAt = DateTime.Now }
//);

