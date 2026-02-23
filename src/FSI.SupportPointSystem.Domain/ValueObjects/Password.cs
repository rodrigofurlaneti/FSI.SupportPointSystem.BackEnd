using FSI.SupportPointSystem.Domain.Exceptions;
namespace FSI.SupportPointSystem.Domain.ValueObjects
{
    public record Password
    {
        public string Hash { get; }
        private Password(string hash) => Hash = hash;

        public static Password CreateFromRaw(string raw, Func<string, string> hasher)
        {
            if (raw.Length < 6) throw new DomainException("Senha muito curta.");
            return new Password(hasher(raw));
        }
        public static Password FromHash(string hash) => new Password(hash);
    }
}
