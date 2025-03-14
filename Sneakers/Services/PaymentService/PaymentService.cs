using Domain.Entities;
using Domain.Interfaces;

namespace Sneakers.Services.PaymentService
{
    public class PaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable <Payment> GetAllPayments()
        {
            return _unitOfWork.Payment.GetAll();
        }
    }
}
