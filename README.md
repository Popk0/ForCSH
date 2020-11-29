
```html
<meta name="viewport" content="width=device-width, initial-scale=1.0">
```

![screenshot of sample](http://webdesign.ru.net/images/Heydon_min.jpg)


**Шалом алейхем!**
=====

>Уточки уже там, за горизонтом, студёный ветер беспощадно обдувает нас.
>Но не все так плохо, ведь вот она, с пылу, с жару 3-яя работа, а пахне як

-----

Занимательным отличием от 2-ой работы является наличие конфигурационный файлов c расширением **.xml** и **.js**, в моем случае это *config.xml* и config.js*.
С помощью **парсеров** мы достаем из них сведения для дальнейшей работы с файлами (`SourcePath` и `TargetPath` , `IsForEncrypt` и `IsForArchive` [*в то время как они были захардкожены в пред. работе*]).

Скажи кто ты , и я скажу кто ты
-----

Service оснащен классом Logger, как и предыдущая модель, но с усовершенствованным арсеналом для успешной работы с `xml` и `js`, а также для последующего заполнения полей экземляра класса Options.

Не важно кто где, важно где кто
-----

Происходит десериализация - процесс чтения `XML-документа` и создания объекта, строго типизированного для `XML-схемы (XSD)` документа.
Перед десериализацией **XmlSerializer** должен быть сконструирован с использованием типа объекта, который десериализуется, в нашем случае это **Options**
```cs
public Options XMLDeserialize()
{
    try
    {
        XmlSerializer xml = new XmlSerializer(typeof(Options));

        using (FileStream fs = new FileStream(xmlPath, FileMode.OpenOrCreate))
        {
            return (Options)xml.Deserialize(fs);
        }                
    }
    catch
    {
        throw new Exception(@"Error in xml deserialize");
    }
}
```
Одна ошибка и ты ошибся
-----

> Наш **ConfigurationManager** помогает нам определить, с чем мы будем иметь дело, посредством проверки расширения конфигурации.
```cs
class ConfigurationManager
{
    readonly IParser parser;
    public ConfigurationManager(string path, string config)
    {
        if (Path.GetExtension(path) == ".xml")
        {
            parser = new XMLParser(path, config);
        }            
        parser = new JSParser(path);                        
    }
    public Options GetParseOptions() => parser.GetParseOptions();
}
```
F
-----

Парсеры **XMLParser** и **JSParser** наследуются от интерфейса `IParser`, реализующем метод `GetParseOptions`, с возвращением уже заполненного объекта **Options**
>Причем философия парсеров немного отличается, в отличие от JSParser'а, XMLParser проходит проверку по схеме с помощью **XPathNavigator**. Метод `Validate` класса **XmlDocument** проверяет `XML-документ`, содержащийся в объекте **XmlDocument**, по схеме, указанной в свойстве **XmlDocument** объекта `Schemas`, и выполняет приращение >информационного набора. Результатом является замена в объекте **XmlDocument** ранее нетипизированного `XML-документа` типизированным документом.
```cs
...
XmlDocument xmlDoc = new XmlDocument();
xmlDoc.Load(xmlPath);
XPathNavigator navigator = xmlDoc.CreateNavigator();
xmlDoc.Schemas.Add("", xsdPath);
ValidationEventHandler validation = new ValidationEventHandler(SchemaValidationHandler);
...
xmlDoc.Validate(validation);
```
И как только виднеется свет в конце туннеля, класс **Operations** и его процедуры уже на готове сделать все трюки с уже известными параметрами)

