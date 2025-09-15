using System.Text.Json.Serialization;
using CEN4090L_Project.Models;

[JsonSerializable(typeof(Project))]
[JsonSerializable(typeof(ProjectTask))]
[JsonSerializable(typeof(ProjectsJson))]
[JsonSerializable(typeof(Category))]
[JsonSerializable(typeof(Tag))]
public partial class JsonContext : JsonSerializerContext
{
}