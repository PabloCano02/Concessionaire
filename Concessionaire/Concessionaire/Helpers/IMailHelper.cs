﻿using Concessionaire.Common;

namespace Concessionaire.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);
    }
}
