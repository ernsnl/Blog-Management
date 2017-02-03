
using System;
using BlogApplication.Framework.Attribute;

namespace BlogApplication.Data.GlobalTypes
{
    public enum UserType : byte
    {
        [AttributeHelper("System Admin")]
        SystemAdmin = 1,

        [AttributeHelper("Admin")]
        Admin = 2,

        [AttributeHelper("Editor")]
        Editor = 3,

        [AttributeHelper("Translator")]
        Translator = 4,

        [AttributeHelper("Writer")]
        Writer = 5,

        [AttributeHelper("Normal")]
        Normal = 6
    }

    public enum StatusType
    {
        Active = 1,
        Passive = 0,
        Deleted = 2
    }

    public class VariableValue
    {
        public static byte ConvertUserTypesByte(UserType u)
        {
            return (byte) u;
        }

        public static byte ConvertStatusTypesByte(StatusType u)
        {
            return (byte)u;
        }

        public static byte ActiveStatusID
        {
            get { return (byte) StatusType.Active; }
        }

        public static byte DeletedStatusID
        {
            get { return (byte)StatusType.Deleted; }
        }

        public static byte PassiveStatusID
        {
            get { return (byte)StatusType.Passive; }
        }

        public static byte PublishedStatus
        {
            get { return (byte) BlogStatusType.Published; }
        }

        public static byte InReviewStatus
        {
            get { return (byte)BlogStatusType.InReview; }
        }

        public static byte Denied
        {
            get { return (byte)BlogStatusType.Denied; }
        }

        public static byte WaitingReview
        {
            get { return (byte)BlogStatusType.WaitingReview; }
        }

    }
}