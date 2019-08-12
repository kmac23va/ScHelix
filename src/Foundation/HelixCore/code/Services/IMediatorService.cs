using System.ComponentModel.DataAnnotations;
using ScHelix.Foundation.HelixCore.Models;

namespace ScHelix.Foundation.HelixCore.Services {
    public interface IMediatorService {
        MediatorResponse<T> GetMediatorResponse<T>(string code, T viewModel = default(T),
            ValidationResult validationResult = null, object parameters = null, string message = null);
    }
}
