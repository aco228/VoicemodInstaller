using Installer.Domain.Gitlab.Models;

namespace Installer.Services.Gitlab;

public interface IGitlabUserService
{
    public Task<GitlabUser> GetUser();
}