// **************************************************************************
//Start Finance - An to manage your personal finances.
//Copyright(C) 2016  Jijo Bose

//Start Finance is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//Start Finance is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with Start Finance.If not, see<http://www.gnu.org/licenses/>.
// ***************************************************************************

//ContactDetails created by Malcolm Billinghurst

using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartFinance.Models
{
    class ContactInfo
    {
        [PrimaryKey, AutoIncrement]
        public int ContactID {get; set; }  //unique id for each contact

        [Unique]
        public string CustFName { get; set; } //the customer's first name
        //note would not use first name as unique field in the real world

        [NotNull]
        public string CustLName { get; set; } //the customer's last name

        [NotNull]
        public string CompanyName { get; set; } //the customer's company name
        
        [NotNull]
        public double PhoneNum { get; set; } //the customer's phone number

    }
}
