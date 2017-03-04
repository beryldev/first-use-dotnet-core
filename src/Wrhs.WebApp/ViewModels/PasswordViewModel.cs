using System.Collections.Generic;

namespace Wrhs.WebApp.ViewModels
{
    public class PasswordViewModel
    {
        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }
}