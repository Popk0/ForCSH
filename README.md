# Здрав буди, боярин!
***
Сейчас потолкую чутка о работе проведенной. 
И дабы у Вас не было перегрузки, от кол-ва текста, буду чуть сиять.

![](https://media.giphy.com/media/HBMCmtsPEUShG/giphy.gif)
***
# Описание проги:
  Таки что мы имеем. Коли раньше наша работа заключалась в разработке менеджера поставки конфигурации на сервер, где производилась манипуляция с данными, то на сей раз эти файлы уже будут сожержать данные, вот такой levelup.

Кажется только вчера config.xml был наполнен лишь Source и Target, ну и еще парочкой, однако сейчас мы имеем дело с действительно занятным и грандиозным - базами данных, уххх

>Пожалуй, покажу Вам свои прелести, что же черканул

Кидать код в readme не особо хочу, поэтому накидаю киборгссылок, чтоб сразу видеть, о чем идет речь

  Подключение к бд осуществляется при помощи строки подключение - ConnectionString. Ее я вытягиваю [отсюдова](https://github.com/Popk0/ForCSH/blob/lab_4/Pattern/App.config), заранее прописав.

  Имея ее и, конечно же, при успешном подключении к бд, нам открывается свой мирок. Причем и здесь есть мои корни, в виде встроенной процедуры - dbo.GetEmployeeByRegion, с одним входным параметром @RegionID. 

![](https://github.com/Popk0/ForCSH/blob/lab_4/Screenshots/dbP.png)

![](https://github.com/Popk0/ForCSH/blob/lab_4/Screenshots/dbT.png)

>С помощью нескольких join'ов и сопоставлений таблиц на наших глазах рождается новая таблица, как приятно дарить ей жизнь

Теперь-то нам эту таблицу нужно поместить в **xml-файл**. Заглянем в код.

![](https://media.giphy.com/media/qQI16x8tgp7nW/source.gif)
***
# Структура проекта:

Проект Pattern содержит функцию, которая подключается к бд, создает сущность [SearchCriteria](https://github.com/Popk0/ForCSH/blob/lab_4/Models/SearchModels/SearchCriteria.cs) для встроенной процедуры, получает в result лист из таблицы, после чего на его основе генерирует xml-файл. Reference'ится на 2 проекта: [DataAccess](https://github.com/Popk0/ForCSH/tree/lab_4/DataAccess) и [Models](https://github.com/Popk0/ForCSH/tree/lab_4/Models).

**Чтобы организовать работу хранимой процедурки красиво, создал 3 **модели**, находящиеся в проекте [Models](https://github.com/Popk0/ForCSH/tree/lab_4/Models):**

- Модель [Region](https://github.com/Popk0/ForCSH/blob/lab_4/Models/Region.cs) 
>(проста модель для полей хранимой процедуры, которые в дальнейшем будут заполняться по ходу чтения таблицы)

- Модель [SearchCriteria](https://github.com/Popk0/ForCSH/blob/lab_4/Models/SearchModels/SearchCriteria.cs) 
>(которая содержит параметр для хранимой процедуры)

- Модель [SearchResult](https://github.com/Popk0/ForCSH/blob/lab_4/Models/SearchModels/SearchResult.cs) 
>(модель результата, с набором сущностей Entities, при этом он Generic, то есть никак не привязанный к контексту, то есть, можно юзать для абсолютно любой таблицы)

**В [DataAccess](https://github.com/Popk0/ForCSH/tree/lab_4/DataAccess) хранятся сущности, которые позволют подключится и вытянуть что-нибудь из базы данных:**

- [RegionsRepository](https://github.com/Popk0/ForCSH/blob/lab_4/DataAccess/RegionsRepository.cs). 
>Сущность, которая предоставляет API публичных методов, для работы с какой-то конкретной таблицей (в моем случае это таблица Regions. В нем выставлен метод GetOrders, который своей сигнатурой в качестве input-параметра принимает SearchCriteria и возвращает SearchResult, параметризованный Region'ом.

- [SqlCommandExtentions](https://github.com/Popk0/ForCSH/blob/lab_4/DataAccess/Extentions/SqlCommandExtentions.cs). 
>ReadMany - generic метод расширения для сущности SqlCommand, написанный с целью упрощения работы. Тажке доп. абстракция поверх Reader'a, для вызова ReadMany и получиния IEnumerable, то есть коллекцию Region'ов. В нем написан ReadManyInternal - internal реализация, возвращает при наличие в наборе. Еще один метод SqlDataReader, где reader содержит результирующий набор колонок, по которым мы идем while'ом, утрируемся и получаем значения.
***
Вот черт, пообещал сиять, а тут так нудненько балаболил. Боялся что-то упустить, чтоб все понятненько. Пасибо за внимание)

![](https://media.giphy.com/media/dOJt6XZlQw8qQ/giphy.gif)
