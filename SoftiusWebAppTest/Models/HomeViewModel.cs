using System.ComponentModel.DataAnnotations;

namespace SoftiusWebAppTest.Models
{
    public class HomeViewModel
    {
        [Display(Name = "Входные данные")]
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string InputData { get; set; }

        [Display(Name = "Результат")]
        public string OutputData { get; set; }

    }
}
