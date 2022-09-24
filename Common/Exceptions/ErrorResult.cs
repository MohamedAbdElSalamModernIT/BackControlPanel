namespace Common {
  public class ErrorResult {
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
    public ErrorResult(string propertyName,string errorMessage) {
      PropertyName = propertyName;
      ErrorMessage = errorMessage;
    }
  }
}
