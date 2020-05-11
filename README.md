# ExampleBackendApiWebDotNetCore

Резюме в каталоге Doc

Файл Postman для тестирования api в каталоге Doc


# Test task description

# Задача

Разработать Web API сервис, который позволяет работать с набором ресурсов. Каждый ресурс представляет из себя строку с идентификатором. Например:

```
[1] -> "foo"
[2] -> "bar"
[4] -> "baz"
```

Тип идентификатора остаётся на усмотрение разработчика.

Данные хранятся только в оперативной памяти, сохранения на диск в каком-либо виде (БД, файлы) не требуется.

## **Список необходимых методов API**

- Получить список всех ресурсов
- Получить ресурс

    *здесь и далее работа с каждым ресурсом ведётся по его ID*

- Получить подстроку ресурса по индексу начала и длине подстроки
- Создать ресурс (с возможностью предоставить начальное значение)
- Перезаписать ресурс
- Удалить ресурс
- Частичное обновление ресурса по ID:
    - Вставка подстроки
        - в начало
        - в конец
        - в любое место по индексу
    - Удаление подстроки по индексу и длине
    - Замена подстроки на другую

**Важно!** Необходимо предусмотреть ситуации, когда одновременно несколько клиентов могут вызывать методы API, т.е. среда многопоточная, доступ к ресурсам конкурентный.

## Вызов стороннего Webhook

При любом изменении ресурсов необходимо вызывать HTTP Webhook на заданном адресе. В payload должны входить:

- ID ресурса
- тип события
- обновлённое значение

Вызов Webhook считается успешным, если вернулся HTTP код 200.

Необходимо предусмотреть возможные сбои в работе Webhook и по возможности увеличить шансы успешной доставки: несколько попыток вызова.

Сбои при вызове webhook или его долгое выполнение не должны влиять на работоспособность и производительность сервиса.

*Hint: для тестирования можно использовать [https://reqres.in/](https://reqres.in/).*
