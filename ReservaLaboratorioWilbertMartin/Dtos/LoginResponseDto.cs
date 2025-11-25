namespace ReservaLaboratorioWilbertMartin.Dtos
{
    public class LoginResponseDto
    {
        public string AccesToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}
