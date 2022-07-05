using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Models
{
    public class GenderViewModel
    {
        public string Gender { get; set; }
        public List<SelectListItem> GenderList { get; } = new List<SelectListItem>
    {
        new SelectListItem { Value = "", Text = "Select" },
        new SelectListItem { Value = "Male", Text = "Male" },
        new SelectListItem { Value = "Female", Text = "Female"  },
    };
    }
}
