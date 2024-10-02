namespace CorporateBankingApp.GlobalException
{
    public class CustomException:Exception
    {
        public CustomException(string message):base(message) { }
    }
}
