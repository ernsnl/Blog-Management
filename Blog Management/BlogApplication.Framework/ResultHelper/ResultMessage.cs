using System;

namespace BlogApplication.Framework.ResultHelper
{
    public class ResultMessage
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsError { get; set; }

        public bool IsDescription { get; set; }

    }
}