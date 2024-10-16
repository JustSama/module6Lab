using System;
using System.Collections.Generic;

public interface IPrototype
{
    IPrototype Clone();
}

public class Section : IPrototype
{
    public string Title { get; set; }
    public string Content { get; set; }

    public IPrototype Clone()
    {
        return new Section { Title = Title, Content = Content };
    }
}

public class Image : IPrototype
{
    public string URL { get; set; }

    public IPrototype Clone()
    {
        return new Image { URL = URL };
    }
}

public class Document : IPrototype
{
    public string Title { get; set; }
    public List<Section> Sections { get; set; } = new List<Section>();
    public List<Image> Images { get; set; } = new List<Image>();

    public IPrototype Clone()
    {
        Document clone = new Document
        {
            Title = Title
        };
        foreach (var section in Sections)
        {
            clone.Sections.Add((Section)section.Clone());
        }
        foreach (var image in Images)
        {
            clone.Images.Add((Image)image.Clone());
        }
        return clone;
    }
}

public class DocumentManager
{
    public IPrototype CreateDocument(IPrototype prototype)
    {
        return prototype.Clone();
    }
}

class Program
{
    static void Main(string[] args)
    {
        DocumentManager manager = new DocumentManager();

        Document originalDocument = new Document
        {
            Title = "Документ 1"
        };
        originalDocument.Sections.Add(new Section { Title = "Раздел 1", Content = "Содержимое раздела 1" });
        originalDocument.Images.Add(new Image { URL = "http://example.com/image1.png" });

        Document clonedDocument = (Document)manager.CreateDocument(originalDocument);
        clonedDocument.Title = "Клонированный документ 1";
        clonedDocument.Sections[0].Content = "Измененное содержимое раздела 1";
        clonedDocument.Images.Add(new Image { URL = "http://example.com/image2.png" });

        Console.WriteLine($"Оригинальный документ: {originalDocument.Title}");
        foreach (var section in originalDocument.Sections)
        {
            Console.WriteLine($"Раздел: {section.Title}, Содержимое: {section.Content}");
        }
        foreach (var image in originalDocument.Images)
        {
            Console.WriteLine($"Изображение: {image.URL}");
        }

        Console.WriteLine($"\nКлонированный документ: {clonedDocument.Title}");
        foreach (var section in clonedDocument.Sections)
        {
            Console.WriteLine($"Раздел: {section.Title}, Содержимое: {section.Content}");
        }
        foreach (var image in clonedDocument.Images)
        {
            Console.WriteLine($"Изображение: {image.URL}");
        }
    }
}
