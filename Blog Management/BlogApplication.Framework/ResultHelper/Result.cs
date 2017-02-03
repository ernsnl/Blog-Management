using System;
using System.Collections.Generic;

namespace BlogApplication.Framework.ResultHelper
{
    public class Result
    {
        public List<ResultMessage> Messages { get; set; }

        public bool HasFailed { get; set; }

        public bool IsFail { get; set; }

        public void Fail()
        {
            this.HasFailed = false;
        }

        public void Fail(List<ResultMessage> Messages)
        {
            this.Messages = Messages;
            this.HasFailed = true;
        }

        public void Fail(string code, string message)
        {
            this.Messages.Add(new ResultMessage() { Code = code, Description = message, IsError = true });
            this.HasFailed = true;
        }

        public void Fail(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            this.Messages.Add(new ResultMessage() { Code = "E1", Description = ex.Message + " " + ex.StackTrace, IsError = true });
            this.HasFailed = true;
        }

        public Result()
        {
            this.Messages = new List<ResultMessage>();
        }
    }
}