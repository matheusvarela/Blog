using FriendlyUrlHelper;

var builder = WebApplication.CreateBuilder(args);

var host = builder.Configuration.GetValue<string>("Host");

var app = builder.Build();

List<Post> posts = new();


app.MapGet("v1/posts", () => Results.Ok(posts));

app.MapGet("v1/posts/{slug}", (string slug) => Results.Ok(posts.FirstOrDefault(x => x.Slug == slug)));

app.MapPost("v1/posts", (CreatePostModel model) =>
{

    var slug = model.Title.ToUrl();

    var post = new Post(
        Id: posts.Count + 1,
        model.Title,
        slug,
        model.Body,
        Url: $"{host}/v1/posts/{slug}",
        CreatedAt: DateTime.UtcNow,
        UpdatedAt: DateTime.UtcNow
    );

    posts.Add(post);

    return Results.Created(uri: "v1/posts", post);
});


app.Run();

public record CreatePostModel(
    string Title,
    string Body
);

public record Post(
    int Id,
    string Title,
    string Slug,
    string Body,
    string Url,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
