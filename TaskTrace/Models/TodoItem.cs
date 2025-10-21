namespace TaskTrace.Models;

public class TodoItem
{
	public string FilePath { get; set; } = "";
	public int LineNumber { get; set; }
	public string Text { get; set; } = "";
	public string Tag { get; set; } = ""; // can contain: TODO, FIXME, NOTE
}