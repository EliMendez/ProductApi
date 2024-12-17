using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models.Dto
{
    public class UserDataDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
    }
}
