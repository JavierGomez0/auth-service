namespace AuthService.Application.Interfaces;

public interface IFileData
{
    string FileName { get; }
    string ContentType { get; }
    Stream Content { get; }
}