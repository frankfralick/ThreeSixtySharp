using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeSixtySharp.Objects
{
    public class Issue
    {
        public DateTime Status_Changed_At { get; set; }
        //Not sure if Release needs to be DateTime?
        public string Release { get; set; }
        public string Unit { get; set; }
        public double Quantity { get; set; }
        public string Status { get; set; }
        public string Root_Cause_Id { get; set; }
        public List<string> Document_References { get; set; }
        public string Method { get; set; }
        public string Group_Id { get; set; }
        public string Crew { get; set; }
        public string Parent_Source_Type { get; set; }
        public DateTime? Due_Date { get; set; }
        public string Spec_Ref { get; set; }
        public List<Uri> Uri_References { get; set; }
        public string Issue_Type_Id { get; set; }
        public string Bare_Material_Cost { get; set; }
        public bool Clarification_Needed { get; set; }
        public string Created_By { get; set; }
        public string Full_Description { get; set; }
        public string Issue_Type { get; set; }
        public List<string> Attachments { get; set; }
        public double Labor_Hours { get; set; }
        public bool Backcharge { get; set; }
        public DateTime Updated_At { get; set; }
        public string Source_Id { get; set; }
        public bool Void { get; set; }
        public List<string> Tags { get; set; }
        public string Master_Format_Id { get; set; }
        public string Created_From { get; set; }
        public string Total_Cost { get; set; }
        public DateTime Created_At { get; set; }
        public List<string> Comments { get; set; }
        public string Cost_Type { get; set; }
        public List<string> Signatures { get; set; }
        public string Creator_Role_Name { get; set; }
        public string Postal_Prefix { get; set; }
        public string Bare_Labor_Cost { get; set; }
        public string Creator_Company_Id { get; set; }
        public List<string> Custom_Field_Values { get; set; }
        public string Currency { get; set; }
        public string Location_Detail { get; set; }
        public string Issue_Id { get; set; }
        public string Fbi_Pushpin { get; set; }
        public List<Document_Path> Document_Folder_References { get; set; }
        public string Daily_Output { get; set; }
        public string Source_Type { get; set; }
        public string Description { get; set; }
        public DateTime? Commented_At { get; set; }
        public string Pushpin { get; set; }
        public string Backcharge_Cost { get; set; }
        public string Area_Id { get; set; }
        public string Company_Id { get; set; }
        public string Identifier { get; set; }
        public DateTime? Closed_At { get; set; }
        public bool Has_Pushpin { get; set; }
        public string Cost_With_Op { get; set; }
        public string Bare_Equipment_Cost { get; set; }
        public string Parent_Source_Id { get; set; }
        public string Priority { get; set; }
    }
}
