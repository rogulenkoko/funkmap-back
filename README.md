## Шина на Redis

Инструмент, позволяющий коммуникации сервисов через шину RedisMQ
На данный момент реализован только синхронный Request + Reply MQ Pattern (привычный механизм запрос/ответ)



Пример использования можно посмотреть в тесте Scout.Utils.ServiceBus.Tests.Redis.RedisServiceBusTest


#### Модуль Autofac

Так же при использовании IoC контейнера Autofac, можно использовать пакет Scout.Utils.ServiceBus.Redis.Autofac, в котором находится модуль RedisServiceBusModule (IIocModule), который регистрирует все необходимые зависимости для типов IRedisMessageConsumer и наследников RedisMessageProducerService.

При его использовании в App.config приложения необходимо добавить секцию вида
```xml
<configSections>
    <section name="redis" type="Scout.Utils.ServiceBus.Redis.Autofac.Configuration.RedisServiceBusSection, Scout.Utils.ServiceBus.Redis.Autofac" />
</configSections>
<redis redisConnection="localhost:6379"></redis>
```


