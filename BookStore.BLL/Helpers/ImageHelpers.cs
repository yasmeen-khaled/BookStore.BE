using BookStore.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BookStore.BLL.Helpers
{
    public static class ImageHelpers
    {
        public static IFormFile ByteArrayToImage(byte[]? bytes)
        {
            IFormFile imageFile = null;
            if (bytes != null && bytes.Length > 0)
            {
                var content = new ByteArrayContent(bytes);
                content.Headers.Add("Content-Type", "image/jpeg"); // or the appropriate content type

                var headerDictionary = new HeaderDictionary(content.Headers.ToDictionary(kv => kv.Key, kv => new StringValues(kv.Value.First())));

                imageFile = new FormFile(content.ReadAsStream(), 0, bytes.Length, "Cover", "cover.jpg")
                {
                    Headers = headerDictionary
                };
            }
            return imageFile;
        }
        public static string ByteArrayToString(byte[] bytes)
        {
            string url = "";
            if (bytes != null && bytes.Length > 0)
            {
                string img = Convert.ToBase64String(bytes, 0, bytes.Length);
                url = "data:image/jpeg;base64," + img;
            }
            if (bytes == null)
            {
                url = "~/Images/RandomImage.png";
            }
            return url;
        }

        public static byte[]? ImageToByteArray(IFormFile? image)
        {
            byte[]? byteArr = null;
            using (var memoryStream = new MemoryStream())
            {
                if (image != null)
                {
                    image.CopyTo(memoryStream);

                    if (memoryStream.Length < 2097152)//less than 2 MB
                    {
                        byteArr = memoryStream.ToArray();
                    }
                    else
                    {
                        throw new Exception("File is too large");
                    }
                }
                return byteArr;
            }
        }
    }
}
