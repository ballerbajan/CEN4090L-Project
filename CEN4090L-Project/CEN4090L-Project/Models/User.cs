using System;

public class User
{
	public User()
	{}

	public string? Name { get; set; }

	public string? Email { get; set; }

	public string? Password { get; set; }

	public string? Username { get; set; }


    public override string ToString()
    {
        return $"[{Username}] {Name} - {Email}";
    }
}
