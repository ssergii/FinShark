namespace FinShark.Services.Interfaces.Domain;

public class User
{
    #region constructor
    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }
    #endregion

    #region properties
    public string Name { get; }
    public string Email { get; }
    #endregion
}
