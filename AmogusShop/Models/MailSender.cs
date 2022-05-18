using System.Net.Mail;

namespace AmogusShop.Models
{
    public class MailSender
    {
        public static void SendVerificationEmail(string emailBody, string addreas)
        {
            MailMessage mailMessage = new MailMessage("AmogusShopClientService@gmail.com", addreas);
            mailMessage.Subject = "Authentification";
            mailMessage.Body = emailBody;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "AmogusShopClientService@gmail.com",
                Password = "921**365@"
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }
    }
}
