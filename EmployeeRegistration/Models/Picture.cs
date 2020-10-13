using System;
using System.Collections.Generic;

namespace EmployeeRegistration
{
    public partial class Picture
    {
        public Picture()
        {
            Workers = new HashSet<Workers>();
        }

        public int Id { get; set; }
        public string PicturePath { get; set; }

        public virtual ICollection<Workers> Workers { get; set; }
    }
}
