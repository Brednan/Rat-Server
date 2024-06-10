namespace Rat_Server.Model
{
    /// <summary>
    /// This Dto is what gets sent to the rat client 
    /// as the current command for it to execute
    /// </summary>
    public class CurrentCommandDto
    {
        public string commandId { get; set; }
        public string CommandValue { get; set; }
    }
}
