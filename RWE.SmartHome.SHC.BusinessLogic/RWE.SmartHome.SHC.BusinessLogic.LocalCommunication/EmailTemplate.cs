namespace RWE.SmartHome.SHC.BusinessLogic.LocalCommunication;

public class EmailTemplate
{
	public string Title { get; set; }

	public string Subject { get; set; }

	public string Body { get; set; }

	public EmailTemplate(string title, string subject, string body)
	{
		Title = title;
		Subject = subject;
		Body = body;
	}
}
