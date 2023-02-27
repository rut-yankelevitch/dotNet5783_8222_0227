
using System.Reflection.Metadata.Ecma335;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
namespace Dal;
static class XMLTools
{
    const string s_dir = @"..\xml\";
    static XMLTools()
    {
        if (!Directory.Exists(s_dir))
            Directory.CreateDirectory(s_dir);
    }

    #region Extension Fuctions
    public static T ToEnum<T>(this XElement element, string name) where T : struct, Enum
    {
        Enum.TryParse<T>((string?)element.Element(name), out var result);
        return result;
    }

    public static DateTime? ToDateTimeNullable(this XElement element, string name) =>
        DateTime.TryParse((string?)element.Element(name), out var result) ? (DateTime?)result : null;

    public static double? ToDoubleNullable(this XElement element, string name) =>
        double.TryParse((string?)element.Element(name), out var result) ? (double?)result : null;

    public static int? ToIntNullable(this XElement element, string name) =>
        int.TryParse((string?)element.Element(name), out var result) ? (int?)result : null;
    #endregion

    #region SaveLoadWithXElement
    public static void SaveListToXMLElement(XElement rootElem, string entity)
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            rootElem.Save(filePath);
        }
        catch (Exception ex)
        {
            // DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            throw new Exception($"fail to create xml file: {filePath}", ex);
        }
    }


    public static int getNextID(string element)
    {
        string filePath = $"{s_dir}config.xml";

        if (!File.Exists(filePath))
            throw new Exception($"fail to load xml file: {filePath}");

        XElement rootElem = XElement.Load(filePath);
        XElement elem = rootElem.Element(element) ?? throw new Exception($"could not find {element} element on {filePath}");

        int id;

        if (!int.TryParse(elem.Value, out id))
            throw new Exception($"{element} value was invalid");

        elem.Value = (id + 1).ToString();

        try { rootElem.Save(filePath); }
        catch (Exception ex) { throw new Exception($"fail to save xml file: {filePath}", ex); }

        return id;
    }


    public static XElement LoadListFromXMLElement(string entity)
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            if (File.Exists(filePath))
                return XElement.Load(filePath);
            XElement rootElem = new(entity);
            rootElem.Save(filePath);
            return rootElem;
        }
        catch (Exception ex)
        {
            throw new DO.XMLFileLoadCreateException(filePath, ex);
            //throw new Exception($"fail to load xml file: {filePath}", ex);
        }
    }
    #endregion

    #region SaveLoadWithXMLSerializer
    public static void SaveListToXMLSerializer<T>(List<T?> list, string entity) where T : struct
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            using FileStream file = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            XmlSerializer serializer = new(typeof(List<T?>));
            serializer.Serialize(file, list);
        }
        catch (Exception ex)
        {
            throw new DO.XMLFileLoadCreateException(filePath, ex);
            //throw new Exception($"fail to create xml file: {filePath}", ex);
        }
    }

    public static List<T?> LoadListFromXMLSerializer<T>(string entity) where T : struct
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            if (!File.Exists(filePath)) return new();
            using FileStream file = new(filePath, FileMode.Open);
            XmlSerializer x = new(typeof(List<T?>));
            return x.Deserialize(file) as List<T?> ?? new();
        }
        catch (Exception ex)
        {
            throw new DO.XMLFileLoadCreateException(filePath, ex);
            //throw new Exception($"fail to load xml file: {filePath}", ex);
        }
    }
    #endregion
}
