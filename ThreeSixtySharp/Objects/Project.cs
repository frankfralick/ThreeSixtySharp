using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeSixtySharp.Objects
{
    public class Project
    {
        public bool Is_Active { get; set; }
        public string Brand_Color { get; set; }
        public List<Issue_Filter> Issue_Filters { get; set; }
        public bool Is_Sub { get; set; }
        public bool Task_Edit_By_Assignee_Only { get; set; }
        public List<RealmPermission> Permissions { get; set; }
        public List<Document_Path> Document_Paths { get; set; }
        public DateTime? Default_Issue_Due_Date { get; set; }
        public bool Lock_Closed_Checklists { get; set; }
        public bool Needs_Duns { get; set; }
        public bool Always_Allow_Attachments { get; set; }
        public bool Lock_Closed_Attachments { get; set; }
        public bool Lock_Closed_Tasks { get; set; }
        public string Issue_Workflow_Rule { get; set; }
        public string Brand { get; set; }
        public string User_Roles { get; set; }
        public string Name { get; set; }
        public string Project_ID { get; set; }
        public bool Is_Trial { get; set; }
        public string User_Company_Id { get; set; }
    }
}
