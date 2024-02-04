using PdfSharp.Fonts;

namespace MasteryTest3.Repositories
{
    public class FontResolverRepository : IFontResolver
    {
        public byte[]? GetFont(string faceName)
        {
            return File.ReadAllBytes(faceName);
        }

        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            
            string fontDirectory = "/Fonts/" + familyName + "/";
            if (isBold)
            {
                return new FontResolverInfo(Directory.GetCurrentDirectory()+ fontDirectory + familyName + "b.ttf");
            }
            else if (isItalic)
            {
                return new FontResolverInfo(Directory.GetCurrentDirectory()+ fontDirectory + familyName + "i.ttf");
            }
            else { 
                return new FontResolverInfo(Directory.GetCurrentDirectory()+ fontDirectory + familyName + ".ttf");
            }
        }
    }
}
