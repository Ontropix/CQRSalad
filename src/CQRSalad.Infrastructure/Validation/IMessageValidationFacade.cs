namespace CQRSalad.Infrastructure.Validation
{
    public interface IMessageValidationFacade
    {
        bool IsValid<TMessage>(TMessage instance) where TMessage : class;
        void Validate<TMessage>(TMessage instance) where TMessage : class;
    }
}
