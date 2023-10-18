using Giantnodes.Infrastructure;

namespace Giantnodes.Service.Dashboard.Domain.Enumerations;

public sealed class VideoFileExtension : Enumeration
{
    public static readonly VideoFileExtension Webm = new(1, ".webm");
    public static readonly VideoFileExtension M4V = new(2, ".m4v");
    public static readonly VideoFileExtension Nsv = new(3, ".nsv");
    public static readonly VideoFileExtension Ty = new(4, ".ty");
    public static readonly VideoFileExtension Strm = new(5, ".strm");
    public static readonly VideoFileExtension Rm = new(6, ".rm");
    public static readonly VideoFileExtension Rmbv = new(7, ".rmvb");
    public static readonly VideoFileExtension M3U = new(8, ".m3u");
    public static readonly VideoFileExtension Ifo = new(9, ".ifo");
    public static readonly VideoFileExtension Mov = new(10, ".mov");
    public static readonly VideoFileExtension Qt = new(11, ".qt");
    public static readonly VideoFileExtension Divx = new(12, ".divx");
    public static readonly VideoFileExtension Xvid = new(13, ".xvid");
    public static readonly VideoFileExtension Bivx = new(14, ".bivx");
    public static readonly VideoFileExtension Nrg = new(15, ".nrg");
    public static readonly VideoFileExtension Pva = new(16, ".pva");
    public static readonly VideoFileExtension Mwv = new(17, ".wmv");
    public static readonly VideoFileExtension Asf = new(18, ".asf");
    public static readonly VideoFileExtension Ask = new(19, ".asx");
    public static readonly VideoFileExtension Ogm = new(20, ".ogm");
    public static readonly VideoFileExtension Ogv = new(21, ".ogv");
    public static readonly VideoFileExtension M2V = new(22, ".m2v");
    public static readonly VideoFileExtension Avi = new(23, ".avi");
    public static readonly VideoFileExtension Bin = new(24, ".bin");
    public static readonly VideoFileExtension Dat = new(25, ".dat");
    public static readonly VideoFileExtension DvrMs = new(26, ".dvr-ms");
    public static readonly VideoFileExtension Mpg = new(27, ".mpg");
    public static readonly VideoFileExtension Mpeg = new(28, ".mpeg");
    public static readonly VideoFileExtension Mp4 = new(29, ".mp4");
    public static readonly VideoFileExtension Avc = new(30, ".avc");
    public static readonly VideoFileExtension Vp3 = new(31, ".vp3");
    public static readonly VideoFileExtension Svq3 = new(32, ".svq3");
    public static readonly VideoFileExtension Nuv = new(33, ".nuv");
    public static readonly VideoFileExtension Viv = new(34, ".viv");
    public static readonly VideoFileExtension Dv = new(35, ".dv");
    public static readonly VideoFileExtension Fli = new(36, ".fli");
    public static readonly VideoFileExtension Flv = new(37, ".flv");
    public static readonly VideoFileExtension Wpl = new(38, ".wpl");
    public static readonly VideoFileExtension Img = new(39, ".img");
    public static readonly VideoFileExtension Iso = new(40, ".iso");
    public static readonly VideoFileExtension Vob = new(41, ".vob");
    public static readonly VideoFileExtension Mkv = new(42, ".mkv");
    public static readonly VideoFileExtension Ts = new(43, ".ts");
    public static readonly VideoFileExtension Wtv = new(44, ".wtv");
    public static readonly VideoFileExtension M2Ts = new(45, ".m2ts");

    private VideoFileExtension(int id, string name)
        : base(id, name)
    {
    }
}