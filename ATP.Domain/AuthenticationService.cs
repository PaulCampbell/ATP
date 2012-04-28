using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATP.Domain.Models;
using Raven.Client;
using System.Security.Cryptography;

namespace ATP.Domain
{
    public interface IAuthenticationService
    {
        LoginResult Login(string email, string password);
        UpdatePasswordResult UpdatePassword(User user, string newPassword);

    }

    public class AuthenticationService : IAuthenticationService
    {
            private readonly IDocumentSession _session;
            private readonly IPasswordHasher _passwordHasher;

            public AuthenticationService(IDocumentSession session, IPasswordHasher passwordHasher)
            {
                _passwordHasher = passwordHasher;
                _session = session;
            }

            public LoginResult Login(string email, string password)
            {
                var u = _session.Query<User>()
                    .FirstOrDefault<User>(x=>x.Email == email);
                if (u == null)
                {
                    return LoginResult.unsuccessful;
                }

                if (!_passwordHasher.VerifyHash(password, u.HashedPassword))
                {
                    return LoginResult.unsuccessful;
                }

                return LoginResult.successful;
            }

            public UpdatePasswordResult UpdatePassword(User user, string newPassword)
            {
                if (newPassword.Length < 5)
                {
                    return UpdatePasswordResult.notLongEnough;
                }

                var salt = GenerateSalt();

                var newHash = _passwordHasher.ComputeHash(newPassword, salt);

                user.HashedPassword = newHash;

                return UpdatePasswordResult.successful;

            }

            private byte[] GenerateSalt()
            {
                byte[] saltBytes;
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);

                return saltBytes;
            }
        }

        public enum LoginResult
        {
            unknown = 0,
            successful = 1,
            unsuccessful = 2
        }

        public enum UpdatePasswordResult
        {
            unknown = 0,
            successful = 1,
            notLongEnough = 2,
            reservedWord = 3
        }
    }
