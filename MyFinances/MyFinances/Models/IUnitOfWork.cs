namespace MyFinances.Models
{
    public interface IUnitOfWork
    {
        IOperationRepository Operation { get; set; }
    }
}