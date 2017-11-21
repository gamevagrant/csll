
public class MessageMailMessage : NetMessage
{
    public MessageMailData data;

}

public class MessageMailData
{
    public MessageResponseData[] messages;
    public MailData[] user_mail;
}
