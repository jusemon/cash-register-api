namespace CashRegister.Models
{
  public class Task
  {
    public long TaskId { get; set; }

    public string Text { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }
  }
}