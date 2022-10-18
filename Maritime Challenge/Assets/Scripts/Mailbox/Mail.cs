using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mail
{
    public int MailID;
    public string MailTitle;
    public string MailDescription;
    public int MailItemAmount;

    public Mail(int mailID, string mailTitle, string mailDescription, int mailItemAmount)
    {
        MailID = mailID;
        MailTitle = mailTitle;
        MailDescription = mailDescription;
        MailItemAmount = mailItemAmount;
    }
}
