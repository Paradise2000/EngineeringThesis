namespace EngineeringThesisAPI.DTOs.Account
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string RetypedNewPassword { get; set; }
    }
}
