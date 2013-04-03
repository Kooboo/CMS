using System;

namespace NntpClientLib
{
    /// <summary>
    /// See http://tools.ietf.org/html/rfc4643 for more details of this.
    /// </summary>
    internal sealed class Rfc4643ResponseCodes
    {
        public const int AuthenticationAccepted = 281;
        public const int PasswordRequired = 381;
        public const int AuthenticationRequired = 480;
        public const int AuthenticationFailed = 481;
        public const int AuthenticationCommandsOutOfSequence = 482;
        public const int CommandUnavailable = 502;
    }
}

