using Giantnodes.Infrastructure;

namespace Giantnodes.Service.Dashboard.Domain.Enumerations;

public sealed class MediaFileExtension : Enumeration
{
    public static readonly MediaFileExtension Webm = new(1, ".webm");
    public static readonly MediaFileExtension M4V = new(2, ".m4v");
    public static readonly MediaFileExtension Nsv = new(3, ".nsv");
    public static readonly MediaFileExtension Ty = new(4, ".ty");
    public static readonly MediaFileExtension Strm = new(5, ".strm");
    public static readonly MediaFileExtension Rm = new(6, ".rm");
    public static readonly MediaFileExtension Rmbv = new(7, ".rmvb");
    public static readonly MediaFileExtension M3U = new(8, ".m3u");
    public static readonly MediaFileExtension Ifo = new(9, ".ifo");
    public static readonly MediaFileExtension Mov = new(10, ".mov");
    public static readonly MediaFileExtension Qt = new(11, ".qt");
    public static readonly MediaFileExtension Divx = new(12, ".divx");
    public static readonly MediaFileExtension Xvid = new(13, ".xvid");
    public static readonly MediaFileExtension Bivx = new(14, ".bivx");
    public static readonly MediaFileExtension Nrg = new(15, ".nrg");
    public static readonly MediaFileExtension Pva = new(16, ".pva");
    public static readonly MediaFileExtension Mwv = new(17, ".wmv");
    public static readonly MediaFileExtension Asf = new(18, ".asf");
    public static readonly MediaFileExtension Ask = new(19, ".asx");
    public static readonly MediaFileExtension Ogm = new(20, ".ogm");
    public static readonly MediaFileExtension Ogv = new(21, ".ogv");
    public static readonly MediaFileExtension M2V = new(22, ".m2v");
    public static readonly MediaFileExtension Avi = new(23, ".avi");
    public static readonly MediaFileExtension Bin = new(24, ".bin");
    public static readonly MediaFileExtension Dat = new(25, ".dat");
    public static readonly MediaFileExtension DvrMs = new(26, ".dvr-ms");
    public static readonly MediaFileExtension Mpg = new(27, ".mpg");
    public static readonly MediaFileExtension Mpeg = new(28, ".mpeg");
    public static readonly MediaFileExtension Mp4 = new(29, ".mp4");
    public static readonly MediaFileExtension Avc = new(30, ".avc");
    public static readonly MediaFileExtension Vp3 = new(31, ".vp3");
    public static readonly MediaFileExtension Svq3 = new(32, ".svq3");
    public static readonly MediaFileExtension Nuv = new(33, ".nuv");
    public static readonly MediaFileExtension Viv = new(34, ".viv");
    public static readonly MediaFileExtension Dv = new(35, ".dv");
    public static readonly MediaFileExtension Fli = new(36, ".fli");
    public static readonly MediaFileExtension Flv = new(37, ".flv");
    public static readonly MediaFileExtension Wpl = new(38, ".wpl");
    public static readonly MediaFileExtension Img = new(39, ".img");
    public static readonly MediaFileExtension Iso = new(40, ".iso");
    public static readonly MediaFileExtension Vob = new(41, ".vob");
    public static readonly MediaFileExtension Mkv = new(42, ".mkv");
    public static readonly MediaFileExtension Ts = new(43, ".ts");
    public static readonly MediaFileExtension Wtv = new(44, ".wtv");
    public static readonly MediaFileExtension M2Ts = new(45, ".m2ts");

    private MediaFileExtension(int id, string name)
        : base(id, name)
    {
    }
}