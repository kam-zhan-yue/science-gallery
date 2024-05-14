using System;
using System.Collections.Generic;
using Ink.Runtime;

public class DialoguePayload
{
    public string body = string.Empty;
    public List<Choice> choices = new();
    public bool stop = false;
}