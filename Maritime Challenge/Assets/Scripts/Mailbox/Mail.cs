using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mail
{
    public string MailTitle;
    public string MailDescription;
    public int MailItemAmount;

    public Mail(string mailTitle, string mailDescription, int mailItemAmount)
    {
        MailTitle = mailTitle;
        MailDescription = mailDescription;
        MailItemAmount = mailItemAmount;
    }
}
