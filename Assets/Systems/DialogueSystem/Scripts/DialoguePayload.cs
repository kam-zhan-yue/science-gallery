using System;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class DialoguePayload
{
    public string title = string.Empty;
    public string body = string.Empty;
    public List<Choice> choices = new();
    public Sprite portrait;
    public bool stop = false;
}