using System.ComponentModel.DataAnnotations.Schema;

using eShop.Auth.Domain.Entities.MetaInfo;

namespace eShop.Auth.Domain.Entities;

public class RefreashToken : IModificationMetadata, ICreationMetadata
{
    public long Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresOnUtc { get; set; }
    public long UserId { get; set; }
    public bool IsRevoked { get; set; }
    public User? User { get; set; }
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; init; }
    [NotMapped] public string PreviousToken { get; set; } = string.Empty;
    public static RefreashTokenBuilder CreateBuilder() => new();

    private RefreashToken()
    {
    }

    public class RefreashTokenBuilder
    {
        private string _refreshToken = string.Empty;
        private long _userId;
        private DateTime _expiresOnUtc;
        private bool _isRevoked;
        private long _createdBy;
        private string _previousRefreshToken = string.Empty;
        private DateTime _createdAt = DateTime.Now;

        public RefreashTokenBuilder WithRefreshToken(string refreshToken)
        {
            _refreshToken = refreshToken;
            return this;
        }

        public RefreashTokenBuilder WithUserId(long userId)
        {
            _userId = userId;
            return this;
        }

        public RefreashTokenBuilder WithExpiresOnUtc(DateTime expiresOnUtc)
        {
            _expiresOnUtc = expiresOnUtc;
            return this;
        }

        public RefreashTokenBuilder WithIsRevoked(bool isRevoked)
        {
            _isRevoked = isRevoked;
            return this;
        }

        public RefreashTokenBuilder WithCreatedBy(long createdBy)
        {
            _createdBy = createdBy;
            return this;
        }

        public RefreashTokenBuilder WithCreatedAt(DateTime createdAt)
        {
            _createdAt = createdAt;
            return this;
        }

        public RefreashTokenBuilder WithPreviousRefreashToken(string previousRefreshToken)
        {
            _previousRefreshToken = previousRefreshToken;
            return this;
        }


        public RefreashToken Build()
        {
            return new RefreashToken
            {
                Token = _refreshToken,
                PreviousToken = _previousRefreshToken,
                UserId = _userId,
                ExpiresOnUtc = _expiresOnUtc,
                IsRevoked = _isRevoked,
                CreatedBy = _createdBy,
                CreatedAt = _createdAt
            };
        }
    }
}