using CommsManager.Core.Enums;

namespace CommsManager.Core.Entities;

public class User : BaseEntity
{
    public User(string email, byte[] passwordHash, byte[] passwordSalt)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        PasswordSalt = passwordSalt ?? throw new ArgumentNullException(nameof(passwordSalt));
        Role = UserRole.Client;
        CreatedDate = DateTime.UtcNow;
        IsActive = true;
    }

    public string Email { get; private set; }
    public byte[] PasswordHash { get; private set; }
    public byte[] PasswordSalt { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public bool IsActive { get; private set; }

    // Optional links to domain profiles (nullable)
    public Guid? CustomerId { get; set; }
    public Guid? ArtistProfileId { get; set; }

    public void PromoteToCreator(Guid artistProfileId)
    {
        if (Role == UserRole.Creator)
            return;

        Role = UserRole.Creator;
        ArtistProfileId = artistProfileId;
    }

    public void SetPassword(byte[] hash, byte[] salt)
    {
        PasswordHash = hash ?? throw new ArgumentNullException(nameof(hash));
        PasswordSalt = salt ?? throw new ArgumentNullException(nameof(salt));
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
