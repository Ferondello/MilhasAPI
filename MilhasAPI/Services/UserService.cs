using MilhasAPI.Models;
using MilhasAPI.Repositories.Interfaces;
using MilhasAPI.Services.Interfaces;

namespace MilhasAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
        => await _userRepository.GetAllAsync();

    public async Task<User?> GetByIdAsync(int id)
        => await _userRepository.GetByIdAsync(id);

    public async Task<User> CreateAsync(CreateUserDto dto)
    {
        var user = new User { Name = dto.Name, Email = dto.Email };

        if (dto.CreditCards != null && dto.CreditCards.Count > 0)
            user.CreditCards = dto.CreditCards.Select(c => new CreditCard
            {
                CardNumber = c.CardNumber,
                Brand = c.Brand
            }).ToList();

        return await _userRepository.CreateAsync(user);
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        if (dto.Name != null) user.Name = dto.Name;
        if (dto.Email != null) user.Email = dto.Email;

        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        await _userRepository.DeleteAsync(user);
        return true;
    }
}
