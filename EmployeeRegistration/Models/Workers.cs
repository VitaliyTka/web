using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace EmployeeRegistration
{
    public partial class Workers
    {
        public int Id { get; set; }
        [Display(Name = "Дата прийняття на роботу")]
        public DateTime? Date { get; set; }
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }
        [Display(Name = "По батькові")]
        public string Patronymic { get; set; }
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; }
        [Display(Name = "Зарплата")]
        public int? Salary { get; set; }
        public int? PicturePathId { get; set; }

        [Display(Name = "Фото")]
        public virtual Picture PicturePath { get; set; }
    }
}
