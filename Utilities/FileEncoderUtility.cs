using MasteryTest3.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Security.AccessControl;

namespace MasteryTest3.Utilities
{
    public class FileEncoderUtility : IFileEncoderUtility
    {
        public bool VerifyEncodedPdf(string encodedFile) {
            if (encodedFile.IsNullOrEmpty()) {
                return true;
            }

            return encodedFile.Substring(0, 5).ToUpper() == "JVBER";
        }
    }
}
