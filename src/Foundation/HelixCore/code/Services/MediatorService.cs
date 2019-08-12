using System.ComponentModel.DataAnnotations;
using ScHelix.Foundation.HelixCore.DI;
using ScHelix.Foundation.HelixCore.Models;

namespace ScHelix.Foundation.HelixCore.Services {
    [Service(typeof(IMediatorService))]
    public class MediatorService : IMediatorService {
        public MediatorResponse<T> GetMediatorResponse<T>(string code, T viewModel = default(T),
            ValidationResult validationResult = null, object parameters = null, string message = null) {
            MediatorResponse<T> response = new MediatorResponse<T> {
                Code = code,
                ViewModel = viewModel,
                ValidationResult = validationResult,
                Parameters = parameters,
                Message = message
            };

            return response;
        }
    }
}
