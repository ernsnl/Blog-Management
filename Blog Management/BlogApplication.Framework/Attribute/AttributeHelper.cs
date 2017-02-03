namespace BlogApplication.Framework.Attribute
{
    public class AttributeHelper : System.Attribute
    {
        public object AttributeValue { get; protected set; }

        public AttributeHelper(object value)
        {
            this.AttributeValue = value;
        }
    }
}