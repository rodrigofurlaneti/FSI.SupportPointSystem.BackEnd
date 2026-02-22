namespace FSI.SupportPoint.Domain.Exceptions
{
    public class BusinessRuleException : DomainException
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}