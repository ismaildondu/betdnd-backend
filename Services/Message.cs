namespace BetDND.Services
{
    public class MessageService
    {
        public static string PasswordTooShort = "Parolanız en az 8 karakterden oluşmalıdır.";
        public static string EmailExists = "Bu e-posta adresi zaten kullanılmaktadır.";
        public static string InvalidEmailFormat = "Lütfen geçerli bir e-posta adresi giriniz.";
        public static string InvalidCredentials = "E-posta adresi veya parola hatalı.";
        public static string InvalidToken = "Geçersiz token.";
        public static string UserNotFound = "Kullanıcı bulunamadı.";
        public static string NameSurnameTooShort = "Adınız ve soyadınız en az 5 karakterden oluşmalıdır.";
        public static string InvalidMatchId = "Geçersiz maç id'si.";
        public static string MatchIdNotUniq = "Aynı maç id'si birden fazla kez kullanılamaz.";
        public static string InvalidMainBet = "Geçersiz ana bahis.";
        public static string InvalidSelect = "Geçersiz seçim.";
        public static string InvalidAmount = "Geçersiz bahis miktarı.";
    }
}