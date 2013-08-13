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
        private string Base_Name { get; set; }
        public string Thumb_Image_Name { get; set; }
        public string Base_Revision_Id { get; set; }
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
        public Document_Path Document_Path 
        { 
            get
            {
                return new Document_Path() { Path = Path };
            } 
            set
            {;
            } 
        }
        public int Revision_Count { get; set; }
        public List<string> Tags { get; set; }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsRevision()
        {
            var id = System.IO.Path.GetFileNameWithoutExtension(Filename).Split('_').Last();
            int rev_id;
            if (int.TryParse(id,out rev_id))
            {
                return true;
            }

            else
            {
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Get_Original_Name()
        {
            if (IsRevision() && Base_Revision_Id != null)
            {
                string upper = System.IO.Path.GetFileNameWithoutExtension(Filename);
                string ext = System.IO.Path.GetExtension(Filename);
                //This is janky as hell.
                string original_name = upper.Remove(upper.LastIndexOf('_')) + ext;
                return original_name;  
            }

            else
	        {
                //Client code should test "IsRevision" first but maybe raise exception here.
                return null;
	        }
        }
    }
}
