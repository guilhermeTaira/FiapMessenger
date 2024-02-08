namespace Core;


public class Order
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public DateTime CreateDate { get; set; }

    public Order(Guid id, User user)
    {
        Id = id;
        User = user;
    }

    public override string ToString() =>
        $"OrderId: {Id}, USer: {User.Name}, CreateDate:{CreateDate:dd/MM/yyyy}";
}
