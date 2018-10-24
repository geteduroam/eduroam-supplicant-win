﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduroamApp
{
    static class ImageFunctions
    {
        /// <summary>
        /// Converts base64 string to image.
        /// </summary>
        /// <param name="base64String">Base64 string.</param>
        /// <returns>Image.</returns>
        public static Image Base64ToImage(string base64String)
        {
            // converts base64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // converts byte[] to Image
            using (var stream = new MemoryStream(imageBytes))
            {
                Image image = Image.FromStream(stream, true);
                return image;
            }
        }

        /// <summary>
        /// Resize image to specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        /// Generates HTML code for displaying logos in SVG format.
        /// </summary>
        /// <param name="logo">Base64 encoded logo.</param>
        /// <param name="maxWidth">Width of the logo container.</param>
        /// <param name="maxHeight">Height of the logo container.</param>
        /// <returns>Html code.</returns>
        public static string GenerateLogoHtml(string logo, int maxWidth, int maxHeight)
        {
            return 
                "<!DOCTYPE html>" +
                    "<html>" +
                        "<head>" +
                            "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">" +
                            "<style>" +
                                "img {" +
                                    "position: absolute;" +
                                    "top: 0;" +
                                    "left: 0;" +
                                    "display: block;" +
                                    "max-width:" + maxWidth + "px;" +
                                    "max-height:" + maxHeight + "px;" +
                                    "width: auto;" +
                                    "height: auto;" +
                                "}" +
                            "</style>" +
                       "</head>" +
                       "<body>" +
                            "<img src=\'data:image/svg+xml;base64," + logo + "\'>" +
                       "</body>" +
                   "</html>";
        }


        /// <summary>
        /// Eduroam logo in svg format, encoded to base64
        /// </summary>
        public const string EduroamLogo =
            "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4KPCEtLSBHZW5lcmF0b3I6IEFkb2JlIElsbHVzdHJhdG9yIDE2LjAuMywgU1ZHIEV4cG9ydCBQbHVnLUluIC4gU1ZHIFZlcnNpb246IDYuMDAgQnVpbGQgMCkgIC0tPgo8IURPQ1RZUEUgc3ZnIFBVQkxJQyAiLS8vVzNDLy9EVEQgU1ZHIDEuMS8vRU4iICJodHRwOi8vd3d3LnczLm9yZy9HcmFwaGljcy9TVkcvMS4xL0RURC9zdmcxMS5kdGQiPgo8c3ZnIHZlcnNpb249IjEuMSIgaWQ9IkxheWVyXzEiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgeG1sbnM6eGxpbms9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkveGxpbmsiIHg9IjBweCIgeT0iMHB4IgoJIHdpZHRoPSI2MjUuMzJweCIgaGVpZ2h0PSIyNzEuMjhweCIgdmlld0JveD0iNDUgMjggNTM1IDIxNSIgZW5hYmxlLWJhY2tncm91bmQ9Im5ldyAwIDAgNjI1LjMyIDI3MS4yOCIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSI+CjxnPgoJPGc+CgkJPHBhdGggZmlsbC1ydWxlPSJldmVub2RkIiBjbGlwLXJ1bGU9ImV2ZW5vZGQiIGZpbGw9IiNDOERCRUIiIGQ9Ik0xODkuNjM2LDE1My43MDZjLTIwLjE4NCwwLTQwLjM2OC00LjQ3OS01NS43NDQtMTMuNDMKCQkJYy0yMy4wODctMTMuNDMzLTI4Ljg1OC0zMy4xNDItMTcuMzE5LTQ5LjcxMmw1LjQ1Mi02LjAyOGwzMi42OTQsMS4zODNsMC40NSwwLjAwM2wtNS45OTUsNC43MjIKCQkJYy0xMi4xNzMsMTEuODM3LTEwLjE0NywyNy45NDQsNi4wNzgsMzguMzYxYzkuMjc0LDUuOTU1LDIxLjQzOCw4LjkzNSwzMy42MDEsOC45MzVzMjQuMzI3LTIuOTgsMzMuNTk3LTguOTM1CgkJCWMxNi4yMjMtMTAuNDE3LDE4LjI1NS0yNi41MjQsNi4wODUtMzguMzYxbC01LjczMS00LjUwN2wxNS4zODMtMC4yMThsMTkuNTYzLTAuODI4bDQuOTQ4LDUuNDczCgkJCWMxMS41NCwxNi41NzEsNS43NjksMzYuMjgtMTcuMzA4LDQ5LjcxMkMyMzAuMDA0LDE0OS4yMjgsMjA5LjgyLDE1My43MDYsMTg5LjYzNiwxNTMuNzA2TDE4OS42MzYsMTUzLjcwNnoiLz4KCQk8cGF0aCBmaWxsLXJ1bGU9ImV2ZW5vZGQiIGNsaXAtcnVsZT0iZXZlbm9kZCIgZmlsbD0iI0M4REJFQiIgZD0iTTMyMi41MzIsODMuODQ0bC0xLjY3OC0yLjczN2wtMy4wMjcsMC4zMDhsLTI3Ljc2NywxLjk4MgoJCQlsMy40ODIsNS42MzljMTAuOTY2LDIwLjk5MywxLjgyNiw0NC42NDEtMjcuNDE5LDYxLjE5OWMtOC4wMSw0LjUzNS0xNi45NzEsOC4yMDEtMjYuNTA5LDExLjAyN2g0OS45MTQKCQkJQzMyNS4yNzQsMTQwLjA1NiwzMzYuMjk2LDExMC4zMTIsMzIyLjUzMiw4My44NDR6Ii8+CgkJPHBhdGggZmlsbC1ydWxlPSJldmVub2RkIiBjbGlwLXJ1bGU9ImV2ZW5vZGQiIGZpbGw9IiNDOERCRUIiIGQ9Ik04OC4yNzEsMTYxLjI3NEgxMzguNWMtOS41MzktMi44MjYtMTguNDk0LTYuNTA0LTI2LjUwNS0xMS4wMzkKCQkJYy0zMS45MTEtMTguMDYyLTM5Ljg4MS00NC41NjMtMjMuOTI5LTY2Ljg1MmwwLjgwNi0wLjk4OGwtMTMuNzk2LTAuOTgxbC0xNy4yNDgtMS43NTJsLTIuNTU5LDQuMTgxCgkJCUM0MS41MDIsMTEwLjMxNSw1Mi41MTUsMTQwLjA2Nyw4OC4yNzEsMTYxLjI3NHoiLz4KCQk8cGF0aCBmaWxsLXJ1bGU9ImV2ZW5vZGQiIGNsaXAtcnVsZT0iZXZlbm9kZCIgZmlsbD0iI0M4REJFQiIgZD0iTTMyOC4yMzMsMTYxLjI2M2gzNi4xMgoJCQljMjcuNzI3LTI2LjAwOSwzMy45NjItNTYuOTc5LDE4LjY1MS04NS4yMWwtMS4yNjQtMS45NzlsLTI2LjM5OCwzLjUzMWwtNS4wMzksMC41MTFsMi44NjcsNC4zNjcKCQkJQzM2Ny44MTIsMTA4Ljg3NSwzNTkuNDgyLDEzOC4xMTYsMzI4LjIzMywxNjEuMjYzeiIvPgoJCTxwYXRoIGZpbGwtcnVsZT0iZXZlbm9kZCIgY2xpcC1ydWxlPSJldmVub2RkIiBmaWxsPSIjQzhEQkVCIiBkPSJNMzk3LjQxOCwxNjEuMjYzaDMzLjg2MwoJCQljMjQuMDMyLTI5Ljc1OSwyNy41Mi02My4xMDEsMTAuMzk0LTkzLjg5NWwtMi4xNTQtMy4yNzNsLTE0LjgyLDMuMDc0bC0xNS4yNTYsMi41N2wyLjMwNywzLjQ4CgkJCUM0MjgsMTAyLjIxMiw0MjMuMjAyLDEzMy43OTEsMzk3LjQxOCwxNjEuMjYzeiIvPgoJCTxwYXRoIGZpbGwtcnVsZT0iZXZlbm9kZCIgY2xpcC1ydWxlPSJldmVub2RkIiBmaWxsPSIjQzhEQkVCIiBkPSJNNDY0LjEwNCwxNjEuMjYzaDM0LjIyNgoJCQljMjAuOTAxLTMzLjA1LDIyLjE4Ny02OC44OTcsMy43OTEtMTAyLjI4MmwtNS44ODMtOS4wMjRsLTExLjEwMiwzLjM5NGwtMTguMjk1LDQuNjA0bDQuMTg2LDYuNDIKCQkJQzQ4OC41MTIsOTYuMDY2LDQ4Ni4xODQsMTMwLjIyNiw0NjQuMTA0LDE2MS4yNjN6Ii8+CgkJPHBhdGggZmlsbC1ydWxlPSJldmVub2RkIiBjbGlwLXJ1bGU9ImV2ZW5vZGQiIGZpbGw9IiNDOERCRUIiIGQ9Ik01MzAuNDA5LDE2MS4yNzRoMzEuNTU1CgkJCWMyMi4yMTktNDIuNzA1LDE3LjcyMy04OC41ODgtMTMuNTQzLTEzMC4wMDFsLTAuNDc1LTAuNTgxbC0xMi45MzYsNS45MDRsLTEzLDQuODA5bDkuMzI0LDE0LjE1NQoJCQlDNTUwLjU0LDkwLjAyLDU1MC4yMTYsMTI2Ljg5NCw1MzAuNDA5LDE2MS4yNzR6Ii8+Cgk8L2c+Cgk8Zz4KCQk8Zz4KCQkJPGc+CgkJCQk8Zz4KCQkJCQk8cGF0aCBmaWxsPSIjMUQxRDFCIiBkPSJNMTM0LjMyNCwyMDQuMDU4Yy0zLjYzMywxMS44NzUtMTYuNTIzLDE3LjMzMi0zMC43MDMsMTcuMzMyYy04Ljc4OSwwLTE3LjM0My0xLjUzMS0yNC4xNC02LjQxNAoJCQkJCQljLTYuNDQ1LTQuNjkzLTkuODQzLTExLjM5Ni05LjcyNy0xOC4zODdjLTAuMTE3LTYuNjA3LDMuMDQ3LTEzLjc4OSw4LjY3Mi0xOC40OGM2LjA5NC01LjE3MiwxNS4yMzQtNy41NjYsMjMuNzg5LTcuNTY2CgkJCQkJCWMxOS45MjIsMCwzMi4xMDksMTAuNTM1LDMyLjEwOSwyNi42MjF2Mi41ODZIOTQuMTNjMCw1LjI2OCwyLjEwOSw5LjA5Niw5LjE0LDkuMDk2YzUuMjczLDAsOC4yMDMtMS43MjMsOS4xNDEtNS45MzYKCQkJCQkJTDEzNC4zMjQsMjA0LjA1OHogTTExMC44ODcsMTkwLjE3M2MwLTQuNzg5LTEuODc1LTguNDI2LTguNDM4LTguNDI2Yy02LjA5NCwwLTguMzIsMy45MjYtOC4zMiw4LjQyNkgxMTAuODg3eiIvPgoJCQkJCTxwYXRoIGZpbGw9IiMxRDFEMUIiIGQ9Ik0xNzcuMzMsMTI1LjE1MXY1MC4zNzFjLTMuNjMyLTIuOTY5LTguNTU0LTQuOTgtMTQuMjk2LTQuOThjLTE4LjA0NiwwLTIzLjkwNiwxMy4xMTktMjMuOTA2LDI1LjY2NAoJCQkJCQljMCwxMi4wNjYsNS45NzcsMjUuMTg0LDIzLjIwMiwyNS4xODRjNy41LDAsMTIuNTM5LTIuMTA1LDE1LjkzOC03LjQ2OWgwLjIzNHY2LjUxMmgyMi4wMzF2LTk0LjcxNAoJCQkJCQlDMTkzLjcyNywxMzEuMTI4LDE4My45MjUsMTMwLjkzOSwxNzcuMzMsMTI1LjE1MXogTTE3NS45MjQsMjA2LjE2NWMtMS4yOSwxLjYyNy0yLjgxMywyLjQ4OC01LjE1NywyLjQ4OAoJCQkJCQljLTcuMjY1LDAtNy42MTctNy42Ni03LjYxNy0xMS45NjljMC0yLjY4MiwwLjQ2OS02LjQxNiwxLjE3My05LjA5OGMwLjcwMi0yLjk2NywyLjQ2LTUuMTcsNi41NjItNS4xNwoJCQkJCQljMS44NzUsMCwzLjk4NCwwLjY3LDUuMDQsMS45MTRjMS41MjMsMS41MzMsMS40MDYsMy44MzIsMS40MDYsNS43NDZ2OS44NjNDMTc3LjMzLDIwMi4zMzUsMTc3LjU2NCwyMDQuMDU4LDE3NS45MjQsMjA2LjE2NXoiCgkJCQkJCS8+CgkJCQkJPHBhdGggZmlsbD0iIzFEMUQxQiIgZD0iTTI0Ni4zNSwyMjAuNDMzdi04LjA0M2gtMC4yMzRjLTIuNDYxLDYuMzItMTAuNDMsOS0xOC4xNjQsOWMtNC45MjIsMC05LjcyNy0wLjk1Ny0xMy4zNTgtMy43MzQKCQkJCQkJYy00LjIyLTMuNDQ3LTQuNjg4LTcuNzU2LTQuNjg4LTEyLjM1MnYtMzMuODAzaDIzLjIwM3YyOS41ODhjMCwzLjE2LTAuMTE3LDYuNTEyLDUuMTU1LDYuNTEyYzIuMzQ1LDAsNC41NzEtMS4wNTMsNS42MjYtMi43NzUKCQkJCQkJYzEuMDU1LTEuNjI5LDEuMTcyLTMuMzUyLDEuMTcyLTUuMTcydi0yOC4xNTJoMjMuMjAydjQ4LjkzMkgyNDYuMzV6Ii8+CgkJCQkJPHBhdGggZmlsbC1ydWxlPSJldmVub2RkIiBjbGlwLXJ1bGU9ImV2ZW5vZGQiIGZpbGw9IiMwMDY1OEQiIGQ9Ik0zMDAuMTM2LDE4MS41NTZjMS45OTItNi43OTksNi4zMjctMTEuNTg4LDE1LjU4NS0xMS4wMTQKCQkJCQkJbDMuMTYyLDAuMTkxdjE3LjE0M2MtMS40MDQtMC4yODktMi45MjgtMC40OC00LjQ1MS0wLjQ4Yy0zLjc1LDAtNy42MTcsMC41NzYtOS40OTEsMy4zNTQKCQkJCQkJYy0xLjY0MSwyLjM5My0xLjY0MSw1LjU1My0xLjY0MSw4LjQyNnYyMS4yNThoLTIzLjIwM3YtNDguOTMyaDE5LjgwNHYxMC4wNTVIMzAwLjEzNnoiLz4KCQkJCQk8cGF0aCBmaWxsLXJ1bGU9ImV2ZW5vZGQiIGNsaXAtcnVsZT0iZXZlbm9kZCIgZmlsbD0iIzAwNjU4RCIgZD0iTTM4OC4xNCwxOTYuMzk3YzAsMTQuODQyLTE0LjY0OCwyNC45OTItMzIuOTMsMjQuOTkyCgkJCQkJCXMtMzIuOTI4LTEwLjE1LTMyLjkyOC0yNC45OTJjMC0xNS43MDUsMTQuMTgtMjUuODU1LDMyLjkyOC0yNS44NTVDMzczLjk2LDE3MC41NDIsMzg4LjE0LDE4MC42OTIsMzg4LjE0LDE5Ni4zOTd6CgkJCQkJCSBNMzQ3LjEyNCwxOTUuNzI4YzAsMTMuMTE3LDMuODY3LDE0LjA3Niw4LjA4NiwxNC4wNzZzOC4wODYtMC45NTksOC4wODYtMTQuMDc2YzAtNC42OTEtMC4xMTctMTMuMjE1LTguMDg2LTEzLjIxNQoJCQkJCQlTMzQ3LjEyNCwxOTEuMDM2LDM0Ny4xMjQsMTk1LjcyOHoiLz4KCQkJCQk8cGF0aCBmaWxsLXJ1bGU9ImV2ZW5vZGQiIGNsaXAtcnVsZT0iZXZlbm9kZCIgZmlsbD0iIzAwNjU4RCIgZD0iTTQzMy44NDEsMjIwLjQzM2MtMC41ODgtMi4yMDEtMC44MjItNC41LTAuODIyLTYuNzk5CgkJCQkJCWMtNC42ODYsNS42NS0xMS4zNjUsNy43NTYtMTkuNTY4LDcuNzU2Yy0xMS4xMzMsMC0yMC44NTktNC45NzktMjAuODU5LTE0Ljc0NmMwLTcuNzU2LDcuMzgzLTEyLjkyNiwxNS43MDMtMTUuMDMzCgkJCQkJCWM3LjYxNy0xLjkxNiwxNS43MDMtMi42ODIsMjMuNTU1LTMuMDY0di0wLjM4M2MwLTQuNDA2LTEuNDA2LTYuMTI5LTcuMDMxLTYuMTI5Yy00LjU3LDAtOC40MzgsMS4zNC05LjE0MSw1LjQ1N2wtMjEuMzI4LTEuNjI3CgkJCQkJCWMzLjUxNi0xMi4xNiwxNy42OTUtMTUuMzIyLDMxLjE3Mi0xNS4zMjJjNy4wMzEsMCwxNi4wNTUsMC43NjgsMjEuOTEyLDQuMjE1YzcuODUyLDQuNSw2LjkxNiwxMC44Miw2LjkxNiwxNy44MTF2MTcuMTQxCgkJCQkJCWMwLDMuNjM5LDAuMTE1LDcuMjc3LDEuODczLDEwLjcyNUg0MzMuODQxeiBNNDMxLjg0OSwxOTcuNTQ4Yy01LjYyNSwwLjQ3OS0xNSwxLjkxNC0xNSw3Ljg1MmMwLDMuMTYsMi40NjEsNC41LDYuMDk0LDQuNQoJCQkJCQljOC43ODksMCw4LjkwNi02LjAzMyw4LjkwNi0xMS4zOTVWMTk3LjU0OHoiLz4KCQkJCQk8cGF0aCBmaWxsLXJ1bGU9ImV2ZW5vZGQiIGNsaXAtcnVsZT0iZXZlbm9kZCIgZmlsbD0iIzAwNjU4RCIgZD0iTTQ4NC40NjIsMTcxLjUwMXY2LjYwN2gwLjIzNAoJCQkJCQljMy4yODEtNC45OCw5LjI1OC03LjU2NiwxNi4yODktNy41NjZjNy4zODMsMCwxMy4yNCwyLjI5OSwxNi44NzUsNy41NjZjNC4xMDItNS40NTksMTAuMDc4LTcuNTY2LDE3LjkyOC03LjU2NgoJCQkJCQljNS4wMzksMCwxMC40MywxLjI0NCwxMy45NDcsNC4yMTVjNC4zMzYsMy42MzksNC4zMzYsOC40MjgsNC4zMzYsMTMuNTk4djMyLjA3OGgtMjIuNzM2di0yOS4zOTYKCQkJCQkJYzAtMy4xNiwwLjQ2OS02LjcwNS00LjkyLTYuNzA1Yy02LjU2MywwLTYuMzI4LDQuOTgtNi4zMjgsOC45MDZ2MjcuMTk1aC0yMi43MzR2LTI5LjM5NmMwLTMuMDY0LTAuMjM0LTYuOTkyLTUuMjczLTYuOTkyCgkJCQkJCWMtNS41MDgsMC01Ljk3NywzLjM1Mi01Ljk3Nyw2Ljk5MnYyOS4zOTZINDYzLjM3di00OC45MzJINDg0LjQ2MnoiLz4KCQkJCTwvZz4KCQkJCTxwYXRoIGZpbGwtcnVsZT0iZXZlbm9kZCIgY2xpcC1ydWxlPSJldmVub2RkIiBmaWxsPSIjMUQxRDFCIiBkPSJNMjAwLjAzLDEyMC44MDZjLTUuOTMxLDUuOTMtMTUuNTU4LDUuOTMtMjEuNDg4LDAKCQkJCQljLTUuOTMxLTUuOTMtNS45MzEtMTUuNTU3LDAtMjEuNDg4YzUuOTMxLTUuOTI5LDE1LjU1OC01LjkyOSwyMS40ODgsMEMyMDUuOTYsMTA1LjI0OSwyMDUuOTYsMTE0Ljg3NSwyMDAuMDMsMTIwLjgwNgoJCQkJCUwyMDAuMDMsMTIwLjgwNnoiLz4KCQkJPC9nPgoJCTwvZz4KCTwvZz4KPC9nPgo8L3N2Zz4=";
    }
}