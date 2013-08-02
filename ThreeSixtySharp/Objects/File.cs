using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ThreeSixtySharp.Objects
{
    public class File
    {
        public string Thumb_Image_Name { get; set; }
        public int Base_Revision_Id { get; set; }
        public int Revision_Position { get; set; }
        public string Vela_Viewer_Image_Names { get; set; }
        public string Composite_Image_Names { get; set; }
        public string Caption { get; set; }
        public DateTime Updated_At { get; set; }
        public int Width { get; set; }
        public DateTime Created_At { get; set; }
        public string Parent_Id { get; set; }
        public int Height { get; set; }
        public string Document_Id { get; set; }
        public int Num_Pages { get; set; }
        public DateTime FCreate_Date { get; set; }
        public int Size { get; set; }
        public string Filename { get; set; }
        public string Content_Type { get; set; }
        public DateTime? FMod_Date { get; set; }
        public string Path { get; set; }
        public int Revision_Count { get; set; }

    }
}
