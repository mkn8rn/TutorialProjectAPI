using Microsoft.EntityFrameworkCore;
using TutorialProjectAPI.Contexts;
using TutorialProjectAPI.Repositories;

public record PostCreateDto(
    Guid UserId,
    string Body,
    List<ReplyCreateDto>? Replies);